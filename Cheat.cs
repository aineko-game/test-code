using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Text;

public class Cheat : _Singleton<Cheat>
{
    public static GameObject PosCheck_Circle;

    public int _inputStopperCountPeek;
    public int _seedOnAdventure;

    public float _gameSpeedTempSave = 0;

    public string _inputState;
    public int _setGoldTo = -1;
    public int _setLevelTo = -1;
    public string _setAllBattleSpotTo;
    public string _setCurrentSpotTo;
    public int _setFoodTo = -1;

    public int _MapGenerateIndex = -1;

    public bool _isSkipMainMenu = true;
    public bool _isSkipBattle = false;

    public bool _isEnemyNeverMove = false;
    public bool _isMovableAnytime = false;
    public bool _isActableAnytime = false;
    public bool _isSkipAnimatingUI = false;
    public bool _isSkillNoCd = false;
    public bool _isApplyInputState = false;

    public bool _triggerRestoreHp = false;
    public bool _triggerSetHpTo1 = false;
    public bool _triggerLevelUp = false;

    public Vector3 _newFieldCenter = new Vector3(0f, 0f, 0f);

    protected override void Awake()
    {
        base.Awake();

        PosCheck_Circle = transform.Find("PosCheck_Circle").gameObject;
        Globals.isAutoSave = false;
    }

    private void Start()
    {
        if (Instance._isSkipMainMenu)
        {
            MainMenu.Instance.gameObject.SetActive(false);
            General.StartAdventure();
        }
    }

    private void Update()
    {
        CheatOnUnits();
        CheatOnGlobals();
        CheatOnSetting();
        CheatOnPrefabs();
        CheatCommand();

        _seedOnAdventure = Globals.Instance.seedOnAdventure;
        _inputStopperCountPeek = Globals.inputStopperCount;
    }

