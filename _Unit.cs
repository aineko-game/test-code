using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class _Unit : MonoBehaviour
{
    private static GameObject _goUnits = null;
    public static GameObject _GoUnits
    {
        get
        {
            if (_goUnits == null)
                _goUnits = GameObject.FindGameObjectWithTag("Units");

            return _goUnits;
        }
    }

    public static Dictionary<string, Sprite> iconList = new Dictionary<string, Sprite>();
    public Sprite _Sprite
    {
        get
        {
            if (iconList.ContainsKey(_parameter._class) == false)
            {
                Sprite sprite_ = (Resources.Load<Sprite>("UnitIcon/" + _parameter._class) is Sprite out_) ? out_ : Prefabs.Instance.spDummy;
                iconList.Add(_parameter._class, sprite_);
            }

            return iconList[_parameter._class];
        }
    }

    public _Parameter _parameter = new _Parameter();
    [Serializable]
    public class _Parameter
    {
        public float _p_x, _p_y, _p_z;
        public float _r_x, _r_y, _r_z, _r_w;

        public string _class;
        public string _unitType;
        public List<string> _tags;
        public List<string> _unitTypesThroughable;
        //public List<string> _unitTypesAttackable;
        public string _positioningType = "Middle";
        public string _entranceType = "Stay";
        public string _iconPath = "Resources/UI/UnitIcon_Dummy";

        public int _lv = 01;
        public int _hp;
        public int _hpMax;
        public int _hpOnView;
        public int _hpToSlide;
        public int _hpToLose;
        public int _hpToRestore;
        public int _barrierValue;

        public int _ad;
        public int _ar;
        public int _md;
        public int _mr;
        public int _sp;

        public int _hpBuff;
        public int _adBuff;
        public int _arBuff;
        public int _mdBuff;
        public int _mrBuff;
        public int _spBuff;

        //public int _hpApplied;
        public int _adApplied;
        public int _arApplied;
        public int _mdApplied;
        public int _mrApplied;
        public int _spApplied;

        public float _hpPerLevel;
        public float _adPerLevel;
        public float _arPerLevel;
        public float _mdPerLevel;
        public float _mrPerLevel;
        public float _spPerLevel;


        [Serializable]
        public class _StatusCondition
        {
            public string _name;
            public string _type;
            public bool _isDecreaseEndOfTurn;
            public int _count = 0;

            public _StatusCondition(string name_, string type_, bool isDecreaseEndOfTurn_ = true)
            {
                _name = name_;
                _type = type_;
                _isDecreaseEndOfTurn = isDecreaseEndOfTurn_;
            }
        }

        public List<_StatusCondition> _statusConditions = new List<_StatusCondition>
        {
            new _StatusCondition("Artifact", "Good", false),
            new _StatusCondition("Stealth", "Good", false),
            new _StatusCondition("Protection", "Good", false),
            new _StatusCondition("Fury", "Good", false),
            new _StatusCondition("Stun", "Bad"),
            new _StatusCondition("Snare", "Bad"),
            new _StatusCondition("Blind", "Bad"),
        };

        public int _exp;
        public int _expFraction;
        public int _expEnemyHave;
        public int _goldEnemyHave;

        public int _actableCount = 1;
        public int _actableCountMax = 1;
        public int _movableCount = 1;
        public int _movableCountMax = 1;

        public float _movableRange;
        public float _colliderRange;

        public bool isBoss = false;

        public _Skill[] _skills = new _Skill[4];
        public List<_Skill> _classPassives = new List<_Skill>();
        public List<_Skill> _additionalPassives = new List<_Skill>();
        public _Equip[] _equips = new _Equip[4];

        public _Skill[] _skillTree = new _Skill[5];
        public _Skill[] _skillTreeSave = new _Skill[5];
        public _Skill[] _skillsSave = new _Skill[4];
    }

    public int _BarrierValue
    {
        get { return _parameter._barrierValue; }
        set
        {
            if (value > 0 && _parameter._barrierValue <= 0)
                _Barrier_Gain();
            else if (value <= 0 && _parameter._barrierValue > 0)
                _Barrier_Lost();

            _parameter._barrierValue = value;
            _DisplayBuffAndStatusIcon();
        }
    }

    [HideInInspector] public int _damagePopup = 0;
    [HideInInspector] public int _healingPopup = 0;
    [HideInInspector] public int _popupCoroutineCount = 0;
    /*             */ public float _charaHeight;
    /*             */ public float _moveSpeed = 1;
    [HideInInspector] public float _timeLeftToUpdateHpSlider;
    /*             */ public float _animationTimeToDead;

    [HideInInspector] public _Skill _skillOnActive = default;

    [HideInInspector] public GameObject _goBody;
    [HideInInspector] public GameObject _goComps;
    [HideInInspector] public GameObject _goComps_Y5;
    [HideInInspector] public GameObject _goCanvas;

    [HideInInspector] public Animator _animator;
    [HideInInspector] public Canvas _canvas;
    [HideInInspector] public _Clickable _clickable;
    [HideInInspector] public Image _imHpSliderToSlide;
    [HideInInspector] public Image _imHpSliderToLose;
    [HideInInspector] public Image _imHpSliderToRestore;
    [HideInInspector] public Image _imHpSliderMain;
    [HideInInspector] public Image []_imStatusIcons;
    /*             */ public SkinnedMeshRenderer _skinnedMeshRenderer;
    [HideInInspector] public Outline _outline;
    [HideInInspector] public RectTransform _rtCanvasPos;
    [HideInInspector] public RectTransform _rtHpSliderMain;
    [HideInInspector] public RectTransform _rtHpSliderToLose;
    [HideInInspector] public RectTransform _rtHpSliderToSlide;
    [HideInInspector] public RectTransform _rtHpSliderToRestore;
    [HideInInspector] public RectTransform _rtHpSliderBarrier;
    [HideInInspector] public SphereCollider _sphereCollider;
    [HideInInspector] public SpriteRenderer _srColliderArea;
    [HideInInspector] public SpriteRenderer _srMovableArea;
    [HideInInspector] public Transform _modelTransform;
    [HideInInspector] public TextMeshProUGUI _txDamagePopup;
    [HideInInspector] public TextMeshProUGUI[] _txStatusCount;
    [HideInInspector] public Vector3 _posCenter;
    [HideInInspector] public Vector3 _posSOB = Vector3.down;
    [HideInInspector] public Vector3 _posDistSOB = Vector3.down;
    [HideInInspector] public Quaternion _qrtSOB = Quaternion.identity;

    [HideInInspector] private _Unit _originalUnit;
    [HideInInspector] public _Unit _OriginalUnit
    {
        get
        {
            if (_originalUnit == null)
            {
                foreach (_Unit unit_i_ in Globals.originalUnitsList)
                {
                    if (unit_i_._parameter._class == _parameter._class)
                    {
                        _originalUnit = unit_i_;
                    }
                }
            }
            return _originalUnit;
        }
        set { _originalUnit = value; }
    }
    
    protected virtual void Awake()
    {
        Globals.unitList.Add(this);

        tag = "Unit";

        _goBody = transform.Find("TransformAdjuster").FindTagInChildren("Body").gameObject;
        _goComps = transform.Find("Components").gameObject;
        _goComps_Y5 = transform.Find("Components_Y5").gameObject;

        if (transform.Find("Canvas"))
        {
            _goCanvas = transform.Find("Canvas").gameObject;
        }
        else
        {
            if (_parameter.isBoss == true)
                _goCanvas = Instantiate(Prefabs.Instance.UnitComponents.Find(m => m.name == "BossCanvas"), transform);
            else if (_parameter._unitType == "Object")
                _goCanvas = Instantiate(Prefabs.Instance.UnitComponents.Find(m => m.name == "ObjectCanvas"), transform);
            else
                _goCanvas = Instantiate(Prefabs.Instance.UnitComponents.Find(m => m.name == "UnitCanvas"), transform);

            _goCanvas.name = "Canvas";
        }

        _animator = gameObject.GetComponent<Animator>();
        _canvas = _goCanvas.GetComponent<Canvas>();
        _clickable = (GetComponent<_Clickable>() is _Clickable) ? GetComponent<_Clickable>() : gameObject.AddComponent<_Clickable>();
        _imHpSliderToSlide = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerToSlide").GetComponent<Image>();
        _imHpSliderToLose = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerToLose").GetComponent<Image>();
        _imHpSliderToRestore = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerToRestore").GetComponent<Image>();
        _imHpSliderMain = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerMain").GetComponent<Image>();
        _imStatusIcons = _goCanvas.transform.Find("TransformCenter").Find("StatusIcons").GetComponentsInChildren<Image>(true);
        _rtCanvasPos = _goCanvas.transform.Find("TransformCenter").GetComponent<RectTransform>();
        _rtHpSliderMain = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerMain").GetComponent<RectTransform>();
        _rtHpSliderToLose = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerToLose").GetComponent<RectTransform>();
        _rtHpSliderToRestore = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerToRestore").GetComponent<RectTransform>();
        _rtHpSliderToSlide = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerToSlide").GetComponent<RectTransform>();
        _rtHpSliderBarrier = _goCanvas.transform.Find("TransformCenter").Find("HpSlider").Find("FillerBarrier").GetComponent<RectTransform>();
        _sphereCollider = _goComps.transform.Find("SphereCollider").GetComponent<SphereCollider>();
        _srColliderArea = _goComps.transform.Find("ColliderArea").GetComponent<SpriteRenderer>();
        _srMovableArea = _goComps.transform.Find("MovableArea").GetComponent<SpriteRenderer>();
        _modelTransform = transform.Find("TransformAdjuster").transform;
        _txDamagePopup = _goCanvas.transform.Find("TransformCenter").Find("DamagePopup").GetComponent<TextMeshProUGUI>();
        _txStatusCount = _goCanvas.transform.Find("TransformCenter").Find("StatusIcons").GetComponentsInChildren<TextMeshProUGUI>(true);
        _srColliderArea.gameObject.SetActive(false);
        _outline = (GetComponent<Outline>() is Outline) ? GetComponent<Outline>() : gameObject.AddComponent<Outline>();

        _srColliderArea.gameObject.SetActive(true);
        _srMovableArea.gameObject.SetActive(false);
        _outline.enabled = false;

        //_imHpSliderMain.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
        //_rtCanvasPos.transform.Find("StatusIcons").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 65);

        _animator.speed = Globals.Instance.gameSpeed;
        _outline.OutlineWidth = 5f;

        _RecomputeComponents();
    }

    public virtual void OnDestroy()
    {
        Globals.unitList.Remove(this);
    }

    public void _ApplyLevel()
    {
        if (_OriginalUnit == null) return;

        _parameter._hpMax = _parameter._hpMax + (_OriginalUnit._parameter._hpPerLevel * (_parameter._lv - 1)).ToInt();
        _parameter._ad = _parameter._ad + (_OriginalUnit._parameter._adPerLevel * (_parameter._lv - 1)).ToInt();
        _parameter._ar = _parameter._ar + (_OriginalUnit._parameter._arPerLevel * (_parameter._lv - 1)).ToInt();
        _parameter._md = _parameter._md + (_OriginalUnit._parameter._mdPerLevel * (_parameter._lv - 1)).ToInt();
        _parameter._mr = _parameter._mr + (_OriginalUnit._parameter._mrPerLevel * (_parameter._lv - 1)).ToInt();
        _parameter._sp = _parameter._sp + (_OriginalUnit._parameter._spPerLevel * (_parameter._lv - 1)).ToInt();
    }

    public void _ApplyEquips()
    {
        for (int i = 0; i < _parameter._equips.Length; i++)
        {
            if (_parameter._equips[i] == null) continue;
            if (_parameter._equips[i] == default) continue;
            if (_parameter._equips[i]._name.IsNullOrEmpty()) continue;

            _Equip equip_i_ = _parameter._equips[i];

            _parameter._hpMax += equip_i_._hpPlus;
            _parameter._ad += equip_i_._adPlus;
            _parameter._ar += equip_i_._arPlus;
            _parameter._md += equip_i_._mdPlus;
            _parameter._mr += equip_i_._mrPlus;
            _parameter._sp += equip_i_._spPlus;

            if (equip_i_._passive is _Skill passive_i_ && passive_i_._parameter._name.IsNullOrEmpty() == false)
            {
                bool isAddPassive_ = true;
                foreach (_Skill passive_j_ in _parameter._additionalPassives)
                {
                    if (passive_i_._parameter._isUnique && passive_j_._parameter._isUnique && passive_i_._parameter._name == passive_j_._parameter._name)
                        isAddPassive_ = false;
                }

                if (isAddPassive_)
                    _parameter._additionalPassives.Add(equip_i_._passive);
            }
            if (equip_i_._active is _Skill active_i_ && active_i_._parameter._name.IsNullOrEmpty() == false)
            {
                for (int j = 0; j < i; j++)
                {
                    if (_parameter._equips[j] == null || _parameter._equips[j] == default || _parameter._equips[j]._name.IsNullOrEmpty()) continue;

                    if (_parameter._equips[j]._active is _Skill active_j_ && active_j_._parameter._name.IsNullOrEmpty() == false)
                    {
                        if (active_i_._parameter._isUnique && active_j_._parameter._isUnique && active_i_._parameter._name == active_j_._parameter._name)
                        {
                            _parameter._equips[i]._castableLimitCount = 0;
                        }
                    }
                }
            }
        }
    }

    public void _ApplyBuff()
    {
        //float coef00_ = (Globals.Instance.globalEffectList.Find(m => m._parameter._tags.Contains("Hunger")) == null) ? 1f : 0.5f;

        //if (_parameter._hpBuff > 0) _parameter._hpApplied = (_parameter._hp * (100 + _parameter._hpBuff) / 100f).ToInt();
        //if (_parameter._hpBuff < 0) _parameter._hpApplied = (_parameter._hp * 100f / (100 - _parameter._hpBuff)).ToInt();

        if (_parameter._adBuff >= 0) _parameter._adApplied = (_parameter._ad * (100 + _parameter._adBuff) / 100f).ToInt();
        else/*         _adBuff <  0*/_parameter._adApplied = (_parameter._ad * 100f / (100 - _parameter._adBuff)).ToInt();

        if (_parameter._arBuff >= 0) _parameter._arApplied = (_parameter._ar * (100 + _parameter._arBuff) / 100f).ToInt();
        else/*         _arBuff <  0*/_parameter._arApplied = (_parameter._ar * 100f / (100 - _parameter._arBuff)).ToInt();

        if (_parameter._mdBuff >= 0) _parameter._mdApplied = (_parameter._md * (100 + _parameter._mdBuff) / 100f).ToInt();
        else/*         _mdBuff <  0*/_parameter._mdApplied = (_parameter._md * 100f / (100 - _parameter._mdBuff)).ToInt();

        if (_parameter._mrBuff >= 0) _parameter._mrApplied = (_parameter._mr * (100 + _parameter._mrBuff) / 100f).ToInt();
        else/*         _mrBuff <  0*/_parameter._mrApplied = (_parameter._mr * 100f / (100 - _parameter._mrBuff)).ToInt();

        if (_parameter._spBuff >= 0) _parameter._spApplied = (_parameter._sp * (100 + _parameter._spBuff) / 100f).ToInt();
        else/*         _spBuff <  0*/_parameter._spApplied = (_parameter._sp * 100f / (100 - _parameter._spBuff)).ToInt();

        foreach (_Skill skill_i_ in Globals.Instance.globalEffectList)
        {
            if (skill_i_._parameter._triggerTiming?.Contains("OnApplyBuff") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, skill_i_, null, "OnApplyBuff"));
        }
                                                                                       
        for (int i = 0; i < _parameter._classPassives.Count; i++)
        {
            if (_parameter._classPassives[i]._parameter._triggerTiming?.Contains("OnApplyBuff") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, _parameter._classPassives[i], null, "OnApplyBuff"));
        }

        for (int i = 0; i < _parameter._additionalPassives.Count; i++)
        {
            if (_parameter._additionalPassives[i]._parameter._triggerTiming?.Contains("OnApplyBuff") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, _parameter._additionalPassives[i], null, "OnApplyBuff"));
        }

        _parameter._movableRange = (float)_parameter._spApplied / 10;
        _srMovableArea.transform.localScale = new Vector3(_parameter._movableRange * 2, _parameter._movableRange * 2, 1);
        _DisplayBuffAndStatusIcon();
    }

    public void _ApplyPassive_Static()
    {
        List<_Skill> classPassives_ = new List<_Skill>(_parameter._classPassives);
        classPassives_.Sort((a, b) => a._parameter._applyOrder - b._parameter._applyOrder);
        foreach (_Skill skill_i_ in classPassives_)
        {
            if (skill_i_._parameter._triggerTiming?.Contains("Static") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, skill_i_, null, "Static"));
        }

        List<_Skill> additionalPassives_ = new List<_Skill>(_parameter._additionalPassives);
        additionalPassives_.Sort((a, b) => a._parameter._applyOrder - b._parameter._applyOrder);
        foreach (_Skill skill_i_ in additionalPassives_)
        {
            if (skill_i_._parameter._triggerTiming?.Contains("Static") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, skill_i_, null, "Static"));
        }
    }

    public void _Barrier_Gain()
    {
        GameObject Barrier = Instantiate(Prefabs.goParticles.Find(m => m.name == "Barrier_Orange"), _posCenter, Quaternion.identity);
        Barrier.name = "Barrier";
        Barrier.transform.localScale = (0.15f * _charaHeight + 0.12f).ToVector3();
        Barrier.transform.SetParent(_goComps.transform);
    }

    public void _Barrier_Lost()
    {
        foreach (Transform child_i_ in _goComps.transform)
        {
            if (child_i_.name == "Barrier")
            {
                Destroy(child_i_.gameObject);
            }
        }
    }

    public bool[] _CalclateSkillPointRemaining()
    {
        bool[] result = new bool[3] { (_parameter._lv + 2) / 3 > 0, (_parameter._lv + 2) / 3 > 1, (_parameter._lv + 2) / 3 > 2 };

        foreach (_Skill skill_i_ in _parameter._skillTree)
        {
            if (skill_i_._parameter._isActive)
                result[skill_i_._parameter._rank - 1] = false;
        }
        return result;
    }

    public static _Unit CloneFromString(string className_)
    {
        _Unit result_ = default;

        if (className_ == "Random Hero")
        {
            foreach (_Unit unit_i_ in Globals.originalUnitsList.Shuffle(Globals.Instance.randomOnBattle))
            {
                if (unit_i_._parameter._unitType == "Hero")
                {
                    result_ = Instantiate(unit_i_, _GoUnits.transform);
                    result_.gameObject.SetActive(true);
                    result_._OriginalUnit = unit_i_;
                    result_.name = unit_i_._parameter._class;

                    return result_;
                }
            }
        }
        else if (className_ == "Random Enemy")
        {
            foreach (_Unit unit_i_ in Globals.originalUnitsList.Shuffle(Globals.Instance.randomOnBattle))
            {
                if (unit_i_._parameter._unitType == "Enemy")
                {
                    result_ = Instantiate(unit_i_, _GoUnits.transform);
                    result_.gameObject.SetActive(true);
                    result_._OriginalUnit = unit_i_;
                    result_.name = unit_i_._parameter._class;

                    return result_;
                }
            }
        }

        foreach (_Unit unit_i_ in Globals.originalUnitsList)
        {
            if (unit_i_._parameter._class == className_)
            {
                result_ = Instantiate(unit_i_, _GoUnits.transform);
                result_.gameObject.SetActive(true);
                result_._OriginalUnit = unit_i_;
                result_.name = unit_i_._parameter._class;

                return result_;
            }
        }

        Debug.LogError("Invalid className : " + className_);
        return result_;
    }

    public static _Unit CloneFromTableData(string[] data_)
    {
        if (data_.Length < 1) return default;

        _Unit result_ = default;
        string class_ = (data_[0] == "Random Object") ? Table.ObjectList["Object"].GetRandom(Globals.Instance.randomOnBattle) :
                                                        data_[0];

        foreach (_Unit unit_i_ in Globals.originalUnitsList)
        {
            if (class_ == unit_i_._parameter._class)
            {
                result_ = Instantiate(unit_i_, _GoUnits.transform);
                result_.gameObject.SetActive(true);
                result_._OriginalUnit = unit_i_;
                result_.name = unit_i_._parameter._class;
                break;
            }

            if (unit_i_ == Globals.originalUnitsList.Last())
            {
                Debug.LogError("Invalid class name : " + class_);
                return result_;
            }
        }

        foreach (string datum_ in data_)
        {
            if (datum_.Contains("Pos:"))
                result_._posSOB = datum_.Replace("Pos:", "").ToVector3();

            if (datum_.Contains("PosDist:"))
                result_._posDistSOB = datum_.Replace("PosDist:", "").ToVector3();

            if (datum_.Contains("Qrt:"))
                result_._qrtSOB = Quaternion.Euler(datum_.Replace("Qrt:", "").ToVector3());

            if (datum_.Contains("Entrance:"))
                result_._parameter._entranceType = datum_.Replace("Entrance:", "");
        }

        return result_;
    }

    public void _CopyStatusFromOriginal()
    {
        if (_OriginalUnit == null) return;

        _parameter._hpMax = _OriginalUnit._parameter._hpMax;
        _parameter._ad = _OriginalUnit._parameter._ad;
        _parameter._ar = _OriginalUnit._parameter._ar;
        _parameter._md = _OriginalUnit._parameter._md;
        _parameter._mr = _OriginalUnit._parameter._mr;
        _parameter._sp = _OriginalUnit._parameter._sp;
        _parameter._hpBuff = _OriginalUnit._parameter._hpBuff;
        _parameter._adBuff = _OriginalUnit._parameter._adBuff;
        _parameter._arBuff = _OriginalUnit._parameter._arBuff;
        _parameter._mdBuff = _OriginalUnit._parameter._mdBuff;
        _parameter._mrBuff = _OriginalUnit._parameter._mrBuff;
        _parameter._spBuff = _OriginalUnit._parameter._spBuff;

        foreach (_Parameter._StatusCondition statusCondition_i_ in _parameter._statusConditions)
        {
            statusCondition_i_._count = 0;
        }

        if (this is _Hero)
            _parameter._unitTypesThroughable = new List<string> { "Hero" };
        else
            _parameter._unitTypesThroughable = new List<string> { "Enemy" };

        _SetClassParameter();
    }

    public static void DealDamage(_Unit unitSelf_, _Unit unitToBeAttacked_, int value_, _Skill skill_, string type_)
    {
        if (unitToBeAttacked_._IsAlive() == false) return;
        if (unitToBeAttacked_?._parameter._statusConditions.Find(m => m._name == "Protection")._count > 0)
        {
            unitToBeAttacked_._parameter._statusConditions.Find(m => m._name == "Protection")._count--;
            unitToBeAttacked_._SetAnimatorCondition();
        }
        if (value_ < 1) return;
        if (type_ != "Physical" && type_ != "Magic" && type_ != "Static") { Debug.Log("Invalid type : " + type_); return; }
        if (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Golden Armor") is _Skill out_ && unitToBeAttacked_._parameter._unitType == "Hero" && out_._parameter._iCount-- > 0)
        {
            value_ = 0;
            return;
        }

        Globals.triggerStateBasedAction = true;

        unitToBeAttacked_._damagePopup = unitToBeAttacked_._damagePopup + value_;
        unitToBeAttacked_.StartCoroutine(unitToBeAttacked_._PopupDamage());

        for (int i = 0; i < unitSelf_?._parameter._classPassives.Count; i++)
        {
            _Skill passive_i_ = unitSelf_._parameter._classPassives[i];

            if (passive_i_._parameter._triggerTiming?.Contains("OnDealDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnDealDamage", value_, skill_));

            if (type_ == "Physical" && passive_i_._parameter._triggerTiming?.Contains("OnDealPhysicalDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnDealPhysicalDamage", value_, skill_));

            if (type_ == "Magic" && passive_i_._parameter._triggerTiming?.Contains("OnDealMagicDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnDealMagicDamage", value_, skill_));
                
            if (unitToBeAttacked_?._parameter._unitType == "Enemy" && unitToBeAttacked_._parameter._hp < value_ && passive_i_._parameter._triggerTiming?.Contains("OnKillEnemy") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnKillEnemy", value_, skill_));
        }
        for (int i = 0; i < unitSelf_?._parameter._additionalPassives.Count; i++)
        {
            _Skill passive_i_ = unitSelf_._parameter._additionalPassives[i];

            if (passive_i_._parameter._triggerTiming?.Contains("OnDealDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnDealDamage", value_, skill_));

            if (type_ == "Physical" && passive_i_._parameter._triggerTiming?.Contains("OnDealPhysicalDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnDealPhysicalDamage", value_, skill_));

            if (type_ == "Magic" && passive_i_._parameter._triggerTiming?.Contains("OnDealMagicDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnDealMagicDamage", value_, skill_));

            if (unitToBeAttacked_?._parameter._unitType == "Enemy" && unitToBeAttacked_._parameter._hp < value_ && passive_i_._parameter._triggerTiming?.Contains("OnKillEnemy") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, passive_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnKillEnemy", value_, skill_));
        }

        for (int i = 0; i < unitToBeAttacked_?._parameter._classPassives.Count; i++)
        {
            _Skill passive_i_ = unitToBeAttacked_._parameter._classPassives[i];

            if (passive_i_._parameter._triggerTiming?.Contains("OnTakenDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitToBeAttacked_, passive_i_, new List<_Unit>() { unitSelf_ }, "OnTakenDamage", value_, skill_));

            if (type_ == "Physical" && passive_i_._parameter._triggerTiming?.Contains("OnTakenPhysicalDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitToBeAttacked_, passive_i_, new List<_Unit>() { unitSelf_ }, "OnTakenPhysicalDamage", value_, skill_));

            if (type_ == "Magic" && passive_i_._parameter._triggerTiming?.Contains("OnTakenMagicDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitToBeAttacked_, passive_i_, new List<_Unit>() { unitSelf_ }, "OnTakenMagicDamage", value_, skill_));
        }
        for (int i = 0; i < unitToBeAttacked_?._parameter._additionalPassives.Count; i++)
        {
            _Skill passive_i_ = unitToBeAttacked_._parameter._additionalPassives[i];

            if (passive_i_._parameter._triggerTiming?.Contains("OnTakenDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitToBeAttacked_, passive_i_, new List<_Unit>() { unitSelf_ }, "OnTakenDamage", value_, skill_));

            if (type_ == "Physical" && passive_i_._parameter._triggerTiming?.Contains("OnTakenPhysicalDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitToBeAttacked_, passive_i_, new List<_Unit>() { unitSelf_ }, "OnTakenPhysicalDamage", value_, skill_));

            if (type_ == "Magic" && passive_i_._parameter._triggerTiming?.Contains("OnTakenMagicDamage") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitToBeAttacked_, passive_i_, new List<_Unit>() { unitSelf_ }, "OnTakenMagicDamage", value_, skill_));
        }

        foreach (_Skill skill_i_ in new List<_Skill>(Globals.Instance.globalEffectList))
        {
            if (unitSelf_ != null && unitToBeAttacked_?._parameter._unitType == "Enemy" && unitToBeAttacked_._parameter._hp < value_ && skill_i_._parameter._triggerTiming?.Contains("OnKillEnemy") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, skill_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnKillEnemy", value_, skill_));

            if (unitSelf_ != null && unitToBeAttacked_?._parameter._unitType == "Object" && unitToBeAttacked_._parameter._hp < value_ && skill_i_._parameter._triggerTiming?.Contains("OnDestroyObject") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(unitSelf_, skill_i_, new List<_Unit>() { unitToBeAttacked_ }, "OnDestroyObject", value_, skill_));
        }

        if (value_ > unitToBeAttacked_._BarrierValue)
        {
            value_ -= unitToBeAttacked_._BarrierValue;
            unitToBeAttacked_._BarrierValue = 0;
        }
        else
        {
            unitToBeAttacked_._BarrierValue -= value_;
            value_ = 0;
        }

        if (unitSelf_?._parameter._statusConditions.Find(m => m._name == "Fury")._count > 0)
        {
            unitSelf_._parameter._statusConditions.Find(m => m._name == "Fury")._count--;
            unitSelf_._SetAnimatorCondition();
        }

        unitToBeAttacked_._LoseHp(value_, skill_);
        Instantiate(Prefabs.goParticles.Find(m => m.name == "DamageEffect"), unitToBeAttacked_._goComps_Y5.transform, false);
        if (value_ > 0)
            unitToBeAttacked_._animator.SetTrigger("triggerGetHit");
    }

    public void _DealDamage(_Unit unitToBeAttacked_, int value_, _Skill  skill_, string type_)
    {
        DealDamage(this, unitToBeAttacked_, value_, skill_, type_);
    }

    public void _DecayBuffStatus(int value_ = 10)
    {
        _parameter._hpBuff = _parameter._hpBuff.ReduceAbsValue(value_);
        _parameter._adBuff = _parameter._adBuff.ReduceAbsValue(value_);
        _parameter._arBuff = _parameter._arBuff.ReduceAbsValue(value_);
        _parameter._mdBuff = _parameter._mdBuff.ReduceAbsValue(value_);
        _parameter._mrBuff = _parameter._mrBuff.ReduceAbsValue(value_);
        _parameter._spBuff = _parameter._spBuff.ReduceAbsValue(value_);

        _ApplyBuff();
    }

    public void _DecayDisable(int value_)
    {
        foreach (_Parameter._StatusCondition statusCondition_i_ in _parameter._statusConditions)
        {
            if (statusCondition_i_._isDecreaseEndOfTurn == false) continue;
            statusCondition_i_._count = statusCondition_i_._count.ReduceAbsValue(value_);
        }

        _SetAnimatorCondition();
    }

    public void _RemoveDisable()
    {
        foreach (_Parameter._StatusCondition statusCondition_i_ in _parameter._statusConditions)
        {
            statusCondition_i_._count = 0;
        }

        _SetAnimatorCondition();
    }

    public void _DisplayBuffAndStatusIcon()
    {
        int index_ = 0;
        int indexMax_ = _imStatusIcons.Length;

        if (_BarrierValue > 0)
        {
            ConfigureIcon("", "StatusIcon/Barrier");
        }

        foreach (_Parameter._StatusCondition statusCondition_i_ in _parameter._statusConditions)
        {
            if (statusCondition_i_._count > 0)
                ConfigureIcon("", "StatusIcon/" + statusCondition_i_._name);
        }

        List<int> buffValuesList_ = new List<int> { _parameter._hpBuff, _parameter._adBuff, _parameter._arBuff, _parameter._mdBuff, _parameter._mrBuff, _parameter._spBuff, };
        List<string> buffNamesList_ = new List<string> { "HP", "AD", "AR", "MD", "MR", "SP" };
        for (int i = 0; i < buffValuesList_.Count; i++)
        {
            if (buffValuesList_[i] == 0) continue;

            ConfigureIcon(buffValuesList_[i].Abs().ToString(), "StatusIcon/" + buffNamesList_[i] + ((buffValuesList_[i] > 0) ? " Buff" : " Debuff"));
        }

        for (int i = index_; i < indexMax_; i++)
        {
            _imStatusIcons[i].gameObject.SetActive(false);
        }

        void ConfigureIcon(string text_, string iconPath_)
        {
            if (index_ >= indexMax_) return;

            _imStatusIcons[index_].gameObject.SetActive(true);
            _imStatusIcons[index_].sprite = Resources.Load<Sprite>(iconPath_);
            _txStatusCount[index_].text = text_;
            index_++;
        }
    }

    public void _GainExp(int exp_)
    {
        _parameter._exp += exp_;
        _parameter._expFraction += exp_;

        for (int i = 0; i < 99; i++)
        {
            if (_parameter._expFraction < Table.ExpTable[_parameter._class][_parameter._lv]) break;

            _parameter._expFraction -= Table.ExpTable[_parameter._class][_parameter._lv];
            _parameter._lv += 1;
        }
    }

    public void _GainBuff(string type_, int value_)
    {
        if (_parameter._statusConditions.Find(m => m._name == "Artifact")._count > 0 && value_ < 0)
        {
            _parameter._statusConditions.Find(m => m._name == "Artifact")._count--;
            return;
        }

        if (value_ > 0 && Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Force of Nature") is _Skill && _parameter._unitType == "Hero") value_ += 20;
        if (value_ < 0 && Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Binding Grasp") is _Skill && _parameter._unitType == "Enemy") value_ -= 20;

        if (type_.ToUpper() == "HP")
        {
            _parameter._hpBuff = (_parameter._hpBuff + value_).Clamp(-100, 100);
        }
        else if (type_.ToUpper() == "AD")
        {
            _parameter._adBuff = (_parameter._adBuff + value_).Clamp(-100, 100);
        }
        else if (type_.ToUpper() == "AR")
        {
            _parameter._arBuff = (_parameter._arBuff + value_).Clamp(-100, 100);
        }
        else if (type_.ToUpper() == "MD")
        {
            _parameter._mdBuff = (_parameter._mdBuff + value_).Clamp(-100, 100);
        }
        else if (type_.ToUpper() == "MR")
        {
            _parameter._mrBuff = (_parameter._mrBuff + value_).Clamp(-100, 100);
        }
        else if (type_.ToUpper() == "SP")
        {
            _parameter._spBuff = (_parameter._spBuff + value_).Clamp(-100, 100);
        }

        for (int i = 0; i < _parameter._classPassives.Count; i++)
        {
            if (_parameter._classPassives[i]._parameter._triggerTiming?.Contains("OnTakeBuffOrDisable") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, _parameter._classPassives[i], null, refTiming_: "OnTakenBuffOrDisable"));
        }
        for (int i = 0; i < _parameter._additionalPassives.Count; i++)
        {
            if (_parameter._additionalPassives[i]._parameter._triggerTiming?.Contains("OnTakeBuffOrDisable") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, _parameter._additionalPassives[i], null, refTiming_: "OnTakenBuffOrDisable"));
        }

        _ApplyBuff();
        _DisplayBuffAndStatusIcon();
    }

    public void _GainBuff(List<string> types_, int value_)
    {
        foreach (string type_i_ in types_)
        {
            _GainBuff(type_i_, value_);
        }
    }

    public void _GainStatus(string name_, int duration_)
    {
        if (_parameter._statusConditions.Find(m => m._name == "Artifact")._count > 0 && _parameter._statusConditions.Find(m => m._name == name_)._type == "Bad")
        {
            _parameter._statusConditions.Find(m => m._name == "Artifact")._count--;
            return;
        }

        foreach (_Parameter._StatusCondition statusCondition_i_ in _parameter._statusConditions)
        {
            if (statusCondition_i_._name == name_)
            {
                statusCondition_i_._count += duration_;
                break;
            }
        }

        for (int i = 0; i < _parameter._classPassives.Count; i++)
        {
            if (_parameter._classPassives[i]._parameter._triggerTiming?.Contains("OnTakenBuffOrDisable") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, _parameter._classPassives[i], null, "OnTakenBuffOrDisable"));
        }
        for (int i = 0; i < _parameter._additionalPassives.Count; i++)
        {
            if (_parameter._additionalPassives[i]._parameter._triggerTiming?.Contains("OnTakenBuffOrDisable") == false) continue;
            General.Instance.StartCoroutine(Battle.ActivatePassive(this, _parameter._additionalPassives[i], null, "OnTakenBuffOrDisable"));
        }

        _SetAnimatorCondition();
        _DisplayBuffAndStatusIcon();
    }

    public static void Heal(_Unit unitSelf_, _Unit unitToHealed_, int value_, _Skill skill_)
    {
        if (unitToHealed_._IsAlive() == false) return;
        if (value_ < 1) return;

        Globals.triggerStateBasedAction = true;

        unitToHealed_._RestoreHp(value_, skill_);

        Instantiate(Prefabs.goParticles.Find(m => m.name == "Healing"), unitToHealed_._goComps.transform, false);
    }

    public void _Heal(_Unit unitToHealed_, int value_, _Skill skill_)
    {
        Heal(this, unitToHealed_, value_, skill_);
    }

    public void _InitializeLevel()
    {
        _parameter._lv = 1;
        _parameter._exp = 0;
        _parameter._expFraction = 0;
    }

    public bool _IsAlive()
    {
        return (_parameter._hp > 0);
    }

    public bool _IsTargetable()
    {
        if (_IsAlive() == false) return false;

        return true;
    }

    public bool _IsHitable()
    {
        if (_IsAlive() == false) return false;

        return true;
    }

    public bool _IsActavle()
    {
        if (_parameter._statusConditions.Find(m => m._name == "Stun")._count > 0) return false;
        if (_parameter._actableCount < 1) return false;
        return true;
    }

    public bool _IsMovable()
    {
        if (_parameter._statusConditions.Find(m => m._name == "Stun")._count > 0) return false;
        if (_parameter._movableCount < 1) return false;
        return true;
    }

    public bool _IsCastableThisSkill(_Skill skill_)
    {
        if (skill_ == null || skill_ == default) return false;
        if (skill_._parameter._cooldownRemaining > 0) return false;
        if (skill_._parameter._castableStacks < 1) return false;
        if (_parameter._actableCount > 0) return true;
        if (skill_._parameter._isQuickCast == true) return true;

        return false;
    }

    public bool _IsMovableToThisPos(Vector3 posToMove_, List<string> unitTyepsThroughable_)
    {
        if (posToMove_.IsInThisCuboid(Globals.Instance.fieldPosMin, Globals.Instance.fieldPosMax) == false) return false;

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_ == this) continue;
            //if (unit_i_._IsAlive() == false && unit_i_.gameObject.activeSelf == false) continue;
            if (unit_i_._IsOverlapColliderArea(this, posToMove_)) return false;
            if (_parameter._statusConditions.Find(m => m._name == "Stealth")._count > 0) continue;
            if (unit_i_._parameter._tags.Contains("Throughable")) continue;
            //if (_parameter._stealthCount > 0) continue;
            if (unit_i_._IsOnPath(transform.position, posToMove_) && unitTyepsThroughable_.Contains(unit_i_._parameter._unitType) == false &&
                unit_i_._parameter._statusConditions.Find(m => m._name == "Stealth")._count == 0) return false;
        }
        return true;
    }

    public bool _IsOnPath(Vector3 posStart_, Vector3 posEnd_)
    {
        Vector3 vector_ = posEnd_ - posStart_;
        if (_parameter._hp < 1) return false;
        if (vector_ == Vector3.zero) return false;

        return _sphereCollider.Raycast(new Ray(posStart_, vector_), out RaycastHit hit_, vector_.magnitude);
    }

    public bool _IsOverlapColliderArea(_Unit unit_, Vector3 posOnMove_)
    {
        if (gameObject.activeSelf == false) return false;

        return (transform.position - posOnMove_).sqrMagnitude < (_parameter._colliderRange + unit_._parameter._colliderRange).Square() ;
    }

    public void _LateUpdate()
    {
        _posCenter = transform.position + new Vector3(0f, _charaHeight / 2, 0f);
        _rtCanvasPos.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, _charaHeight, 0f));

        if (Physics.Raycast(Camera.main.transform.position, _posCenter - Camera.main.transform.position, out RaycastHit hit, Mathf.Infinity/*, 1 << 11*/))
        {
            _goComps_Y5.transform.position = hit.point;
        }

        _LateUpdateHpSlider();
    }

    public void _LateUpdateHpSlider()
    {
        _timeLeftToUpdateHpSlider = (_timeLeftToUpdateHpSlider - Globals.timeDeltaFixed).Clamp(-1, 10);
        if (_timeLeftToUpdateHpSlider < 0)
        {
            _parameter._hpToSlide = (_parameter._hpToSlide - (_parameter._hpMax * Globals.timeDeltaFixed).ToInt().Clamp(1, _parameter._hpMax)).Clamp(0, _parameter._hpMax);
        }

        int valueMax_ = (_parameter._hpOnView + _parameter._hpToSlide + _BarrierValue).Clamp(_parameter._hpMax, 99999);

        float hpNormalized_ = ((float)_parameter._hpOnView / valueMax_).Clamp(0, 1);
        float hpToLoseNormalized_ = ((float)(_parameter._hpOnView + _BarrierValue - _parameter._hpToLose) / valueMax_).Clamp(0, 1);
        float hpToRestoreNormalized_ = ((float)(_parameter._hpOnView + _parameter._hpToRestore) / valueMax_).Clamp(0, 1);
        float hpToSlideNormalized_ = ((float)(_parameter._hpOnView + _parameter._hpToSlide) / valueMax_).Clamp(0, 1);
        float hpBarrierNormalized = ((float)(_parameter._hpOnView + _parameter._hpToSlide + _BarrierValue) / valueMax_).Clamp(0, 1);

        _rtHpSliderMain.StretchToThisAnchor(new Vector2(0, 0), new Vector2(hpToLoseNormalized_, 1));
        _rtHpSliderToLose.StretchToThisAnchor(new Vector2(hpToLoseNormalized_, 0), new Vector2(hpBarrierNormalized, 1));
        _rtHpSliderToRestore.StretchToThisAnchor(new Vector2(hpNormalized_, 0), new Vector2(hpToRestoreNormalized_, 1));
        _rtHpSliderToSlide.StretchToThisAnchor(new Vector2(hpNormalized_, 0), new Vector2(hpToSlideNormalized_, 1));
        _rtHpSliderBarrier.StretchToThisAnchor(new Vector2(hpToSlideNormalized_, 0), new Vector2(hpBarrierNormalized, 1));
    }

    public void _LoadParameter(_Parameter parameter_)
    {
        //_Parameter temp_ = _parameter.DeepCopy();
        _parameter = parameter_.DeepCopy();

        transform.position = new Vector3(_parameter._p_x, _parameter._p_y, _parameter._p_z);
        _modelTransform.rotation = new Quaternion(_parameter._r_x, _parameter._r_y, _parameter._r_z, _parameter._r_w);
        _LateUpdate();

        if (_BarrierValue > 0)
            _Barrier_Gain();

        for (int i_ = 0; i_ < _parameter._skills.Length; i_++)
        {
            if (_parameter._skills[i_]._parameter._name.IsNullOrEmpty()) continue;
            _parameter._skills[i_] = _Skill.CloneFromString(_parameter._skills[i_]._parameter._name);
            _parameter._skills[i_]._parameter = parameter_._skills[i_]._parameter.DeepCopy();
        }

        for (int i_ = 0; i_ < _parameter._classPassives.Count; i_++)
        {
            _parameter._classPassives[i_] = _Skill.CloneFromString(_parameter._classPassives[i_]._parameter._name);
            _parameter._classPassives[i_]._parameter = parameter_._classPassives[i_]._parameter.DeepCopy();
        }
        for (int i_ = 0; i_ < _parameter._additionalPassives.Count; i_++)
        {
            _parameter._additionalPassives[i_] = _Skill.CloneFromString(_parameter._additionalPassives[i_]._parameter._name);
            _parameter._additionalPassives[i_]._parameter = parameter_._additionalPassives[i_]._parameter.DeepCopy();
        }

        for (int i_ = 0; i_ < _parameter._equips.Length; i_++)
        {
            if (_parameter._equips[i_] == null || _parameter._equips[i_] == default) continue;
            if (_Equip.CloneFromString(_parameter._equips[i_]._name) == default) continue;

            _Equip tempEquip_ = _parameter._equips[i_].DeepCopy();
            
            _parameter._equips[i_] = _Equip.CloneFromString(tempEquip_._name);
            _parameter._equips[i_]._castableLimitCount = tempEquip_._castableLimitCount;
            _parameter._equips[i_]._castableStacks = tempEquip_._castableStacks;
            _parameter._equips[i_]._cooldownRemaining = tempEquip_._cooldownRemaining;
        }

        if (_IsAlive())
        {
            _animator.Rebind();
        }
        _SetAnimatorCondition();

        if (this is _Hero)
        {
            if (_IsAlive() == false)
            {
                _animator.Play("DeadPose");
            }
        }
        else
        {
            gameObject.SetActive(_parameter._hp > 0);
        }

        Globals.triggerStateBasedAction = true;
    }

    public void _LoseHp(int value_, _Skill  skill_)
    {
        if (value_ <= 0)
            return;

        UI.ConfigureHeroesUI();
        _parameter._hp = (_parameter._hp - value_).Clamp(0, _parameter._hpMax);
        _parameter._hpToSlide = _parameter._hpToSlide + value_.Clamp(0, _parameter._hpOnView);
        _parameter._hpOnView = (_parameter._hpOnView - value_).Clamp(0, _parameter._hpMax);
        _timeLeftToUpdateHpSlider = Globals.TIME_MAX_TO_UPDATE_HP_SLIDER;
        
        if (_parameter._hp <= 0)
        {
            _animator.SetBool("isDead", true);

            Globals.runningAnimationCount++;
            General.Instance.StartCoroutine(General.DelayForSeconds(_animationTimeToDead / Globals.Instance.gameSpeed, () =>
            {
                Globals.runningAnimationCount--;
            }));
        }
    }

    public IEnumerator _MoveHeroTo(Vector3 posEnd_, bool isSaveParameter_ = true)
    {
        Globals.inputStopperCount++;
        Globals.triggerStateBasedAction = true;

        //List<_Parameter> parameterList_ = new List<_Parameter>();
        if (this is _Hero && isSaveParameter_)
        {
            StartCoroutine(_MoveHeroTo_SaveParameter());
            //foreach (_Unit unit_i_ in Globals.unitList)
            //{
            //    parameterList_.Add(unit_i_._SaveParameter());
            //}

            //Globals.Instance.paramsOnStackList.Add(parameterList_);
            //Globals.Instance.itemsOnStackList.Add(Globals.itemsInBagList.DeepCopy());
        }

        Battle.SetInactiveMoveSuggestion(Globals.unitOnActive);
        _parameter._movableCount -= 1;
        _modelTransform.LookAt(posEnd_);
        _animator.SetBool("isWalk", true);

        yield return StartCoroutine(General.MoveTowards(gameObject, posEnd_, Globals.UNIT_MOVE_PER_SEC));
        _animator.SetBool("isWalk", false);

        Globals.inputStopperCount--;

        IEnumerator _MoveHeroTo_SaveParameter()
        {
            List<_Parameter> parameterList_ = new List<_Parameter>();
            List<Vector3> vector3List_ = new List<Vector3>();
            List<Quaternion> quaternionList_ = new List<Quaternion>();

            foreach (_Unit unit_i_ in Globals.unitList)
            {
                vector3List_.Add(unit_i_.transform.position);
                quaternionList_.Add(unit_i_._modelTransform.rotation);
            }
            for (int i = 0; i < Globals.unitList.Count; i++)
            {
                _Parameter parameter_ = Globals.unitList[i]._SaveParameter();
                parameter_._p_x = vector3List_[i].x;
                parameter_._p_y = vector3List_[i].y;
                parameter_._p_z = vector3List_[i].z;
                parameter_._r_x = quaternionList_[i].x;
                parameter_._r_y = quaternionList_[i].y;
                parameter_._r_z = quaternionList_[i].z;
                parameter_._r_w = quaternionList_[i].w;
                parameterList_.Add(parameter_);
                yield return null;
            }

            Globals.Instance.paramsOnStackList.Add(parameterList_);
            Globals.Instance.itemsOnStackList.Add(Globals.itemsInBagList.DeepCopy());
        }
    }

    public IEnumerator MoveEnemyTo(Vector3 posEnd_, float movePerSec)
    {
        Globals.triggerStateBasedAction = true;
        if ((posEnd_ - transform.position).sqrMagnitude < 0.1f) yield break;

        _animator.SetBool("isWalk", true);
        _modelTransform.LookAt(posEnd_);
        yield return StartCoroutine(General.MoveTowards(gameObject, posEnd_, movePerSec));
        _animator.SetBool("isWalk", false);

        yield return new WaitForSeconds(0.3f);
    }

    public void _PlaceHeroRandomly(int zIndex_ = -1) //ToMod
    {
        Vector3 posStartProv_ = default;
        //Vector3 posSOB_ = default;
        List<String> unitTypeAll = new List<string> { "Hero", "Enemy", "Object" };
        _Random random_ = Globals.Instance.randomOnBattle;

        //if (_parameter._positioningType == "Front") posSOB_ = _posSOB + new Vector3(+2, 0, 0);
        //if (_parameter._positioningType == "Middle") posSOB_ = _posSOB + new Vector3(+0, 0, 0);
        //if (_parameter._positioningType == "Back") posSOB_ = _posSOB + new Vector3(-2, 0, 0);

        //if (zIndex_ == 0) posSOB_ = posSOB_ + new Vector3(0, 0, +0);
        //if (zIndex_ == 1) posSOB_ = posSOB_ + new Vector3(0, 0, +2);
        //if (zIndex_ == 2) posSOB_ = posSOB_ + new Vector3(0, 0, -2);

        for (int loop_ = 0; loop_ < 1000; loop_++)
        {
            posStartProv_ = _posSOB + new Vector3(random_.Range(-_posDistSOB.x, _posDistSOB.x), 0, random_.Range(-_posDistSOB.z, _posDistSOB.z));

            if (Battle.DetectUnitsByCircle(posStartProv_, _parameter._colliderRange, unitTypeAll, true).Exclude(this).Count == 0)
                break;

            if (loop_ == 999)
                Debug.LogError("This unit can't be placed to pos : " + _posSOB);
        }
        
        //for (int loop_ = 0; loop_ < 99; loop_++)
        //{
        //    x_ = (_parameter._positioningType == "Front") ?  Globals.Instance.randomOnBattle.Range(-08f, -06f) :
        //         (_parameter._positioningType == "Middle") ? Globals.Instance.randomOnBattle.Range(-10f, -08f) :
        //       /*(_parameter._positioningType == "Back")*/   Globals.Instance.randomOnBattle.Range(-12f, -10f);

        //    z_ = (zIndex_ == 0) ? Globals.Instance.randomOnBattle.Range(-1, +1) :
        //            (zIndex_ == 1) ? Globals.Instance.randomOnBattle.Range(+1, +3) :
        //            (zIndex_ == 2) ? Globals.Instance.randomOnBattle.Range(-3, -1) :
        //            /*          */   Globals.Instance.randomOnBattle.Range(-3, +3);

        //    posStartProv_.Set(x_, 0f, z_);
        //    if (Battle.DetectUnitsByCircle(posStartProv_, _parameter._colliderRange, unitTypeAll, true).Exclude(this).Count == 0)
        //        break;
        //}

        transform.position = posStartProv_;
        _modelTransform.localRotation = _qrtSOB;
    }

    public void _PlaceRandomlyOrDestroy(out bool isDestroy_)
    {
        List<String> unitTypeAll = new List<string> { "Hero", "Enemy", "Object" };

        if (this is _Hero)
        {
            if (_posSOB == Vector3.down)
            {
                _posSOB = new Vector3(-8.0f, 0.0f, 0.0f);
                _posDistSOB = new Vector3(2.0f, 0.0f, 3.5f);
            }
        }
        else
        {
            if (_posSOB == Vector3.down)
            {
                _posSOB = new Vector3(+6.0f, 0.0f, 0.0f);
                _posDistSOB = new Vector3(4.0f, 0.0f, 3.5f);
            }
        }

        for (int loop_ = 0; loop_ < 999; loop_++)
        {
            float x_ = _posSOB.x + Globals.Instance.randomOnBattle.Range(-_posDistSOB.x, _posDistSOB.x);
            float z_ = _posSOB.z + Globals.Instance.randomOnBattle.Range(-_posDistSOB.z, _posDistSOB.z);

            if (x_.IsBetween(Globals.Instance.fieldPosMin.x, Globals.Instance.fieldPosMax.x) == false) continue;
            if (z_.IsBetween(Globals.Instance.fieldPosMin.z, Globals.Instance.fieldPosMax.z) == false) continue;

            if (Battle.DetectUnitsByCircle(new Vector3(x_, 0f, z_), _parameter._colliderRange, unitTypeAll, /*isOnlyAive = */ false).Exclude(this).Count == 0)
            {
                transform.position = new Vector3(x_, 0f, z_);
                isDestroy_ = false;
                return;
            }
        }

        isDestroy_ = true;
        Destroy(gameObject);
        Debug.LogError("This unit can't be placed to pos : " + transform.position);
    }

    public IEnumerator _PopupDamage()
    {
        Vector3 posStart_ = Vector3.zero;
        Vector3 posMiddle00_ = new Vector3(0.0f, 50.0f, 0.0f);
        Vector3 posMiddle01_ = new Vector3(0.0f, 30.0f, 0.0f);
        Vector3 posEnd_ = Vector3.zero;
        Vector3 sizeStart_ = 0.6f.ToVector3();
        Vector3 sizeEnd_ = 1f.ToVector3();

        _txDamagePopup.gameObject.SetActive(true);
        _txDamagePopup.text = _damagePopup.ToString();
        _txDamagePopup.color = new Color32(255, 140, 045, 255);
        _popupCoroutineCount++;

        General.Instance.StartCoroutine(General.DelayForSeconds(2.0f, () =>
        {
            if (this != null && --_popupCoroutineCount <= 0)
                _txDamagePopup.gameObject.SetActive(false);
        }));

        for (float timeSum = 0, timeMax = 0.35f; timeSum < timeMax; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum / timeMax).Clamp(0, 1);

            _txDamagePopup.transform.localPosition = Library.BezierQuadratic(posStart_, posMiddle00_, posEnd_, p_);
            _txDamagePopup.transform.localScale = Library.BezierLiner(sizeStart_, sizeEnd_, p_);

            yield return null;
        }
        for (float timeSum = 0, timeMax = 0.20f; timeSum < timeMax; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum / timeMax).Clamp(0, 1);

            _txDamagePopup.transform.localPosition = Library.BezierQuadratic(posStart_, posMiddle00_, posEnd_, p_);

            yield return null;
        }
        _txDamagePopup.transform.localPosition = posEnd_;
    }

    public IEnumerator PopupHealing()
    {
        Vector3 posStart_ = Vector3.zero;
        Vector3 posMiddle00_ = new Vector3(0.0f, 50.0f, 0.0f);
        Vector3 posMiddle01_ = new Vector3(0.0f, 30.0f, 0.0f);
        Vector3 posEnd_ = Vector3.zero;
        Vector3 sizeStart_ = 0.6f.ToVector3();
        Vector3 sizeEnd_ = 1f.ToVector3();

        _txDamagePopup.gameObject.SetActive(true);
        _txDamagePopup.text = _healingPopup.ToString();
        _txDamagePopup.color = new Color32(50, 255, 000, 255);
        _popupCoroutineCount++;

        General.Instance.StartCoroutine(General.DelayForSeconds(2.0f / Globals.Instance.gameSpeed, () =>
        {
            if (this != null && --_popupCoroutineCount <= 0)
                _txDamagePopup.gameObject.SetActive(false);
        }));

        for (float timeSum = 0, timeMax = 0.35f; timeSum < timeMax; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum / timeMax).Clamp(0, 1);

            _txDamagePopup.transform.localPosition = Library.BezierQuadratic(posStart_, posMiddle00_, posEnd_, p_);
            _txDamagePopup.transform.localScale = Library.BezierLiner(sizeStart_, sizeEnd_, p_);

            yield return null;
        }
        for (float timeSum = 0, timeMax = 0.20f; timeSum < timeMax; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum / timeMax).Clamp(0, 1);

            _txDamagePopup.transform.localPosition = Library.BezierQuadratic(posStart_, posMiddle00_, posEnd_, p_);

            yield return null;
        }
        _txDamagePopup.transform.localPosition = posEnd_;
    }

    public void _RecomputeComponents()
    {
        _parameter._hpOnView = _parameter._hp;
        _parameter._hpToSlide = 0;
        
        _parameter._movableRange = (float)_parameter._spApplied / 10;
        _srMovableArea.transform.localScale = new Vector3(_parameter._movableRange * 2, _parameter._movableRange * 2, 1);
        _srColliderArea.transform.localScale = new Vector3(_parameter._colliderRange * 2, _parameter._colliderRange * 2, 1);
        _sphereCollider.transform.localScale = new Vector3(_parameter._colliderRange * 2, _parameter._colliderRange * 2, 1);
    }

    public void _RestoreHp(int value_, _Skill  skill_)
    {
        if (value_ <= 0) return;

        if (_parameter._unitType == "Hero" && Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Boon Reflection") != null)
            value_ = (value_ * 1.5f).ToInt();

        _parameter._hp = (_parameter._hp + value_).Clamp(0, _parameter._hpMax);

        _parameter._hpOnView = (_parameter._hpOnView + value_).Clamp(0, _parameter._hpMax);
        _parameter._hpToSlide = _parameter._hpToSlide.Clamp(0, _parameter._hpMax - _parameter._hpOnView);
        _healingPopup = _healingPopup + value_;

        StartCoroutine(PopupHealing());
    }

    public _Parameter _SaveParameter()
    {
        _Parameter result_ = _parameter.DeepCopy();

        result_._p_x = transform.position.x;
        result_._p_y = transform.position.y;
        result_._p_z = transform.position.z;

        result_._r_x = _modelTransform.rotation.x;
        result_._r_y = _modelTransform.rotation.y;
        result_._r_z = _modelTransform.rotation.z;
        result_._r_w = _modelTransform.rotation.w;

        return result_;
    }

    public void _SetAnimatorCondition()
    {
        _animator.SetBool("isStunning", _parameter._statusConditions.Find(m => m._name == "Stun")._count > 0);
        _animator.SetBool("isDead", _IsAlive() == false);

        CreateStatusConditionEffect("Protection", _posCenter);
        CreateStatusConditionEffect("Fury", _goComps.transform.position);
        CreateStatusConditionEffect("Artifact", _posCenter);

        if (_skinnedMeshRenderer != null)
            _SetModelAlpha((_parameter._statusConditions.Find(m => m._name == "Stealth")._count <= 0) ? 255 : 120);

        void CreateStatusConditionEffect(string name_, Vector3 pos_)
        {
            if (_parameter._statusConditions.Find(m => m._name == name_) == null) Debug.LogError("Invalid name_ : " + name_);

            if (_parameter._statusConditions.Find(m => m._name == name_)._count > 0)
            {
                if (_goComps.Find(name_) == null)
                {
                    GameObject go_ = Instantiate(Prefabs.goParticles.Find(m => m.name == name_), pos_, Quaternion.identity);
                    go_.name = name_;
                    go_.transform.SetParent(_goComps.transform);
                }
            }
            else
            {
                Destroy(_goComps.Find(name_)?.gameObject);
            }
        }
    }

    public void _SetClassParameter()
    {
        _parameter._classPassives.Clear();
        _parameter._additionalPassives.Clear();

        if (_parameter._class == "Warrior")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Warrior"));
            _parameter._skills[0] = _Skill.CloneFromString("Basic Attack_Warrior");
        }
        else if (_parameter._class == "Hunter")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Hunter"));
            _parameter._skills[0] = _Skill.CloneFromString("Basic Attack_Hunter");
        }
        else if (_parameter._class == "Mage")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Mage"));
            _parameter._skills[0] = _Skill.CloneFromString("Basic Attack_Mage");
        }
        else if (_parameter._class == "Demon")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Demon"));
            _parameter._skills[0] = _Skill.CloneFromString("Demon Attack");
            _parameter._skills[1] = _Skill.CloneFromString("Demonic Frenzy");
            _parameter._skills[2] = _Skill.CloneFromString("Call Wolves");
            _parameter._skills[3] = _Skill.CloneFromString("Savage Smash");
        }
        else if (_parameter._class == "Slime")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Slime"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
        }
        else if (_parameter._class == "Met")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Met"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[1] = _Skill.CloneFromString("Shell Attack");
        }
        else if (_parameter._class == "Wolf")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Wolf"));
            _parameter._skills[0] = _Skill.CloneFromString("Wolf Attack");
            _parameter._skills[1] = _Skill.CloneFromString("Howling");
        }
        else if (_parameter._class == "Golem")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Golem"));
            _parameter._skills[0] = _Skill.CloneFromString("Golem Attack");
            _parameter._skills[1] = _Skill.CloneFromString("Double Punch");
        }
        else if (_parameter._class == "Explosive Bug")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Explosive Bug"));
            _parameter._classPassives.Add(_Skill.CloneFromString("Flying"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
        }
        else if (_parameter._class == "Faerie")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Faerie"));
            _parameter._classPassives.Add(_Skill.CloneFromString("Flying"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.2f;
            _parameter._skills[1] = _Skill.CloneFromString("Inspire");
            _parameter._skills[2] = _Skill.CloneFromString("Protect");
        }
        else if (_parameter._class == "Frightfly")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Frightfly"));
            _parameter._classPassives.Add(_Skill.CloneFromString("Flying"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.2f;
            _parameter._skills[1] = _Skill.CloneFromString("Sting");
            _parameter._actableCountMax = 2;
        }
        else if (_parameter._class == "Fungusa")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Fungusa"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
            _parameter._skills[1] = _Skill.CloneFromString("Blind Spore");
        }
        else if (_parameter._class == "Fungee")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Fungee"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Treant")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Treant"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.6f;
            _parameter._skills[1] = _Skill.CloneFromString("Photosynthesis");
        }
        else if (_parameter._class == "Snowman")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Snowman"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Longtail")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Longtail"));
            _parameter._classPassives.Add(_Skill.CloneFromString("Flying"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Stump")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Stump"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Scorpion")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Scorpion"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Leech")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Leech"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Caterpillar")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Caterpillar"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Venusa")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Venusa"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Egglet")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Egglet"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Sicklus")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Sicklus"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Serpent")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Serpent"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Crystal Guardian")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Crystal Guardian"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Rat")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Rat"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Bee")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Crystal Guardian"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Spider")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Crystal Guardian"));
            _parameter._skills[0] = _Skill.CloneFromString("Slime Attack");
            _parameter._skills[0]._parameter._targetRange = 2.0f;
            _parameter._skills[0]._parameter._delayTimeToDealDamage = 0.3f;
        }
        else if (_parameter._class == "Rock")
        {

        }
        else if (_parameter._class == "Rune Stone")
        {
            _goComps.Find("EffectArea").gameObject.SetActive(true);
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Rune Stone"));
        }
        else if (_parameter._class == "Explosive Crystal")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Explosive Crystal"));
        }
        else if (_parameter._class == "Healing Crystal")
        {
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Healing Crystal"));
        }
        else if (_parameter._class == "Red Gems")
        {
            _parameter._tags.Add("Throughable");
            _parameter._classPassives.Add(_Skill.CloneFromString("Passive_Explosive Crystal"));
        }
        else
        {
            Debug.LogError("Invalid class : " + _parameter._class);
        }
    }

    public void _SetModelAlpha(int a_)
    {
        _skinnedMeshRenderer.material.color = new Color32(255, 255, 255, (byte)a_);
        //_materialToTransparent.color = new Color32(255, 255, 255, (byte)a_);
    }

    public void _SkillTree_Save()
    {
        _parameter._skillTreeSave = _parameter._skillTree.DeepCopy();
        _parameter._skillsSave = _parameter._skills.DeepCopy();
        //    }
        //    public void _Load()
        //    {
        //        if (_skillsArraySave != null)
        //            _skillsArray = _skillsArraySave.DeepCopy();
    }

    public void _SkillTree_Load()
    {
        _parameter._skillTree = _parameter._skillTreeSave.DeepCopy();
        
        for (int i = 0; i < _parameter._skillsSave.Length; i++)
        {
            if (_parameter._skillsSave[i] == null || _parameter._skillsSave[i]._parameter._name.IsNullOrEmpty())
            {
                _parameter._skills[i] = new _Skill();
            }
            else
            {
                _parameter._skills[i]._parameter = _parameter._skillsSave[i]._parameter.DeepCopy();
            }
        }
    }

    public void _Skills_ApplyAbilities(_Skill skill_)
    {
        foreach (_SkillAbility ability_j_ in skill_._parameter._skillAbilities)
        {
            if (ability_j_ == null) continue;
            if (ability_j_._isActive == false) continue;

            ability_j_._functionEffect?.Invoke(this, skill_, ability_j_, "Static");
        }
    }

    //public void _Skills_InitializeAndApplyAbilities()
    //{
    //    //for (int i = 0; i < _parameter._skills.Length; i++)
    //    //{
    //    //    if (_parameter._skills[i] == null) continue;
    //    //    if (_parameter._skills[i]._parameter._name.IsNullOrEmpty()) continue;

    //    //    _Skill skill_i_ = _parameter._skills[i];
    //    //    skill_i_._parameter = _parameter._skillTree[i]._parameter;

    //    //    foreach (_SkillAbility ability_j_ in skill_i_._parameter._skillAbilities)
    //    //    {
    //    //        if (ability_j_ == null) continue;
    //    //        ability_j_._functionEffect?.Invoke(this, skill_i_, ability_j_);
    //    //    }
    //    //}

    //    //foreach (_Skill skill_i_ in _parameter._skills)
    //    //{
    //    //    if (skill_i_ == null) continue;
    //    //    if (skill_i_._parameter._name.IsNullOrEmpty()) continue;

    //    //    _Skill temp_ = skill_i_.DeepCopy();
    //    //    skill_i_._parameter = _parameter._skillTree[1]._parameter;
    //    //    //skill_i_._parameter._skillAbilities = temp_._parameter._skillAbilities.DeepCopy();
    //    //    //skill_i_._parameter._skillAbilitiesTable = temp_._parameter._skillAbilitiesTable.DeepCopy();

    //    //    foreach (_SkillAbility ability_j_ in skill_i_._parameter._skillAbilities)
    //    //    {
    //    //        if (ability_j_ == null) continue;
    //    //        if (ability_j_._name.IsNullOrEmpty()) continue;

    //    //        ability_j_._functionEffect?.Invoke(this, skill_i_, ability_j_);
    //    //    }
    //    //}
    //}
}

public static class ExtansionsUnit
{
    public static List<_Unit> Exclude(this List<_Unit> self, _Unit elemToExclude)
    {
        if (self == null) return null;

        for (int i = self.Count - 1; i >= 0; i--)
        {
            if (self[i] == elemToExclude)
                self.RemoveAt(i);
        }
        return self;
    }

    public static List<_Unit> ExcludeStealth(this List<_Unit> self, string unitTypeNotExclude_)
    {
        if (self == null) return null;

        for (int i = self.Count - 1; i >= 0; i--)
        {
            if (self[i]._parameter._statusConditions.Find(m => m._name == "Stealth")._count > 0 && self[i]._parameter._unitType != unitTypeNotExclude_)
                self.RemoveAt(i);
        }
        return self;
    }
}
