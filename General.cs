using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;
using TMPro;

public class General : _Singleton<General>
{
    public static IEnumerator ChangeSceneWithFade(string scene_, string wayToTransition_, float timeLength_ = 0.4f)
    {
        Globals.inputStopperCount++;

        Sprite sprite_ = null;
        if (wayToTransition_ == "BattleToMap")
            sprite_ = Prefabs.Instance.transitionSprites[2];
        else if (wayToTransition_ == "MapToBattle")
            sprite_ = Prefabs.Instance.transitionSprites[1];
        else if (wayToTransition_ == "TreasureToMap")
            sprite_ = Prefabs.Instance.transitionSprites[4];

        yield return Instance.StartCoroutine(TransitionScreen(Prefabs.Instance.transitionIn, sprite_, timeLength_));

        ChangeScene(scene_);

        //yield return Instance.StartCoroutine(FadeScreen(new Color32(000, 000, 000, 255), new Color32(000, 000, 000, 000), 0.3f));
        yield return Instance.StartCoroutine(TransitionScreen(Prefabs.Instance.transitionOut, sprite_, timeLength_));
        Globals.inputStopperCount--;
    }

    public static void ChangeScene(string scene_)
    {
        Globals.Instance.sceneState = scene_;

        if (scene_ == "GoToNextSpot")
        {
            Globals.InputState = "GoToNextSpot";
            Battle.SetUnitOnInactive("GoToNextSpot");

            UI.goBattle.SetActive(false);
            UI.goMap.SetActive(false);
            UI.goFieldMap.SetActive(true);
            UI.goOpenShop.SetActive(Globals.Instance.spotCurrent._type == "Shop");
            UI.ConfigureGlobalEffectUI();
            Map.HungerCheck();

            Globals.Instance.cameraPosOffset = new Vector3(0f, 50, 42f);
            Camera.main.transform.rotation = Quaternion.Euler(65, 0, 0);

            Map.DrawMap();
        }
        else if (scene_ == "Battle")
        {
            Globals.InputState = "HeroMain";
            Battle.SetUnitOnInactive("HeroMain");

            UI.goBattle.SetActive(true);
            UI.goMap.SetActive(true);
            UI.goFieldMap.SetActive(false);
            UI.goOpenShop.SetActive(false);

            if (Battle.IsWinBattle() == true)
            {
                UI.ConfigureWinBattle();
            }
            UI.goWinBattle.SetActive(Battle.IsWinBattle());
            UI.goGameOver.SetActive(Battle.IsGameOver());

            Globals.Instance.cameraPosOffset = new Vector3(0f, 16, -7.9f);
            Camera.main.transform.rotation = Quaternion.Euler(65, 0, 0);
        }
        else if (scene_ == "CheckTheMap")
        {
            Globals.InputState = "CheckTheMap";
            Battle.SetUnitOnInactive("CheckTheMap");

            UI.goBattle.SetActive(false);
            UI.goMap.SetActive(true);
            UI.goFieldMap.SetActive(true);
            UI.goOpenShop.SetActive(false);
            UI.ConfigureGlobalEffectUI();

            Globals.Instance.cameraPosOffset = new Vector3(0f, 50, 42f);
            Camera.main.transform.rotation = Quaternion.Euler(65, 0, 0);
            Map.DrawMap();
        }
        else if (scene_ == "Treasure")
        {
            Globals.InputState = "Treasure";
            Globals.Instance.turnState = "Treasure";
            //Battle.SetUnitOnInactive("HeroMain");

            UI.goBattle.SetActive(true);
            UI.goMap.SetActive(true);
            UI.goFieldMap.SetActive(false);
            UI.goOpenShop.SetActive(false);

            UI.goWinBattle.SetActive(false);
            UI.goGameOver.SetActive(false);

            Globals.Instance.cameraPosOffset = new Vector3(0f, 16, -7.9f);
            Camera.main.transform.rotation = Quaternion.Euler(65, 0, 0);
        }

        UI.ConfigureNotification();
    }

    public static IEnumerator DelayForSeconds(float waitSecond, Action action)
    {
        if (waitSecond > 0)
            yield return new WaitForSeconds(waitSecond);

        action();
    }

    public static IEnumerator FadeScreena(Color colorBegin_, Color colorEnd_, float timeMax_)
    {
        UI.imTransition.color = colorBegin_;

        for (float timeSum_ = 0; timeSum_ < timeMax_ + Globals.timeDeltaFixed; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1);

            UI.imTransition.color = (1 - p_) * colorBegin_ + p_ * colorEnd_;

            yield return null;
        }

