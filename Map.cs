using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Map : _Singleton<Map>
{
    public static List<GameObject> OriginalTokens = new List<GameObject>();

    public static GameObject HeroToken;
    public static Animator heroTokenAnimator;
    public static Image heroTokenImage;

    public static List<GameObject> HeroTokenList = new List<GameObject>();
    public static List<GameObject> UnitTokenList = new List<GameObject>();

    public static List<GameObject> goSpotsList = new List<GameObject>();
    
    public static List<SpriteRenderer> spSpotsEdge = new List<SpriteRenderer>();
    public static SpriteRenderer[,] srPathsArray;

    public static float radius = 3f;

    [Serializable]
    public class _Spot
    {
        public int _index;
        public float _px;
        public float _pz;
        public float _radius = 3;
        public bool _isActive = true;

        public int _distanceFromStart = -1;
        public int _visitCount;
        public string _type;
        
        public List<int> _connectingSpotsIndex = new List<int>();

        public _Item[] _shopItems = new _Item[8] { null, null, null, null, null, null, null, null };
        public _Equip[] _shopEquips = new _Equip[6] { null, null, null, null, null, null };
        //public _Skill[] _shopSkills = new _Skill[4] { null, null, null, null };

        public string[] _treasureItems = new string[3] { "", "", "" };

        public List<_Loot> _lootsList = new List<_Loot>();
        public List<_Event._choice> _choicesList = new List<_Event._choice>();

        [Serializable]
        public class _Loot
        {
            public string _type = "";
            public int _value = 0;

            public _Loot(string type_, int value_)
            {
                _type = type_;
                _value = value_;
            }
        }

        public _Spot(float x_, float z_)
        {
            _px = x_;
            _pz = z_;
            _type = "Battle";
        }
    }

    protected override void Awake()
    {
        base.Awake();

        foreach (Transform child in transform.Find("OriginalTokens"))
        {
            OriginalTokens.Add(child.gameObject);
        }
    }

    public static void ConnectSpots()
    {
        srPathsArray = new SpriteRenderer[Globals.Instance.spotsArray.Length, Globals.Instance.spotsArray.Length];
        Transform trPaths = Instance.transform.Find("Paths");

        for (int i_ = 0; i_ < Globals.Instance.spotsArray.Length; i_++)
        {
            _Spot spot_i_ = Globals.Instance.spotsArray[i_];
            Vector3 pos_i_ = new Vector3(spot_i_._px, 0, spot_i_._pz);

            for (int j_ = i_ + 1; j_ < Globals.Instance.spotsArray.Length; j_++)
            {
                _Spot spot_j_ = Globals.Instance.spotsArray[j_];
                Vector3 pos_j_ = new Vector3(spot_j_._px, 0, spot_j_._pz);
                Vector2 vector_i_j_ = (pos_j_ - pos_i_).ToVector2_XZ();
                float distance = vector_i_j_.magnitude;

                if (distance < 15f)
                {
                    spot_i_._connectingSpotsIndex.Add(spot_j_._index);
                    spot_j_._connectingSpotsIndex.Add(spot_i_._index);

                    float p_ = ((distance - spot_i_._radius - spot_j_._radius) / 2 + spot_i_._radius) / distance;

                    GameObject Path_i_j_ = Instantiate(Prefabs.goInstances.Find(m => m.name == "Path"), trPaths);
                    Path_i_j_.transform.localPosition = new Vector3((pos_i_ + pos_j_).x / 2, 0, (pos_i_ + pos_j_).y / 2);
                    Path_i_j_.transform.localPosition = pos_i_ * (1 - p_) + pos_j_ * p_;
                    Path_i_j_.transform.localRotation = Quaternion.Euler(90, -vector_i_j_.ToAngleFrom(Vector2.up), 00);
                    Path_i_j_.GetComponent<SpriteRenderer>().size = new Vector2(1, (distance - spot_i_._radius - spot_j_._radius) / 4);
                    Path_i_j_.name = "Path(" + i_.ToString("D2") + ", " + j_.ToString("D2") + ")";

                    srPathsArray[i_, j_] = Path_i_j_.GetComponent<SpriteRenderer>();
                    srPathsArray[j_, i_] = Path_i_j_.GetComponent<SpriteRenderer>();
                }
            }
        }

        Globals.Instance.spotsArray[0]._distanceFromStart = 0;
        List<_Spot> spotList_ = new List<_Spot> { Globals.Instance.spotsArray[0] };

        while (spotList_.Count > 0)
        {
            _Spot spot_i_ = spotList_[0];
            for (int j = 0; j < spot_i_._connectingSpotsIndex.Count; j++)
            {
                _Spot spot_i_j_ = Globals.Instance.spotsArray[spot_i_._connectingSpotsIndex[j]];

                if (spot_i_j_._distanceFromStart == -1)
                {
                    spot_i_j_._distanceFromStart = spot_i_._distanceFromStart + 1;
                    spotList_.Add(spot_i_j_);
                }
            }

            spotList_.Remove(spot_i_);
        }
    }

    public static void DraftAndImplementMap()
    {
        _Random random_ = new _Random((uint)(Globals.Instance.seedOnAdventure * (Globals.Instance.stageCount + 1)));

       Initialization();
       DraftSpots(random_);
       ImplementSpots();
       ConnectSpots();
       DraftEvent(random_);
       ImplementEvent();
       DrawMap();
    }

    public static void DraftEvent(_Random random_)
    {
        Globals.Instance.spotsArray[0]._type = "None";

        Globals.Instance.spotsArray.Last()._type = "Boss";
        Globals.Instance.bossName = Table.EnemyTable[new Tuple<int, string>(Globals.Instance.stageCount, "Boss")].GetRandom(random_);
        //Globals.Instance.bossName = Table.BossTable00[random_.Range(0, Table.BossTable00.Length)];

        //_Spot[] spotListShuffle_ = Globals.Instance.spotsArray.Shuffle(random_);
        int makeShopCount_ = random_.Range(2, 4);
        int makeEventCount_ = random_.Range(3, 6);
        int makeTreasureCount_ = 2;
        //int makeEliteCount_ = 0;

        foreach (_Spot spot_i_ in Globals.Instance.spotsArray.Shuffle(random_))
        {
            if (spot_i_._type != "Battle") continue;

            bool isContinue_ = false;
            foreach (int index_j_ in spot_i_._connectingSpotsIndex)
            {
                if (Globals.Instance.spotsArray[index_j_]._type != "Battle")
                {
                    isContinue_ = true;
                    break;
                }
            }
            if (isContinue_ == true) continue;

            spot_i_._type = "Shop";
            if (--makeShopCount_ < 1)
                break;
        }
        foreach (_Spot spot_i_ in Globals.Instance.spotsArray.Shuffle(random_))
        {
            if (spot_i_._type != "Battle") continue;

            bool isContinue_ = false;
            foreach (int index_j_ in spot_i_._connectingSpotsIndex)
            {
                if (Globals.Instance.spotsArray[index_j_]._type == "None")
                {
                    isContinue_ = true;
                    break;
                }
            }
            if (isContinue_ == true) continue;

            spot_i_._type = "Event";
            if (--makeEventCount_ < 1)
                break;
        }
        foreach (_Spot spot_i_ in Globals.Instance.spotsArray.Shuffle(random_))
        {
            if (spot_i_._type != "Battle") continue;
            if (spot_i_._distanceFromStart < 3) continue;

            bool isContinue_ = false;
            foreach (int index_j_ in spot_i_._connectingSpotsIndex)
            {
                if (Globals.Instance.spotsArray[index_j_]._type == "Boss" || Globals.Instance.spotsArray[index_j_]._type == "Treasure")
                {
                    isContinue_ = true;
                    break;
                }
            }
            if (isContinue_ == true) continue;

            spot_i_._type = "Treasure";
            if (--makeTreasureCount_ < 1)
                break;
        }
        //foreach (_Spot spot_i_ in Globals.Instance.spotsArray.Shuffle(random_))
        //{
        //    if (spot_i_._type != "Battle") continue;
        //    if (spot_i_._distanceFromStart < 3) continue;

        //    bool isContinue_ = false;
        //    foreach (int index_j_ in spot_i_._connectingSpotsIndex)
        //    {
        //        if (Globals.Instance.spotsArray[index_j_]._type == "Boss")
        //        {
        //            isContinue_ = true;
        //            break;
        //        }
        //    }
        //    if (isContinue_ == true) continue;

        //    spot_i_._type = "Elite";
        //    if (--makeEliteCount_ < 1)
        //        break;
        //}
    }

    //public static void DraftSpots(_Random random_)
    //{
    //    Vector2 posMin_ = new Vector2(-33.5f, -16f);
    //    Vector2 posMax_ = posMin_ * -1;
    //    int spotsCount_ = 22 - random_.Range(-1, 1 + 1) - 2;
    //    float margin_ = radius * 2 + 3.15f;

    //    List<_Spot> SpotsList_ = new List<_Spot> { new _Spot(-32.5f, -15f), new _Spot(+32.5f, +15f) };

    //    for (int loopCount_ = 0, index_ = 0; loopCount_ < 1000 && index_ < spotsCount_; loopCount_++)
    //    {
    //        float x_ = float.Parse(random_.Range(posMin_.x, posMax_.x).ToString("F1"));
    //        float y_ = float.Parse(random_.Range(posMin_.y, posMax_.y).ToString("F1"));
    //        Vector2 posNew_ = new Vector2(x_, y_);

    //        foreach (_Spot spot_j_ in SpotsList_)
    //        {
    //            Vector2 pos_j_ = new Vector2(spot_j_._px, spot_j_._pz);
    //            if ((posNew_ - pos_j_).sqrMagnitude < margin_.Square())
    //                break;

    //            if (spot_j_ == SpotsList_.Last())
    //            {
    //                SpotsList_.Insert(SpotsList_.Count - 1, new _Spot(posNew_.x, posNew_.y));
    //                index_++;
    //                break;
    //            }
    //        }
    //    }
    //    Globals.Instance.spotsArray = SpotsList_.ToArray();

    //    Debug.Log(spotsCount_ + 2 + ", " + Globals.Instance.spotsArray.Length);
    //}

    public static void DraftSpots(_Random random_)
    {
        int index_ = random_.Range(0, SpotsDataList.Count - 1);
        Globals.Instance.spotsArray = SpotsDataList[index_].DeepCopy();
    }

    public static void DrawMap()
    {
        UI.ConfigureItemsUI();

        for (int i_ = 0; i_ < Globals.Instance.spotsArray.Length; i_++)
        {
            _Spot spot_i_ = Globals.Instance.spotsArray[i_];

            spSpotsEdge[i_].color = new Color32(000, 255, 000, 200);

            if (Globals.Instance.sceneState != "GoToNextSpot" && Globals.Instance.sceneState != "CheckTheMap") continue;

            if (Globals.Instance.spotsArray[i_] == Globals.Instance.spotCurrent)
                spSpotsEdge[i_].color = new Color32(000, 255, 000, 200);
            //if (Globals.Instance.spotCurrent._connectingSpotsIndex.Contains(spot_i_._index))
            //    spSpotsEdge[i_].color = new Color32(255, 255, 000, 200);
            if (Globals.Instance.spotCurrent._connectingSpotsIndex.Contains(spot_i_._index) && spot_i_ == Globals.spotOnMouseover)
                spSpotsEdge[i_].color = new Color32(255, 000, 000, 200);
            if (Globals.Instance.spotsArray[i_]._isActive == false)
                spSpotsEdge[i_].color = new Color32(255, 255, 255, 160);
        }
        
        for (int i_ = 0; i_ < srPathsArray.GetLength(0); i_++)
        {
            for (int j_ = i_ + 1; j_ < srPathsArray.GetLength(1); j_++)
            {
                if ((srPathsArray[i_, j_] is SpriteRenderer sr_) == false) continue;

                _Spot spot_i_ = Globals.Instance.spotsArray[i_];
                _Spot spot_j_ = Globals.Instance.spotsArray[j_];

                srPathsArray[i_, j_].color = new Color32(255, 255, 255, 255);

                if (Globals.Instance.sceneState != "GoToNextSpot")
                    continue;

                if (spot_i_ == Globals.Instance.spotCurrent && Globals.Instance.spotCurrent._connectingSpotsIndex.Contains(spot_j_._index))
                {
                    if (spot_j_ == Globals.spotOnMouseover)
                        srPathsArray[i_, j_].color = new Color32(255, 000, 000, 255);
                    //else if (spot_i_._event == "None")
                    //    srPathsArray[i_, j_].color = new Color32(255, 255, 255, 060);
                    else
                        srPathsArray[i_, j_].color = new Color32(255, 255, 000, 255);
                }
                else if (spot_j_ == Globals.Instance.spotCurrent && Globals.Instance.spotCurrent._connectingSpotsIndex.Contains(spot_i_._index))
                {
                    if (spot_i_ == Globals.spotOnMouseover)
                        srPathsArray[i_, j_].color = new Color32(255, 000, 000, 255);
                    //else if (spot_i_._event == "None")
                    //    srPathsArray[i_, j_].color = new Color32(255, 255, 255, 060);
                    else
                        srPathsArray[i_, j_].color = new Color32(255, 255, 000, 255);
                }
            }
        }
    }

    public static IEnumerator EncounterBoss(int index_)
    {
        _Spot spot_ = Globals.Instance.spotsArray[index_];
        GameObject goSpot_ = goSpotsList[index_];

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        if (goSpot_.transform.Find("Enemy") && goSpot_.transform.Find("Enemy").GetComponent<Animator>() is Animator animator_)
        {
            animator_.SetTrigger("triggerTaunt");
            yield return new WaitForSeconds(1.5f / Globals.Instance.gameSpeed);
        }

        for (int i = 0; i < Globals.heroList.Count; i++)
        {
            _Unit unit_i_ = Globals.heroList[i];

            unit_i_._posSOB = new Vector3(-9, 0, 0);
            unit_i_._posDistSOB = new Vector3(1, 0, 1);
            unit_i_._parameter._entranceType = "FromLeft";

            if (unit_i_._parameter._positioningType == "Front") unit_i_._posSOB = unit_i_._posSOB + new Vector3(+2, 0, 0);
            if (unit_i_._parameter._positioningType == "Middle") unit_i_._posSOB = unit_i_._posSOB + new Vector3(+0, 0, 0);
            if (unit_i_._parameter._positioningType == "Back") unit_i_._posSOB = unit_i_._posSOB + new Vector3(-2, 0, 0);
            if (i == 0) unit_i_._posSOB = unit_i_._posSOB + new Vector3(0, 0, +0);
            if (i == 1) unit_i_._posSOB = unit_i_._posSOB + new Vector3(0, 0, +3);
            if (i == 2) unit_i_._posSOB = unit_i_._posSOB + new Vector3(0, 0, -3);
        }

        yield return General.Instance.StartCoroutine(Battle.StartBattle(Globals.Instance.seedOnAdventure * index_, spot_._type));
    }

    public static IEnumerator EncounterBattle(int index_)
    {
        _Spot spot_ = Globals.Instance.spotsArray[index_];
        GameObject goSpot_ = goSpotsList[index_];

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        heroTokenImage.gameObject.SetActive(true);
        heroTokenImage.transform.position = Camera.main.WorldToScreenPoint(HeroToken.transform.position + Vector3.up * 3) + Vector3.up * 20 * Globals.canvasScale;
        heroTokenImage.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.2f);
        General.Instance.StartCoroutine(General.ScaleWithTime(heroTokenImage.gameObject, new Vector3(1, 0, 1), Vector3.one * 1.0f, 0.08f / Globals.Instance.gameSpeed));

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);
        General.Instance.StartCoroutine(General.ScaleWithTime(heroTokenImage.gameObject, Vector3.one * 1.0f, new Vector3(1, 0, 1), 0.08f / Globals.Instance.gameSpeed));
        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);
        heroTokenImage.gameObject.SetActive(false);

        for (int i = 0; i < Globals.heroList.Count; i++)
        {
            _Unit unit_i_ = Globals.heroList[i];

            unit_i_._posSOB = new Vector3(-9, 0, 0);
            unit_i_._posDistSOB = new Vector3(1, 0, 1);
            unit_i_._parameter._entranceType = "FromLeft";

            if (unit_i_._parameter._positioningType == "Front") unit_i_._posSOB = unit_i_._posSOB + new Vector3(+2, 0, 0);
            if (unit_i_._parameter._positioningType == "Middle") unit_i_._posSOB = unit_i_._posSOB + new Vector3(+0, 0, 0);
            if (unit_i_._parameter._positioningType == "Back") unit_i_._posSOB = unit_i_._posSOB + new Vector3(-2, 0, 0);
            if (i == 0) unit_i_._posSOB = unit_i_._posSOB + new Vector3(0, 0, +0.0f);
            if (i == 1) unit_i_._posSOB = unit_i_._posSOB + new Vector3(0, 0, +2.5f);
            if (i == 2) unit_i_._posSOB = unit_i_._posSOB + new Vector3(0, 0, -2.5f);
        }

        yield return General.Instance.StartCoroutine(Battle.StartBattle(Globals.Instance.seedOnAdventure * index_, spot_._type));
    }

    public static IEnumerator EncounterEvent(int index_)
    {
        _Spot spot_ = Globals.Instance.spotsArray[index_];
        _Random random_ = new _Random((uint)(Globals.Instance.seedOnAdventure + 1000 * Globals.Instance.stageCount + 10 * spot_._index + spot_._visitCount));
        GameObject goSpot_ = goSpotsList[index_];
        GameObject[] goChoices_ = UI.goEvent.Find("Choices").GetChildrenGameObject();
        TextMeshProUGUI tmpro_ = UI.goEvent.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>();
        Graphic[] graphics_ = UI.goEvent.GetComponentsInChildren<Graphic>();
        _Event event_ = _Event.SelectEvent(random_);
        //_Event event_ = Table.EventTable[Globals.Instance.stageCount].GetRandom(random_);

        event_?._functionMakeEvent(spot_, event_, random_);
        string eventText_ = TextData.EventText[event_._name];
        spot_._choicesList = event_._choices.DeepCopy();

        Globals.Instance.currentEvent = event_;
        Globals.Instance.currentEventText = eventText_;
        Globals.Instance.currentEventChoice = null;

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        heroTokenImage.gameObject.SetActive(true);
        heroTokenImage.transform.position = Camera.main.WorldToScreenPoint(HeroToken.transform.position + Vector3.up * 3) + Vector3.up * 20 * Globals.canvasScale;
        heroTokenImage.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.2f);
        General.Instance.StartCoroutine(General.ScaleWithTime(heroTokenImage.gameObject, new Vector3(1, 0, 1), Vector3.one * 1.0f, 0.08f / Globals.Instance.gameSpeed));

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);
        General.Instance.StartCoroutine(General.ScaleWithTime(heroTokenImage.gameObject, Vector3.one * 1.0f, new Vector3(1, 0, 1), 0.08f / Globals.Instance.gameSpeed));
        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);
        heroTokenImage.gameObject.SetActive(false);

        UI.goEvent.SetActive(true);
        tmpro_.text = "";

        for (int i = 0; i < goChoices_.Length; i++)
        {
            GameObject go_i_ = goChoices_[i];
            go_i_.GetComponent<Button>().interactable = false;
            go_i_.SetActive(false);
            if (i < spot_._choicesList.Count)
            {
                go_i_.Find("Text").GetComponent<TextMeshProUGUI>().text = spot_._choicesList[i]._text;
                go_i_.Find("Dim").gameObject.SetActive(spot_._choicesList[i]._functionIsSelectable(spot_._choicesList[i]) == false);

                if (spot_._choicesList[i]._tooltip._title.IsNullOrEmpty() == false)
                {
                    _Tooltip tooltip_i_ = go_i_.GetComponent<_Clickable>()._tooltip;
                    tooltip_i_._title = spot_._choicesList[i]._tooltip._title;
                    tooltip_i_._effectText = spot_._choicesList[i]._tooltip._effectText;
                }
                else
                {
                    go_i_.GetComponent<_Clickable>()._tooltip = new _Tooltip();
                }
            }
        }

        for (float timeSum_ = 0, timeMax_ = 0.4f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
        {
            float p_ = timeSum_ / timeMax_;
            UI.ConfigureGraphicsAlpha(graphics_, p_);

            yield return null;
        }
        UI.ConfigureGraphicsAlpha(graphics_, 1);
        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        yield return General.Typewriter(tmpro_, eventText_, 0.01f / Globals.Instance.gameSpeed);

        for (int i = 0; i < spot_._choicesList.Count; i++)
        {
            Instance.StartCoroutine(Event_Event_SlideAndShow(goChoices_[i]));
            goChoices_[i].GetComponent<Button>().interactable = spot_._choicesList[i]._functionIsSelectable(spot_._choicesList[i]);
            yield return new WaitForSeconds(0.3f);
        }

        IEnumerator Event_Event_SlideAndShow(GameObject go_i_)
        {
            Graphic[] graphics_i_ = go_i_.GetComponentsInChildren<Graphic>();
            RectTransform rectTransform_ = go_i_.GetComponent<RectTransform>();
            Vector3 posStart_ = rectTransform_.localPosition.MulVector(Vector3.up) + Vector3.right * 100;
            Vector3 posEnd_ = rectTransform_.localPosition.MulVector(Vector3.up);
            go_i_.SetActive(true);

            for (float timeSum_ = 0, timeMax_ = 0.4f; timeSum_ < timeMax_; timeSum_ += Time.deltaTime)
            {
                float p_ = timeSum_ / timeMax_;
                UI.ConfigureGraphicsAlpha(graphics_i_, p_);
                rectTransform_.localPosition = Library.BezierLiner(posStart_, posEnd_, p_);

                yield return null;
            }
            UI.ConfigureGraphicsAlpha(graphics_i_, 1);
            rectTransform_.localPosition = posEnd_;
        }
    }

    public static IEnumerator EncounterTreasure(int index_)
    {
        _Spot spot_ = Globals.Instance.spotsArray[index_];
        GameObject goSpot_ = goSpotsList[index_];
        Destroy(_Unit._GoUnits.Find("TreasureChest")?.gameObject);

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_._parameter._unitType == "Hero") continue;

            Destroy(unit_i_.gameObject);
        }

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        yield return General.TransitionScreen(Prefabs.Instance.transitionIn, Prefabs.Instance.transitionSprites[4], 0.6f);
        General.ChangeScene("Treasure");

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            unit_i_._modelTransform.localRotation = Quaternion.Euler(new Vector3(0, +90, 0));
        }

        EncountreTreasure_DropAndShowTreasure();

        Globals.heroList[0].transform.position = new Vector3(-2.5f, 0, +0.0f);
        Globals.heroList[1].transform.position = new Vector3(-3.0f, 0, +2.5f);
        Globals.heroList[2].transform.position = new Vector3(-3.0f, 0, -2.5f);

        GameObject TreasureChest = Instantiate(Prefabs.goOriginalUnits.Find("TreasureChest").gameObject, Vector3.zero, Quaternion.Euler(0, 180, 0), _Unit._GoUnits.transform);
        TreasureChest.name = "TreasureChest";

        yield return General.TransitionScreen(Prefabs.Instance.transitionOut, Prefabs.Instance.transitionSprites[4], 0.6f);

        void EncountreTreasure_DropAndShowTreasure()
        {
            _Random random_ = new _Random((uint)(Globals.Instance.seedOnAdventure + 1000 * Globals.Instance.stageCount + 10 * spot_._index + spot_._visitCount));
            List<string> Treasures_ = Table.TreasureTable[0].ToList();
            Treasures_.AddRange(Table.TreasureTable[Globals.Instance.stageCount]);
            Treasures_.RemoveAll(m => Globals.Instance.TreasuresAlreadyPicked_.Contains(m));
            Globals.Instance.spotCurrent._treasureItems = Treasures_.Shuffle(random_).Take(3).ToArray();

            UI.ConfigureTreasureChestUI();
        }
    }

    public static void HungerCheck()
    {
        Globals.Instance.globalEffectList.RemoveAll(HungerCheck_IsHunger);

        if (Globals.Instance.Food < 1)
            Globals.Instance.globalEffectList.Insert(0, _Skill.CloneFromString("Hunger"));

        UI.ConfigureGlobalEffectUI();

        bool HungerCheck_IsHunger(_Skill skill_)
        {
            return skill_?._parameter?._tags?.Contains("Hunger") == true;
        }
    }

    public static void ImplementMap()
    {
        Initialization();
        ImplementSpots();
        ConnectSpots();
        ImplementEvent();
        DrawMap();
    }

    public static void ImplementEvent()
    {
        foreach (_Spot spot_i in Globals.Instance.spotsArray)
        {
            GameObject goSpot_i = goSpotsList[spot_i._index];

            if (spot_i._type == "Boss")
            {
                //goSpotsList[spot_i._index].transform.localScale = 8f.ToVector3();
                GameObject Enemy_ = Instantiate(OriginalTokens.Find(m => m.name == Globals.Instance.bossName.Replace("Boss:", "")), goSpotsList[spot_i._index].transform, true);
                Enemy_.transform.position = goSpot_i.transform.position;
                Enemy_.name = "Enemy";
                Enemy_.transform.LookAt(Vector3.zero);

                UnitTokenList.Add(Enemy_);
            }
            else if (spot_i._type == "Shop")
            {
                goSpotsList[spot_i._index].Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MapIcon/Shop");
            }
            else if (spot_i._type == "Event")
            {
                goSpotsList[spot_i._index].Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MapIcon/Event");
            }
            else if (spot_i._type == "Battle")
            {
                goSpotsList[spot_i._index].Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MapIcon/Battle");
            }
            else if (spot_i._type == "Treasure")
            {
                goSpotsList[spot_i._index].Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MapIcon/Treasure");
            }
            else if (spot_i._type == "Elite")
            {
                goSpotsList[spot_i._index].Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MapIcon/Elite");
            }
        }
    }

    public static void ImplementSpots()
    {
        Transform trSpots_ = Instance.transform.Find("Spots");
        Globals.Instance.spotsArray.Last()._radius = 4;

        for (int i_ = 0; i_ < Globals.Instance.spotsArray.Length; i_++)
        {
            _Spot spot_i_ = Globals.Instance.spotsArray[i_];
            Globals.Instance.spotsArray[i_]._index = i_;

            GameObject goSpot_i_ = Instantiate(Prefabs.goInstances.Find(m => m.name == "Spot"), trSpots_);
            goSpot_i_.transform.localPosition = new Vector3(spot_i_._px, 0, spot_i_._pz);
            goSpot_i_.name = "Spot" + i_.ToString("D2");
            goSpot_i_.transform.localScale = (spot_i_._radius * 2).ToVector3();
            goSpotsList.Add(goSpot_i_);
            spSpotsEdge.Add(goSpot_i_.transform.Find("Edge").GetComponent<SpriteRenderer>());
        }
    }

    public static void Initialization()
    {
        goSpotsList.Clear();
        spSpotsEdge.Clear();

        foreach (Transform child_ in Instance.transform.Find("Spots"))
        {
            Destroy(child_.gameObject);
        }
        foreach (Transform child_ in Instance.transform.Find("Paths"))
        {
            Destroy(child_.gameObject);
        }

        foreach (GameObject go_ in HeroTokenList)
        {
            Destroy(go_);
        }

        HeroTokenList.Clear();
        UnitTokenList.Clear();

        foreach (_Hero hero_i_ in Globals.heroList)
        {
            GameObject Hero_ = Instantiate(OriginalTokens.Find(m => m.name == hero_i_._parameter._class), Instance.transform);
            Hero_.name = "Token " + hero_i_._parameter._class;
            Hero_.SetActive(false);
            HeroTokenList.Add(Hero_);
            UnitTokenList.Add(Hero_);
        }
        UnitTokenList[0].SetActive(true);
        HeroToken = UnitTokenList[0];
        heroTokenAnimator = HeroToken.GetComponent<Animator>();
        heroTokenImage = HeroToken.transform.Find("Canvas").Find("Image").GetComponent<Image>();

        HeroToken.transform.localPosition = new Vector3(-32.5f, 0f, -15f);
        HeroToken.transform.LookAt(Instance.transform);
    }

    public static IEnumerator MoveTokenToSpot(string name_)
    {
        if (Globals.Instance.sceneState != "GoToNextSpot") yield break;
        if (int.TryParse(name_.Slice(4, 6), out int index_) == false) yield break;
        if (index_.IsBetween(0, Globals.Instance.spotsArray.Length) == false) yield break;
        if (Globals.Instance.spotCurrent._connectingSpotsIndex.Contains(index_) == false) yield break;

        Globals.inputStopperCount++;

        index_ = int.Parse(name_.Slice(4, 6));
        _Spot spot_ = Globals.Instance.spotsArray[index_];
        GameObject goSpot = goSpotsList[index_];
        Globals.Instance.spotCurrent = spot_;
        Globals.Instance.Food -= 1;
        spot_._visitCount++;
        UI.goOpenShop.SetActive(false);
        DrawMap();
        foreach (_Unit unit_i_ in Globals.heroList)
            unit_i_._SkillTree_Save();

        Vector3 posEnd_ = goSpot.transform.position;
        if (spot_._type == "Boss")
        {
            posEnd_ = goSpot.transform.position + (HeroToken.transform.position - goSpot.transform.position).normalized * 3.5f;
        }
        //else if (spot_._type == "Shop")
        //{
        //    posEnd_ = goSpot.transform.position + Vector3.back * 2.5f;
        //}

        heroTokenAnimator.SetBool("isWalk", true);
        HeroToken.transform.LookAt(goSpot.transform.position);
        yield return General.MoveTowards(HeroToken, posEnd_, /*MoveSpeed=*/ 8);
        heroTokenAnimator.SetBool("isWalk", false);

        if (spot_._isActive == false)
        {
            HungerCheck();
        }
        else if (spot_._type == "Start")
        {
            HungerCheck();
        }
        else if (spot_._type == "Boss")
        {
            yield return General.Instance.StartCoroutine(EncounterBoss(index_));
        }
        else if (spot_._type == "Battle")
        {
            yield return General.Instance.StartCoroutine(EncounterBattle(index_));
            HeroToken.transform.position = goSpot.transform.position;
        }
        else if (spot_._type == "Event")
        {
            HungerCheck();
            yield return General.Instance.StartCoroutine(EncounterEvent(index_));
        }
        else if (spot_._type == "Shop")
        {
            HungerCheck();
            HeroToken.transform.LookAt(goSpot.transform.position);
            General.ChangeScene("GoToNextSpot");

            yield return new WaitForSeconds(0.6f);
            RestockShop(spot_._index);
            Globals.Instance.shopUndoSave._Save();
            UI.ConfigureShopUI();
            UI.goShop.SetActive(true);
        }
        else if (spot_._type == "Treasure")
        {
            yield return EncounterTreasure(index_);
            HungerCheck();
        }
        else if (spot_._type == "None")
        {
            HungerCheck();
        }
        else
        {
            HungerCheck();
            Debug.Log("Invalid event : " + spot_._type);
        }

        Globals.inputStopperCount--;
    }

    public static IEnumerator OpenTreasureChest(GameObject go_)
    {
        Animator animator_ = go_.GetComponent<Animator>();

        if (animator_.GetBool("IsOpen") == true) yield break;

        animator_.SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);

        UI.goTreasureChest.SetActive(true);

        Graphic[] graphics_ = UI.goTreasureChest.GetComponentsInChildren<Graphic>();
        for (float timeSum = 0, timeMax = 0.8f; timeSum < timeMax; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = timeSum / timeMax;
            UI.ConfigureGraphicsAlpha(graphics_, p_);

            yield return null;
        }
    }

    public static void RestockShop(int spotIndex_)
    {
        _Spot spot_ = Globals.Instance.spotsArray[spotIndex_];
        _Random random_ = new _Random((uint)(Globals.Instance.seedOnAdventure + 1000 * Globals.Instance.stageCount + 10 * spot_._index + spot_._visitCount));

        for (int i = 0; i < spot_._shopItems.Length; i++)
        {
            spot_._shopItems[i] = _Item.DropItem(random_);

            if (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Discount Ticket") != null)
                spot_._shopItems[i]._price = (spot_._shopItems[i]._price * 0.8f).ToInt();
        }
        for (int i = 0; i < spot_._shopEquips.Length; i++)
        {
            spot_._shopEquips[i] = _Equip.DropEquip(spot_, random_);

            if (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Discount Ticket") != null)
                spot_._shopEquips[i]._price = (spot_._shopEquips[i]._price * 0.8f).ToInt();

            spot_._shopEquips[i]._price = (spot_._shopEquips[i]._price * random_.Range(0.95f, 1.05f)).ToInt();
        }

        List<string> skillsAlreadyHave_ = new List<string>();
        foreach (_Unit unit_i_ in Globals.unitList)
        {
            foreach (_Skill skill_j_i_ in unit_i_._parameter._skills)
            {
                if (skill_j_i_ != null)
                    skillsAlreadyHave_.Add(skill_j_i_._parameter._name);
            }
        }
    }

    public static void SelectTreasure(int treasureIndex_)
    {
        _Skill skill_ = _Skill.CloneFromString(Globals.Instance.spotCurrent._treasureItems[treasureIndex_]);

        Globals.Instance.globalEffectList.Add(skill_);

        if (skill_._parameter._triggerTiming.Contains("OnGetTreasure"))
            General.Instance.StartCoroutine(skill_._functionCastSkill?.Invoke(null, null, skill_));

        if (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Goldgrubber") != null)
            Globals.Instance.Gold += 200;

        UI.ConfigureGlobalEffectUI();

        UI.goTreasureChest.SetActive(false);
        Globals.Instance.spotCurrent._isActive = false;

        General.Instance.StartCoroutine(SelectTreasure_Coroutine());

        IEnumerator SelectTreasure_Coroutine()
        {
            yield return new WaitForSeconds(0.2f);
            yield return General.ChangeSceneWithFade("GoToNextSpot", "TreasureToMap", 0.8f);
        }
    }

    public static void SwitchHeroToken(GameObject go_)
    {
        if (go_.name.Slice(0, 5) != "Token") return;

        int index_ = HeroTokenList.IndexOf(HeroToken);

        HeroToken.SetActive(false);
        Vector3 v3Temp_ = HeroToken.transform.position;
        Quaternion qrTemp = HeroToken.transform.rotation;

        HeroToken = HeroTokenList[(index_ + 1).Mod(HeroTokenList.Count)];
        HeroToken.SetActive(true);
        heroTokenAnimator = HeroToken.GetComponent<Animator>();
        heroTokenImage = HeroToken.transform.Find("Canvas").Find("Image").GetComponent<Image>();
        HeroToken.transform.position = v3Temp_;
        HeroToken.transform.rotation = qrTemp;
    }

    public static List<_Spot[]> SpotsDataList = new List<_Spot[]>()
    {
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+21.3f, +03.7f), new _Spot(-14.5f, -01.6f), new _Spot(-21.3f, +14.7f), new _Spot(-08.7f, -09.3f), new _Spot(-01.3f, +10.5f), new _Spot(-25.8f, -07.3f), new _Spot(+08.6f, -00.8f),
            new _Spot(+10.6f, +10.4f), new _Spot(+24.1f, -05.8f), new _Spot(-00.3f, -03.9f), new _Spot(-27.7f, +04.0f), new _Spot(+14.2f, -09.9f), new _Spot(-33.2f, +14.8f), new _Spot(-11.8f, +07.4f), new _Spot(+22.5f, +12.8f),
            new _Spot(+31.9f, +00.8f), new _Spot(+30.2f, -13.6f), new _Spot(-19.5f, -15.6f), new _Spot(+02.1f, -13.8f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-12.7f, -12.8f), new _Spot(+00.3f, -12.0f), new _Spot(+10.0f, -04.8f), new _Spot(-17.2f, +02.8f), new _Spot(-02.3f, +15.2f), new _Spot(-19.7f, +15.0f), new _Spot(+33.1f, -08.2f),
            new _Spot(+15.6f, -11.9f), new _Spot(+30.0f, +05.0f), new _Spot(-27.1f, -06.4f), new _Spot(+08.8f, +09.5f), new _Spot(+00.3f, +04.2f), new _Spot(+19.0f, +09.6f), new _Spot(+23.8f, -01.9f), new _Spot(-32.1f, +12.6f),
            new _Spot(+24.8f, -14.3f), new _Spot(-10.2f, +10.3f), new _Spot(-33.0f, +01.8f), new _Spot(-08.5f, -03.4f), new _Spot(-23.2f, -16.0f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-32.8f, +02.0f), new _Spot(+31.6f, -05.2f), new _Spot(-03.6f, -05.2f), new _Spot(-28.2f, +13.9f), new _Spot(+14.2f, -14.8f), new _Spot(+04.2f, -11.7f), new _Spot(+26.3f, -15.7f),
            new _Spot(-05.2f, +05.4f), new _Spot(-08.9f, -13.5f), new _Spot(+03.0f, +15.0f), new _Spot(+23.0f, +01.0f), new _Spot(-15.0f, -02.6f), new _Spot(+22.5f, +13.0f), new _Spot(-14.7f, +11.3f), new _Spot(+10.3f, -03.1f),
            new _Spot(+13.4f, +08.5f), new _Spot(-20.7f, -13.8f), new _Spot(-25.3f, -04.1f), new _Spot(+03.8f, +03.5f), new _Spot(+32.0f, +04.9f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+17.5f, -04.9f), new _Spot(-22.3f, -12.8f), new _Spot(-17.7f, +08.1f), new _Spot(+06.3f, -12.4f), new _Spot(+01.5f, +05.0f), new _Spot(+10.8f, +15.6f), new _Spot(+32.5f, +00.8f),
            new _Spot(-06.6f, -05.4f), new _Spot(+22.1f, +06.8f), new _Spot(-11.7f, -15.2f), new _Spot(+27.8f, -09.1f), new _Spot(-28.7f, +15.2f), new _Spot(-20.6f, -03.0f), new _Spot(-31.4f, +03.8f), new _Spot(+20.9f, -15.7f),
            new _Spot(-04.5f, +14.3f), new _Spot(-32.3f, -05.5f), new _Spot(+12.8f, +06.4f), new _Spot(+08.5f, -02.1f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+04.9f, -08.0f), new _Spot(-17.8f, -13.2f), new _Spot(+02.7f, +04.4f), new _Spot(+13.9f, -04.0f), new _Spot(-18.7f, +14.0f), new _Spot(+15.3f, +07.4f), new _Spot(-08.5f, +00.1f),
            new _Spot(-32.2f, +03.0f), new _Spot(+33.3f, -09.8f), new _Spot(+26.5f, +05.0f), new _Spot(-07.2f, +11.6f), new _Spot(-30.3f, +15.7f), new _Spot(-22.1f, +03.5f), new _Spot(+07.3f, +15.2f), new _Spot(-04.9f, -09.1f),
            new _Spot(+24.3f, -15.4f), new _Spot(-24.4f, -07.1f), new _Spot(+20.1f, +15.4f), new _Spot(+14.6f, -14.2f), new _Spot(+23.9f, -05.0f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+19.7f, -15.4f), new _Spot(-20.0f, -08.2f), new _Spot(-26.2f, +14.6f), new _Spot(-30.6f, -02.0f), new _Spot(+15.8f, +08.2f), new _Spot(-17.6f, +05.7f), new _Spot(-0735f, -14.6f),
            new _Spot(+04.9f, +01.9f), new _Spot(+22.2f, -00.6f), new _Spot(+32.2f, -05.0f), new _Spot(+04.0f, +14.4f), new _Spot(+02.1f, -09.6f), new _Spot(-10.4f, -03.4f), new _Spot(-07.0f, +05.2f), new _Spot(-12.8f, +15.7f),
            new _Spot(+33.0f, -14.8f), new _Spot(+31.8f, +04.2f), new _Spot(+23.0f, +14.5f), new _Spot(-33.4f, +08.0f), new _Spot(+12.8f, -03.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-06.8f, -13.3f), new _Spot(-00.9f, -00.1f), new _Spot(+09.4f, -01.3f), new _Spot(-17.0f, +16.0f), new _Spot(+11.4f, +12.0f), new _Spot(-22.7f, +03.0f), new _Spot(+28.1f, +01.1f),
            new _Spot(+21.1f, -06.2f), new _Spot(-21.5f, -14.1f), new _Spot(-33.4f, +06.0f), new _Spot(-09.8f, +07.1f), new _Spot(+07.4f, -11.0f), new _Spot(-00.2f, +12.2f), new _Spot(+32.3f, -07.7f), new _Spot(-15.5f, -03.8f),
            new _Spot(+20.4f, +07.9f), new _Spot(-29.7f, +15.5f), new _Spot(+20.9f, -15.6f), new _Spot(-32.6f, -05.6f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-21.7f, -05.5f), new _Spot(-29.6f, +11.3f), new _Spot(-01.2f, -02.8f), new _Spot(+21.6f, +12.7f), new _Spot(+28.8f, +01.0f), new _Spot(-02.5f, -15.5f), new _Spot(+31.7f, -12.7f),
            new _Spot(+08.9f, +14.7f), new _Spot(+05.6f, -10.3f), new _Spot(-02.9f, +14.2f), new _Spot(+03.4f, +06.6f), new _Spot(+14.5f, +03.3f), new _Spot(+23.8f, -09.2f), new _Spot(-16.2f, -15.6f), new _Spot(-08.0f, +04.6f),
            new _Spot(-17.9f, +07.8f), new _Spot(-12.2f, -04.6f), new _Spot(+14.8f, -07.0f), new _Spot(-31.9f, -03.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+32.0f, +02.8f), new _Spot(+01.2f, +08.9f), new _Spot(-30.5f, -02.4f), new _Spot(+19.9f, +10.4f), new _Spot(+10.0f, -01.3f), new _Spot(-04.4f, -03.0f), new _Spot(-10.0f, -15.4f),
            new _Spot(+19.5f, -15.2f), new _Spot(-13.7f, -00.9f), new _Spot(-25.5f, +06.2f), new _Spot(-15.6f, +12.6f), new _Spot(+19.3f, -01.4f), new _Spot(+32.3f, -09.8f), new _Spot(-23.3f, -11.8f), new _Spot(+10.6f, -11.9f),
            new _Spot(-28.7f, +15.7f), new _Spot(+01.3f, -12.0f), new _Spot(+11.1f, +14.5f), new _Spot(-06.9f, +15.6f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-05.6f, +04.1f), new _Spot(-31.5f, -04.2f), new _Spot(+05.7f, -15.9f), new _Spot(+15.4f, -10.4f), new _Spot(+24.6f, -03.8f), new _Spot(+27.8f, -13.3f), new _Spot(+21.0f, +07.1f),
            new _Spot(-24.0f, +16.0f), new _Spot(-25.8f, +03.9f), new _Spot(-12.5f, -02.5f), new _Spot(+11.0f, +07.5f), new _Spot(-17.8f, -14.5f), new _Spot(-03.8f, -09.2f), new _Spot(+02.1f, +11.8f), new _Spot(-09.8f, +15.0f),
            new _Spot(-33.0f, +14.0f), new _Spot(-21.4f, -05.6f), new _Spot(+16.6f, +15.4f), new _Spot(+32.7f, +05.1f), new _Spot(+09.2f, -02.1f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-09.4f, +08.0f), new _Spot(+11.7f, -01.0f), new _Spot(+00.8f, +03.3f), new _Spot(-23.5f, -11.0f), new _Spot(-26.4f, +08.4f), new _Spot(+30.5f, -07.3f), new _Spot(+12.8f, -14.3f),
            new _Spot(-04.1f, -07.4f), new _Spot(+23.2f, +00.6f), new _Spot(+05.2f, +15.8f), new _Spot(-18.8f, +01.5f), new _Spot(-10.6f, -15.1f), new _Spot(+18.1f, +11.1f), new _Spot(+24.4f, -15.4f), new _Spot(-31.0f, -02.1f),
            new _Spot(-19.0f, +15.6f), new _Spot(+32.9f, +02.2f), new _Spot(-32.6f, +15.5f), new _Spot(+05.1f, -07.4f), new _Spot(-13.7f, -06.4f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-23.7f, +00.3f), new _Spot(-16.4f, +07.4f), new _Spot(-07.6f, +08.7f), new _Spot(+30.5f, -12.0f), new _Spot(+32.3f, +01.1f), new _Spot(-10.8f, -06.2f), new _Spot(+21.6f, -01.3f),
            new _Spot(+11.3f, +07.0f), new _Spot(+03.8f, -00.9f), new _Spot(-23.5f, -10.3f), new _Spot(+16.3f, -14.4f), new _Spot(+01.9f, -14.1f), new _Spot(-32.4f, +10.9f), new _Spot(+05.5f, +14.4f), new _Spot(+23.3f, +11.7f),
            new _Spot(-13.1f, -15.3f), new _Spot(-22.1f, +15.9f), new _Spot(-32.1f, -04.9f), new _Spot(+12.6f, -04.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-05.8f, -13.0f), new _Spot(-23.6f, -00.5f), new _Spot(+15.9f, -08.2f), new _Spot(-02.5f, -03.4f), new _Spot(+30.8f, -10.7f), new _Spot(+12.6f, +10.2f), new _Spot(+33.5f, +01.0f),
            new _Spot(+08.3f, -00.9f), new _Spot(-13.3f, -00.3f), new _Spot(-14.9f, +12.7f), new _Spot(-29.5f, +15.7f), new _Spot(-02.0f, +11.0f), new _Spot(-31.0f, +06.6f), new _Spot(-21.3f, -12.1f), new _Spot(+04.9f, -15.1f),
            new _Spot(+20.9f, +00.3f), new _Spot(+25.8f, +08.4f), new _Spot(+21.1f, -15.9f), new _Spot(-33.3f, -02.4f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-04.2f, -12.3f), new _Spot(+26.1f, -08.1f), new _Spot(+31.5f, +01.4f), new _Spot(-25.9f, +05.5f), new _Spot(-15.1f, -04.9f), new _Spot(+13.2f, -05.2f), new _Spot(+02.4f, -00.5f),
            new _Spot(+18.2f, +14.1f), new _Spot(-05.7f, +13.5f), new _Spot(-32.0f, -01.7f), new _Spot(-31.0f, +15.9f), new _Spot(+16.0f, -15.8f), new _Spot(-09.2f, +04.8f), new _Spot(+04.2f, +12.9f), new _Spot(+20.3f, +04.3f),
            new _Spot(-13.4f, -15.4f), new _Spot(-22.8f, -15.6f), new _Spot(-18.0f, +12.5f), new _Spot(+05.7f, -11.2f), new _Spot(+30.4f, -16.2f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+26.7f, -10.4f), new _Spot(+20.1f, +10.1f), new _Spot(+03.8f, -13.1f), new _Spot(-17.3f, +10.5f), new _Spot(-30.3f, +10.2f), new _Spot(-08.8f, +05.0f), new _Spot(-13.8f, -06.8f),
            new _Spot(+33.3f, -04.1f), new _Spot(+16.0f, -01.0f), new _Spot(+00.3f, +08.7f), new _Spot(-08.4f, -15.9f), new _Spot(-20.9f, -15.1f), new _Spot(-21.3f, -01.2f), new _Spot(-31.4f, -04.5f), new _Spot(+11.6f, +14.0f),
            new _Spot(+01.4f, -01.7f), new _Spot(+28.9f, +04.7f), new _Spot(+15.9f, -15.5f), new _Spot(-06.5f, +15.4f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-08.4f, +01.7f), new _Spot(-29.6f, -05.6f), new _Spot(+16.8f, -12.4f), new _Spot(+31.3f, -14.2f), new _Spot(+18.5f, +12.0f), new _Spot(-11.1f, +12.3f), new _Spot(-26.4f, +12.5f),
            new _Spot(+02.3f, -02.7f), new _Spot(+27.6f, -02.8f), new _Spot(+03.2f, -14.6f), new _Spot(-18.2f, +00.2f), new _Spot(-19.2f, -13.0f), new _Spot(+04.1f, +06.4f), new _Spot(-07.3f, -08.9f), new _Spot(+13.3f, +01.4f),
            new _Spot(-02.2f, +14.8f), new _Spot(+27.2f, +07.3f), new _Spot(-33.1f, +06.9f), new _Spot(+07.7f, +15.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+15.4f, +08.1f), new _Spot(-20.0f, -10.3f), new _Spot(+27.4f, -12.5f), new _Spot(-02.3f, +03.8f), new _Spot(+14.2f, -13.5f), new _Spot(-23.0f, +10.2f), new _Spot(-03.2f, -06.8f),
            new _Spot(+02.5f, +14.1f), new _Spot(-29.8f, -01.7f), new _Spot(+18.0f, -03.9f), new _Spot(-09.9f, -15.0f), new _Spot(+07.1f, +02.2f), new _Spot(-15.7f, +00.5f), new _Spot(+33.2f, -01.2f), new _Spot(-09.6f, +12.5f),
            new _Spot(+26.5f, +07.6f), new _Spot(-33.2f, +10.3f), new _Spot(+20.7f, +15.8f), new _Spot(+01.6f, -15.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+33.0f, +02.5f), new _Spot(+18.9f, +10.7f), new _Spot(+05.7f, +06.6f), new _Spot(-22.2f, -13.0f), new _Spot(-23.2f, +02.4f), new _Spot(-05.7f, -06.3f), new _Spot(-10.9f, +10.5f),
            new _Spot(-14.6f, -04.1f), new _Spot(+27.8f, -05.3f), new _Spot(+11.0f, -13.3f), new _Spot(+30.3f, -15.7f), new _Spot(-21.2f, +14.1f), new _Spot(-02.0f, +14.3f), new _Spot(+17.8f, -02.4f), new _Spot(-30.8f, +08.1f),
            new _Spot(-11.3f, -15.4f), new _Spot(+03.7f, -07.4f), new _Spot(-32.2f, -01.2f), new _Spot(-02.8f, +02.2f), new _Spot(+20.6f, -13.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-14.6f, -06.5f), new _Spot(-24.3f, +02.8f), new _Spot(-33.2f, -04.6f), new _Spot(-24.0f, -07.4f), new _Spot(-07.1f, -13.5f), new _Spot(-13.4f, +14.5f), new _Spot(+22.9f, -06.0f),
            new _Spot(+07.5f, +02.1f), new _Spot(+20.0f, +07.8f), new _Spot(-00.6f, +06.8f), new _Spot(+28.5f, +04.1f), new _Spot(+07.8f, +11.8f), new _Spot(+11.4f, -11.5f), new _Spot(+25.0f, -15.1f), new _Spot(+32.9f, -04.3f),
            new _Spot(-32.7f, +12.9f), new _Spot(-22.5f, +12.1f), new _Spot(-14.1f, +04.9f), new _Spot(-01.2f, -04.9f), new _Spot(+02.3f, -13.5f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+17.8f, -01.2f), new _Spot(-21.2f, +06.3f), new _Spot(-02.6f, -07.1f), new _Spot(+30.0f, -15.9f), new _Spot(+05.7f, +03.8f), new _Spot(-03.4f, +13.5f), new _Spot(+09.6f, +15.4f),
            new _Spot(+20.7f, +10.3f), new _Spot(-15.1f, +15.5f), new _Spot(-31.5f, +11.7f), new _Spot(-19.4f, -08.5f), new _Spot(+26.2f, -05.2f), new _Spot(+18.5f, -15.2f), new _Spot(-08.5f, +04.3f), new _Spot(+02.7f, -14.8f),
            new _Spot(-33.0f, -00.4f), new _Spot(+32.9f, +05.4f), new _Spot(-08.3f, -14.3f), new _Spot(+08.7f, -05.4f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+21.1f, -03.9f), new _Spot(+18.3f, +15.4f), new _Spot(-25.4f, -06.3f), new _Spot(-20.5f, -14.5f), new _Spot(+00.1f, -05.1f), new _Spot(-01.0f, +06.2f), new _Spot(-31.9f, +06.5f),
            new _Spot(-12.5f, -04.3f), new _Spot(+06.2f, +14.0f), new _Spot(-10.0f, +10.4f), new _Spot(-06.9f, -12.7f), new _Spot(+11.3f, -15.0f), new _Spot(+08.2f, +02.2f), new _Spot(-19.7f, +04.6f), new _Spot(+24.1f, +08.0f),
            new _Spot(+25.1f, -12.5f), new _Spot(-23.9f, +15.9f), new _Spot(+33.3f, -07.8f), new _Spot(+32.6f, +02.4f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+16.8f, +03.7f), new _Spot(+17.6f, +14.4f), new _Spot(-23.9f, +07.1f), new _Spot(-08.1f, +07.0f), new _Spot(-09.1f, -10.6f), new _Spot(-24.4f, -09.0f), new _Spot(+03.4f, -04.2f),
            new _Spot(+33.1f, -07.1f), new _Spot(-33.0f, -00.3f), new _Spot(+29.1f, +04.8f), new _Spot(+06.0f, +05.0f), new _Spot(+09.7f, -11.8f), new _Spot(+23.3f, -15.4f), new _Spot(+03.2f, +15.7f), new _Spot(-00.8f, -14.8f),
            new _Spot(-15.0f, +13.5f), new _Spot(-13.9f, -00.9f), new _Spot(-28.5f, +15.6f), new _Spot(+20.8f, -05.4f), new _Spot(-17.1f, -15.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+19.0f, -09.8f), new _Spot(+13.4f, +04.2f), new _Spot(+00.8f, +08.4f), new _Spot(-21.4f, -12.4f), new _Spot(-06.2f, -03.9f), new _Spot(-27.2f, +13.8f), new _Spot(+01.5f, -13.8f),
            new _Spot(-22.9f, -02.5f), new _Spot(+21.4f, +12.6f), new _Spot(+31.8f, -10.7f), new _Spot(+23.5f, +01.7f), new _Spot(-12.3f, +12.1f), new _Spot(-31.4f, -00.5f), new _Spot(+08.6f, -04.4f), new _Spot(-12.8f, +02.7f),
            new _Spot(+32.7f, +01.0f), new _Spot(-08.5f, -14.7f), new _Spot(+11.2f, -15.9f), new _Spot(+12.1f, +13.9f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+18.0f, +14.0f), new _Spot(+17.6f, -02.5f), new _Spot(+31.2f, -05.7f), new _Spot(-08.2f, -11.5f), new _Spot(+26.9f, -15.3f), new _Spot(-27.6f, +02.5f), new _Spot(-20.2f, +13.1f),
            new _Spot(+09.0f, +03.2f), new _Spot(-22.8f, -06.5f), new _Spot(+01.1f, +12.0f), new _Spot(-16.9f, +02.9f), new _Spot(+14.7f, -11.5f), new _Spot(-31.0f, +15.7f), new _Spot(+00.4f, -06.6f), new _Spot(-11.0f, +15.4f),
            new _Spot(+04.8f, -15.6f), new _Spot(+28.2f, +05.6f), new _Spot(-02.9f, +02.9f), new _Spot(-18.9f, -15.7f), new _Spot(-33.0f, -05.8f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-04.4f, +15.1f), new _Spot(+17.4f, +03.1f), new _Spot(-29.5f, +03.7f), new _Spot(-20.2f, -04.4f), new _Spot(-03.8f, -10.2f), new _Spot(-08.4f, +02.8f), new _Spot(-31.7f, +14.7f),
            new _Spot(+18.2f, +12.5f), new _Spot(+05.2f, -01.7f), new _Spot(+29.5f, -08.1f), new _Spot(-16.9f, +12.4f), new _Spot(-13.2f, -13.0f), new _Spot(+28.4f, +01.5f), new _Spot(+06.6f, +11.1f), new _Spot(+23.3f, -15.9f),
            new _Spot(+15.3f, -07.5f), new _Spot(+08.8f, -14.7f), new _Spot(-23.1f, -13.1f), new _Spot(-32.1f, -05.8f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+18.3f, +14.0f), new _Spot(-22.0f, +04.7f), new _Spot(+23.2f, -04.6f), new _Spot(-02.0f, +11.7f), new _Spot(-10.3f, -09.1f), new _Spot(+13.9f, +03.8f), new _Spot(-11.7f, +15.8f),
            new _Spot(+11.9f, -11.9f), new _Spot(-23.3f, -08.3f), new _Spot(+01.1f, +00.4f), new _Spot(-10.8f, +01.0f), new _Spot(+07.4f, +12.4f), new _Spot(+02.1f, -13.9f), new _Spot(-25.9f, +13.5f), new _Spot(-30.8f, -00.1f),
            new _Spot(+30.7f, -12.1f), new _Spot(+32.9f, +01.1f), new _Spot(+23.5f, +06.0f), new _Spot(+20.6f, -14.8f), new _Spot(-17.9f, -15.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+25.3f, +02.1f), new _Spot(-32.2f, +09.2f), new _Spot(+18.6f, +10.8f), new _Spot(-02.4f, +11.8f), new _Spot(+10.7f, -09.6f), new _Spot(-22.7f, -10.8f), new _Spot(-07.4f, +02.8f),
            new _Spot(+01.8f, -04.3f), new _Spot(+08.5f, +15.8f), new _Spot(-21.0f, +00.6f), new _Spot(-23.6f, +14.9f), new _Spot(+08.0f, +04.3f), new _Spot(-13.3f, -10.9f), new _Spot(+33.3f, -02.5f), new _Spot(+22.7f, -08.8f),
            new _Spot(-32.0f, -01.8f), new _Spot(+33.2f, -14.3f), new _Spot(-14.5f, +12.4f), new _Spot(-02.1f, -13.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-01.0f, -04.8f), new _Spot(+11.6f, -12.7f), new _Spot(-27.0f, +13.1f), new _Spot(+21.8f, +12.6f), new _Spot(-21.8f, -12.7f), new _Spot(+23.1f, -12.9f), new _Spot(+03.5f, +06.3f),
            new _Spot(-07.4f, +09.9f), new _Spot(-16.5f, +00.9f), new _Spot(+20.4f, -01.1f), new _Spot(-09.5f, -10.6f), new _Spot(+33.1f, -15.7f), new _Spot(+29.9f, -04.1f), new _Spot(+09.8f, +13.9f), new _Spot(-14.9f, +15.2f),
            new _Spot(+27.7f, +05.1f), new _Spot(-30.8f, -00.9f), new _Spot(+02.8f, -15.8f), new _Spot(+10.6f, -00.4f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+22.8f, -03.9f), new _Spot(-08.9f, -03.2f), new _Spot(+15.4f, -15.0f), new _Spot(-01.2f, -15.9f), new _Spot(-23.3f, -09.8f), new _Spot(+06.6f, +12.2f), new _Spot(+16.1f, +12.8f),
            new _Spot(-29.6f, +13.6f), new _Spot(-32.9f, -02.4f), new _Spot(+01.6f, -02.5f), new _Spot(-03.9f, +06.1f), new _Spot(-15.8f, +11.2f), new _Spot(+29.9f, -12.9f), new _Spot(+25.3f, +07.9f), new _Spot(-22.8f, +00.5f),
            new _Spot(+09.7f, +02.9f), new _Spot(-11.1f, -14.8f), new _Spot(+09.1f, -07.9f), new _Spot(-03.7f, +15.9f), new _Spot(+32.3f, +00.5f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-15.8f, +10.0f), new _Spot(+11.9f, -13.0f), new _Spot(-11.8f, -04.1f), new _Spot(-33.2f, -00.4f), new _Spot(-21.0f, -13.3f), new _Spot(-06.3f, -12.0f), new _Spot(+26.3f, +05.8f),
            new _Spot(-04.8f, +10.5f), new _Spot(+26.6f, -07.4f), new _Spot(+17.7f, +13.5f), new _Spot(+14.0f, -00.7f), new _Spot(-27.8f, +09.2f), new _Spot(-21.0f, -03.7f), new _Spot(+02.0f, -05.1f), new _Spot(+21.6f, -15.6f),
            new _Spot(+32.6f, -14.6f), new _Spot(+03.6f, +04.0f), new _Spot(+33.4f, -01.0f), new _Spot(+02.6f, -15.2f), new _Spot(+04.1f, +14.8f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+04.9f, +14.6f), new _Spot(+21.4f, +14.4f), new _Spot(+31.0f, -06.5f), new _Spot(-25.8f, +16.3f), new _Spot(-23.4f, +04.6f), new _Spot(-21.9f, -04.9f), new _Spot(+30.9f, +05.6f),
            new _Spot(+11.7f, -12.2f), new _Spot(-06.8f, -00.3f), new _Spot(+02.6f, -02.5f), new _Spot(-12.6f, +13.1f), new _Spot(+22.2f, -13.1f), new _Spot(+22.6f, +01.4f), new _Spot(-33.0f, -03.7f), new _Spot(+00.0f, -14.2f),
            new _Spot(+11.5f, +04.9f), new _Spot(-13.3f, -08.8f), new _Spot(-33.1f, +08.2f), new _Spot(-19.8f, -15.7f), new _Spot(-02.1f, +08.5f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+22.6f, +04.4f), new _Spot(-22.4f, +09.1f), new _Spot(+12.2f, -06.1f), new _Spot(-03.4f, -04.4f), new _Spot(-13.2f, -09.1f), new _Spot(-03.4f, +14.8f), new _Spot(+01.9f, -15.4f),
            new _Spot(-13.9f, +02.9f), new _Spot(+30.7f, -08.2f), new _Spot(-26.4f, -04.3f), new _Spot(+08.3f, +03.3f), new _Spot(+17.6f, +14.7f), new _Spot(-31.8f, +05.0f), new _Spot(+22.4f, -15.3f), new _Spot(-32.3f, +15.6f),
            new _Spot(-12.9f, +14.1f), new _Spot(+33.5f, +03.4f), new _Spot(-18.6f, -15.6f), new _Spot(+06.4f, +14.3f), new _Spot(-03.1f, +05.5f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-22.2f, +03.8f), new _Spot(+14.6f, -06.5f), new _Spot(+24.6f, -15.1f), new _Spot(+29.8f, -07.0f), new _Spot(+04.3f, -13.6f), new _Spot(+30.4f, +05.6f), new _Spot(+05.1f, +02.0f),
            new _Spot(-03.9f, +12.6f), new _Spot(+19.2f, +03.4f), new _Spot(-17.2f, +12.1f), new _Spot(-20.3f, -14.4f), new _Spot(-03.9f, -08.4f), new _Spot(+11.2f, +12.2f), new _Spot(-11.0f, +02.5f), new _Spot(-32.8f, +01.6f),
            new _Spot(-26.9f, +15.6f), new _Spot(+22.6f, +13.8f), new _Spot(-12.2f, -10.2f), new _Spot(+13.3f, -16.0f), new _Spot(-27.9f, -06.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+04.0f, +10.3f), new _Spot(-32.3f, -03.1f), new _Spot(+22.8f, -00.1f), new _Spot(-05.0f, +03.9f), new _Spot(+00.8f, -09.8f), new _Spot(-11.8f, -04.3f), new _Spot(-08.8f, +15.5f),
            new _Spot(+11.7f, -15.1f), new _Spot(-22.6f, +06.2f), new _Spot(+30.2f, -08.3f), new _Spot(+11.6f, -05.8f), new _Spot(-19.1f, +15.2f), new _Spot(-21.0f, -03.8f), new _Spot(+12.2f, +04.5f), new _Spot(+31.6f, +04.5f),
            new _Spot(+23.7f, -15.2f), new _Spot(-12.6f, -15.3f), new _Spot(+22.1f, +10.5f), new _Spot(-23.0f, -14.8f), new _Spot(-33.1f, +11.2f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-08.7f, -14.5f), new _Spot(-28.1f, +05.4f), new _Spot(+32.3f, +00.1f), new _Spot(+04.5f, +02.0f), new _Spot(-16.2f, +09.0f), new _Spot(-19.1f, -02.2f), new _Spot(+18.5f, +10.0f),
            new _Spot(+27.1f, -09.1f), new _Spot(+07.8f, +12.8f), new _Spot(-22.0f, -11.9f), new _Spot(-29.4f, +16.0f), new _Spot(+06.3f, -11.2f), new _Spot(-07.8f, +04.1f), new _Spot(+13.1f, -04.0f), new _Spot(-28.3f, -04.2f),
            new _Spot(+18.9f, -16.3f), new _Spot(-05.5f, +14.5f), new _Spot(-03.4f, -06.9f), new _Spot(+23.0f, +01.8f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+29.2f, +05.8f), new _Spot(-22.9f, -13.0f), new _Spot(-30.6f, +09.9f), new _Spot(+07.8f, +14.1f), new _Spot(+25.2f, -06.6f), new _Spot(+00.6f, +07.0f), new _Spot(-10.8f, +13.5f),
            new _Spot(-09.1f, -00.8f), new _Spot(-18.2f, +00.6f), new _Spot(-19.9f, +15.6f), new _Spot(+07.6f, -04.3f), new _Spot(+18.5f, +10.7f), new _Spot(-03.4f, -14.3f), new _Spot(-14.2f, -09.4f), new _Spot(+12.0f, -15.7f),
            new _Spot(-28.8f, -03.2f), new _Spot(+16.5f, -01.5f), new _Spot(+32.8f, -13.6f), new _Spot(+21.5f, -15.1f), new _Spot(-01.8f, +15.9f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-30.3f, -04.1f), new _Spot(-13.6f, -10.9f), new _Spot(+07.1f, -04.2f), new _Spot(+21.4f, -09.2f), new _Spot(+21.1f, +03.5f), new _Spot(-00.8f, +13.0f), new _Spot(-23.8f, +15.0f),
            new _Spot(+29.9f, -13.0f), new _Spot(-13.0f, +12.9f), new _Spot(-01.7f, -13.9f), new _Spot(-08.5f, -01.3f), new _Spot(+11.7f, +07.3f), new _Spot(-18.4f, +04.5f), new _Spot(-33.4f, +07.9f), new _Spot(+19.5f, +14.2f),
            new _Spot(+30.3f, +00.7f), new _Spot(+01.6f, +03.3f), new _Spot(+08.8f, -15.2f), new _Spot(-23.0f, -11.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-20.0f, -07.7f), new _Spot(+19.7f, +14.4f), new _Spot(+19.9f, -07.2f), new _Spot(-20.3f, +06.8f), new _Spot(+06.6f, -13.3f), new _Spot(+30.6f, +03.0f), new _Spot(-05.3f, +04.7f),
            new _Spot(+05.4f, +06.7f), new _Spot(-31.6f, +05.6f), new _Spot(-03.5f, -04.9f), new _Spot(+16.5f, +04.4f), new _Spot(-11.6f, +14.7f), new _Spot(+26.3f, -15.8f), new _Spot(-33.4f, -05.1f), new _Spot(-11.9f, -13.0f),
            new _Spot(-00.6f, +14.7f), new _Spot(-13.5f, -01.0f), new _Spot(+33.2f, -06.0f), new _Spot(+09.7f, -04.0f), new _Spot(-26.9f, +13.6f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+18.5f, +10.1f), new _Spot(-33.3f, +14.2f), new _Spot(-27.7f, -03.0f), new _Spot(-20.6f, -10.1f), new _Spot(+05.8f, -02.5f), new _Spot(+26.1f, -07.7f), new _Spot(-22.5f, +13.7f),
            new _Spot(-09.2f, +14.9f), new _Spot(+26.2f, +02.5f), new _Spot(+06.9f, -14.5f), new _Spot(-08.9f, +00.6f), new _Spot(+07.8f, +10.8f), new _Spot(-07.3f, -10.3f), new _Spot(-19.1f, +03.9f), new _Spot(+15.3f, +00.1f),
            new _Spot(+00.3f, +05.1f), new _Spot(+16.8f, -09.2f), new _Spot(+30.5f, -15.1f), new _Spot(-33.4f, +05.0f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+18.5f, +05.3f), new _Spot(+03.5f, -03.9f), new _Spot(-00.1f, +14.5f), new _Spot(-20.9f, +09.6f), new _Spot(+31.0f, -05.8f), new _Spot(-33.2f, +16.0f), new _Spot(+20.4f, -12.6f),
            new _Spot(-11.7f, -01.7f), new _Spot(-25.3f, -07.1f), new _Spot(+32.5f, +03.9f), new _Spot(-00.6f, +05.1f), new _Spot(-30.0f, +04.0f), new _Spot(-10.8f, +14.8f), new _Spot(+10.2f, +11.1f), new _Spot(-19.5f, -14.4f),
            new _Spot(-03.8f, -11.3f), new _Spot(+15.9f, -04.6f), new _Spot(+06.8f, -13.6f), new _Spot(+31.0f, -15.4f), new _Spot(+23.3f, +13.8f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-17.8f, -04.1f), new _Spot(+19.9f, -06.6f), new _Spot(-09.5f, -13.1f), new _Spot(+13.2f, +15.2f), new _Spot(+28.7f, +01.1f), new _Spot(-08.0f, +01.0f), new _Spot(+00.4f, +08.5f),
            new _Spot(-24.1f, +15.3f), new _Spot(-28.6f, -04.7f), new _Spot(+21.8f, +07.1f), new _Spot(-23.9f, +05.0f), new _Spot(+02.6f, -02.2f), new _Spot(+13.2f, -14.8f), new _Spot(+10.5f, +03.7f), new _Spot(-12.3f, +13.5f),
            new _Spot(+28.0f, -12.8f), new _Spot(-33.1f, +08.2f), new _Spot(+02.9f, -12.7f), new _Spot(-21.2f, -15.8f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-01.2f, +14.5f), new _Spot(-20.0f, -01.2f), new _Spot(-02.8f, -01.8f), new _Spot(+10.0f, +00.3f), new _Spot(-10.6f, +12.7f), new _Spot(-08.6f, -11.7f), new _Spot(+21.4f, +06.1f),
            new _Spot(+02.2f, -14.8f), new _Spot(+30.5f, -10.1f), new _Spot(+12.6f, -12.8f), new _Spot(-19.7f, +08.5f), new _Spot(+29.2f, +00.5f), new _Spot(-28.2f, +04.7f), new _Spot(+10.9f, +11.9f), new _Spot(-29.5f, +14.8f),
            new _Spot(-31.7f, -05.5f), new _Spot(-10.9f, +02.5f), new _Spot(-20.8f, -15.6f), new _Spot(+20.9f, +15.9f), new _Spot(+20.2f, -06.6f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-15.5f, -06.4f), new _Spot(+29.3f, -11.6f), new _Spot(-11.4f, +03.1f), new _Spot(+22.3f, +04.1f), new _Spot(-01.2f, +12.1f), new _Spot(+17.3f, -13.7f), new _Spot(-03.4f, -05.6f),
            new _Spot(+32.4f, +00.3f), new _Spot(-32.2f, +12.0f), new _Spot(-28.4f, -03.5f), new _Spot(-23.3f, +15.5f), new _Spot(+17.7f, +15.4f), new _Spot(-14.5f, +12.2f), new _Spot(+02.2f, -15.1f), new _Spot(+07.2f, +06.3f),
            new _Spot(+18.1f, -04.1f), new _Spot(-23.1f, -11.8f), new _Spot(-21.9f, +03.8f), new _Spot(-07.1f, -14.1f), new _Spot(+09.0f, -07.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+04.5f, +09.9f), new _Spot(-13.6f, +14.4f), new _Spot(-00.2f, +01.8f), new _Spot(+12.4f, +00.9f), new _Spot(-31.6f, +11.7f), new _Spot(+16.6f, -11.8f), new _Spot(+17.6f, +13.5f),
            new _Spot(-09.0f, -13.9f), new _Spot(-26.0f, +02.0f), new _Spot(+32.7f, -03.2f), new _Spot(+04.6f, -08.2f), new _Spot(-18.0f, -11.4f), new _Spot(+28.8f, -13.7f), new _Spot(-11.8f, -00.1f), new _Spot(+26.0f, +05.6f),
            new _Spot(-32.7f, -04.6f), new _Spot(-04.9f, +10.0f), new _Spot(+21.1f, -02.2f), new _Spot(-23.1f, +15.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+26.4f, -06.7f), new _Spot(+09.9f, +07.9f), new _Spot(+18.6f, +03.5f), new _Spot(+01.5f, -06.1f), new _Spot(-17.7f, +14.0f), new _Spot(-31.8f, -01.9f), new _Spot(-19.5f, -10.5f),
            new _Spot(+13.9f, -14.5f), new _Spot(-10.6f, +05.3f), new _Spot(+28.8f, -15.8f), new _Spot(-08.1f, -15.4f), new _Spot(+11.9f, -03.7f), new _Spot(-28.4f, +11.6f), new _Spot(-00.8f, +04.4f), new _Spot(+18.8f, +12.7f),
            new _Spot(-02.0f, +14.1f), new _Spot(+32.6f, +00.7f), new _Spot(-20.8f, -01.0f), new _Spot(-09.1f, -04.0f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+29.1f, -08.5f), new _Spot(+21.1f, +02.6f), new _Spot(-04.9f, +10.8f), new _Spot(-19.2f, +06.8f), new _Spot(-19.0f, -08.6f), new _Spot(+02.1f, -06.6f), new _Spot(-28.7f, +03.1f),
            new _Spot(-08.5f, -15.5f), new _Spot(-08.3f, -01.4f), new _Spot(+04.9f, +15.4f), new _Spot(+08.3f, -15.3f), new _Spot(+17.6f, +15.0f), new _Spot(+22.5f, -14.7f), new _Spot(-28.3f, +14.8f), new _Spot(+32.7f, +02.5f),
            new _Spot(+07.2f, +04.8f), new _Spot(+14.6f, -07.0f), new _Spot(-16.2f, +15.9f), new _Spot(-32.7f, -05.7f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-03.4f, +13.5f), new _Spot(-17.7f, -14.8f), new _Spot(+05.4f, -01.4f), new _Spot(-15.9f, +15.1f), new _Spot(-22.7f, -04.0f), new _Spot(-06.9f, -06.7f), new _Spot(+21.9f, -03.3f),
            new _Spot(+11.6f, -13.3f), new _Spot(+27.5f, +04.7f), new _Spot(+05.3f, +08.5f), new _Spot(-23.6f, +06.9f), new _Spot(+00.1f, -14.6f), new _Spot(+31.3f, -08.2f), new _Spot(+18.0f, +15.4f), new _Spot(-30.0f, +14.0f),
            new _Spot(-33.5f, -00.3f), new _Spot(-12.0f, +01.5f), new _Spot(+15.6f, +06.4f), new _Spot(+24.7f, -15.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+24.1f, -01.7f), new _Spot(-04.6f, +09.3f), new _Spot(+17.5f, -14.2f), new _Spot(-25.3f, +13.7f), new _Spot(+07.4f, -08.3f), new _Spot(-18.7f, -13.8f), new _Spot(+13.0f, +03.1f),
            new _Spot(-28.0f, -05.7f), new _Spot(-08.1f, -06.4f), new _Spot(-16.3f, +02.3f), new _Spot(+33.2f, -00.4f), new _Spot(-32.8f, +03.1f), new _Spot(+18.7f, +14.2f), new _Spot(+31.0f, -14.0f), new _Spot(+04.5f, +14.8f),
            new _Spot(+26.1f, +08.0f), new _Spot(-01.7f, -15.5f), new _Spot(+00.3f, -00.1f), new _Spot(-14.0f, +15.3f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(-32.5f, +05.1f), new _Spot(+04.5f, -08.8f), new _Spot(-06.1f, +13.8f), new _Spot(-06.3f, -11.5f), new _Spot(+33.0f, -04.8f), new _Spot(+21.6f, -06.6f), new _Spot(+11.8f, -00.4f),
            new _Spot(+03.3f, +05.5f), new _Spot(-20.5f, +01.9f), new _Spot(+17.7f, +14.0f), new _Spot(-22.3f, -08.2f), new _Spot(+25.1f, +03.5f), new _Spot(-31.4f, -04.3f), new _Spot(-19.6f, +11.1f), new _Spot(-09.7f, +02.3f),
            new _Spot(+05.9f, +14.3f), new _Spot(-32.1f, +14.7f), new _Spot(-15.0f, -15.8f), new _Spot(+29.3f, -13.5f), new _Spot(+15.5f, -15.5f), new _Spot(+32.5f, +15.0f),
        },
        new _Spot[]
        {
            new _Spot(-32.5f, -15.0f), new _Spot(+21.4f, -05.0f), new _Spot(-10.3f, +03.8f), new _Spot(-07.7f, -05.9f), new _Spot(+07.8f, +03.4f), new _Spot(+10.4f, -14.4f), new _Spot(-00.6f, -12.5f), new _Spot(+28.4f, +06.3f),
            new _Spot(-18.6f, -13.8f), new _Spot(-32.4f, +13.0f), new _Spot(+31.0f, -07.0f), new _Spot(-20.8f, +10.9f), new _Spot(+06.7f, +15.3f), new _Spot(+17.3f, +06.8f), new _Spot(-28.6f, -01.1f), new _Spot(-01.9f, +08.1f),
            new _Spot(+23.0f, +15.5f), new _Spot(-18.1f, -04.4f), new _Spot(+20.7f, -15.3f), new _Spot(-07.9f, +15.6f), new _Spot(+05.5f, -05.5f), new _Spot(+32.5f, +15.0f),
        },
    };
}