    public static void CheatOnUnits()
    {
        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (Instance._triggerRestoreHp)
            {
                if (unit_i_._IsAlive())
                    unit_i_._RestoreHp(999, CheatSkill);
            }
            if (Instance._triggerSetHpTo1)
            {
                unit_i_._LoseHp(unit_i_._parameter._hp - 1, CheatSkill);
            }

            if (unit_i_ is _Hero hero_i_)
            {
                if (Instance._isMovableAnytime)
                    hero_i_._parameter._movableCount = 1;

                if (Instance._isActableAnytime)
                    hero_i_._parameter._actableCount = 1;

                if (Instance._triggerLevelUp)
                    hero_i_._parameter._lv += 1;

                if (Instance._setLevelTo > 0)
                    hero_i_._parameter._lv = (Instance._setLevelTo);

                if (Instance._isSkillNoCd)
                {
                    foreach(_Skill skill_j_ in hero_i_._parameter._skills)
                    {
                        skill_j_._parameter._cooldownRemaining = 0;
                    }
                    foreach (_Equip equip_j_ in hero_i_._parameter._equips)
                    {
                        if (equip_j_ != null)
                            equip_j_._cooldownRemaining = 0;
                    }
                }
            }

            if (unit_i_ is _Enemy enemy_i_)
            {
                if (Instance._isEnemyNeverMove)
                {
                    enemy_i_._parameter._spApplied = 0;
                    enemy_i_._parameter._movableRange = 0;
                }
            }
        }
        Instance._triggerRestoreHp = false;
        Instance._triggerSetHpTo1 = false;
        Instance._triggerLevelUp = false;
    }

    public static void CheatOnGlobals()
    {
        if (Instance._setGoldTo > 0)
        {
            Globals.Instance.Gold = Instance._setGoldTo;
        }
        if (Instance._setAllBattleSpotTo.IsNullOrEmpty() == false)
        {
            foreach (Map._Spot spot_i_ in Globals.Instance.spotsArray)
                if (spot_i_._type != "Boss" && spot_i_._type != "Shop")
                    spot_i_._type = Instance._setAllBattleSpotTo;
        }
        if (Instance._setCurrentSpotTo.IsNullOrEmpty() == false)
        {
            Globals.Instance.spotCurrent._type = Instance._setCurrentSpotTo;
        }
        if (Instance._setFoodTo >= 0)
        {
            Globals.Instance.Food = Instance._setFoodTo;
        }
    }

    public static void CheatOnPrefabs()
    {
        if (Instance._newFieldCenter != Globals.Instance.fieldPosCenter)
        {
            Battle.ConfigureField(Instance._newFieldCenter);
        }
    }

    public static void CheatCommand()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPaused = true;
            #endif
        }

        if (Keyboard.current.gKey.isPressed && Keyboard.current.rKey.wasPressedThisFrame)
        {
            General.StartAdventure();
        }

        if (Keyboard.current.cKey.isPressed && Keyboard.current.rKey.wasPressedThisFrame)
        {
            foreach (Transform child_ in Map.Instance.transform.Find("Paths"))
            {
                Destroy(child_.gameObject);
            }

            foreach (Map._Spot spot_i_ in Globals.Instance.spotsArray)
            {
                spot_i_._px = Map.goSpotsList[spot_i_._index].transform.localPosition.x;
                spot_i_._pz = Map.goSpotsList[spot_i_._index].transform.localPosition.z;
                Map.goSpotsList[spot_i_._index].Find("Index").GetComponent<TMPro.TextMeshProUGUI>().text = spot_i_._index.ToString();
            }

            Map.ConnectSpots();
        }

        if (Keyboard.current.mKey.isPressed && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Map.Initialization();
            Globals.Instance.spotsArray = Map.SpotsDataList[++Instance._MapGenerateIndex].DeepCopy();
            Map.ImplementSpots();
            Map.ConnectSpots();
            Map.DraftEvent(new _Random());
            Map.ImplementEvent();
            Map.DrawMap();

            Debug.Log(Instance._MapGenerateIndex);


            //Globals.Instance.seedOnAdventure = Random.Range(int.MinValue, int.MaxValue);
            //Map.DraftAndImplementMap();

            Globals.Instance.spotCurrent = Globals.Instance.spotsArray[0];
        }

        if (Keyboard.current.mKey.isPressed && Keyboard.current.sKey.wasPressedThisFrame)
        {
            #if UNITY_EDITOR
                WriteSpotsData();
            #endif
        }
    }

    public static void CheatOnSetting()
    {
        if (Instance._isApplyInputState)
            Globals.InputState = Instance._inputState;
        else
            Instance._inputState = Globals.InputState;

        if (Instance._isSkipBattle)
        {
            if (Globals.Instance.sceneState == "Battle")
            {
                General.ChangeScene("GoToNextSpot");
            }
        }

        if (Instance._isSkipAnimatingUI)
        {
            if (Globals.InputState == "AnimatingUI" && Globals.Instance.gameSpeed != 20)
            {
                if (Globals.Instance.gameSpeed != 20)
                {
                    Instance._gameSpeedTempSave = Globals.Instance.gameSpeed;
                    Globals.Instance.gameSpeed = 20;
                }
            }
            else if (Globals.Instance.gameSpeed == 20)
                Globals.Instance.gameSpeed = Instance._gameSpeedTempSave;
        }
    }

    public void CheatButton_Hide()
    {
        if (Globals.runningAnimationCount != 0) return;
        if (Globals.inputStopperCount != 0) return;

        transform.Find("Canvas").Find("Hide").gameObject.SetActive(false);
        transform.Find("Canvas").Find("Show").gameObject.SetActive(true);
        transform.Find("Canvas").Find("DebugCommand").gameObject.SetActive(false);
    }

    public void CheatButton_Show()
    {
        if (Globals.runningAnimationCount != 0) return;
        if (Globals.inputStopperCount != 0) return;
        
        transform.Find("Canvas").Find("Hide").gameObject.SetActive(true);
        transform.Find("Canvas").Find("Show").gameObject.SetActive(false);
        transform.Find("Canvas").Find("DebugCommand").gameObject.SetActive(true);
    }

    public void CheatButton_Save()
    {
        if (Globals.runningAnimationCount != 0) return;
        if (Globals.inputStopperCount != 0) return;

        General.SaveDataAll();
    }

    public void CheatButton_Load()
    {
        if (Globals.runningAnimationCount != 0) return;
        if (Globals.inputStopperCount != 0) return;

        General.LoadDataAll();
    }

    public void CheatButton_KillEnemies()
    {
        if (Globals.Instance.sceneState != "Battle") return;
        if (Globals.runningAnimationCount != 0) return;
        if (Globals.inputStopperCount != 0) return;

        foreach (_Unit unit_i_ in Globals.enemyList)
        {
            unit_i_._animationTimeToDead = 0;
            unit_i_._LoseHp(9999, CheatSkill);
            unit_i_._parameter._additionalPassives.Clear();
            unit_i_._parameter._classPassives.Clear();
        }
        Globals.triggerStateBasedAction = true;
    }

    public void CheatButton_Dead()
    {
        if (Globals.Instance.sceneState != "Battle") return;
        if (Globals.runningAnimationCount != 0) return;
        if (Globals.inputStopperCount != 0) return;

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            unit_i_._animationTimeToDead = 0;
            unit_i_._LoseHp(9999, CheatSkill);
            unit_i_._parameter._additionalPassives.Clear();
            unit_i_._parameter._classPassives.Clear();
        }
        Globals.triggerStateBasedAction = true;
    }

    public void CheatButton_LevelUP()
    {
        foreach (_Unit unit_i_ in Globals.heroList)
        {
            unit_i_._parameter._lv = (unit_i_._parameter._lv + 1).Clamp(0, 9);
        }
    }

    public void CheatButton_NewBattle()
    {
        if (Globals.Instance.sceneState != "Battle") return;
        if (Globals.runningAnimationCount != 0) return;
        if (Globals.inputStopperCount != 0) return;

        General.Instance.StartCoroutine(Battle.StartBattle(Random.Range(int.MinValue, int.MaxValue), Globals.Instance.spotCurrent._type));
    }

    public static void WriteSpotsData()
    {
        Map._Spot[] spotList_ = Globals.Instance.spotsArray;
        using (StreamWriter writer_ = new StreamWriter(Directory.GetCurrentDirectory() + "/SpotsDataList.txt", true, Encoding.GetEncoding("utf-8")))
        {
            string text_ = "        new _Spot[]\n        {";
            foreach (Map._Spot spot_i_ in spotList_)
            {
                if (spot_i_._index % 8 == 0)
                    text_ += "\n            ";

                text_ += "new _Spot(" + spot_i_._px.ToString("+00.0;-00.0") + "f, " + spot_i_._pz.ToString("+00.0;-00.0") + "f), ";
            }
            text_ += "\n        },";

            writer_.WriteLine(text_);
        }
        Debug.Log("Spots data save!");
    }


    public static _Skill CheatSkill = new _Skill
    {
        //_parameter = new _Skill._Parameter
        //{
        //    _name = "Cheat",
        //    _adDamageBase = 00,
        //    _mdDamageBase = 00,
        //    _adRatio = 0,
        //    _mdRatio = 0,
        //    _targetRange = 4.5f,
        //    _animatorSetBool = "",
        //    _animatorSetTrigger = "triggerAttack",
        //    _castType = "Target",
        //},
        //_tooltip = new _Tooltip(),
        //_functionCastSkill = default,
        //_functionDetectUnits = default
    };
}
