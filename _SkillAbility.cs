using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class _SkillAbility
{
    public static Dictionary<string, Sprite> spriteList = new Dictionary<string, Sprite>();

    public string _name;

    public int _iValue00;
    public float _fValue00;
    public string _string00;
    public bool _isActive = false;
    public string _activeTiming;
    public string _skillDescriptionAdditional = null;

    public string _pathIcon;
    public Sprite _Sprite
    {
        get
        {
            if (spriteList.ContainsKey(_pathIcon) == false)
            {
                Sprite sprite_ = (Resources.Load<Sprite>(_pathIcon) is Sprite out_) ? out_ : Prefabs.Instance.spDummy;
                spriteList.Add(_pathIcon, sprite_);
            }

            return spriteList[_pathIcon];
        }
    }

    public _Tooltip _tooltip;
    public _Tooltip _miniTooltip;

    public delegate void _FunctionEffect(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_ = null, int killCount_ = 0, bool isTrigger_ = false);
    public _FunctionEffect _functionEffect
    {
        get
        {
            return Function_SkillAbilityEffectList[_name];
        }
    }


    public static void ActivateAbility(_Unit unit_, _Skill skill_, _SkillAbility ability_)
    {
        if (Globals.statusTabIndex.IsBetween(1, Globals.heroList.Count) == false) return;
        if (skill_._parameter._isActive == false) return;
        if (skill_._CalculateAbilityPointRemaining(unit_) < 1) return;

        ability_._isActive = true;
        ability_._functionEffect?.Invoke(unit_, skill_, ability_, "Static");
        unit_._parameter._skills[skill_._parameter._rank]._parameter = skill_._parameter.DeepCopy();
    }

    public static _SkillAbility CloneFromString(string name_)
    {
        if (OriginalSkillAbilityList.ContainsKey(name_) == false)
        {
            Debug.Log("Invalid skill ablity name : " + name_);
            return null;
        }

        return OriginalSkillAbilityList[name_];
    }

    public static readonly Dictionary<string, _FunctionEffect> Function_SkillAbilityEffectList = new Dictionary<string, _FunctionEffect>()
    {
        { "AD Damage+", Ability_ADDamagePlus },
        { "AD Ratio+", Ability_ADRatioPlus },
        { "Armor Break", Ability_ArmorBreak },
        { "Basic Attack Bonus", Ability_BasicAttackBonus },
        { "Bloodlust", Ability_Bloodlust },
        { "Buff+", Ability_BuffPlus },
        { "Chain Range+", Ability_ChainRangePlus },
        { "Cooldown-", Ability_CooldownMinus },
        { "Debuff+", Ability_DebuffPlus },
        { "Execution+", Ability_ExecutionPlus },
        { "Gain Barrier", Ability_GainBarrier },
        { "Gain Buff", Ability_GainBuff },
        { "Heal", Ability_Heal },
        { "Hit Angle+", Ability_HitAnglePlus },
        { "Hit Count+", Ability_HitCountPlus },
        { "Hit Range+", Ability_HitRangePlus },
        { "Magic Burst", Ability_MagicBurst },
        { "Killing Spree", Ability_KillingSpree },
        { "MD Damage+", Ability_MDDamagePlus },
        { "MD Ratio+", Ability_MDRatioPlus },
        { "Move Range+", Ability_MoveRangePlus },
        { "Sword Break", Ability_SwordBreak },
        { "Tumble+", Ability_TumblePlus },
    };

    public static readonly Dictionary<string, _SkillAbility> OriginalSkillAbilityList = new Dictionary<string, _SkillAbility>()
    {
        {
            "AD Damage+",
            new _SkillAbility
            {
                _name = "AD Damage+",
                _iValue00 = 20,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "AD Damage+20",
            new _SkillAbility
            {
                _name = "AD Damage+",
                _iValue00 = 20,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "AD Damage+40",
            new _SkillAbility
            {
                _name = "AD Damage+",
                _iValue00 = 40,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "AD Damage+50",
            new _SkillAbility
            {
                _name = "AD Damage+",
                _iValue00 = 50,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "AD Ratio+",
            new _SkillAbility
            {
                _name = "AD Ratio+",
                _fValue00 = 0.2f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/AD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "AD Ratio+0.2",
            new _SkillAbility
            {
                _name = "AD Ratio+",
                _fValue00 = 0.2f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/AD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "AD Ratio+0.4",
            new _SkillAbility
            {
                _name = "AD Ratio+",
                _fValue00 = 0.4f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/AD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "AD Ratio+0.5",
            new _SkillAbility
            {
                _name = "AD Ratio+",
                _fValue00 = 0.5f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/AD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("AD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "Armor Break30",
            new _SkillAbility
            {
                _name = "Armor Break",
                _iValue00 = 30,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Armor Break",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Armor Break").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Armor Break").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Armor Break").GetIndexOf(1),
                }
            }
        },
        {
            "Armor Break40",
            new _SkillAbility
            {
                _name = "Armor Break",
                _iValue00 = 40,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Armor Break",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Armor Break").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Armor Break").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Armor Break").GetIndexOf(1),
                }
            }
        },
        {
            "Basic Attack Bonus Battlecry",
            new _SkillAbility
            {
                _name = "Basic Attack Bonus",
                _iValue00 = 120,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Basic Attack Bonus",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Basic Attack Bonus Battlecry").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Basic Attack Bonus Battlecry").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Basic Attack Bonus Battlecry").GetIndexOf(1),
                }
            }
        },
        {
            "Bloodlust+30",
            new _SkillAbility
            {
                _name = "Bloodlust",
                _iValue00 = 30,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Bloodlust",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Bloodlust").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Bloodlust").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Bloodlust").GetIndexOf(1),
                }
            }
        },
        {
            "Buff+30AD",
            new _SkillAbility
            {
                _name = "Buff+",
                _iValue00 = 30,
                _string00 = "AD",
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Buff+ AD",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Buff+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Buff+").GetIndexOf(1),
                }
            }
        },
        {
            "Buff+30AR",
            new _SkillAbility
            {
                _name = "Buff+",
                _iValue00 = 30,
                _string00 = "AR",
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Buff+ AR",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Buff+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Buff+").GetIndexOf(1),
                }
            }
        },
        {
            "Buff+30SP",
            new _SkillAbility
            {
                _name = "Buff+",
                _iValue00 = 30,
                _string00 = "SP",
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Buff+ SP",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Buff+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Buff+").GetIndexOf(1),
                }
            }
        },
        {
            "Chain Range+10",
            new _SkillAbility
            {
                _name = "Chain Range+",
                _iValue00 = 10,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Chain Range+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Chain Range+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Chain Range+").GetIndexOf(1),
                }
            }
        },
        {
            "Cooldown-",
            new _SkillAbility
            {
                _name = "Cooldown-",
                _iValue00 = 1,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Cooldown-",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Cooldown-").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Cooldown-").GetIndexOf(1),
                }
            }
        },
        {
            "Cooldown-1",
            new _SkillAbility
            {
                _name = "Cooldown-",
                _iValue00 = 1,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Cooldown-",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Cooldown-").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Cooldown-").GetIndexOf(1),
                }
            }
        },
        {
            "Debuff+30SP",
            new _SkillAbility
            {
                _name = "Debuff+",
                _iValue00 = 30,
                _string00 = "SP",
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Debuff+ SP",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Debuff+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Debuff+").GetIndexOf(1),
                }
            }
        },
        {
            "Execution+",
            new _SkillAbility
            {
                _name = "Execution+",
                _iValue00 = 15,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Upgrade",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Execution+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Execution+").GetIndexOf(1),
                }
            }
        },
        {
            "Gain Buff+20AD",
            new _SkillAbility
            {
                _name = "Gain Buff",
                _iValue00 = 20,
                _string00 = "AD",
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Buff+ AD",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Gain Buff").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Gain Buff").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Gain Buff").GetIndexOf(1),
                }
            }
        },
        {
            "Gain Barrier60",
            new _SkillAbility
            {
                _name = "Gain Barrier",
                _iValue00 = 60,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Gain Barrier",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Gain Barrier").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Gain Barrier").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Gain Barrier").GetIndexOf(1),
                }
            }
        },
        {
            "Heal0.2",
            new _SkillAbility
            {
                _name = "Heal",
                _fValue00 = 0.2f,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Heal",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Heal").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Heal").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Heal").GetIndexOf(1),
                }
            }
        },
        {
            "Hit Angle+15",
            new _SkillAbility
            {
                _name = "Hit Angle+",
                _iValue00 = 15,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Hit Angle+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Angle+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Angle+").GetIndexOf(1),
                }
            }
        },
        {
            "Hit Count+1",
            new _SkillAbility
            {
                _name = "Hit Count+",
                _iValue00 = 1,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Hit Count+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Hit Count+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Hit Count+").GetIndexOf(1),
                }
            }
        },
        {
            "Hit Range+",
            new _SkillAbility
            {
                _name = "Hit Range+",
                _iValue00 = 10,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Hit Range+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(1),
                }
            }
        },
        {
            "Hit Range+10",
            new _SkillAbility
            {
                _name = "Hit Range+",
                _iValue00 = 10,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Hit Range+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(1),
                }
            }
        },
        {
            "Hit Range+15",
            new _SkillAbility
            {
                _name = "Hit Range+",
                _iValue00 = 15,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Hit Range+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(1),
                }
            }
        },
        {
            "Hit Range+20",
            new _SkillAbility
            {
                _name = "Hit Range+",
                _iValue00 = 15,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Hit Range+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Hit Range+").GetIndexOf(1),
                }
            }
        },
        {
            "Killing Spree-1",
            new _SkillAbility
            {
                _name = "Killing Spree",
                _iValue00 = 1,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Killing Spree",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Killing Spree").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Killing Spree").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Killing Spree").GetIndexOf(1),
                }
            }
        },
        {
            "Killing Spree-2",
            new _SkillAbility
            {
                _name = "Killing Spree",
                _iValue00 = 2,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Killing Spree",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Killing Spree").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Killing Spree").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Killing Spree").GetIndexOf(1),
                }
            }
        },
        {
            "Magic Burst+40",
            new _SkillAbility
            {
                _name = "Magic Burst",
                _iValue00 = 40,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Magic Burst",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Magic Burst").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Magic Burst").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Magic Burst").GetIndexOf(1),
                }
            }
        },
        {
            "MD Damage",
            new _SkillAbility
            {
                _name = "MD Damage+",
                _iValue00 = 30,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Damage+30",
            new _SkillAbility
            {
                _name = "MD Damage+",
                _iValue00 = 30,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Damage+50",
            new _SkillAbility
            {
                _name = "MD Damage+",
                _iValue00 = 50,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Damage+60",
            new _SkillAbility
            {
                _name = "MD Damage+",
                _iValue00 = 60,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Base Damage+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Base Damage+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Ratio+",
            new _SkillAbility
            {
                _name = "MD Ratio+",
                _fValue00 = 0.2f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/MD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Ratio+0.1",
            new _SkillAbility
            {
                _name = "MD Ratio+",
                _fValue00 = 0.1f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/MD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Ratio+0.3",
            new _SkillAbility
            {
                _name = "MD Ratio+",
                _fValue00 = 0.3f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/MD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Ratio+0.4",
            new _SkillAbility
            {
                _name = "MD Ratio+",
                _fValue00 = 0.4f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/MD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "MD Ratio+0.6",
            new _SkillAbility
            {
                _name = "MD Ratio+",
                _fValue00 = 0.6f,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/MD Ratio+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("MD Ratio+").GetIndexOf(1),
                }
            }
        },
        {
            "Move Range+15",
            new _SkillAbility
            {
                _name = "Move Range+",
                _iValue00 = 15,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Move Range+",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Move Range+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Move Range+").GetIndexOf(1),
                }
            }
        },
        {
            "Sword Break30",
            new _SkillAbility
            {
                _name = "Sword Break",
                _iValue00 = 30,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Sword Break",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Sword Break").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Sword Break").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Sword Break").GetIndexOf(1),
                }
            }
        },
        {
            "Sword Break40",
            new _SkillAbility
            {
                _name = "Sword Break",
                _iValue00 = 40,
                _activeTiming = "AfterCastSkill",
                _pathIcon = "SkillAbilityIcon/Sword Break",
                _skillDescriptionAdditional = "\n" + TextData.DescriptionText.GetValue("Sword Break").GetIndexOf(1),
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Sword Break").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Sword Break").GetIndexOf(1),
                }
            }
        },
        {
            "Tumble+",
            new _SkillAbility
            {
                _name = "Tumble+",
                _iValue00 = 20,
                _activeTiming = "Static",
                _pathIcon = "SkillAbilityIcon/Upgrade",
                _miniTooltip = new _Tooltip
                {
                    _type = "SkillAbility",
                    _title = TextData.DescriptionText.GetValue("Tumble+").GetIndexOf(0),
                    _effectText = TextData.DescriptionText.GetValue("Tumble+").GetIndexOf(1),
                }
            }
        },
    };


    public static void Ability_ADDamagePlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._adDamageBase += ability_._iValue00;
    }
    public static void Ability_ADRatioPlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._adRatio += ability_._fValue00;
    }
    public static void Ability_ArmorBreak(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;
        if (unitsList_ == null) return;

        foreach (_Unit unit_i_ in unitsList_)
        {
            unit_i_._GainBuff("AR", -ability_._iValue00);
        }
    }
    public static void Ability_BasicAttackBonus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        foreach (_Skill passive_i_ in unit_._parameter._additionalPassives)
        {
            if (passive_i_._parameter._name == "Basic Attack Bonus Battlecry")
                return;
        }
        unit_._parameter._additionalPassives.Add(_Skill.OriginalSkillList.Find(m => m._parameter._name == "Basic Attack Bonus Battlecry"));
    }
    public static void Ability_Bloodlust(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        if (killCount_ > 0)
            unit_._GainBuff("AD", ability_._iValue00 * killCount_);
    }
    public static void Ability_BuffPlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        for (int i = 0; i < skill_._parameter._buffType.Count; i++)
        {
            if (skill_._parameter._buffType[i] == ability_._string00)
                skill_._parameter._buffValue[i] += ability_._iValue00;
        }
    }
    public static void Ability_CooldownMinus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._cooldownDuration -= ability_._iValue00;
    }
    public static void Ability_ChainRangePlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._chainRange += ability_._iValue00 / 10f;
    }
    public static void Ability_DebuffPlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        for (int i = 0; i < skill_._parameter._buffType.Count; i++)
        {
            if (skill_._parameter._buffType[i] == ability_._string00)
                skill_._parameter._buffValue[i] -= ability_._iValue00;
        }
    }
    public static void Ability_ExecutionPlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._iValue += ability_._iValue00;
    }
    public static void Ability_Heal(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        unit_._Heal(unit_, (unit_._parameter._hpMax * ability_._fValue00).ToInt(), null);
    }
    public static void Ability_HitAnglePlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._angle += ability_._iValue00;
    }
    public static void Ability_HitRangePlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._hitRange += ability_._iValue00 / 10f;
    }
    public static void Ability_GainBarrier(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        unit_._BarrierValue += ability_._iValue00;
    }
    public static void Ability_GainBuff(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        unit_._GainBuff(ability_._string00, ability_._iValue00);
    }
    public static void Ability_HitCountPlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._hitCount += ability_._iValue00;
    }
    public static void Ability_KillingSpree(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._cooldownRemaining -= ability_._iValue00 * killCount_;
    }
    public static void Ability_MagicBurst(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        if (killCount_ > 0)
            unit_._GainBuff("MD", ability_._iValue00 * killCount_);
    }
    public static void Ability_MDDamagePlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._mdDamageBase += ability_._iValue00;
    }
    public static void Ability_MDRatioPlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._mdRatio += ability_._fValue00;
    }
    public static void Ability_MoveRangePlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._moveRange += ability_._iValue00 / 10f;
    }
    public static void Ability_SwordBreak(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;
        if (unitsList_ == null) return;

        foreach (_Unit unit_i_ in unitsList_)
        {
            unit_i_._GainBuff("AD", -ability_._iValue00);
        }
    }
    public static void Ability_TumblePlus(_Unit unit_, _Skill skill_, _SkillAbility ability_, string timing_, List<_Unit> unitsList_, int killCount_, bool isTrigger_)
    {
        if (ability_._activeTiming != timing_) return;
        if (ability_._isActive == false) return;

        skill_._parameter._baseValue += ability_._iValue00;
    }


}
