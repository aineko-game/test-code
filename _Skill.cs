using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class _Skill
{
    public _Parameter _parameter = new _Parameter();
    [Serializable]
    public class _Parameter
    {
        public string _name;
        public string _descriptiveName;
        public string _type = "Skill";
        public List<string> _tags = new List<string>();
        public string[] _triggerTiming = new string[0];
        public bool _isUnique = true;
        public string[] _classType = new string[0];
        public int _level = 1;
        public int _rank = 1;
        public int _abilityPoint = 0;
        public int _applyOrder = 0;
        public bool _isShowAsIcon = true;
        public bool _isActive = false;
        public bool _isShowICount = false;
        //public bool _isDestroySelfOnTrigger = false;

        public string _damageType;
        public int _adDamageBase;
        public int _mdDamageBase;
        public int _sdDamageBase;
        public int _restoreValue;
        public int _baseValue;
        public int _iValue;
        public int _iCount;
        public int _hitCount = 1;
        public int _restoreValueBase;
        public int _barrierValueBase;
        public int _cooldownRemaining;
        public int _cooldownDuration;
        public int _stack;
        public int _castableStacks = 99;
        //public List<string> _buffTarget_ = new List<string>();
        public List<int> _targetHeroIndex = new List<int>();
        public List<string> _buffType = new List<string>();
        public List<int> _buffValue = new List<int>();

        public float _damageCoef = 1;

        public float _hpRatio;
        public float _adRatio;
        public float _arRatio;
        public float _mdRatio;
        public float _mrRatio;
        public float _spRatio;
        public float _fValue;
        public float _delayTimeToLaunchBullet;
        public float _delayTimeToDealDamage;
        public float _delayTimeToResolveEffect;
        public float _targetRange;
        public float _hitRange;
        public float _chainRange;
        public float _moveRange;
        public float _knockbackRange;
        public float _angle;
        public float _effectSize = 1.0f;
        public string _string;
        public string _effectColor = "";

        public bool _isQuickCast = false;
        public bool _isCastableWhileDisabled = false;

        public string _animatorSetBool = "";
        public string _animatorSetTrigger = "";
        public string _castType = ""; //"Target", "AOE"
        public List<string> _unitTypesTargetableList;

        public string _pathIcon;

        //public _Skill._Parameter skillTreeParameter_;
        public _SkillAbility[] _skillAbilities = new _SkillAbility[4];
        public List<string> _skillAbilitiesTable = new List<string>();

        //public _Tooltip _miniTooltip = new _Tooltip();
    }

    //public _Equip _equipRef;
    public string _refTiming = "";
    public int _refValue = 0;
    public _Skill _refSkill;
    public string _refType = "";

    public _Skill _originalSkill;
    public _Tooltip _tooltip;
    public _Tooltip _miniTooltip;

    public delegate IEnumerator _FunctionCastSkill(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_);
    public delegate void _FunctionSimulateEffect(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_);
    public delegate void _FunctionDisplaySkillArea(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_);
    public delegate List<_Unit> _FunctionDetectUnits(Vector3 posOnUnit_, Vector3 posOnCursor_, _Skill skill_);
    public delegate List<_Unit> _FunctionFindUnits(Vector3 posOnUnit_, Vector3 posOnCursor_, _Skill skill_);
    public delegate Vector3 _FunctionComputeBestPos(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_);
    public delegate bool _FunctionIsCastThisSkill(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_);
    public _FunctionCastSkill _functionCastSkill;
    public _FunctionSimulateEffect _functionSimulateEffect;
    public _FunctionDisplaySkillArea _functionDisplaySkillArea;
    public _FunctionFindUnits _functionFindUnits;
    public _FunctionDetectUnits _functionDetectUnits;
    public _FunctionComputeBestPos _functionComputeBestPos;
    public _FunctionIsCastThisSkill _functionIsCastThisSkill;

    public static void ActivateSkill(_Unit unit_, _Skill skill_)
    {
        int rank_ = skill_._parameter._rank;

        if ((unit_._parameter._lv + 2) / 3 < rank_) return;
        foreach (_Skill skill_i_ in unit_._parameter._skillTree)
        {
            if (skill_i_._parameter._rank == rank_ && skill_i_._parameter._isActive)
                return;
        }

        unit_._parameter._skills[rank_] = CloneFromString(skill_._parameter._name);
        unit_._parameter._skills[rank_]._parameter = skill_._parameter.DeepCopy();
        //unit_._parameter._skills[rank_]._parameter.skillTreeParameter_ = skill_._parameter.DeepCopy();
        unit_._parameter._skills[rank_]._parameter._isActive = true;
        skill_._parameter._isActive = true;
    }

    //public static void Buy(int shopIndex_, int heroIndex_, int skillIndex_)
    //{
    //    if (shopIndex_.IsBetween(0, Globals.Instance.spotCurrent._shopSkills.Length) == false) return;
    //    if (Globals.Instance.spotCurrent._shopSkills[shopIndex_] == null || Globals.Instance.spotCurrent._shopSkills[shopIndex_]._parameter._name.IsNullOrEmpty()) return;
    //    if (Globals.Instance.spotCurrent._shopSkills[shopIndex_]._parameter._price > Globals.Instance.Gold) return;
    //    if (Globals.Instance.spotCurrent._shopSkills[shopIndex_]._parameter._classType.IsContains(Globals.heroList[heroIndex_]._parameter._class) == false) return;

    //    _Skill shopSkill_ = Globals.Instance.spotCurrent._shopSkills[shopIndex_];

    //    //if (Globals.heroList[heroIndex_]._parameter._skills[skillIndex_] == null || Globals.heroList[heroIndex_]._parameter._skills[skillIndex_]._parameter._name.IsNullOrEmpty())
    //    //{
    //    Globals.heroList[heroIndex_]._parameter._skills[skillIndex_] = shopSkill_;
    //    Globals.Instance.Gold -= Globals.Instance.spotCurrent._shopSkills[shopIndex_]._parameter._price;
    //    Globals.Instance.spotCurrent._shopSkills[shopIndex_] = null;
    //    UI.ConfigureItemsUI();
    //    UI.ConfigureShopUI();
    //    //}
    //}

    public int _CalculateAbilityPointRemaining(_Unit unit_)
    {
        //if (_parameter._isActive == false) return 0;

        int reslut = (unit_._parameter._lv - 3 * _parameter._rank + 2).Clamp(0, 2);

        foreach (_SkillAbility ability_i_ in _parameter._skillAbilities)
        {
            if (ability_i_._isActive == true)
            {
                reslut = (reslut - 1).Clamp(0, 2);
            }
        }

        return reslut;
    }

    public static _Skill CloneFromString(string skillName_)
    {
        if (skillName_.IsNullOrEmpty()) return default;

        _Skill result_ = default;
        
        foreach (_Skill skill_i_ in OriginalSkillList)
        {
            if (skill_i_._parameter._name == skillName_)
            {
                result_ = skill_i_._Clone();
                return result_;
            }
        }
        Debug.LogError("Invalid skill name : " + skillName_);
        return result_;
    }

    public _Skill _Clone()
    {
        _Skill result_ = this.DeepCopy();
        result_._originalSkill = this;

        return result_;
    }

    public bool _IsCastable()
    {
        if (_parameter._cooldownRemaining > 0) return false;
        if (_parameter._castableStacks < 1) return false;

        return true;
    }

    public static readonly List<_Skill> OriginalSkillList = new List<_Skill>()
    {
        new _Skill //#Abyss
        {
            _parameter = new _Parameter
            {
                _name = "Abyss",
                _descriptiveName = "Abyss",
                _triggerTiming = new string[] { "OnCalculateDealDamage" },
                _applyOrder = 1,
                _pathIcon = "SkillIcon/Abyss",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Abyss").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Abyss").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Abyss,
        },
        new _Skill //#Adrenaline
        {
            _parameter = new _Parameter
            {
                _name = "Adrenaline",
                _descriptiveName = "Adrenaline",
                _triggerTiming = new string[] { "OnApplyBuff" },
                _applyOrder = 1,
                _pathIcon = "SkillIcon/Adrenaline",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Adrenaline").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Adrenaline").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Adrenaline,
        },
        new _Skill //#Aegis
        {
            _parameter = new _Parameter
            {
                _name = "Aegis",
                _descriptiveName = "Aegis",
                _triggerTiming = new string[] { "StartOfBattle" },
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Aegis",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Aegis").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Aegis").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Aegis,
        },
        new _Skill //#Assassinate
        {
            _parameter = new _Parameter
            {
                _name = "Assassinate",
                _descriptiveName = "Assassinate",
                _triggerTiming = new string[]{ "OnKillEnemy" },
                _delayTimeToResolveEffect = 0.2f,
                _pathIcon = "SkillIcon/Assassinate",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Assassinate").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Assassinate").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Assassinate,
        },
        new _Skill //#Basic Attack_Warrior
        {
            _parameter = new _Parameter
            {
                _name = "Basic Attack_Warrior",
                _descriptiveName = "Basic Attack",
                _adRatio = 1,
                _delayTimeToDealDamage = 0.4f,
                _targetRange = 2f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "Target",
                _unitTypesTargetableList = new List<string> { "Enemy", "Object" },
                _pathIcon = "SkillIcon/Basic Attack_Warrior",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Basic Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Basic Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.WarriorAttack,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = default,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
            _functionComputeBestPos = default
        },
        new _Skill //#Basic Attack_Hunter
        {
            _parameter = new _Parameter
            {
                _name = "Basic Attack_Hunter",
                _descriptiveName = "Basic Attack",
                _adRatio = 1,
                _delayTimeToLaunchBullet = 0.55f,
                _targetRange = 4.2f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "Target",
                _unitTypesTargetableList = new List<string> { "Enemy", "Object" },
                _pathIcon = "SkillIcon/Basic Attack_Hunter",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Basic Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Basic Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HunterAttack,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = default,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
            _functionComputeBestPos = default
        },
        new _Skill //#Basic Attack_Mage
        {
            _parameter = new _Parameter
            {
                _name = "Basic Attack_Mage",
                _descriptiveName = "Basic Attack",
                _adRatio = 1.0f,
                _delayTimeToLaunchBullet = 0.4f,
                _targetRange = 3.2f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "Target",
                _unitTypesTargetableList = new List<string> { "Enemy", "Object" },
                _pathIcon = "SkillIcon/Basic Attack_Mage",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Basic Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Basic Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.MageAttack,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = default,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
            _functionComputeBestPos = default
        },
        new _Skill //#Basic Attack Bonus Battlecry
        {
            _parameter = new _Parameter
            {
                _name = "Basic Attack Bonus Battlecry",
                _descriptiveName = "Basic Attack Bonus",
                _type = "Passive_AfterCastSkill",
                _triggerTiming = new string[]{ "AfterCastSkill", "OnCalculateDealDamage" },
                _baseValue = 120,
                _pathIcon = "SkillIcon/Basic Attack Bonus",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Basic Attack Bonus").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Basic Attack Bonus").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BasicAttackBonus,
        },
        new _Skill //#Basic Attack Bonus Spellblade
        {
            _parameter = new _Parameter
            {
                _name = "Basic Attack Bonus Spellblade",
                _descriptiveName = "Basic Attack Bonus",
                _type = "Passive_AfterCastSkill",
                _triggerTiming = new string[]{ "AfterCastSkill", "OnCalculateDealDamage" },
                _baseValue = 130,
                _pathIcon = "SkillIcon/Basic Attack Bonus",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Basic Attack Bonus").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Basic Attack Bonus").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BasicAttackBonus,
        },
        new _Skill //#Basic Attack Bonus Tumble
        {
            _parameter = new _Parameter
            {
                _name = "Basic Attack Bonus Tumble",
                _descriptiveName = "Basic Attack Bonus",
                _type = "Passive_AfterCastSkill",
                _triggerTiming = new string[]{ "AfterCastSkill", "OnCalculateDealDamage" },
                _baseValue = 130,
                _pathIcon = "SkillIcon/Basic Attack Bonus",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Basic Attack Bonus").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Basic Attack Bonus").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BasicAttackBonus,
        },
        new _Skill //#Battlecry
        {
            _parameter = new _Parameter
            {
                _name = "Battlecry",
                _descriptiveName = "Battlecry",
                _classType = new string[]{ "Warrior" },
                _cooldownDuration = 5,
                _buffType = new List<string>{ "AD", "AR" },
                _buffValue = new List<int> { 50, 50 },
                _isQuickCast = true,
                _skillAbilitiesTable = new List<string>{ "Buff+30AD", "Buff+30AR", "Heal0.2", "Basic Attack Bonus Battlecry"  },
                _delayTimeToResolveEffect = 0f,
                _effectSize = 1.0f,
                _effectColor = "Orange",
                _animatorSetTrigger = "triggerCastBuff01",
                _castType = "Self",
                _pathIcon = "SkillIcon/Battlecry",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Battlecry").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Battlecry").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HeroBuff_Target,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Berserker
        {
            _parameter = new _Parameter
            {
                _name = "Berserker",
                _descriptiveName = "Berserker",
                _type = "Passive_OnBasicAttack",
                _triggerTiming = new string[]{ "OnTakenDamage" },
                _pathIcon = "SkillIcon/Berserker",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Berserker").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Berserker").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Berserker
        },
        new _Skill //#Blazing Star
        {
            _parameter = new _Parameter
            {
                _name = "Blazing Star",
                _descriptiveName = "Blazing Star",
                _triggerTiming = new string[] { "Static" },
                _applyOrder = 1,
                _pathIcon = "SkillIcon/Blazing Star",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Blazing Star").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Blazing Star").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BlazingStar,
        },
        new _Skill //#Bleeding
        {
            _parameter = new _Parameter
            {
                _name = "Bleeding",
                _descriptiveName = "Bleeding",
                _triggerTiming = new string[] { "StartOfBattle" },
                _pathIcon = "SkillIcon/Bleeding",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Bleeding").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Bleeding").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Bleeding,
        },
        new _Skill //#Blessing
        {
            _parameter = new _Parameter
            {
                _name = "Blessing",
                _descriptiveName = "Blessing",
                _triggerTiming = new string[] { "OnApplyBuff", "EndOfBattle" },
                _iCount = 3,
                _sdDamageBase = 40,
                _pathIcon = "SkillIcon/Blessing",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Blessing").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Blessing").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Blessing,
        },
        new _Skill //#Blind Spore
        {
            _parameter = new _Parameter
            {
                _name = "Blind Spore",
                _descriptiveName = "Blind Spore",
                _mdRatio = 1.0f,
                _delayTimeToDealDamage = 0.5f,
                _targetRange = 2.2f,
                _cooldownDuration = 4,
                _animatorSetTrigger = "triggerCastSkill00",
                _castType = "Target",
                _pathIcon = "SkillIcon/Blind Spore",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Blind Spore").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Blind Spore").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BlindSpore,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Blood Thirst
        {
            _parameter = new _Parameter
            {
                _name = "Blood Thirst",
                _descriptiveName = "Blood Thirst",
                _type = "Passive_OnDealPhysicalDamage",
                _triggerTiming = new string[]{ "OnDealPhysicalDamage" },
                _iValue = 10,
                _pathIcon = "SkillIcon/Blood Thirst",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Blood Thirst").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Blood Thirst").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.StealLife
        },
        new _Skill //#Call Wolves
        {
            _parameter = new _Parameter
            {
                _name = "Call Wolves",
                _descriptiveName = "Call Wolves",
                _cooldownDuration = 6,
                _delayTimeToResolveEffect = 0.65f,
                _effectSize = 1.5f,
                _animatorSetTrigger = "triggerCastSkill01",
                _castType = "Other",
                _pathIcon = "SkillIcon/Call Minions",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Call Wolves").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Call Wolves").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.CallMinions,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindHero_Null,
            _functionDetectUnits = SkillUtility.DetectUnits_Null,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Carve
        {
            _parameter = new _Parameter
            {
                _name = "Carve",
                _descriptiveName = "Carve",
                _type = "Passive_OnDealPhysicalDamage",
                _triggerTiming = new string[]{ "OnDealPhysicalDamage" },
                _buffValue = new List<int>{ -20 },
                _buffType = new List<string>{ "AR" },
                _effectColor = "Blue",
                _delayTimeToResolveEffect = 0.2f,
                _pathIcon = "SkillIcon/Carve",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Carve").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Carve").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HeroDebuff
        },
        new _Skill //#Circle of Healing
        {
            _parameter = new _Parameter
            {
                _name = "Circle of Healing",
                _descriptiveName = "Circle of Healing",
                _restoreValueBase = 20,
                _hpRatio = 0.3f,
                _castableStacks = 1,
                _delayTimeToDealDamage = 0.15f,
                _hitRange = 3.0f,
                //_isQuickCast = true,
                _animatorSetTrigger = "triggerCastBuff00",
                _castType = "AOE",
                _pathIcon = "SkillIcon/Circle of Healing",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Circle of Healing").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Circle of Healing").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.CircleOfHealing,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalHeal,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Cone of Flame
        {
            _parameter = new _Parameter
            {
                _name = "Cone of Flame",
                _descriptiveName = "Cone of Flame",
                _classType = new string[]{ "Mage" },
                _damageType = "MD",
                _mdDamageBase = 10,
                _mdRatio = 0.25f,
                _cooldownDuration = 6,
                _delayTimeToLaunchBullet = 0.5f,
                _hitRange = 5f,
                _hitCount = 4,
                _angle = 45,
                _skillAbilitiesTable = new List<string>{ "Hit Count+1", "Hit Angle+15", "Cooldown-", "MD Ratio+0.1"  },
                _animatorSetBool = "isChanneling00",
                _castType = "AOE",
                _unitTypesTargetableList = new List<string> { "Enemy" },
                _pathIcon = "SkillIcon/Cone of Flame",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Cone of Flame").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Cone of Flame").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.ConeOfFlame,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfToMouseHitArc,
            _functionFindUnits = default,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitArc,
            _functionComputeBestPos = default
        },
        new _Skill //#Cosmic Plasma
        {
            _parameter = new _Parameter
            {
                _name = "Cosmic Plasma",
                _descriptiveName = "Cosmic Plasma",
                _mdRatio = 1.0f,
                _cooldownDuration = 5,
                _targetRange = 5.0f,
                _castType = "Direction",
                _pathIcon = "SkillIcon/Cosmic Plasma",
                _unitTypesTargetableList = new List<string>() { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Cosmic Plasma").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Cosmic Plasma").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.CosmicPlasma,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_TargetLine_ClosestUnit,
            _functionDetectUnits = SkillUtility.DetectUnits_TargetLine_ClosestUnit,
        },
        new _Skill //#Chain Lightning
        {
            _parameter = new _Parameter
            {
                _name = "Chain Lightning",
                _descriptiveName = "Chain Lightning",
                _classType = new string[]{ "Mage" },
                _damageType = "MD",
                _mdDamageBase = 40,
                _mdRatio = 1.0f,
                _targetRange = 4.5f,
                _chainRange = 3.0f,
                _cooldownDuration = 6,
                _skillAbilitiesTable = new List<string>{ "MD Damage+50", "Chain Range+10", "Cooldown-", "MD Ratio+0.4"  },
                _castType = "Target",
                _pathIcon = "SkillIcon/Chain Lightning",
                //_animatorSetTrigger = "triggerAttack",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Chain Lightning").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Chain Lightning").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.ChainLightning,
            _functionSimulateEffect = SkillUtility.SimulateEffect_ChainLightning,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#Charge
        {
            _parameter = new _Parameter
            {
                _name = "Charge",
                _descriptiveName = "Chrage",
                _classType = new string[]{ "Warrior" },
                _damageType = "AD",
                _adDamageBase = 20,
                _adRatio = 1.0f,
                _moveRange = 4,
                _hitRange = 1.5f,
                _angle = 180,
                _cooldownDuration = 6,
                _skillAbilitiesTable = new List<string>{ "Armor Break30", "Move Range+15", "Cooldown-1", "Gain Barrier60"  },
                _castType = "Direction",
                _pathIcon = "SkillIcon/Charge",
                _animatorSetBool = "isCharge",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Charge").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Charge").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Charge,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_Charge,
            _functionDetectUnits = SkillUtility.DetectUnits_Charge,
        },
        new _Skill //#Cleave
        {
            _parameter = new _Parameter
            {
                _name = "Cleave",
                _descriptiveName = "Cleave",
                _triggerTiming = new string[]{ "OnCalculateDealDamage" },
                _delayTimeToResolveEffect = 0.2f,
                _pathIcon = "SkillIcon/Cleave",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Cleave").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Cleave").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Cleave
        },
        new _Skill //#Curse
        {
            _parameter = new _Parameter
            {
                _name = "Curse",
                _descriptiveName = "Curse",
                _triggerTiming = new string[] { "EndOfYourTurn", "EndOfBattle" },
                _tags = new List<string> { "Curse" },
                _iCount = 3,
                //_isShowAsIcon = false,
                _pathIcon = "SkillIcon/Curse",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Curse").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Curse").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Curse,
        },
        new _Skill //#Demon Attack
        {
            _parameter = new _Parameter
            {
                _name = "Demon Attack",
                _descriptiveName = "Basic Attack",
                _adRatio = 1.0f,
                _delayTimeToDealDamage = 0.3f,
                _targetRange = 2.7f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "Target",
                _pathIcon = "SkillIcon/Enemy Attack",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.EnemyMeleeAttackNormal,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Demonic Frenzy
        {
            _parameter = new _Parameter
            {
                _name = "Demonic Frenzy",
                _descriptiveName = "Demonic Frenzy",
                _cooldownDuration = 5,
                _buffType = new List<string>{ "AD" },
                _buffValue = new List<int> { 50 },
                _effectSize = 1.5f,
                _effectColor = "Orange",
                _isQuickCast = true,
                _animatorSetTrigger = "triggerCastBuff00",
                _castType = "Self",
                _pathIcon = "SkillIcon/Demonic Frenzy",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Demonic Frenzy").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Demonic Frenzy").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.EnemyBuff_Self,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindHero_Null,
            _functionDetectUnits = SkillUtility.DetectUnits_Null,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#DoublePunch
        {
            _parameter = new _Parameter
            {
                _name = "Double Punch",
                _descriptiveName = "Double Punch",
                _cooldownDuration = 4,
                _adRatio = 0.7f,
                _delayTimeToDealDamage = 0.1f,
                _targetRange = 2.7f,
                _animatorSetTrigger = "triggerCastSkill00",
                _castType = "Target",
                _pathIcon = "SkillIcon/Double Punch",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Double Punch").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Double Punch").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.DoublePunch,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Dismantle
        {
            _parameter = new _Parameter
            {
                _name = "Dismantle",
                _descriptiveName = "Dismantle",
                _triggerTiming = new string[]{ "OnCalculateDealDamage" },
                _delayTimeToResolveEffect = 0.2f,
                _pathIcon = "SkillIcon/Dismantle",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Dismantle").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Dismantle").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Cleave
        },
        new _Skill //#Divine Barrier
        {
            _parameter = new _Parameter
            {
                _name = "Divine Barrier",
                _descriptiveName = "Divine Barrier",
                _barrierValueBase = 40,
                _arRatio = 1.0f,
                _cooldownDuration = 6,
                _delayTimeToDealDamage = 0.15f,
                _isQuickCast = true,
                _animatorSetTrigger = "triggerCastBuff00",
                _castType = "Target",
                _pathIcon = "SkillIcon/Divine Barrier",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Divine Barrier").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Divine Barrier").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.GainBarrier,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalHeal,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#Enlightenment
        {
            _parameter = new _Parameter
            {
                _name = "Enlightenment",
                _descriptiveName = "Enlightenment",
                _type = "Passive_EndOfYourTurn",
                _triggerTiming = new string[]{ "EndOfYourTurn" },
                _mdDamageBase = 20,
                _mdRatio = 0.5f,
                _hitRange = 3.4f,
                _pathIcon = "SkillIcon/Enlightenment",
                _unitTypesTargetableList = new List<string> { "Hero", "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Enlightenment").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Enlightenment").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Enlightenment,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#Execution
        {
            _parameter = new _Parameter
            {
                _name = "Execution",
                _descriptiveName = "Execution",
                _classType = new string[]{ "Warrior" },
                _damageType = "AD",
                _adRatio = 1.0f,
                _iValue = 25,
                _cooldownDuration = 6,
                _targetRange = 3.0f,
                _skillAbilitiesTable = new List<string>{ "Execution+", "AD Ratio+0.5", "Killing Spree-2", "Bloodlust+30" },
                _delayTimeToDealDamage = 0.7f,
                _animatorSetTrigger = "triggerCastSkill02",
                _castType = "Target",
                _pathIcon = "SkillIcon/Execution",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Execution").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Execution").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Execution,
            _functionSimulateEffect = SkillUtility.SimulateEffect_Execution,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#Fireball
        {
            _parameter = new _Parameter
            {
                _name = "Fireball",
                _descriptiveName = "Fireball",
                _classType = new string[]{ "Mage" },
                _damageType = "MD",
                _mdDamageBase = 40,
                _mdRatio = 1.0f,
                _cooldownDuration = 6,
                _delayTimeToDealDamage = 0.8f,
                _targetRange = 5f,
                _hitRange = 2.0f,
                _skillAbilitiesTable = new List<string>{ "MD Damage+30", "MD Ratio+0.3", "Cooldown-1", "Hit Range+10"  },
                _castType = "Direction",
                _pathIcon = "SkillIcon/Fireball",
                _unitTypesTargetableList = new List<string> { "Enemy", "Object" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Fireball").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Fireball").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Fireball,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_Fireball,
            _functionDetectUnits = SkillUtility.DetectUnits_Fireball,
        },
        new _Skill //#Flying
        {
            _parameter = new _Parameter
            {
                _name = "Flying",
                _descriptiveName = "Flying",
                _type = "Passive_Static",
                _triggerTiming = new string[]{ "Static" },
                _pathIcon = "SkillIcon/Flying",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Flying").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Flying").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Flying,
        },
        new _Skill //#Frost Nova
        {
            _parameter = new _Parameter
            {
                _name = "Frost Nova",
                _descriptiveName = "Frost Nova",
                _classType = new string[]{ "Mage" },
                _damageType = "MD",
                _baseValue = -50,
                _buffType = new List<string>{ "SP" },
                _buffValue = new List<int> { -50 },
                _mdDamageBase = 40,
                _mdRatio = 1.2f,
                _targetRange = 4.0f,
                _hitRange = 2.5f,
                _cooldownDuration = 6,
                _skillAbilitiesTable = new List<string>{ "Debuff+30SP", "MD Damage+50", "Hit Range+15", "Sword Break30"  },
                _castType = "AOE",
                _pathIcon = "SkillIcon/Frost Nova",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Frost Nova").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Frost Nova").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.FrostNova,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_TargetPoint_HitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_TargetPoint_HitCircle,
        },
        new _Skill //#Force
        {
            _parameter = new _Parameter
            {
                _name = "Force",
                _descriptiveName = "Force",
                _triggerTiming = new string[] { "OnCalculateDealDamage" },
                _pathIcon = "SkillIcon/Force",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Force").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Force").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Force,
        },
        new _Skill //#Ghost Step
        {
            _parameter = new _Parameter
            {
                _name = "Ghost Step",
                _descriptiveName = "Ghost Step",
                _baseValue = 10,
                _adRatio = 1.0f,
                _spRatio = 0.4f,
                _targetRange = 2.0f,
                _cooldownDuration = 6,
                _castType = "Direction",
                _pathIcon = "SkillIcon/Ghost Step",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Ghost Step").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Ghost Step").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.GhostStep,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_GhostStep,
            _functionDetectUnits = SkillUtility.DetectUnits_GhostStep,
        },
        new _Skill //#Giant Slayer
        {
            _parameter = new _Parameter
            {
                _name = "Giant Slayer",
                _descriptiveName = "Giant Slayer",
                _triggerTiming = new string[] { "OnCalculateDealDamage" },
                _pathIcon = "SkillIcon/Giant Slayer",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Giant Slayer").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Giant Slayer").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.GiantSlayer,
        },
        new _Skill //#Golem Attack
        {
            _parameter = new _Parameter
            {
                _name = "Golem Attack",
                _descriptiveName = "Basic Attack",
                _adRatio = 1.0f,
                _delayTimeToDealDamage = 0.3f,
                _targetRange = 1.8f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "Target",
                _pathIcon = "SkillIcon/Enemy Attack",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.EnemyMeleeAttackNormal,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Haste
        {
            _parameter = new _Parameter
            {
                _name = "Haste",
                _descriptiveName = "Haste",
                _castableStacks = 1,
                _buffType = new List<string>{ "SP" },
                _buffValue = new List<int> { 30 },
                _targetRange = 3.0f,
                _delayTimeToResolveEffect = 0.3f,
                _effectColor = "Green",
                _isQuickCast = true,
                _animatorSetTrigger = "triggerCastBuff00",
                _castType = "Target",
                _pathIcon = "SkillIcon/Haste",
                _unitTypesTargetableList = new List<string> {"Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Haste").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Haste").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HeroBuff_Target,
            _functionSimulateEffect = SkillUtility.SimulateEffect_None,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle
        },
        new _Skill //#Heavy Strike
        {
            _parameter = new _Parameter
            {
                _name = "Heavy Strike",
                _descriptiveName = "Heavy Strike",
                _classType = new string[]{ "Warrior" },
                _damageType = "AD",
                _adDamageBase = 30,
                _adRatio = 1.0f,
                _cooldownDuration = 7,
                _targetRange = 2.5f,
                _skillAbilitiesTable = new List<string>{ "Sword Break40", "Armor Break40", "Cooldown-1", "AD Damage+50"  },
                _delayTimeToDealDamage = 0.8f,
                _animatorSetTrigger = "triggerCastSkill01",
                _castType = "Target",
                _pathIcon = "SkillIcon/Heavy Strike",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Heavy Strike").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Heavy Strike").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HeavyStrike,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#Howling
        {
            _parameter = new _Parameter
            {
                _name = "Howling",
                _descriptiveName = "Howling",
                _cooldownDuration = 5,
                _buffType = new List<string>{ "AD" },
                _buffValue = new List<int> { 30 },
                _isQuickCast = true,
                _delayTimeToResolveEffect = 0f,
                _effectSize = 1.0f,
                _effectColor = "Orange",
                _animatorSetTrigger = "triggerCastBuff01",
                _castType = "Self",
                _pathIcon = "SkillIcon/Howling",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Howling").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Howling").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.EnemyBuff_Self,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindHero_Null,
            _functionDetectUnits = SkillUtility.DetectUnits_Null,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Hunger
        {
            _parameter = new _Parameter
            {
                _name = "Hunger",
                _descriptiveName = "Hunger",
                _triggerTiming = new string[] { "OnApplyBuff" },
                _tags = new List<string> { "Hunger" },
                //_isShowAsIcon = false,
                _pathIcon = "SkillIcon/Hunger",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Hunger").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Hunger").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Hunger,
        },
        new _Skill //#Ice Shard
        {
            _parameter = new _Parameter
            {
                _name = "Ice Shard",
                _descriptiveName = "Ice Shard",
                _classType = new string[]{ "Mage" },
                _damageType = "MD",
                _mdDamageBase = 20,
                _mdRatio = 1.0f,
                _cooldownDuration = 4,
                _delayTimeToLaunchBullet = 0.55f,
                _targetRange = 5.5f,
                _hitRange = 3.5f,
                _angle = 60,
                _skillAbilitiesTable = new List<string>{ "AD Damage+", "Hit Range+", "Cooldown-", "AD Ratio+"  },
                _castType = "Direction",
                _unitTypesTargetableList = new List<string> { "Enemy", "Object" },
                _pathIcon = "SkillIcon/Ice Shard",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Ice Shard").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Ice Shard").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.IceShard,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_IceShard,
            _functionDetectUnits = SkillUtility.DetectUnits_IceShard,
        },
        new _Skill //#Inflame
        {
            _parameter = new _Parameter
            {
                _name = "Inflame",
                _descriptiveName = "Inflame",
                _triggerTiming = new string[]{ "OnApplyBuff" },
                _pathIcon = "SkillIcon/Passive_Demon"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Inflame").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Inflame").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Demon,
        },
        new _Skill //#Injured
        {
            _parameter = new _Parameter
            {
                _name = "Injured",
                _descriptiveName = "Injured",
                _triggerTiming = new string[] { "StartOfBattle" },
                //_isDestroySelfOnTrigger = true,
                _sdDamageBase = 40,
                _pathIcon = "SkillIcon/Injured",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Injured").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Injured").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Injured,
        },
        new _Skill //#Inspire
        {
            _parameter = new _Parameter
            {
                _name = "Inspire",
                _descriptiveName = "Inspire",
                _buffType = new List<string> { "AD", "AR", "MD", "MR", "SP" },
                _buffValue = new List<int> { 30, 30, 30, 30, 30 },
                _cooldownDuration = 3,
                _targetRange = 5.0f,
                _castType = "Target",
                _animatorSetTrigger = "triggerCastSkill01",
                _unitTypesTargetableList = new List<string> { "Enemy" },
                _pathIcon = "SkillIcon/Inspire",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Inspire").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Inspire").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Inspire,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_BuffAlly,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Knockback Shot
        {
            _parameter = new _Parameter
            {
                _name = "Knockback Shot",
                _descriptiveName = "Knockback Shot",
                _classType = new string[]{ "Hunter" },
                _damageType = "AD",
                _adDamageBase = 40,
                _adRatio = 1.2f,
                _cooldownDuration = 6,
                _delayTimeToLaunchBullet = 0.55f,
                _targetRange = 5.0f,
                _knockbackRange = 3.0f,
                _skillAbilitiesTable = new List<string>{ "AD Damage+50", "AD Ratio+0.5", "Cooldown-1", "Armor Break40"  },
                _castType = "Target",
                _unitTypesTargetableList = new List<string> { "Enemy" },
                _animatorSetTrigger = "triggerAttack",
                _pathIcon = "SkillIcon/Knockback Shot",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Knockback Shot").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Knockback Shot").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.KnockbackShot,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_KnockbackShot,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#Magic Shield
        {
            _parameter = new _Parameter
            {
                _name = "Magic Shield",
                _descriptiveName = "Magic Shield",
                _cooldownDuration = 6,
                _mdRatio = 0.6f,
                _barrierValueBase = 20,
                _targetRange = 4.0f,
                _castType = "Target",
                _animatorSetTrigger = "triggerUseItem00",
                _unitTypesTargetableList = new List<string> { "Hero" },
                _pathIcon = "SkillIcon/Magic Shield",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Magic Shield").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Magic Shield").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Protect,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_BuffAlly,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Mastery of Magic
        {
            _parameter = new _Parameter
            {
                _name = "Mastery of Magic",
                _descriptiveName = "Mastery of Magic",
                _triggerTiming = new string[] { "Static" },
                _applyOrder = 2,
                _pathIcon = "SkillIcon/Mastery of Magic",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Mastery of Magic").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Mastery of Magic").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.MasteryOfMagic,
        },
        new _Skill //#Offering
        {
            _parameter = new _Parameter
            {
                _name = "Offering",
                _descriptiveName = "Offering",
                _buffType = new List<string>{ "MD" },
                _buffValue = new List<int> { 40 },
                _isQuickCast = true,
                _castableStacks = 1,
                _effectSize = 1.0f,
                _effectColor = "Orange",
                _animatorSetTrigger = "triggerCastBuff01",
                _castType = "Self",
                _pathIcon = "SkillIcon/Offering",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Offering").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Offering").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Offering,
            _functionSimulateEffect = SkillUtility.SimulateEffect_None,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_None,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#On the Hunt
        {
            _parameter = new _Parameter
            {
                _name = "On the Hunt",
                _descriptiveName = "On the Hunt",
                _classType = new string[]{ "Hunter" },
                _cooldownDuration = 6,
                _buffType = new List<string>{ "AD", "SP" },
                _buffValue = new List<int> { 50, 50 },
                _isQuickCast = true,
                _skillAbilitiesTable = new List<string>{ "Buff+30AD", "Buff+30SP", "Cooldown-1", "Gain Barrier60"  },
                _effectSize = 1.0f,
                _effectColor = "Orange",
                _animatorSetTrigger = "triggerCastBuff01",
                _castType = "Self",
                _pathIcon = "SkillIcon/On the Hunt",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("On the Hunt").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("On the Hunt").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HeroBuff_Target,
            _functionSimulateEffect = SkillUtility.SimulateEffect_None,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_None,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#Passive_Warrior
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Warrior",
                _descriptiveName = "Passive - Blood Thirst",
                _type = "Class Passive",
                //_type = "Passive_OnDealPhysicalDamage",
                _triggerTiming = new string[]{ "OnDealPhysicalDamage" },
                _iValue = 30,
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Warrior"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Warrior").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Warrior").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.StealLife,
        },
        new _Skill //#Passive_Hunter
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Hunter",
                _descriptiveName = "Passive - Back Attack",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnKillEnemy", "OnCalculateDealDamage" },
                _damageCoef = 1.5f,
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Hunter"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Hunter").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Hunter").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Hunter,
        },
        //new _Skill //#Passive_Hunter
        //{
        //    _parameter = new _Parameter
        //    {
        //        _name = "Passive_Hunter",
        //        _descriptiveName = "Passive - Soul Collector",
        //        _type = "Class Passive",
        //        _triggerTiming = new string[]{ "OnKillEnemy", "OnCalculateDealDamage" },
        //        _isShowAsIcon = false,
        //        _pathIcon = "SkillIcon/Passive_Hunter"
        //    },
        //    _tooltip = new _Tooltip
        //    {
        //        _type = "Passive",
        //        _title = TextData.DescriptionText.GetValue("Passive_Hunter").GetIndexOf(0),
        //        _effectText = TextData.DescriptionText.GetValue("Passive_Hunter").GetIndexOf(1),
        //    },
        //    _functionCastSkill = SkillActivate.Passive_Hunter,
        //},
        new _Skill //#Passive_Mage
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Mage",
                _descriptiveName = "Passive - Magic Burst",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "AfterCastSkill" },
                _cooldownDuration = 3,
                _damageCoef = 1.3f,
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Mage"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Mage").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Mage").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Mage,
        },
        new _Skill //#Passive_Demon
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Demon",
                _descriptiveName = "Passive - Inflame",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnApplyBuff" },
                //_buffValue = new List<int>{ -30 },
                //_buffType = new List<string>{ "AR" },
                //_damageCoef = 1.3f,
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Demon"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Demon").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Demon").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Demon,
        },
        new _Skill //#Passive_Slime
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Slime",
                _descriptiveName = "Passive - Weakness",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnCalculateTakenDamage" },
                _damageCoef = 1.3f,
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Slime"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Slime").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Slime").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Slime,
        },
        new _Skill //#Passive_Met
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Met",
                _descriptiveName = "Passive - Thorn",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnTakenPhysicalDamage" },
                //_damageCoef = 1.3f,
                //_isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Met"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Met").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Met").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Thorn,
        },
        new _Skill //#Passive_Rat
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Rat",
                _descriptiveName = "Passive_Rat",
                _triggerTiming = new string[] { "OnDeath" },
                _pathIcon = "SkillIcon/Passive_Rat",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Rat").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Rat").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Rat,
        },
        new _Skill //#Passive_Wolf
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Wolf",
                _descriptiveName = "Passive - Howling",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnApplyBuff" },
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Wolf"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Wolf").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Wolf").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Wolf,
        },
        new _Skill //#Passive_Golem
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Golem",
                _descriptiveName = "Passive - Stone Skin",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "EndOfYourTurn" },
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Passive_Golem"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Golem").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Golem").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Golem,
        },
        new _Skill //#Passive_Explosive Bug
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Explosive Bug",
                _descriptiveName = "Passive - Explosion",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnDeath" },
                _mdDamageBase = 40,
                _hitRange = 2.5f,
                _pathIcon = "SkillIcon/Passive_Explosive Bug",
                _unitTypesTargetableList = new List<string> { "Hero", "Enemy", "Object" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Passive_Explosive Bug").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Explosive Bug").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Explosion,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#Passive_Faerie
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Faerie",
                _descriptiveName = "Passive_Faerie",
                _triggerTiming = new string[] { "EndOfYourTurn" },
                _restoreValueBase = 30,
                _hitRange = 3.0f,
                _pathIcon = "SkillIcon/Passive_Faerie",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Passive_Faerie").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Faerie").GetIndexOf(1),
            },
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
            _functionCastSkill = SkillActivate.Passive_Faerie,
        },
        new _Skill //#Passive_Frightfly
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Frightfly",
                _descriptiveName = "Passive_Frightfly",
                _pathIcon = "SkillIcon/Passive_Frightfly",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Frightfly").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Frightfly").GetIndexOf(1),
            },
            //_functionCastSkill = SkillActivate.Flying,
        },
        new _Skill //#Passive_Fungusa
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Fungusa",
                _descriptiveName = "Passive_Fungusa",
                _triggerTiming = new string[] { "OnTakenPhysicalDamage" },
                _pathIcon = "SkillIcon/Passive_Fungusa",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Fungusa").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Fungusa").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Fungusa,
        },
        new _Skill //#Passive_Fungee
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Fungee",
                _descriptiveName = "Passive_Fungee",
                _triggerTiming = new string[] { "EndOfYourTurn" },
                _pathIcon = "SkillIcon/Passive_Fungee",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Fungee").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Fungee").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Fungee,
        },
        new _Skill //#Passive_Treant
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Treant",
                _descriptiveName = "Passive_Treant",
                _triggerTiming = new string[] { "OnApplyBuff" },
                _pathIcon = "SkillIcon/Passive_Treant",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Treant").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Treant").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Snowman
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Snowman",
                _descriptiveName = "Passive_Snowman",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Snowman",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Snowman").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Snowman").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Longtail
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Longtail",
                _descriptiveName = "Passive_Longtail",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Longtail",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Longtail").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Longtail").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Stump
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Stump",
                _descriptiveName = "Passive_Stump",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Stump",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Stump").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Stump").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Scorpion
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Scorpion",
                _descriptiveName = "Passive_Scorpion",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Scorpion",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Scorpion").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Scorpion").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Leech
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Leech",
                _descriptiveName = "Passive_Leech",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Leech",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Leech").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Leech").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Caterpillar
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Caterpillar",
                _descriptiveName = "Passive_Caterpillar",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Caterpillar",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Caterpillar").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Caterpillar").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Venusa
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Venusa",
                _descriptiveName = "Passive_Venusa",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Venusa",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Venusa").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Venusa").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Egglet
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Egglet",
                _descriptiveName = "Passive_Egglet",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Egglet",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Egglet").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Egglet").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Sicklus
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Sicklus",
                _descriptiveName = "Passive_Sicklus",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Sicklus",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Sicklus").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Sicklus").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Sicklus
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Serpent",
                _descriptiveName = "Passive_Serpent",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Serpent",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Serpent").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Serpent").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Crysral Guardian
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Crystal Guardian",
                _descriptiveName = "Passive_Crystal Guardian",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/Passive_Crystal Guardian",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Crystal Guardian").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Crystal Guardian").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Treant,
        },
        new _Skill //#Passive_Explosive Crystal
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Explosive Crystal",
                _descriptiveName = "Passive - Explosive Crystal",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnDeath" },
                _mdDamageBase = 40,
                _hitRange = 2.5f,
                _pathIcon = "SkillIcon/Passive_Explosive Bug",
                _unitTypesTargetableList = new List<string> { "Hero", "Enemy", "Object" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Passive_Explosive Crystal").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Explosive Crystal").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Explosion,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#Passive_Healing Crystal
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Healing Crystal",
                _descriptiveName = "Passive - Healing Crystal",
                _type = "Class Passive",
                _triggerTiming = new string[]{ "OnDeath" },
                _restoreValueBase = 40,
                _hitRange = 2.5f,
                _pathIcon = "SkillIcon/Passive_Healing Crystal",
                _unitTypesTargetableList = new List<string> { "Hero", "Enemy", "Object" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Passive_Healing Crystal").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Healing Crystal").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HealingExplosion,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#Passive_Rune Stone
        {
            _parameter = new _Parameter
            {
                _name = "Passive_Rune Stone",
                _descriptiveName = "Passive_Rune Stone",
                _triggerTiming = new string[] { "EndOfHeroTurn" },
                _hitRange = 4.0f,
                _restoreValueBase = 40,
                _pathIcon = "SkillIcon/Passive_Rune Stone",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Passive_Rune Stone").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Passive_Rune Stone").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Passive_Faerie,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#Phantasmagoria
        {
            _parameter = new _Parameter
            {
                _name = "Phantasmagoria",
                _descriptiveName = "Phantasmagoria",
                _type = "Passive_Static",
                _triggerTiming = new string[]{ "Static" },
                _fValue = 1.3f,
                _pathIcon = "SkillIcon/Phantasmagoria"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Phantasmagoria").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Phantasmagoria").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Phantasmagoria
        },
        new _Skill //#Photosynthesis
        {
            _parameter = new _Parameter
            {
                _name = "Photosynthesis",
                _descriptiveName = "Photosynthesis",
                _cooldownDuration = 4,
                _restoreValueBase = 50,
                _castType = "Target",
                _animatorSetTrigger = "triggerCastSkill00",
                _unitTypesTargetableList = new List<string> {  },
                _pathIcon = "SkillIcon/Photosynthesis",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Photosynthesis").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Photosynthesis").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Photosynthesis,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalHeal,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindHero_Null,
            _functionDetectUnits = SkillUtility.DetectUnits_Null,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest,
            _functionIsCastThisSkill = SkillUtility.IsCastThisSkill_Photosynthesis,
        },
        new _Skill //#Pierce
        {
            _parameter = new _Parameter
            {
                _name = "Pierce",
                _descriptiveName = "Pierce",
                _triggerTiming = new string[]{ "OnCalculateDamage" },
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Pierce"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Pierce").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Pierce").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Pierce
        },
        new _Skill //#Piercing Shot
        {
            _parameter = new _Parameter
            {
                _name = "Piercing Shot",
                _descriptiveName = "Piercing Shot",
                _classType = new string[]{ "Hunter" },
                _damageType = "AD",
                _adDamageBase = 30,
                _adRatio = 1.2f,
                _cooldownDuration = 6,
                _delayTimeToLaunchBullet = 0.55f,
                _hitRange = 6.0f,
                _skillAbilitiesTable = new List<string>{ "AD Damage+40", "AD Ratio+0.4", "Hit Range+20", "Killing Spree-1" },
                _animatorSetTrigger = "triggerAttack",
                _castType = "AOE",
                _unitTypesTargetableList = new List<string> { "Enemy", "Object" },
                _pathIcon = "SkillIcon/Piercing Shot",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Piercing Shot").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Piercing Shot").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.PiercingShot,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_HitLine_AllUnits,
            _functionFindUnits = default,
            _functionDetectUnits = SkillUtility.DetectUnits_HitLine_AllUnits,
            _functionComputeBestPos = default
        },
        new _Skill //#Protect
        {
            _parameter = new _Parameter
            {
                _name = "Protect",
                _descriptiveName = "Protect",
                _cooldownDuration = 3,
                _barrierValueBase = 60,
                _targetRange = 5.0f,
                _castType = "Target",
                _animatorSetTrigger = "triggerCastSkill01",
                _unitTypesTargetableList = new List<string> { "Enemy" },
                _pathIcon = "SkillIcon/Protect",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Protect").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Protect").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Protect,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_BuffAlly,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Purify
        {
            _parameter = new _Parameter
            {
                _name = "Purify",
                _descriptiveName = "Purify",
                _cooldownDuration = 4,
                _castableStacks = 1,
                _isQuickCast = true,
                _isCastableWhileDisabled = true,
                _castType = "Target",
                _pathIcon = "SkillIcon/Purify",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Purify").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Purify").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Purify,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalHeal,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#Rage
        {
            _parameter = new _Parameter
            {
                _name = "Rage",
                _descriptiveName = "Rage",
                _triggerTiming = new string[]{ "StartOfBattle" },
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Rage"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Rage").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rage").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Rage
        },
        new _Skill //#Rampage
        {
            _parameter = new _Parameter
            {
                _name = "Rampage",
                _descriptiveName = "Rampage",
                _classType = new string[]{ "Warrior" },
                _damageType = "AD",
                _adDamageBase = 40,
                _adRatio = 1.0f,
                _cooldownDuration = 5,
                _targetRange = 2.5f,
                _skillAbilitiesTable = new List<string>{ "AD Damage+", "Hit Range+", "Cooldown-", "AD Ratio+"  },
                _delayTimeToDealDamage = 0.8f,
                _animatorSetTrigger = "triggerCastSkill01",
                _castType = "Target",
                _pathIcon = "SkillIcon/Rampage",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Rampage").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rampage").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Rampage,
            _functionSimulateEffect = SkillUtility.SimulateEffect_Rampage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#Raining Arrow
        {
            _parameter = new _Parameter
            {
                _name = "Raining Arrow",
                _descriptiveName = "Raining Arrow",
                _classType = new string[]{ "Hunter" },
                _damageType = "AD",
                _adDamageBase = 40,
                _baseValue = -40,
                _adRatio = 1.2f,
                _cooldownDuration = 5,
                _targetRange = 5.5f,
                _hitRange = 2.0f,
                _skillAbilitiesTable = new List<string>{ "AD Damage+40", "Hit Range+10", "Cooldown-1", "AD Ratio+0.4"  },
                _castType = "AOE",
                _pathIcon = "SkillIcon/Raining Arrow",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Raining Arrow").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Raining Arrow").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.RainingArrow,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_TargetPoint_HitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_TargetPoint_HitCircle,
        },
        new _Skill //#Resurrection
        {
            _parameter = new _Parameter
            {
                _name = "Resurrection",
                _descriptiveName = "Resurrection",
                _type = "Passive_OnDeath",
                _triggerTiming = new string[]{ "OnDeath" },
                _pathIcon = "SkillIcon/Resurrection",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Resurrection").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Resurrection").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Resurrection,
        },
        new _Skill //#Return to Origin
        {
            _parameter = new _Parameter
            {
                _name = "Return to Origin",
                _descriptiveName = "Return to Origin",
                _castableStacks = 1,
                _targetRange = 3.0f,
                //_isQuickCast = true,
                _castType = "Target",
                _pathIcon = "SkillIcon/Return to Origin",
                _unitTypesTargetableList = new List<string> { "Hero", "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Return to Origin").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Return to Origin").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.ReturnToOrigin,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle
        },
        new _Skill //#Rimefrost
        {
            _parameter = new _Parameter
            {
                _name = "Rimefrost",
                _descriptiveName = "Rimefrost",
                _type = "Passive_OnDealMagicDamage",
                _triggerTiming = new string[]{ "OnDealMagicDamage" },
                _buffValue = new List<int>{ -20 },
                _buffType = new List<string>{ "SP" },
                _effectColor = "Blue",
                _delayTimeToResolveEffect = 0.2f,
                _pathIcon = "SkillIcon/Rimefrost",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Rimefrost").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Rimefrost").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.HeroDebuff,
        },
        new _Skill //#SavageSmash
        {
            _parameter = new _Parameter
            {
                _name = "Savage Smash",
                _descriptiveName = "Savage Smash",
                _cooldownDuration = 6,
                _adRatio = 2.0f,
                _delayTimeToDealDamage = 0.5f,
                _targetRange = 2.7f,
                _animatorSetTrigger = "triggerCastSkill00",
                _castType = "Target",
                _pathIcon = "SkillIcon/Savage Smash",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Savage Smash").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Savage Smash").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.SavageSmash,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Skullcrusher
        {
            _parameter = new _Parameter
            {
                _name = "Skullcrusher",
                _descriptiveName = "Skullcrusher",
                _triggerTiming = new string[]{ "StartOfBattle", "OnDealDamage" },
                _pathIcon = "SkillIcon/Skullcrusher",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Skullcrusher").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Skullcrusher").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Skullcrusher
        },
        new _Skill //#Shadow Hide You
        {
            _parameter = new _Parameter
            {
                _name = "Shadow Hide You",
                _descriptiveName = "Shadow Hide You",
                _pathIcon = "SkillIcon/Shadow Hide You",
                _castableStacks = 1,
                _isQuickCast = true,
                _animatorSetTrigger = "triggerUseItem00",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Shadow Hide You").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Shadow Hide You").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.GainStealth,
            _functionSimulateEffect = SkillUtility.SimulateEffect_None,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
        },
        new _Skill //#ShellAttack
        {
            _parameter = new _Parameter
            {
                _name = "Shell Attack",
                _descriptiveName = "Shell Attack",
                _cooldownDuration = 3,
                _adDamageBase = 40,
                _targetRange = 3.0f,
                _castType = "Target",
                _pathIcon = "SkillIcon/Shell Attack",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Shell Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Shell Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.ShellAttack,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Siphon Soul
        {
            _parameter = new _Parameter
            {
                _name = "Siphon Soul",
                _descriptiveName = "Siphon Soul",
                _type = "Passive_OnDealMagicDamage",
                _triggerTiming = new string[]{ "OnDealMagicDamage" },
                _iValue = 10,
                _pathIcon = "SkillIcon/Siphon Soul",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Siphon Soul").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Siphon Soul").GetIndexOf(1),
            },
            //_functionDetectUnits = SkillUtility.DetectUnits_Null,
            _functionCastSkill = SkillActivate.StealLife,
        },
        new _Skill //#SlimeAttack
        {
            _parameter = new _Parameter
            {
                _name = "Slime Attack",
                _descriptiveName = "Basic Attack",
                _adRatio = 1.0f,
                _delayTimeToDealDamage = 0.15f,
                _targetRange = 1.8f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "Target",
                _pathIcon = "SkillIcon/Enemy Attack",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.EnemyMeleeAttackNormal,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Spark!
        {
            _parameter = new _Parameter
            {
                _name = "Spark!",
                _descriptiveName = "Spark!",
                _triggerTiming = new string[]{ "OnCalculateDealDamage", "AfterCastSkill", "EndOfYourTurn" },
                _isShowICount = true,
                _pathIcon = "SkillIcon/Spark!",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Spark!").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Spark!").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Spark
        },
        new _Skill //#Spellblade
        {
            _parameter = new _Parameter
            {
                _name = "Spellblade",
                _descriptiveName = "Spellblade",
                _type = "Passive_AfterCastSkill",
                _triggerTiming = new string[]{ "AfterCastSkill" },
                _pathIcon = "SkillIcon/Spellblade",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Spellblade").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Spellblade").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Spellblade,
        },
        new _Skill //#StartOfBattle_BuffHeroes
        {
            _parameter = new _Parameter
            {
                _name = "Start of Battle - Buff Heroes",
                _descriptiveName = "Start of Battle - Buff Heroes",
                _type = "Effect_StartOfBattle",
                _effectColor = "Orange",
                _effectSize = 1.0f,
                //_pathIcon = "SkillIcon/Spellblade",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                //_title = TextData.DescriptionText.GetValue("Spellblade").GetIndexOf(0),
                //_effectText = TextData.DescriptionText.GetValue("Spellblade").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.StartOfBattle_BuffHeroes
        },
        new _Skill //#Stigma
        {
            _parameter = new _Parameter
            {
                _name = "Stigma",
                _descriptiveName = "Stigma",
                _triggerTiming = new string[] { "OnKillEnemy" },
                _tags = new List<string> { "Stigma" },
                //_isShowAsIcon = false,
                _pathIcon = "SkillIcon/Stigma",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Stigma").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Stigma").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Stigma,
        },
        new _Skill //#Sting
        {
            _parameter = new _Parameter
            {
                _name = "Sting",
                _descriptiveName = "Sting",
                _adRatio = 1.2f,
                _buffType = new List<string> { "AR" },
                _buffValue = new List<int> { -40 },
                _delayTimeToDealDamage = 0.2f,
                _targetRange = 2.2f,
                _cooldownDuration = 4,
                _animatorSetTrigger = "triggerCastSkill00",
                _castType = "Target",
                _pathIcon = "SkillIcon/Sting",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Sting").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Sting").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.EnemyMeleeAttackNormal,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
        new _Skill //#Swift
        {
            _parameter = new _Parameter
            {
                _name = "Swift",
                _descriptiveName = "Swift",
                _type = "Passive_OnTakenPhysicalDamage",
                _triggerTiming = new string[]{ "StartOfBattle", "OnApplyBuff", "OnTakenDamage" },
                _isShowICount = false,
                _pathIcon = "SkillIcon/Swift",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Swift").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Swift").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Swift,
        },
        new _Skill //#Thorn
        {
            _parameter = new _Parameter
            {
                _name = "Thorn",
                _descriptiveName = "Thorn",
                _type = "Passive_OnTakenPhysicalDamage",
                _triggerTiming = new string[]{ "OnTakenPhysicalDamage" },
                _pathIcon = "SkillIcon/Thorn",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Thorn").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Thorn").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Thorn,
        },
        new _Skill //#Thunder Strike
        {
            _parameter = new _Parameter
            {
                _name = "Thunder Strike",
                _descriptiveName = "Thunder Strike",
                _classType = new string[]{ "Mage" },
                _damageType = "MD",
                _mdDamageBase = 0,
                _mdRatio = 2.0f,
                _targetRange = 5.0f,
                _cooldownDuration = 8,
                _skillAbilitiesTable = new List<string>{ "MD Damage+60", "MD Ratio+0.6", "Cooldown-1", "Magic Burst+40"  },
                _castType = "Target",
                _pathIcon = "SkillIcon/Thunder Strike",
                _unitTypesTargetableList = new List<string> { "Enemy" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Thunder Strike").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Thunder Strike").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.ThunderStrike,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfTargetCircle,
        },
        new _Skill //#Treasure_Amulet Coin
        {
            _parameter = new _Parameter
            {
                _name = "Amulet Coin",
                _descriptiveName = "Amulet Coin",
                _pathIcon = "SkillIcon/Amulet Coin",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Amulet Coin").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Amulet Coin").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Artifact Ward
        {
            _parameter = new _Parameter
            {
                _name = "Artifact Ward",
                _descriptiveName = "Artifact Ward",
                _triggerTiming = new string[] { "StartOfBattle" },
                _pathIcon = "SkillIcon/Artifact Ward",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Artifact Ward").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Artifact Ward").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.ArtifactWard,
        },
        new _Skill //#Treasure_Battle Frenzy
        {
            _parameter = new _Parameter
            {
                _name = "Battle Frenzy",
                _descriptiveName = "Battle Frenzy",
                _triggerTiming = new string[] { "OnApplyBuff" },
                _pathIcon = "SkillIcon/Battle Frenzy",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Battle Frenzy").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Battle Frenzy").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BattleFrenzy,
        },
        new _Skill //#Treasure_Battle Rage
        {
            _parameter = new _Parameter
            {
                _name = "Battle Rage",
                _descriptiveName = "Battle Rage",
                _isShowICount = true,
                _triggerTiming = new string[] { "StartOfBattle" },
                _effectColor = "Orange",
                _buffType = new List<string> { "AD", "MD" },
                _buffValue = new List<int> { 40, 40 },
                _pathIcon = "SkillIcon/Battle Rage",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Battle Rage").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Battle Rage").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BattleRage,
        },
        new _Skill //#Treasure_Berserker Soul
        {
            _parameter = new _Parameter
            {
                _name = "Berserker Soul",
                _descriptiveName = "Berserker Soul",
                _iCount = 0,
                _triggerTiming = new string[]{ "StartOfBattle", "AfterCastSkill", "OnCalculateDealDamage" },
                _pathIcon = "SkillIcon/Berserker Soul",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Berserker Soul").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Berserker Soul").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BerserkerSoul,
        },
        new _Skill //#Treasure_Big Game Hunter
        {
            _parameter = new _Parameter
            {
                _name = "Big Game Hunter",
                _descriptiveName = "Big Game Hunter",
                _triggerTiming = new string[] { "OnApplyBuff" },
                _pathIcon = "SkillIcon/Big Game Hunter",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Big Game Hunter").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Big Game Hunter").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.BigGameHunter,
        },
        new _Skill //#Treasure_Binding Grasp
        {
            _parameter = new _Parameter
            {
                _name = "Binding Grasp",
                _descriptiveName = "Binding Grasp",
                _pathIcon = "SkillIcon/Binding Grasp",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Binding Grasp").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Binding Grasp").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Boon Reflection
        {
            _parameter = new _Parameter
            {
                _name = "Boon Reflection",
                _descriptiveName = "Boon Reflection",
                _pathIcon = "SkillIcon/Boon Reflection",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Boon Reflection").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Boon Reflection").GetIndexOf(1),
            },
            //_functionCastSkill = SkillActivate.BattleRage,
        },
        //new _Skill //#Treasure_Carnage
        //{
        //    _parameter = new _Parameter
        //    {
        //        _name = "Carnage",
        //        _descriptiveName = "Carnage",
        //        _triggerTiming = new string[] { "OnKillEnemy" },
        //        _pathIcon = "SkillIcon/Carnage",
        //    },
        //    _miniTooltip = new _Tooltip
        //    {
        //        _type = "Skill",
        //        _title = TextData.DescriptionText.GetValue("Carnage").GetIndexOf(0),
        //        _effectText = TextData.DescriptionText.GetValue("Carnage").GetIndexOf(1),
        //    },
        //    _functionCastSkill = SkillActivate.Carnage,
        //},
        new _Skill //#Treasure_DeathBringer
        {
            _parameter = new _Parameter
            {
                _name = "DeathBringer",
                _descriptiveName = "DeathBringer",
                _triggerTiming = new string[] { "OnDealDamage" },
                _pathIcon = "SkillIcon/DeathBringer",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("DeathBringer").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("DeathBringer").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Demolisher,
        },
        new _Skill //#Treasure_Demolisher
        {
            _parameter = new _Parameter
            {
                _name = "Demolisher",
                _descriptiveName = "Demolisher",
                _triggerTiming = new string[] { "OnDestroyObject" },
                _pathIcon = "SkillIcon/Demolisher",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Demolisher").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Demolisher").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Demolisher,
        },
        new _Skill //#Treasure_Essence Theft
        {
            _parameter = new _Parameter
            {
                _name = "Essence Theft",
                _descriptiveName = "Essence Theft",
                _triggerTiming = new string[] { "OnDestroyObject" },
                _pathIcon = "SkillIcon/Essence Theft",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Essence Theft").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Essence Theft").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Demolisher,
        },
        new _Skill //#Treasure_Fasting
        {
            _parameter = new _Parameter
            {
                _name = "Fasting",
                _descriptiveName = "Fasting",
                _triggerTiming = new string[] { "OnGetTreasure" },
                _pathIcon = "SkillIcon/Fasting",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Fasting").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Fasting").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Fasting,
        },
        new _Skill //#Treasure_First Blood
        {
            _parameter = new _Parameter
            {
                _name = "First Blood",
                _descriptiveName = "First Blood",
                _triggerTiming = new string[] { "StartOfBattle", "OnKillEnemy" },
                _pathIcon = "SkillIcon/First Blood",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("First Blood").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("First Blood").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.FirstBlood,
        },
        new _Skill //#Treasure_Force of Nature
        {
            _parameter = new _Parameter
            {
                _name = "Force of Nature",
                _descriptiveName = "Force of Nature",
                _pathIcon = "SkillIcon/Force of Nature",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Force of Nature").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Force of Nature").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Golden Armor
        {
            _parameter = new _Parameter
            {
                _name = "Golden Armor",
                _descriptiveName = "Golden Armor",
                _triggerTiming = new string[] { "StartOfBattle" },
                _pathIcon = "SkillIcon/Golden Armor",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Golden Armor").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Golden Armor").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.DivineLight,
        },
        new _Skill //#Treasure_Guardian Shield
        {
            _parameter = new _Parameter
            {
                _name = "Guardian Shield",
                _descriptiveName = "Guardian Shield",
                _pathIcon = "SkillIcon/Guardian Shield",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Guardian Shield").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Guardian Shield").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Harvest
        {
            _parameter = new _Parameter
            {
                _name = "Harvest",
                _descriptiveName = "Harvest",
                _triggerTiming = new string[] { "OnGetTrasure" },
                _pathIcon = "SkillIcon/Harvest",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Harvest").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Harvest").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Harvest,
        },
        new _Skill //#Treasure_Intimidation
        {
            _parameter = new _Parameter
            {
                _name = "Intimidation",
                _descriptiveName = "Intimidation",
                _isShowICount = true,
                _triggerTiming = new string[] { "StartOfBattle" },
                _pathIcon = "SkillIcon/Intimidation",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Intimidation").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Intimidation").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Intimidation,
        },
        new _Skill //#Treasure_Goldgrubber
        {
            _parameter = new _Parameter
            {
                _name = "Goldgrubber",
                _descriptiveName = "Goldgrubber",
                _pathIcon = "SkillIcon/Goldgrubber",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Goldgrubber").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Goldgrubber").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Meal Ticket
        {
            _parameter = new _Parameter
            {
                _name = "Meal Ticket",
                _descriptiveName = "Meal Ticket",
                _pathIcon = "SkillIcon/Meal Ticket",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Meal Ticket").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Meal Ticket").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Discount Ticket
        {
            _parameter = new _Parameter
            {
                _name = "Discount Ticket",
                _descriptiveName = "Discount Ticket",
                _pathIcon = "SkillIcon/Discount Ticket",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Discount Ticket").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Discount Ticket").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Mighty Guard
        {
            _parameter = new _Parameter
            {
                _name = "Mighty Guard",
                _descriptiveName = "Mighty Guard",
                _triggerTiming = new string[] { "StartOfBattle", "StartOfTurn" },
                _isShowICount = true,
                _pathIcon = "SkillIcon/Mighty Guard",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Mighty Guard").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Mighty Guard").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.MightyGuard,
        },
        new _Skill //#Treasure_Outrage
        {
            _parameter = new _Parameter
            {
                _name = "Outrage",
                _descriptiveName = "Outrage",
                _triggerTiming = new string[] { "OnApplyBuff" },
                _pathIcon = "SkillIcon/Outrage",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Outrage").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Outrage").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Outrage,
        },
        new _Skill //#Treasure_Potion Mastery
        {
            _parameter = new _Parameter
            {
                _name = "Potion Mastery",
                _descriptiveName = "Potion Mastery",
                _pathIcon = "SkillIcon/Potion Mastery",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Potion Mastery").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Potion Mastery").GetIndexOf(1),
            },
        },
        new _Skill //#Treasure_Predation
        {
            _parameter = new _Parameter
            {
                _name = "Predation",
                _descriptiveName = "Predation",
                _triggerTiming = new string[] { "OnKillEnemy" },
                _pathIcon = "SkillIcon/Predation",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Predation").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Predation").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Predation,
        },
        new _Skill //#Treasure_Quick Move
        {
            _parameter = new _Parameter
            {
                _name = "Quick Move",
                _descriptiveName = "Quick Move",
                _triggerTiming = new string[] { "StartOfBattle", "StartOfTurn", "OnApplyBuff" },
                _pathIcon = "SkillIcon/Quick Move",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Quick Move").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Quick Move").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.QuickMove,
        },
        new _Skill //#Treasure_Sudden Attack
        {
            _parameter = new _Parameter
            {
                _name = "Sudden Attack",
                _descriptiveName = "Sudden Attack",
                _triggerTiming = new string[] { "StartOfBattle", "OnCalculateDealDamage", "AfterCastSkill" },
                _pathIcon = "SkillIcon/Sudden Attack",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Sudden Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Sudden Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.SuddenAttack,
        },
        new _Skill //#Treasure_Shattering Smash
        {
            _parameter = new _Parameter
            {
                _name = "Shattering Smash",
                _descriptiveName = "Shattering Smash",
                _triggerTiming = new string[] { "OnCalculateDealDamage" },
                _pathIcon = "SkillIcon/Shattering Smash",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Shattering Smash").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Shattering Smash").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.ShatteringSmash,
        },
        new _Skill //#Treasure_Stealth Strike
        {
            _parameter = new _Parameter
            {
                _name = "Stealth Strike",
                _descriptiveName = "Stealth Strike",
                _triggerTiming = new string[] { "OnCalculateDealDamage" },
                _pathIcon = "SkillIcon/Sudden Attack",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Stealth Strike").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Stealth Strike").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.StealthStrike,
        },
        new _Skill //#Treasure_Tranquility
        {
            _parameter = new _Parameter
            {
                _name = "Tranquility",
                _descriptiveName = "Tranquility",
                _triggerTiming = new string[] { "OnCalculateDealDamage" },
                _pathIcon = "SkillIcon/Tranquility",
            },
            _miniTooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Tranquility").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Tranquility").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.SuddenAttack,
        },
        new _Skill //#Tumble
        {
            _parameter = new _Parameter
            {
                _name = "Tumble",
                _descriptiveName = "Tumble",
                _classType = new string[] { "Hunter" },
                _baseValue = 130,
                _moveRange = 3,
                _spRatio = 0.5f,
                _cooldownDuration = 4,
                _isQuickCast = true,
                _skillAbilitiesTable = new List<string>{ "Tumble+", "Move Range+15", "Cooldown-", "Gain Buff+20AD"  },
                _castType = "Direction",
                _pathIcon = "SkillIcon/Tumble",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Tumble").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Tumble").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Tumble,
            _functionSimulateEffect = SkillUtility.SimulateEffect_None,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_MoveTargetDirection,
            _functionDetectUnits = SkillUtility.DetectUnits_Null,
        },
        new _Skill //#Twilight
        {
            _parameter = new _Parameter
            {
                _name = "Twilight",
                _descriptiveName = "Twilight",
                _triggerTiming = new string[]{ "StartOfBattle" },
                _pathIcon = "SkillIcon/Undying Rage",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Twilight").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Twilight").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Twilight,
        },
        new _Skill //#Undying Rage
        {
            _parameter = new _Parameter
            {
                _name = "Undying Rage",
                _descriptiveName = "Undying Rage",
                _type = "Passive_EndOfYourTurn",
                _triggerTiming = new string[]{ "EndOfYourTurn" },
                _pathIcon = "SkillIcon/Undying Rage",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Undying Rage").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Undying Rage").GetIndexOf(1),
            },
            _functionDetectUnits = SkillUtility.DetectUnits_Null,
            _functionCastSkill = SkillActivate.UndyingRage,
        },
        new _Skill //#Vitality
        {
            _parameter = new _Parameter
            {
                _name = "Vitality",
                _descriptiveName = "Vitality",
                _type = "Passive_OnTakeBuffOrDisable",
                _triggerTiming = new string[]{ "OnTakenBuffOrDisable" },
                _pathIcon = "SkillIcon/Vitality",
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Vitality").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Vitality").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Vitality,
        },
        new _Skill //#Volley
        {
            _parameter = new _Parameter
            {
                _name = "Volley",
                _descriptiveName = "Volley",
                _classType = new string[] { "Hunter" },
                _damageType = "AD",
                _adRatio = 1,
                _hitRange = 6f,
                _angle = 60,
                _cooldownDuration = 5,
                _iCount = 5,
                _skillAbilitiesTable = new List<string>{ "AD Damage+", "Hit Range+", "Cooldown-", "AD Ratio+"  },
                _delayTimeToLaunchBullet = 0.55f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "AOE",
                _unitTypesTargetableList = new List<string> { "Enemy" },
                _pathIcon = "SkillIcon/Volley",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Volley").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Volley").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Volley,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_Volley,
            _functionDetectUnits = SkillUtility.DetectUnits_Volley,
        },
        new _Skill //#Whirlwind
        {
            _parameter = new _Parameter
            {
                _name = "Whirlwind",
                _descriptiveName = "Whirlwind",
                _classType = new string[]{ "Warrior" },
                _damageType = "AD",
                _adDamageBase = 40,
                _adRatio = 1.0f,
                _cooldownDuration = 5,
                _hitRange = 2.5f,
                _skillAbilitiesTable = new List<string>{ "AD Damage+20", "AD Ratio+0.2", "Hit Range+10", "Cooldown-1" },
                _delayTimeToDealDamage = 0.4f,
                _animatorSetTrigger = "triggerCastSkill00",
                _castType = "AOE",
                _unitTypesTargetableList = new List<string> { "Enemy", "Object" },
                _pathIcon = "SkillIcon/Whirlwind",
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Whirlwind").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Whirlwind").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.Whirlwind,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfHitCircle,
            _functionFindUnits = default,
            _functionDetectUnits = SkillUtility.DetectUnits_SelfHitCircle,
            _functionComputeBestPos = default
        },
        new _Skill //#Wind Blast
        {
            _parameter = new _Parameter
            {
                _name = "Wind Blast",
                _descriptiveName = "Wind Blast",
                _triggerTiming = new string[]{ "StartOfBattle" },
                _buffType = new List<string> { "SP" },
                _buffValue = new List<int> { 40 },
                _isShowAsIcon = false,
                _pathIcon = "SkillIcon/Wind Blast"
            },
            _tooltip = new _Tooltip
            {
                _type = "Passive",
                _title = TextData.DescriptionText.GetValue("Wind Blast").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Wind Blast").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.WindBlast,
        },
        new _Skill //#WolfAttack
        {
            _parameter = new _Parameter
            {
                _name = "Wolf Attack",
                _descriptiveName = "Basic Attack",
                _adRatio = 1.0f,
                _delayTimeToDealDamage = 0.5f,
                _targetRange = 2.0f,
                _animatorSetTrigger = "triggerAttack",
                _castType = "Target",
                _pathIcon = "SkillIcon/Enemy Attack",
                _unitTypesTargetableList = new List<string> { "Hero" },
            },
            _tooltip = new _Tooltip
            {
                _type = "Skill",
                _title = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(0),
                _effectText = TextData.DescriptionText.GetValue("Enemy Attack").GetIndexOf(1),
            },
            _functionCastSkill = SkillActivate.EnemyMeleeAttackNormal,
            _functionSimulateEffect = SkillUtility.SimulateEffect_NormalDamage,
            _functionDisplaySkillArea = SkillUtility.DisplaySkillArea_SelfTargetCircle,
            _functionFindUnits = SkillUtility.FindUnits_ClosestExcludeSelf,
            _functionDetectUnits = SkillUtility.DetectUnits_Closest,
            _functionComputeBestPos = SkillUtility.ComputeBestPos_Closest
        },
    };
}
