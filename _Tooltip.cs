using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class _Tooltip
{
    public string _type;
    public string _placeType;
    public string _title;
    [TextArea] public string _effectText;

    public static string RichDescriptionText(string text_)
    {
        if (text_ == null) return text_;

        string result = text_;

        result = result.Replace(TextData.Keywords.GetValue("After Cast Skill"), "<color=yellow>" + TextData.Keywords.GetValue("After Cast Skill") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("End of Your Turn"), "<color=yellow>" + TextData.Keywords.GetValue("End of Your Turn") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("End of Enemy Turn"), "<color=yellow>" + TextData.Keywords.GetValue("End of Enemy Turn") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("End of Turn"), "<color=yellow>" + TextData.Keywords.GetValue("End of Turn") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("magic damage"), "<color=#ff00e0ff>" + TextData.Keywords.GetValue("magic damage") + "</color> ");
        result = result.Replace(TextData.Keywords.GetValue("On Cast Skill"), "<color=yellow>" + TextData.Keywords.GetValue("On Cast Skill") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Deal Damage"), "<color=yellow>" + TextData.Keywords.GetValue("On Deal Damage") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Deal Magic Damage"), "<color=yellow>" + TextData.Keywords.GetValue("On Deal Magic Damage") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Deal Physical Damage"), "<color=yellow>" + TextData.Keywords.GetValue("On Deal Physical Damage") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Death"), "<color=yellow>" + TextData.Keywords.GetValue("On Death") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Destroy Object"), "<color=yellow>" + TextData.Keywords.GetValue("On Destroy Object") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Taken Basic Attack"), "<color=yellow>" + TextData.Keywords.GetValue("On Taken Basic Attack") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Taken Damage"), "<color=yellow>" + TextData.Keywords.GetValue("On Taken Damage") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Taken Physical Damage"), "<color=yellow>" + TextData.Keywords.GetValue("On Taken Physical Damage") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Taken Magic Damage"), "<color=yellow>" + TextData.Keywords.GetValue("On Taken Magic Damage") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("On Kill Enemy"), "<color=yellow>" + TextData.Keywords.GetValue("On Kill Enemy") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("physical damage"), "<color=#ff5000ff>" + TextData.Keywords.GetValue("physical damage") + "</color>");
        result = result.Replace("<" + TextData.Keywords.GetValue("protection") + ">", "<color=yellow>" + TextData.Keywords.GetValue("protection") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("Quick cast"), "<color=green>" + TextData.Keywords.GetValue("Quick cast") + "</color>");
        result = result.Replace(TextData.Keywords.GetValue("static damage"), "<color=#00ffffff>" + TextData.Keywords.GetValue("static damage") + "</color> ");
        result = result.Replace(TextData.Keywords.GetValue("Start of Turn"), "<color=yellow>" + TextData.Keywords.GetValue("Start of Turn") + "</color> ");
        result = result.Replace(TextData.Keywords.GetValue("Start of Battle"), "<color=yellow>" + TextData.Keywords.GetValue("Start of Battle") + "</color> ");
        result = result.Replace(TextData.Keywords.GetValue("stun"), "<color=yellow>" + TextData.Keywords.GetValue("stun") + "</color>");

        result = result.Replace("<flavor>", "<i><color=#00ffff>").Replace("</flavor>", "</i></color>");

        return result;
    }

    public static string MakeTextFromEquip(_Equip equip_)
    {
        string result = "";
        //string result = "<align=center><color=green>- " + equip_._type + " Equip -</color></align>\n";

        result += ((equip_._hpPlus != 0) ? "<sprite name=HP>+" + equip_._hpPlus + "\n" : "") +
                  ((equip_._adPlus != 0) ? "<sprite name=AD>+" + equip_._adPlus + "\n" : "") +
                  ((equip_._arPlus != 0) ? "<sprite name=AR>+" + equip_._arPlus + "\n" : "") +
                  ((equip_._mdPlus != 0) ? "<sprite name=MD>+" + equip_._mdPlus + "\n" : "") +
                  ((equip_._mrPlus != 0) ? "<sprite name=MR>+" + equip_._mrPlus + "\n" : "") +
                  ((equip_._spPlus != 0) ? "<sprite name=SP>+" + equip_._spPlus + "\n" : "");

        result += "\n" + equip_._tooltip._effectText;

        result = ReplaceTag_Equip(equip_, result);

        return result;
    }

    public static string ReplaceTag_Equip(_Equip equip_, string text_ = "")
    {
        string result = (text_ != "") ? text_ : equip_._tooltip._effectText;

        result = result.Replace("<Unique Passive>", "<color=#00ffff>" + TextData.Keywords.GetValue("UNIQUE Passive") + " - ").Replace("</Unique Passive>", ":</color>\n");
        result = result.Replace("<Unique Active>", "<color=#00ff00>" + TextData.Keywords.GetValue("UNIQUE Active") + " - ").Replace("</Unique Active>", ":</color>\n");

        if (equip_._active != null && equip_._active._tooltip._title.IsNullOrEmpty() == false)
        {
            string skillText_ = MakeTextFromSkill(equip_._active);
            result = result.Replace("<Description>" + equip_._active._parameter._name + "</Description>", skillText_);
        }
        if (equip_._passive != null && equip_._passive._tooltip._title.IsNullOrEmpty() == false)
        {
            string skillText_ = MakeTextFromSkill(equip_._passive);
            result = result.Replace("<Description>" + equip_._passive._parameter._name + "</Description>", skillText_);
        }

        return result;
    }

    public static string MakeDescriptionText_StatusEquip(_Equip equip_)
    {
        string result = "";
        string text_ = ((equip_._hpPlus != 0) ? "<sprite name=HP><mspace=11>+" + equip_._hpPlus.ToString("-XX;+XX") + "</mspace>\n" : "") +
                       ((equip_._adPlus != 0) ? "<sprite name=AD><mspace=11>+" + equip_._adPlus.ToString("-XX;+XX") + "</mspace>\n" : "") +
                       ((equip_._arPlus != 0) ? "<sprite name=AR><mspace=11>+" + equip_._arPlus.ToString("-XX;+XX") + "</mspace>\n" : "") +
                       ((equip_._mdPlus != 0) ? "<sprite name=MD><mspace=11>+" + equip_._mdPlus.ToString("-XX;+XX") + "</mspace>\n" : "") +
                       ((equip_._mrPlus != 0) ? "<sprite name=MR><mspace=11>+" + equip_._mrPlus.ToString("-XX;+XX") + "</mspace>\n" : "") +
                       ((equip_._spPlus != 0) ? "<sprite name=SP><mspace=11>+" + equip_._spPlus.ToString("-XX;+XX") + "</mspace>\n" : "");

        string[] parts_ = result.Split('\n');
        for (int i = 0; i < parts_.Length; i++)
        {
            if (i > 2)
            {
                result += "<line-height=12>\n</line-height><align=right>...</align>";
                break;
            }

            result += "\n" + parts_[i];
        }
        result.Trim('\n');

        return result;
    }

    public static string ReplaceTag_Skill(string text_, _Skill skill_)
    {
        if (text_ == null) return "";
        string result = text_;

        result = result.Replace("<AD Base>", skill_._parameter._adDamageBase.ToString());
        result = result.Replace("<MD Base>", skill_._parameter._mdDamageBase.ToString());

        result = result.Replace("<HP Ratio>", skill_._parameter._hpRatio.ToString("F2"));
        result = result.Replace("<AD Ratio>", skill_._parameter._adRatio.ToString("F2"));
        result = result.Replace("<AR Ratio>", skill_._parameter._arRatio.ToString("F2"));
        result = result.Replace("<MD Ratio>", skill_._parameter._mdRatio.ToString("F2"));
        result = result.Replace("<MR Ratio>", skill_._parameter._mrRatio.ToString("F2"));
        result = result.Replace("<SP Ratio>", skill_._parameter._spRatio.ToString("F2"));

        result = result.Replace("<iValue>", skill_._parameter._iValue.ToString());
        result = result.Replace("<iCount>", skill_._parameter._iCount.ToString());
        result = result.Replace("<String>", skill_._parameter._string);
        result = result.Replace("<Hit Count>", skill_._parameter._hitCount.ToString());
        result = result.Replace("<Base Value>", skill_._parameter._baseValue.ToString());
        result = result.Replace("<Barrier Value>", skill_._parameter._barrierValueBase.ToString());
        result = result.Replace("<Restore Value>", skill_._parameter._restoreValueBase.ToString());
        result = result.Replace("<Move Range>", (skill_._parameter._moveRange * 10).ToString());

        if (skill_._parameter._buffValue.Count > 0)
            result = result.Replace("<Buff Value00>", skill_._parameter._buffValue[0].ToString());
        if (skill_._parameter._buffValue.Count > 1)
            result = result.Replace("<Buff Value01>", skill_._parameter._buffValue[1].ToString());

        return result;
    }

    public static string MakeTextFromSkill(_Skill skill_)
    {
        string result = "";
        string text_ = "";
        string out_;

        if (skill_._parameter._classType.Length > 0)
        {
            result += "<color=#00ffffff><align=center>-";
            for (int i = 0; i < skill_._parameter._classType.Length; i++)
            {
                result += skill_._parameter._classType[i] + " ";
            }
            result += "Class Skill-</align></color>\n";
        }
        else if (skill_._parameter._type == "Class Passive")
        {
            result += "<color=#00ffffff><align=center>-Passive-</align></color>\n";
        }

        result += skill_._tooltip._effectText;

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
        {
            if (ability_i_ == null) continue;
            if (ability_i_._isActive == false) continue;
            if (ability_i_._skillDescriptionAdditional.IsNullOrEmpty()) continue;

            result += ReplaceTag_SkillAbility(ability_i_._skillDescriptionAdditional, ability_i_);
        }


        if (skill_._parameter._castableStacks == 1 && TextData.Keywords.TryGetValue("Can cast once", out out_))
            text_ += out_ + "\n";

        if (skill_._parameter._isQuickCast == true && TextData.Keywords.TryGetValue("Quick cast", out out_))
            text_ += out_ + "\n";

        if (skill_._parameter._isCastableWhileDisabled == true && TextData.Keywords.TryGetValue("Can cast while disable", out out_))
            text_ += out_ + "\n";

        if (skill_._parameter._targetRange != 0 && TextData.Keywords.TryGetValue("Target range", out out_))
            text_ += out_ + ":" + (skill_._parameter._targetRange * 10).ToInt() + "\n";

        if (skill_._parameter._hitRange != 0 && TextData.Keywords.TryGetValue("Hit range", out out_))
            text_ += out_ + ":" + (skill_._parameter._hitRange * 10).ToInt() + "\n";

        if (skill_._parameter._chainRange != 0 && TextData.Keywords.TryGetValue("Chain range", out out_))
            text_ += out_ + ":" + (skill_._parameter._chainRange * 10).ToInt() + "\n";

        if (skill_._parameter._knockbackRange != 0 && TextData.Keywords.TryGetValue("Knockback range", out out_))
            text_ += out_ + ":" + (skill_._parameter._knockbackRange * 10).ToInt() + "\n";

        if (skill_._parameter._angle != 0 && TextData.Keywords.TryGetValue("Angle", out out_))
            text_ += out_ + ":" + skill_._parameter._angle + "°\n";

        if (skill_._parameter._cooldownDuration != 0 && TextData.Keywords.TryGetValue("Cooldown", out out_))
            text_ += out_ + ":" + skill_._parameter._cooldownDuration + "\n";

        if (text_ != "")
            result += "\n\n" + text_;

        result = ReplaceTag_Skill(result, skill_);

        return result;
    }

    public static string ReplaceTag_SkillAbility(string text_, _SkillAbility ability_)
    {
        if (text_ == null) return "";
        string result = text_;

        result = result.Replace("<Base Value>", ability_._iValue00.ToString());
        result = result.Replace("<iValue00>", ability_._iValue00.ToString());
        result = result.Replace("<fValue00>", ability_._fValue00.ToString("F2"));
        result = result.Replace("<string00>", ability_._string00);

        return result;
    }

    public static string ReplaceTag_Item(string text_, _Item item_)
    {
        if (text_ == null) return "";
        string result = text_;

        int powerCoef_ = (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Potion Mastery") == null) ? 1 : 2;

        result = result.Replace("<AD Damage>", (item_._adDamage * powerCoef_).ToString());
        result = result.Replace("<MD Damage>", (item_._mdDamage * powerCoef_).ToString());
        result = result.Replace("<SD Damage>", (item_._sdDamage * powerCoef_).ToString());
        result = result.Replace("<Restore Value>", (item_._restoreValue * powerCoef_).ToString());
        result = result.Replace("<iValue00>", (item_._iValue00 * powerCoef_).ToString());
        result = result.Replace("<Hit Range>", (item_._hitRange * 10).ToString());

        if (item_._buffTypes.Count > 0)
        {
            result = result.Replace("<Buff Value00>", item_._buffValues[0].ToString());
            result = result.Replace("<Buff Type00>", item_._buffTypes[0]);
        }

        return result;
    }

    public static string ReplaceTag_GlobalEffect(string text_, _Skill skill_)
    {
        if (text_ == null) return "";
        string result = text_;

        result = result.Replace("<iValue>", skill_._parameter._iValue.ToString());
        result = result.Replace("<iCount>", skill_._parameter._iCount.ToString());

        string heroNames = "";
        foreach (int index_i_ in skill_._parameter._targetHeroIndex)
        {
            if (heroNames != "")
                heroNames = heroNames + ", ";

            heroNames = heroNames + Globals.heroList[index_i_]._parameter._class;
        }

        result = result.Replace("<Target Hero>", heroNames);

        return result;
    }

    public static string ReplaceTag_EventText(string text_, _Event._choice choice_)
    {
        if (text_ == null) return "";
        if (choice_ == null) return text_;

        string result = text_;

        result = result.Replace("<Gold>", choice_._goldValue.ToString());
        result = result.Replace("<Equip Name>", choice_._equipName);
        result = result.Replace("<Food Count>", choice_._foodCount.ToString());
        result = result.Replace("<Hero>", Globals.heroList[choice_._heroIndex]._parameter._class);

        return result;
    }
}
