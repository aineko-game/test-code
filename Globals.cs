using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Globals
{
    public static readonly float ARROW_LENGTH = 0.8f;
    public static readonly float EPSILON = 0.001f;
    public static readonly float FIELD_WIDTH = 24f;
    public static readonly float FIELD_LENGTH = 10f;
    public static readonly float FIELD_HEIGHT = 0.2f;
    public static readonly float UNIT_MOVE_PER_SEC = 8;
    public static readonly float PRJ_MOVE_PER_SEC = 25;
    public static readonly float TIME_MAX_TO_UPDATE_HP_SLIDER = 0.7f;
    public static readonly List<string> ALL_UNIT_TYPES = new List<string>() { "Hero", "Enemy", "Object" };
    public static readonly List<string> ALL_HERO_CLASSES = new List<string>() { "Warrior", "Hunter", "Mage" };

    public static readonly Color32 SR_COLLIDER_AREA_WHITE = new Color32(255, 255, 255, 180);
    public static readonly Color32 SR_COLLIDER_AREA_GREEN = new Color32(000, 255, 000, 180);
    public static readonly Color32 SR_COLLIDER_AREA_RED = new Color32(255, 000, 000, 180);

    public static Globals Instance = new Globals();
    private static Light light;
    public static Light Light
    {
        get
        {
            if (light == null)
                light = GameObject.FindGameObjectWithTag("Light").GetComponent<Light>();

            return light;
        }
    }

    public static int inputStopperCount = 0;
    public static int runningAnimationCount = 0;
    public static int statusTabIndex = 1;
    public static int shopTabIndex = 0;

    public static float canvasScale = 1.0f;
    public static float timeDeltaFixed = 0f;
    public static float timeCountToShowTooltip = 0;

    public static bool isRunningSBA = false;
    public static bool isAutoSave = true;
    public static bool triggerSaveData = false;
    public static bool triggerResetColliderColor = false;
    public static bool triggerResetHpBar = false;
    public static bool triggerResetCanvasOrder = false;
    public static bool triggerStateBasedAction = false;

    private static string inputState = "StartGamae";
    public static string InputState
    {
        get { return inputState; }
        set { inputState = value; UI.ConfigureSuggest(value); }
    }
    //private static string inputExtraState = "";

    public static int bonusDamageOnCalcDamage = 0;
    public static float damageCoefOnCalcDamage = 1;
    public static float resistCoefOnCalcDamage = 1;

    public static _Clickable _clOnActive;

    public static Vector2 posOnCursorAtScreen = Vector2.zero;
    public static Vector3 posOnCursorAtGround = Vector3.zero;

    public static Map._Spot spotOnMouseover;

    public static _Item itemOnActive;
    public static List<_Item> itemsInBagList = new List<_Item>() { null, null, null, null, null, null };
    public static List<_Equip> inventoryList = new List<_Equip>() { null, null, null, null };
    public static _Equip equipToDiscard = null;
    public static _Item itemToDiscard = null;

    public static List<_Unit> originalUnitsList = new List<_Unit>();
    public static _Unit unitOnActive = default;
    public static _Unit unitOnMouseover = default;
    public static List<_Unit> unitList = new List<_Unit>();
    public static List<_Hero> heroList = new List<_Hero>();
    public static List<_Enemy> bossList = new List<_Enemy>();
    public static List<_Enemy> enemyList = new List<_Enemy>();
    public static List<_Object> objectList = new List<_Object>();

    public static _Equip equipOnActive;

    public static List<_Particles> particlesList = new List<_Particles>();

    public bool isEventActive = false;
    public _Event currentEvent;
    public string currentEventText;
    public _Event._choice currentEventChoice;

    public int gold = 0;
    public int Gold
    {
        get { return gold; }
        set { gold = value.Clamp(0, 999999); General.Instance.StartCoroutine(UI.LerpGoldValue()); }
    }
    public int seedOnAdventure;
    public int frameCount = 0;
    public int stageCount = 0;
    public int battleCount = 0;
    public int food = 0;
    public int Food 
    {
        get { return food; }
        set { food = value.Clamp(0, 99); UI.ConfigureFieldMapUI(); }
    }
    public int FoodMax = 12;
    public int hunger = 0;
    public int turnCount;
    public float gameSpeed = 1f;
    public float timeCount = 0f;
    public string turnState = "";
    public string sceneState = "Battle";
    public string bossName = "";
    public Vector3 cameraPosOffset = Camera.main.transform.position;
    public Vector3 fieldPosCenter = new Vector3(0f, 0f, 0f);
    public Vector3 fieldPosMin = new Vector3(-12f, -0.1f, -5f);
    public Vector3 fieldPosMax = new Vector3(+12f, +0.1f, +5f);
    public Vector3[] fieldCubicPointsArray = new Vector3[8];
    public _Random randomOnBattle;
    public List<string> TreasuresAlreadyPicked_ = new List<string>();
    public List<_Event> eventList00_ = new List<_Event>();
    public List<_Event> eventList01_ = new List<_Event>();
    public List<_Event> eventList02_ = new List<_Event>();
    public List<_Event> eventList03_ = new List<_Event>();

    public Map._Spot[] spotsArray;
    public Map._Spot spotCurrent;

    public _Equip[] equipsToCombine = new _Equip[2] { default, default };

    public List<_Unit._Parameter> paramsAtStartOfTurn = new List<_Unit._Parameter>();
    public List<List<_Unit._Parameter>> paramsOnStackList = new List<List<_Unit._Parameter>>();
    public List<_Item> itemsAtStartOfTurn = new List<_Item>();
    public List<List<_Item>> itemsOnStackList = new List<List<_Item>>();
    public List<_Skill> globalEffectList = new List<_Skill>();
    //public List<_Skill> GlobalEffectList
    //{
    //    get { return globalEffectList; }
    //    set { globalEffectList = value; UI.ConfigureGlobalEffectUI(); }
    //}

    public List<_Unit._Parameter> enemyParametersKilledInBattle = new List<_Unit._Parameter>();

    public ShopUndoSave shopUndoSave = new ShopUndoSave();
    [Serializable]
    public class ShopUndoSave
    {
        public int _gold;
        public List<_Item> _itemBagList;
        public List<_Equip> _inventoryList;
        public List<_Unit._Parameter> _heroParameterList = new List<_Unit._Parameter>();

        public _Item[] _shopItemList;
        public _Equip[] _shopEquipList;
        public _Skill[] _shopSkillList;

        public void _Save()
        {
            _gold = Instance.Gold;
            _itemBagList = itemsInBagList.DeepCopy();
            _inventoryList = inventoryList.DeepCopy();

            _heroParameterList.Clear();
            foreach (_Unit unit_i_ in heroList)
                _heroParameterList.Add(unit_i_._parameter.DeepCopy());

            _shopItemList = Instance.spotCurrent._shopItems.DeepCopy();
            _shopEquipList = Instance.spotCurrent._shopEquips.DeepCopy();
            //_shopSkillList = Instance.spotCurrent._shopSkills.DeepCopy();
        }
        public void _Load()
        {
            Instance.Gold = _gold;
            itemsInBagList = _itemBagList.DeepCopy();
            inventoryList = _inventoryList.DeepCopy();

            for (int i = 0; i < heroList.Count; i++)
                heroList[i]._parameter._equips = _heroParameterList[i]._equips.DeepCopy();

            Instance.spotCurrent._shopItems = _shopItemList.DeepCopy();
            Instance.spotCurrent._shopEquips = _shopEquipList.DeepCopy();
            //Instance.spotCurrent._shopSkills = _shopSkillList.DeepCopy();
        }
    }

    //public List<SkillTree> skillTreesList = new List<SkillTree>();
    //[Serializable]
    //public class SkillTree
    //{
    //    public _Skill[] _skillsArray = new _Skill[5];
    //    public _Skill[] _skillsArraySave;

    //    public void _Save()
    //    {
    //        _skillsArraySave = _skillsArray.DeepCopy();
    //    }
    //    public void _Load()
    //    {
    //        if (_skillsArraySave != null)
    //            _skillsArray = _skillsArraySave.DeepCopy();
    //    }
    //}
}