        UI.imTransition.color = colorEnd_;
    }

    public static bool IsSaveDataExist()
    {
        if (SaveData.GetClass<Globals>("Globals", null) == null)
        {
            return false;
        }
        return true;
    }

    public static void LoadDataAll()
    {
        if (SaveData.GetClass<Globals>("Globals", null) == null) return;

        Globals.Instance = SaveData.GetClass("Globals", Globals.Instance);
        Globals.Instance.spotCurrent = Globals.Instance.spotsArray[Globals.Instance.spotCurrent._index];
        Globals.originalUnitsList = new List<_Unit>(Prefabs.goOriginalUnits.GetComponentsInChildren<_Unit>(true));
        //Setting.instance._parameter = SaveData.GetClass("Setting", Setting.Instance._parameter);

        Map.ImplementMap();
        Map.HeroToken.transform.localPosition = new Vector3(Globals.Instance.spotCurrent._px, 0, Globals.Instance.spotCurrent._pz);
        Map.HeroToken.transform.rotation = Quaternion.Euler(000, 180, 000);

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            Destroy(unit_i_.gameObject);
        }
        Globals.unitList.Clear();
        Globals.heroList.Clear();
        Globals.enemyList.Clear();
        Globals.bossList.Clear();
        Globals.objectList.Clear();

        for (int i_ = 0; i_ < 99; i_++)
        {
            if (SaveData.GetClass<_Unit._Parameter>("UnitParameter" + i_, null) is _Unit._Parameter parameter_i_)
            {
                _Unit unitNew_i_;
                if (parameter_i_._unitType == "Hero")
                    unitNew_i_ = _Unit.CloneFromString(parameter_i_._class) as _Hero;
                else if (parameter_i_._unitType == "Enemy")
                    unitNew_i_ = _Unit.CloneFromString(parameter_i_._class) as _Enemy;
                else if (parameter_i_._unitType == "Object")
                    unitNew_i_ = _Unit.CloneFromString(parameter_i_._class) as _Object;
                else
                    unitNew_i_ = _Unit.CloneFromString(parameter_i_._class) as _Unit;

                if (unitNew_i_ != null)
                    unitNew_i_._LoadParameter(parameter_i_);
            }
        }

        for (int i_ = 0; i_ < 99; i_++)
        {
            List<_Unit._Parameter> paramsOnStack_ = SaveData.GetList("ParamsOnStack" + i_, new List<_Unit._Parameter>());

            if (paramsOnStack_.Count > 0)
            {
                Globals.Instance.paramsOnStackList.Add(paramsOnStack_);
            }
        }

        foreach (Map._Spot spot_i_ in Globals.Instance.spotsArray)
        {
            for (int j = 0; j < spot_i_._shopItems.Length; j++)
            {
                spot_i_._shopItems[j] = _Item.CloneFromString(spot_i_._shopItems[j]._name);
            }
            for (int j = 0; j < spot_i_._shopEquips.Length; j++)
            {
                spot_i_._shopEquips[j] = _Equip.CloneFromString(spot_i_._shopEquips[j]._name);
            }
        }

        for (int i_ = 0; i_ < Globals.itemsInBagList.Count; i_++)
        {
            _Item temp_ = SaveData.GetClass<_Item>("ItemInBag" + i_, null);

            if (temp_ is _Item && _Item.CloneFromString(temp_._name) is _Item)
            {
                Globals.itemsInBagList[i_] = _Item.CloneFromString(temp_._name);
                Globals.itemsInBagList[i_]._stackCount = temp_._stackCount;
            }
            else
            {
                Globals.itemsInBagList[i_] = null;
            }
        }

        for (int i_ = 0; i_ < Globals.inventoryList.Count; i_++)
        {
            _Equip temp_ = SaveData.GetClass<_Equip>("Equip" + i_, null);

            if (temp_ is _Equip && _Equip.CloneFromString(temp_._name) is _Equip)
            {
                Globals.inventoryList[i_] = _Equip.CloneFromString(temp_._name);
            }
            else
            {
                Globals.inventoryList[i_] = default;
            }
        }

        for (int i_ = 0; i_ < 9; i_++)
        {
            List<_Item> itemsOnStack_ = SaveData.GetList("ItemsOnStack" + i_, new List<_Item>());

            if (itemsOnStack_.Count > 0)
            {
                Globals.Instance.itemsOnStackList.Add(itemsOnStack_);
            }
        }

        for (int i = 0; i < Globals.Instance.globalEffectList.Count; i++)
        {
            _Skill._Parameter temp_ = Globals.Instance.globalEffectList[i]._parameter.DeepCopy();
            Globals.Instance.globalEffectList[i] = _Skill.CloneFromString(Globals.Instance.globalEffectList[i]._parameter._name);
            Globals.Instance.globalEffectList[i]._parameter = temp_.DeepCopy();
        }

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            unit_i_._ApplyBuff();
            unit_i_._RecomputeComponents();
            unit_i_._DisplayBuffAndStatusIcon();
        }

        Map.HeroToken.transform.position = JsonUtility.FromJson<Vector3>(SaveData.GetString("TokenPosition"));
        Map.HeroToken.transform.rotation = JsonUtility.FromJson<Quaternion>(SaveData.GetString("TokenRotation"));

        Battle.SetUnitOnInactive();
        //Setting.Instance.ConfigureGameSetting();
        UI.ConfigureGoldUI();
        UI.ConfigureFieldMapUI();
        UI.ConfigureGlobalEffectUI();
        //Setting.ApplySetting();
        ChangeScene(Globals.Instance.sceneState);

        if (Globals.Instance.isEventActive)
        {
            UI.goEvent.SetActive(true);
            Instance.StartCoroutine(_Event.ShowEvent(Globals.Instance.currentEventText, Globals.Instance.currentEvent, Globals.Instance.currentEventChoice));
        }

        if (Globals.Instance.sceneState == "Treasure")
        {
            General.Instance.StartCoroutine(Map.EncounterTreasure(Globals.Instance.spotCurrent._index));
        }

        Debug.Log("Load succeeds!");
    }

    public static void LateUpdateCameraPosition()
    {
        Camera.main.transform.position = Globals.Instance.fieldPosCenter + Globals.Instance.cameraPosOffset;
    }

    public static IEnumerator MoveTowards(GameObject go_, Vector3 posEnd_, float movePerSec_, bool isDestroyObject_ = false)
    {
        while ((go_.transform.position - posEnd_).sqrMagnitude > Globals.EPSILON)
        {
            go_.transform.position = Vector3.MoveTowards(go_.transform.position, posEnd_, movePerSec_ * Globals.timeDeltaFixed);
            yield return null;
        }

        if (isDestroyObject_ == true)
            Destroy(go_);
    }

    public static void SaveDataAll()
    {
        Globals.Instance.isEventActive = UI.goEvent.activeSelf;

        SaveData.Clear();
        SaveData.SetClass("Globals", Globals.Instance);
        //SaveData.SetClass("Setting", Setting.Instance._parameter);

        for (int i_ = 0; i_ < Globals.unitList.Count; i_++)
        {
            SaveData.SetClass("UnitParameter" + i_, Globals.unitList[i_]._SaveParameter());
        }

        for (int i_ = 0; i_ < Globals.Instance.paramsOnStackList.Count; i_++)
        {
            SaveData.SetList("ParamsOnStack" + i_, Globals.Instance.paramsOnStackList[i_]);
        }

        for (int i_ = 0; i_ < Globals.itemsInBagList.Count; i_++)
        {
            SaveData.SetClass("ItemInBag" + i_, Globals.itemsInBagList[i_]);
        }

        for (int i_ = 0; i_ < Globals.inventoryList.Count; i_++)
        {
            SaveData.SetClass("Inventory" + i_, Globals.inventoryList[i_]);
        }

        for (int i_ = 0; i_ < Globals.Instance.itemsOnStackList.Count; i_++)
        {
            SaveData.SetList("ItemsOnStack" + i_, Globals.Instance.itemsOnStackList[i_]);
        }

        SaveData.SetString("TokenPosition", JsonUtility.ToJson(Map.HeroToken.transform.position));
        SaveData.SetString("TokenRotation", JsonUtility.ToJson(Map.HeroToken.transform.rotation));

        SaveData.Save();
        Debug.Log("Save succeeds!");
    }

    public static IEnumerator SetLightIntensity(float intensityEnd_, float timeMax_)
    {
        float intensityStart_ = Globals.Light.intensity;

        for (float timeSum_ = 0; timeSum_ < timeMax_; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1);
            Globals.Light.intensity = Mathf.Lerp(intensityStart_, intensityEnd_, p_);

            yield return null;
        }

        Globals.Light.intensity = intensityEnd_;
    }

    public static IEnumerator ScaleWithTime(GameObject go_, Vector3 scaleBegin_, Vector3 scaleEnd_, float timeMax_)
    {
        go_.transform.localScale = scaleBegin_;

        for (float timeSum_ = 0; timeSum_ < timeMax_ - Globals.timeDeltaFixed; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1);

            go_.transform.localScale = Library.BezierLiner(scaleBegin_, scaleEnd_, p_);

            yield return null;
        }

        go_.transform.localScale = scaleEnd_;
    }

    public static Vector3 ScreenToGroundPoint(Vector2 posScreen_)
    {
        Ray ray_ = Camera.main.ScreenPointToRay(posScreen_);

        if (Physics.Raycast(ray_, out RaycastHit hit_, Mathf.Infinity, /*layer = Ground*/ 1 << 8))
            return hit_.point;
        else
            return Vector3.back;
    }

    public static void StartAdventure()
    {
        foreach (_Unit unit_i_ in Globals.enemyList)
            Destroy(unit_i_.gameObject);
        foreach (_Unit unit_i_ in Globals.objectList)
            Destroy(unit_i_.gameObject);

        Globals.heroList = new List<_Hero>(_Unit._GoUnits.GetComponentsInChildren<_Hero>(false));
        Globals.enemyList = new List<_Enemy>(_Unit._GoUnits.GetComponentsInChildren<_Enemy>(false));
        Globals.objectList = new List<_Object>(_Unit._GoUnits.GetComponentsInChildren<_Object>(false));
        Globals.unitList = new List<_Unit>(Globals.heroList);
        Globals.unitList.AddRange(Globals.enemyList);
        Globals.Instance.Food = Globals.Instance.FoodMax;
        Globals.Instance.Gold = 300;
        Globals.Instance.stageCount = 1;
        Globals.Instance.globalEffectList.Clear();
        Globals.Instance.TreasuresAlreadyPicked_ = new List<string>();
        Globals.Instance.eventList00_ = new List<_Event>(Table.EventTable[00]);
        Globals.Instance.eventList01_ = new List<_Event>(Table.EventTable[01]);
        Globals.Instance.eventList02_ = new List<_Event>(Table.EventTable[02]);
        Globals.Instance.eventList03_ = new List<_Event>(Table.EventTable[03]);

        UI.ConfigureGoldUI(Globals.Instance.gold);
        UI.goEvent.SetActive(false);
        UI.goTreasureChest.SetActive(false);
        Globals.originalUnitsList = new List<_Unit>(Prefabs.goOriginalUnits.GetComponentsInChildren<_Unit>(true));

        if (int.TryParse(Setting.Instance._parameter._seedPredefined, out int _seed))
            Globals.Instance.seedOnAdventure = _seed;
        else
            Globals.Instance.seedOnAdventure = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

        _Random random_ = new _Random((uint)Globals.Instance.seedOnAdventure);

        DecideSkillTrees();

        for (int i = 0; i < Globals.heroList.Count; i++)
        {
            Globals.heroList[i]._InitializeLevel();
        }

        Globals.itemsInBagList[0] = _Item.CloneFromString("Healing Potion");
        Globals.itemsInBagList[0]._stackCount = 3;
        //Globals.itemsInBagList[1] = _Item.CloneFromString("Scroll of Protection");
        //Globals.itemsInBagList[2] = _Item.CloneFromString("Scroll of Fury");
        //Globals.itemsInBagList[3] = _Item.CloneFromString("Scroll of Blessing");
        //Globals.itemsInBagList[1]._stackCount = 3;
        //Globals.itemsInBagList[2] = _Item.CloneFromString("Explosive Potion");f
        //Globals.itemsInBagList[2]._stackCount = 3;

        //Globals.heroList[0]._parameter._equips[0] = _Equip.CloneFromString("Heart of Iron");
        //Globals.heroList[0]._parameter._equips[1] = _Equip.CloneFromString("Marisa's Witch Hat");
        //Globals.heroList[0]._parameter._equips[1] = _Equip.CloneFromString("Sword");
        //Globals.heroList[0]._parameter._equips[2] = _Equip.CloneFromString("Shield");
        //Globals.heroList[0]._parameter._equips[3] = _Equip.CloneFromString("Staff");
        //Globals.heroList[1]._parameter._equips[0] = _Equip.CloneFromString("Robe");
        //Globals.heroList[1]._parameter._equips[1] = _Equip.CloneFromString("Boots");
        //Globals.heroList[1]._parameter._equips[2] = _Equip.CloneFromString("Crystal");
        //Globals.heroList[1]._parameter._equips[3] = _Equip.CloneFromString("Sword");
        //Globals.heroList[2]._parameter._equips[0] = _Equip.CloneFromString("Shield");
        //Globals.heroList[2]._parameter._equips[1] = _Equip.CloneFromString("Staff");
        //Globals.heroList[2]._parameter._equips[2] = _Equip.CloneFromString("Robe");
        //Globals.heroList[2]._parameter._equips[3] = _Equip.CloneFromString("Boots");

        //Globals.inventoryList[0] = _Equip.CloneFromString("Twin Force");
        //Globals.inventoryList[1] = _Equip.CloneFromString("Rule Breaker");
        //Globals.inventoryList[2] = _Equip.CloneFromString("Twin Force");
        //Globals.inventoryList[3] = _Equip.CloneFromString("Necronomicon");

        //Globals.Instance.globalEffectList.Add(_Skill.CloneFromString("Amulet Coin"));

        Map.DraftAndImplementMap();
        Globals.Instance.spotCurrent = Globals.Instance.spotsArray[0];

        ChangeScene("GoToNextSpot");

        //SaveDataAll();

        void DecideSkillTrees()
        {
            //Globals.Instance.skillTreesList.Clear();

            foreach (_Unit unit_i_ in Globals.heroList)
            {
                _Skill[] skillTree_ = unit_i_._parameter._skillTree;

                string[] skillNames01_ = Table.HeroSkillTable[new Tuple<string, int>(unit_i_._parameter._class, 1)].Shuffle(random_);
                string[] skillNames02_ = Table.HeroSkillTable[new Tuple<string, int>(unit_i_._parameter._class, 2)].Shuffle(random_);
                string[] skillNames03_ = Table.HeroSkillTable[new Tuple<string, int>(unit_i_._parameter._class, 3)].Shuffle(random_);

                skillTree_[0] = _Skill.CloneFromString(skillNames01_[0]);
                skillTree_[1] = _Skill.CloneFromString(skillNames02_[0]);
                skillTree_[2] = _Skill.CloneFromString(skillNames02_[1]);
                skillTree_[3] = _Skill.CloneFromString(skillNames03_[0]);
                skillTree_[4] = _Skill.CloneFromString(skillNames03_[1]);

                for (int j = 0; j < skillTree_.Length; j++)
                {
                    List<string> skillAbilityList_ = skillTree_[j]._parameter._skillAbilitiesTable.Shuffle();

                    for (int k = 0; k < skillTree_[j]._parameter._skillAbilities.Length; k++)
                    {
                        skillTree_[j]._parameter._skillAbilities[k] = _SkillAbility.CloneFromString(skillAbilityList_[k]).DeepCopy();
                        skillTree_[j]._parameter._rank = (j + 3) / 2; 
                    }
                }

                _Skill.ActivateSkill(unit_i_, skillTree_[0]);

                unit_i_._SkillTree_Save();
            }
        }
    }

    //public static IEnumerator ToCoroutine(Action action)
    //{
    //    action();

    //    if (action == null) yield return null;
    //}

    public static IEnumerator TransitionScreen(Material material_, Sprite sprite_, float timeMax_)
    {
        UI.imTransition.gameObject.SetActive(true);
        UI.imTransition.sprite = sprite_;
        UI.imTransition.material = material_;

        for (float timeSum_ = 0; timeSum_ < timeMax_ + Globals.timeDeltaFixed; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1);

            material_.SetFloat("_Alpha", p_);

            yield return new WaitForEndOfFrame();
        }
        material_.SetFloat("_Alpha", 1);
    }

    public static IEnumerator Typewriter(TextMeshProUGUI tmpro_, string text_, float waitTime_ = 0.01f, bool isSkipable_ = true)
    {
        int bracketsCount = 0;
        for (int i = 1; i < text_.Length + 1;)
        {
            string textFormer_ = text_.Substring(0, i);
            string textLatter_ = text_.Substring(i);
            char char_ = text_[i++ - 1];
            float waitCount_ = (char_ == '.') ? 08:
                               (char_ == '\n') ? 16:
                               (char_ == ',') ? 04 : 01;

            tmpro_.text = textFormer_ + "<color=#ffffff00>" + textLatter_ + "</color>";

            if (isSkipable_ && (Mouse.current.rightButton.isPressed || Mouse.current.leftButton.wasPressedThisFrame))
                break;

            if (char_ == '<') bracketsCount++;
            if (char_ == '>') bracketsCount--;
            if (bracketsCount > 0) continue;

            for (int j = 0; j < waitCount_; j++)
            {
                yield return null;
            }
        }
        tmpro_.text = text_;
    }
}
