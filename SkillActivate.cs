using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class SkillActivate
{
    public static void OnEntryCastSkill(_Unit unit_, _Skill skill_)
    {
        Globals.triggerStateBasedAction = true;
        Battle.SetInactiveSkillSuggestion(Globals.unitOnActive);
        skill_._parameter._damageCoef = 1;
        if (Globals.equipOnActive is _Equip equip_)
        {
            equip_._castableLimitCount = (equip_._castableLimitCount - 1).Clamp(0, 99);
            equip_._castableStacks = (equip_._castableStacks).Clamp(0, 99);
            equip_._cooldownRemaining = equip_._cooldownDuration;
        }
        else
        {
            skill_._parameter._cooldownRemaining = skill_._parameter._cooldownDuration;

            if (skill_._parameter._descriptiveName != "Basic Attack")
            {
                for (int i = 0; i < unit_._parameter._classPassives.Count; i++)
                {
                    if (unit_._parameter._classPassives[i]._parameter._triggerTiming?.Contains("OnCastSkill") == false) continue;

                    General.Instance.StartCoroutine(Battle.ActivatePassive(unit_, unit_._parameter._classPassives[i], refTiming_: "OnCastSkill"));
                }

                for (int i = 0; i < unit_._parameter._additionalPassives.Count; i++)
                {
                    if (unit_._parameter._additionalPassives[i]._parameter._triggerTiming?.Contains("OnCastSkill") == false) continue;

                    General.Instance.StartCoroutine(Battle.ActivatePassive(unit_, unit_._parameter._additionalPassives[i], refTiming_: "OnCastSkill"));
                }
            }
        }

        if (skill_._parameter._animatorSetBool.IsNullOrEmpty() == false)
            unit_._animator.SetBool(skill_._parameter._animatorSetBool, true);

        if (skill_._parameter._animatorSetTrigger.IsNullOrEmpty() == false)
            unit_._animator.SetTrigger(skill_._parameter._animatorSetTrigger);

        if (unit_ is _Hero hero)
        {
            Globals.inputStopperCount++;

            if (skill_._parameter._isQuickCast == false)
            {
                unit_._parameter._movableCount = 0;
                unit_._parameter._actableCount -= 1;
                unit_._parameter._statusConditions.Find(m => m._name == "Stealth")._count = 0;
                //unit_._parameter._stealthCount = 0;
                unit_._SetAnimatorCondition();
                unit_._DisplayBuffAndStatusIcon();
            }

            Globals.Instance.paramsOnStackList.Clear();
            Globals.Instance.itemsOnStackList.Clear();
        }
        else if (unit_ is _Enemy enemy_)
        {

        }

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            unit_i_._damagePopup = 0;
            unit_i_._healingPopup = 0;
        }
    }

    public static void OnExitCastSkill(_Unit unit_, _Skill skill_)
    {
        if (skill_._parameter._animatorSetBool.IsNullOrEmpty() == false)
            unit_._animator.SetBool(skill_._parameter._animatorSetBool, false);

        if (unit_ is _Hero hero)
        {
            Globals.inputStopperCount--;
        }
        else if (unit_ is _Enemy enemy_)
        {

        }

        unit_._skillOnActive = skill_;
        if (Globals.equipOnActive == null)
        {
            for (int i = 0; i < unit_._parameter._classPassives.Count; i++)
            {
                _Skill passive_i_ = unit_._parameter._classPassives[i];
                if (passive_i_._parameter._triggerTiming?.Contains("AfterCastSkill") == false) continue;

                General.Instance.StartCoroutine(Battle.ActivatePassive(unit_, passive_i_, refTiming_: "AfterCastSkill", refSkill_: skill_));
            }

            for (int i = unit_._parameter._additionalPassives.Count - 1; i >= 0; i--)
            {
                _Skill passive_i_ = unit_._parameter._additionalPassives[i];
                if (passive_i_._parameter._triggerTiming?.Contains("AfterCastSkill") == false) continue;

                General.Instance.StartCoroutine(Battle.ActivatePassive(unit_, passive_i_, refTiming_: "AfterCastSkill", refSkill_: skill_));
            }

            foreach (_Skill passive_i_ in new List<_Skill>(Globals.Instance.globalEffectList))
            {
                if (passive_i_._parameter._triggerTiming?.Contains("AfterCastSkill") == false) continue;

                General.Instance.StartCoroutine(Battle.ActivatePassive(unit_, passive_i_, refTiming_: "AfterCastSkill", refSkill_: skill_));
            }
        }

        //unit_._parameter._statusConditions.Find(m => m._name == "Fury")._count = (unit_._parameter._statusConditions.Find(m => m._name == "Fury")._count - 1).Clamp(0, 100);

        //unit_._SetAnimatorCondition();
        Battle.SetInactiveSkillSuggestion(unit_);
        Globals.equipOnActive = null;
    }

    public static void OnEntryActivatePassive(_Unit unit_, _Skill skill_)
    {
        Globals.triggerStateBasedAction = true;
        Globals.inputStopperCount++;

        if (skill_._parameter._animatorSetBool != "" && skill_._parameter._animatorSetBool != null)
            unit_._animator.SetBool(skill_._parameter._animatorSetBool, true);

        if (skill_._parameter._animatorSetTrigger != "" && skill_._parameter._animatorSetTrigger != null)
            unit_._animator.SetTrigger(skill_._parameter._animatorSetTrigger);

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            unit_i_._damagePopup = 0;
            unit_i_._healingPopup = 0;
        }
    }

    public static void OnExitActivatePassive(_Unit unit_, _Skill skill_)
    {
        Globals.inputStopperCount--;
    }

    public static IEnumerator Abyss(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;
        if (skill_._refType != "MD") yield break;

        Globals.resistCoefOnCalcDamage *= 0.5f;
    }

    public static IEnumerator Adrenaline(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (unitSelf_._parameter._hp > unitSelf_._parameter._hpMax / 2)
        {
            unitSelf_._parameter._arApplied += 20;
            unitSelf_._parameter._mrApplied += 20;
        }
    }

    public static IEnumerator Aegis(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        unitSelf_._GainStatus("Protection", 1);
    }

    public static IEnumerator Assassinate(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        unitSelf_._GainStatus("Stealth", 1);
    }

    public static IEnumerator BasicAttackBonus(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._refSkill._parameter._descriptiveName != "Basic Attack") yield break;

        if (skill_._refTiming == "OnCalculateDealDamage")
        {
            Globals.damageCoefOnCalcDamage *= skill_._parameter._baseValue / 100f;
        }
        if (skill_._refTiming == "AfterCastSkill")
        {
            for (int i = 0; i < unitSelf_._parameter._additionalPassives.Count; i++)
            {
                _Skill passive_i_ = unitSelf_._parameter._additionalPassives[i];

                if (passive_i_._parameter._name == skill_._parameter._name)
                {
                    unitSelf_._parameter._additionalPassives.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public static IEnumerator ArtifactWard(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ != null) yield break;

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            unit_i_._GainStatus("Artifact", 1);
        }
    }

    public static IEnumerator BattleFrenzy(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        unitSelf_._parameter._adApplied += 30;
    }

    public static IEnumerator BattleRage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        skill_._parameter._iCount++;
        UI.ConfigureGlobalEffectUI();

        if (skill_._parameter._iCount < 4) yield break;
        skill_._parameter._iCount = 0;
        UI.ConfigureGlobalEffectUI();

        List<Coroutine> runningCoroutineList_ = new List<Coroutine>();

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            for (int j = 0; j < skill_._parameter._buffValue.Count; j++)
            {
                runningCoroutineList_.Add(General.Instance.StartCoroutine(StartOfBattle_BuffHeros_ExecuteBuff(unit_i_, skill_._parameter._buffType[j], skill_._parameter._buffValue[j])));
                yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);
            }
        }

        foreach (Coroutine c in runningCoroutineList_)
            yield return c;

        yield return new WaitForSeconds(1 / Globals.Instance.gameSpeed);

        IEnumerator StartOfBattle_BuffHeros_ExecuteBuff(_Unit unit_i_, string buffType_, int buffValue_)
        {
            GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_" + skill_._parameter._effectColor), unit_i_._goComps.transform);
            Effect_.transform.localScale = skill_._parameter._effectSize.ToVector3();

            yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

            unit_i_._GainBuff(buffType_, buffValue_);
        }
    }

    public static IEnumerator Berserker(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_[0]._IsAlive() == false) yield break;

        unitSelf_._GainBuff("AD", 10);
    }

    public static IEnumerator BerserkerSoul(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if(skill_._refTiming == "StartOfBattle")
        {
            skill_._parameter._iCount = 0;
        }
        else if (skill_._refTiming == "AfterCastSkill")
        {
            if (unitSelf_._parameter._unitType != "Hero") yield break;

            skill_._parameter._iCount = (skill_._parameter._iCount + 1).Mod(10);
        }
        else if (skill_._refTiming == "OnCalculateDealDamage")
        {
            if (skill_._parameter._iCount == 9)
                Globals.damageCoefOnCalcDamage *= 2;
        }
    }

    public static IEnumerator BigGameHunter(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        bool isBossBattle_ = false;

        foreach (_Unit unit_i_ in Globals.enemyList)
        {
            if (unit_i_._parameter.isBoss == true)
            {
                isBossBattle_ = true;
                break;
            }
        }

        if (isBossBattle_ == true)
        {
            foreach (_Unit unit_i_ in Globals.heroList)
            {
                unit_i_._parameter._adApplied += 30;
                unit_i_._parameter._mdApplied += 30;
            }
        }
    }

    public static IEnumerator BlazingStar(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        unitSelf_._parameter._sp += (unitSelf_._parameter._md * 0.1f).ToInt();
    }

    public static IEnumerator Bleeding(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ != null) yield break;

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            _Unit.DealDamage(null, unit_i_, (unit_i_._parameter._hpMax * 0.3f).ToInt(), skill_, "Static");
        }

        Globals.Instance.globalEffectList.Remove(skill_);
        UI.ConfigureGlobalEffectUI();
    }

    public static IEnumerator Blessing(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._refTiming == "OnApplyBuff")
        {
            if (skill_._parameter._targetHeroIndex.Contains(Globals.unitList.IndexOf(unitSelf_)) == false) yield break;

            unitSelf_._parameter._adApplied = (unitSelf_._parameter._adApplied * (100 + skill_._parameter._iValue) / 100f).ToInt();
            unitSelf_._parameter._arApplied = (unitSelf_._parameter._arApplied * (100 + skill_._parameter._iValue) / 100f).ToInt();
            unitSelf_._parameter._mdApplied = (unitSelf_._parameter._mdApplied * (100 + skill_._parameter._iValue) / 100f).ToInt();
            unitSelf_._parameter._mrApplied = (unitSelf_._parameter._mrApplied * (100 + skill_._parameter._iValue) / 100f).ToInt();
            unitSelf_._parameter._spApplied = (unitSelf_._parameter._spApplied * (100 + skill_._parameter._iValue) / 100f).ToInt();
        }
        else if (skill_._refTiming == "EndOfBattle")
        {
            if (skill_._parameter._iCount == 99) yield break;

            if (--skill_._parameter._iCount < 1)
                Globals.Instance.globalEffectList.Remove(skill_);
            UI.ConfigureGlobalEffectUI();
        }
    }

    public static IEnumerator BlindSpore(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_?.Count < 1) yield break;

        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        skill_._functionDisplaySkillArea(unitSelf_, unitsOnTargetList_[0].transform.position, skill_);
        yield return new WaitForSeconds(0.6f / Globals.Instance.gameSpeed);

        OnEntryCastSkill(unitSelf_, skill_);
        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);
        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);
        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "BlindSpore"), unitSelf_._posCenter, unitSelf_._modelTransform.transform.rotation);
        //Effect_.transform.LookAt(unitOnTarget_._posCenter);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_, skill_, damageBase_, unitOnTarget_._parameter._mrApplied, "AP");
        unitSelf_._DealDamage(unitOnTarget_, damage_, skill_, "Magic");

        unitOnTarget_._GainStatus("Blind", 1);

        //for (int i = 0; i < skill_._parameter._buffType.Count; i++)
        //{
        //    unitOnTarget_._GainBuff(skill_._parameter._buffType[i], skill_._parameter._buffValue[i]);
        //}

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator CallMinions(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        OnEntryCastSkill(unitSelf_, skill_);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToResolveEffect / Globals.Instance.gameSpeed);

        List<_Enemy> enemyNewList_ = new List<_Enemy>();
        List<Vector3> posStartSave_ = new List<Vector3>();
        List<Coroutine> coroutines_ = new List<Coroutine>();

        for (int i = 0; i < 3; i++)
        {
            _Enemy enemyNew = _Unit.CloneFromString("Wolf") as _Enemy;
            enemyNew._posSOB = new Vector3(+6, +0, +0);
            enemyNew._posDistSOB = new Vector3(+4, +0, +5);
            enemyNew._parameter._expEnemyHave = 0;
            enemyNew._parameter._goldEnemyHave = 0;

            enemyNew._PlaceRandomlyOrDestroy(out bool isDestroy_);

            if (isDestroy_ == false)
            {
                enemyNewList_.Add(enemyNew);
                posStartSave_.Add(enemyNew.transform.position);
            }
        }

        for (int i = 0; i < enemyNewList_.Count; i++)
        {
            enemyNewList_[i].transform.position = enemyNewList_[i].transform.position + new Vector3(+15, 0, 0);

            coroutines_.Add(General.Instance.StartCoroutine(enemyNewList_[i]._MoveHeroTo(posStartSave_[i])));
        }

        foreach (Coroutine c_ in coroutines_)
            yield return c_;
    }

    //public static IEnumerator Carnage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    //{
    //    if (skill_ == null) yield break;
    //    if (unitSelf_._parameter._unitType != "Hero") yield break;

    //    skill_._parameter._cooldownRemaining = (skill_._parameter._cooldownRemaining - 1).Clamp(0, 99);
    //}

    public static IEnumerator ChainLightning(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);

        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        List<_Unit> unitAlreadyTargeted = new List<_Unit>() { unitOnTarget_ };
        Vector3 vector_ = unitOnTarget_._posCenter - unitSelf_._posCenter;
        Vector3 posStart_ = unitSelf_._posCenter + vector_.normalized * 1f;
        Vector3 posEnd_ = unitOnTarget_._posCenter;
        List<Coroutine> runningCoutourine_ = new List<Coroutine>();
        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        unitSelf_._modelTransform.LookAt(unitOnTarget_.transform.position);

        GameObject Projectile_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "ChainLightning_Projectile"), posStart_, Quaternion.identity);
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        unitSelf_._animator.SetTrigger("triggerAttack");
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        yield return unitOnTarget_.StartCoroutine(General.MoveTowards(Projectile_, posEnd_, Globals.PRJ_MOVE_PER_SEC * 0.8f));
        UnityEngine.Object.Destroy(Projectile_, 0.5f / Globals.Instance.gameSpeed);

        int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_, skill_, damageBase_, unitOnTarget_._parameter._mrApplied, "MD");
        GameObject Explosion_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "ChainLightning_Explosion"), unitOnTarget_._posCenter, Quaternion.identity);
        unitSelf_._DealDamage(unitOnTarget_, damage_, skill_, "Magic");
        runningCoutourine_.Add(General.Instance.StartCoroutine(ChainLightning_DetectUnits(unitOnTarget_)));

        for (int i = 0; i < runningCoutourine_.Count; i++)
            yield return runningCoutourine_[i];

        yield return new WaitForSeconds(0.4f);

        OnExitCastSkill(unitSelf_, skill_);

        IEnumerator ChainLightning_DetectUnits(_Unit unitStart_)
        {
            foreach (_Unit unit_i_ in Globals.unitList)
            {
                if (unitAlreadyTargeted.Contains(unit_i_)) continue;
                if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
                if ((unit_i_.transform.position - unitStart_.transform.position).sqrMagnitude > (skill_._parameter._chainRange + unit_i_._parameter._colliderRange).Square()) continue;

                unitAlreadyTargeted.Add(unit_i_);
                runningCoutourine_.Add(General.Instance.StartCoroutine(ChainLightning_ChainDamage(unitStart_, unit_i_)));
            }

            yield return null;
        }

        IEnumerator ChainLightning_ChainDamage(_Unit unitStart_, _Unit unitToChain_)
        {
            LineRenderer lrChain_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "ChainLightning_Chain")).GetComponent<LineRenderer>();
            Vector3 posMiddle_ = unitStart_._posCenter;

            while ((posMiddle_ - unitToChain_._posCenter).sqrMagnitude > Globals.EPSILON)
            {
                posMiddle_ = Vector3.MoveTowards(posMiddle_, unitToChain_._posCenter, 20f * Globals.timeDeltaFixed);
                lrChain_.SetPositions(new Vector3[] { unitStart_._posCenter, posMiddle_ });
                lrChain_.material.mainTextureScale = new Vector2((posMiddle_ - unitToChain_._posCenter).magnitude / 25, 1);
                lrChain_.material.mainTextureOffset -= new Vector2(Globals.timeDeltaFixed * 20, 0);

                yield return null;
            }

            damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitToChain_, skill_, damageBase_, unitOnTarget_._parameter._mrApplied, "MD");
            Explosion_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "ChainLightning_Explosion"), unitToChain_._posCenter, Quaternion.identity);
            unitSelf_._DealDamage(unitToChain_, damage_, skill_, "Magic");
            runningCoutourine_.Add(General.Instance.StartCoroutine(ChainLightning_DetectUnits(unitToChain_)));

            for (float timeWait = 0.15f; timeWait > 0; timeWait = timeWait - Globals.timeDeltaFixed)
            {
                lrChain_.material.mainTextureScale = new Vector2((posMiddle_ - unitToChain_._posCenter).magnitude / 25, 1);
                lrChain_.material.mainTextureOffset -= new Vector2(Globals.timeDeltaFixed * 20, 0);

                yield return null;
            }

            posMiddle_ = unitStart_._posCenter;
            while ((posMiddle_ - unitToChain_._posCenter).sqrMagnitude > Globals.EPSILON)
            {
                posMiddle_ = Vector3.MoveTowards(posMiddle_, unitToChain_._posCenter, 30f * Globals.timeDeltaFixed);
                lrChain_.SetPositions(new Vector3[] { posMiddle_, unitToChain_._posCenter });
                lrChain_.material.mainTextureScale = new Vector2((posMiddle_ - unitToChain_._posCenter).magnitude / 25, 1);
                lrChain_.material.mainTextureOffset -= new Vector2(Globals.timeDeltaFixed * 20, 0);

                yield return null;
            }

            UnityEngine.Object.Destroy(lrChain_.gameObject);
        }
    }

    public static IEnumerator Charge(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;

        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        float dashRange_ = skill_._parameter._moveRange;
        Vector3 posTarget_ = unitSelf_.transform.position + (Globals.posOnCursorAtGround - unitSelf_._modelTransform.position).ClampByLength(dashRange_);

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_ == unitSelf_) continue;
            if (unit_i_._IsHitable() == false) continue;

            if (unit_i_._IsOverlapColliderArea(unitSelf_, posTarget_))
            {
                Battle.SetInactiveSkillSuggestion(unitSelf_);
                yield break;
            }
            if (unit_i_._IsOnPath(unitSelf_.transform.position, posTarget_) && unitSelf_._parameter._unitTypesThroughable.Contains(unit_i_._parameter._unitType) == false)
            {
                Battle.SetInactiveSkillSuggestion(unitSelf_);
                yield break;
            }
        }

        OnEntryCastSkill(unitSelf_, skill_);

        unitSelf_._modelTransform.LookAt(posTarget_);

        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);
        yield return General.Instance.StartCoroutine(General.MoveTowards(unitSelf_.gameObject, posTarget_, Globals.UNIT_MOVE_PER_SEC * 1.4f));

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
        }

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator CircleOfHealing(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        int resoreValue_ = Battle.ComputeAppliedResotoreValue(unitSelf_, skill_);

        Vector3 posOnMouse = Globals.posOnCursorAtGround;
        unitSelf_._modelTransform.LookAt(posOnMouse);

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Circle of Healing"), unitSelf_.transform.position, Quaternion.identity);
        Effect_.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        yield return new WaitForSeconds(1.2f / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            unitSelf_._Heal(unitOnTarget_i_, resoreValue_, skill_);
        }

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Cleave(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;
        if (unitsOnTargetList_?.Count != 1) yield break;

        Globals.bonusDamageOnCalcDamage += (unitsOnTargetList_[0]._parameter._hp * 0.1f).ToInt();
    }

    public static IEnumerator ConeOfFlame(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        Vector3 vectorNorm_ = (Globals.posOnCursorAtGround - unitSelf_.transform.position).normalized;
        int launchCount_ = skill_._parameter._hitCount;
        float launchInterval = 0.24f / Globals.Instance.gameSpeed;
        float movePerSec_ = Globals.PRJ_MOVE_PER_SEC * 0.5f;
        float margin_ = 1.2f;

        unitSelf_._modelTransform.LookAt(Globals.posOnCursorAtGround);
        yield return new WaitForSeconds(skill_._parameter._delayTimeToLaunchBullet / Globals.Instance.gameSpeed);
        
        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            //int[] damageDevided = damage_.DevideToArray(launchCount_);
            float distance_ = (unitOnTarget_i_.transform.position - unitSelf_.transform.position).magnitude;

            for (int j_ = 0; j_ < launchCount_; j_++)
            {
                float timeDelay_j_ = (distance_ - margin_) / (movePerSec_ * Globals.Instance.gameSpeed) + launchInterval * j_;
                int damage_j_ = damage_;

                General.Instance.StartCoroutine(General.DelayForSeconds(timeDelay_j_, () =>
                {
                    unitSelf_._DealDamage(unitOnTarget_i_, damage_j_, skill_, "Magic");
                }));
            }
        }

        for (int i_ = 0; i_ < launchCount_; i_++)
        {
            for (int j_ = 0; j_ < 5; j_++)
            {
                Vector3 vectorNorm_j_ = vectorNorm_.RotateThisByRadian(0f, (skill_._parameter._angle.ToRadian() / 4 * (2 - j_)), 0f);
                GameObject Projectile_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "FireBallMiddle"), unitSelf_.transform);
                Projectile_.transform.position = unitSelf_._posCenter + vectorNorm_ * margin_;

                General.Instance.StartCoroutine(General.MoveTowards(Projectile_, Projectile_.transform.position + vectorNorm_j_ * skill_._parameter._hitRange, movePerSec_, true));
            }
            yield return new WaitForSeconds(launchInterval);
        }

        yield return new WaitForSeconds(0.4f);

        OnExitCastSkill(unitSelf_, skill_);

        yield return new WaitForSeconds(0.6f);
    }

    public static IEnumerator CosmicPlasma(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);

        Vector3 vectorNorm_ = (Globals.posOnCursorAtGround - unitSelf_.transform.position).normalized;
        Vector3 posTarget_ = unitSelf_._posCenter + vectorNorm_ * skill_._parameter._targetRange;
        if (unitsOnTargetList_.Count > 0)
            posTarget_ = unitSelf_._posCenter + (posTarget_ - unitSelf_._posCenter).ClampByLength((unitsOnTargetList_[0]._posCenter - unitSelf_._posCenter).magnitude);

        GameObject Projectile_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicProjectile_Void"), unitSelf_._posCenter + vectorNorm_ * 1.0f, Quaternion.identity);
        unitSelf_._modelTransform.LookAt(Globals.posOnCursorAtGround);

        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);
        unitSelf_._animator.SetTrigger("triggerUseItem00");
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        yield return General.Instance.StartCoroutine(General.MoveTowards(Projectile_, posTarget_, Globals.PRJ_MOVE_PER_SEC * 0.8f, true));

        if (unitsOnTargetList_.Count > 0)
        {
            GameObject Burst_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicBurst_Void"), unitsOnTargetList_[0]._posCenter, Quaternion.identity);

            int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitsOnTargetList_[0], skill_, damageBase_, unitsOnTargetList_[0]._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitsOnTargetList_[0], damage_, skill_, "Magic");
        }

        OnExitCastSkill(unitSelf_, skill_);

        yield return null;
    }

    public static IEnumerator Curse(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._refTiming == "EndOfYourTurn")
        {
            OnEntryActivatePassive(unitSelf_, skill_);
            _Unit.DealDamage(null, unitSelf_, (unitSelf_._parameter._hp * 0.10f).ToInt(), skill_, "Static");
            OnExitActivatePassive(unitSelf_, skill_);
        }
        else if (skill_._refTiming == "EndOfBattle")
        {
            if (--skill_._parameter._iCount < 1)
                Globals.Instance.globalEffectList.Remove(skill_);
            UI.ConfigureGlobalEffectUI();
        }
        else
        {
            yield break;
        }
    }

    public static IEnumerator Demolisher(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        unitSelf_._GainBuff("AD", 20);
    }

    public static IEnumerator Dismantle(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;
        if (unitsOnTargetList_?.Count != 1) yield break;
        if (unitsOnTargetList_[0]._parameter._hp > unitsOnTargetList_[0]._parameter._hpMax / 2) yield break;

        Globals.damageCoefOnCalcDamage *= 1.2f;
    }

    public static IEnumerator DivineLight(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ != null) yield break;

        skill_._parameter._iCount = 1;
    }

    public static IEnumerator DoublePunch(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count == 0) yield break;
        if (unitsOnTargetList_[0] == null) yield break;

        skill_._functionDisplaySkillArea(unitSelf_, unitsOnTargetList_[0].transform.position, skill_);
        yield return new WaitForSeconds(0.6f / Globals.Instance.gameSpeed);

        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
        }
        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);
        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
        }

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator EnemyBuff_Self(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        OnEntryCastSkill(unitSelf_, skill_);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToResolveEffect / Globals.Instance.gameSpeed);

        GameObject Effect = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_" + skill_._parameter._effectColor), unitSelf_._goComps.transform);
        Effect.transform.localScale = skill_._parameter._effectSize.ToVector3();

        for (int i = 0; i < skill_._parameter._buffType.Count; i++)
        {
            unitSelf_._GainBuff(skill_._parameter._buffType[i], skill_._parameter._buffValue[i]);
        }

        yield return new WaitForSeconds(1f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator EnemyMeleeAttackNormal(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count == 0) yield break;
        if (unitsOnTargetList_[0] == null) yield break;

        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        skill_._functionDisplaySkillArea(unitSelf_, unitsOnTargetList_[0].transform.position, skill_);
        yield return new WaitForSeconds(0.6f / Globals.Instance.gameSpeed);

        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_, skill_, damageBase_, unitOnTarget_._parameter._arApplied, "AD");
        unitSelf_._DealDamage(unitOnTarget_, damage_, skill_, "Physical");

        for (int i = 0; i < skill_._parameter._buffType.Count; i++)
        {
            unitOnTarget_._GainBuff(skill_._parameter._buffType[i], skill_._parameter._buffValue[i]);
        }

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Enlightenment(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;

        OnEntryActivatePassive(unitSelf_, skill_);

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);
        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicRain_Arcane"), unitSelf_.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.2f / Globals.Instance.gameSpeed);

        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Magic");
        }

        yield return new WaitForSeconds(2.0f / Globals.Instance.gameSpeed);

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator Execution(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count == 0) yield break;
        if (unitsOnTargetList_[0] == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        int killCount_ = 0;

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            damageBase_ += ((unitOnTarget_i_._parameter._hpMax - unitOnTarget_i_._parameter._hp) * skill_._parameter._iValue / 100f).ToInt();
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");

            if (unitOnTarget_i_._parameter._hp < 1)
                killCount_++;
        }

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_, killCount_:killCount_);

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Explosion(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Explosion"), unitSelf_.transform.position, Quaternion.identity);
        Effect_.transform.SetParent(Prefabs.Instance.transform);
        Effect_.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.35f / Globals.Instance.gameSpeed);
        unitSelf_.gameObject.SetActive(false);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Magic");
        }

        yield return new WaitForSeconds(1.2f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Fasting (_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        Globals.Instance.FoodMax -= 1;
        Globals.Instance.Food = Globals.Instance.Food;

        Globals.Instance.Gold += 1000;
    }

    public static IEnumerator Fireball(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);

        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();
        Vector3 posStart_ = unitSelf_._posCenter + (Globals.posOnCursorAtGround - unitSelf_.transform.position).normalized * 1.0f;
        Vector3 posTarget_ = unitSelf_._posCenter + (Globals.posOnCursorAtGround - unitSelf_.transform.position).normalized * skill_._parameter._targetRange;
        List<RaycastHit> hits = Physics.RaycastAll(unitSelf_.transform.position, Globals.posOnCursorAtGround - unitSelf_.transform.position, skill_._parameter._targetRange, /*layer = Unit*/ 1 << 10).ToList();
        hits.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsHitable() == false) continue;

            posTarget_ = hit.point + new Vector3(0, unit_i_._charaHeight / 2, 0);
            break;
        }

        unitSelf_._modelTransform.LookAt(posTarget_);

        GameObject Projectile_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicSphere_Fire"), posStart_, Quaternion.identity);
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);
        unitSelf_._animator.SetTrigger("triggerAttack");
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        yield return General.MoveTowards(Projectile_, posTarget_, Globals.PRJ_MOVE_PER_SEC * 0.8f, true);

        GameObject Explosion_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicExplosion_Fire"), posTarget_, Quaternion.identity);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Magic");
        }

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator FirstBlood(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (skill_._refTiming == "StartOfBattle")
        {
            skill_._parameter._iCount = 1;
        }
        else if (skill_._refTiming == "OnKillEnemy")
        {
            if (skill_._parameter._iCount == 0) yield break;

            unitSelf_._GainBuff("AD", 30);
            skill_._parameter._iCount--;
        }
    }

    public static IEnumerator FrostNova(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        Vector3 posTarget_ = unitSelf_.transform.position + (Globals.posOnCursorAtGround - unitSelf_.transform.position).ClampByLength(skill_._parameter._targetRange);
        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        unitSelf_._modelTransform.LookAt(Globals.posOnCursorAtGround + new Vector3(0, unitSelf_._charaHeight / 2, 0));

        yield return new WaitForSeconds(0.1f / Globals.Instance.gameSpeed);
        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "FrostSpike"), posTarget_, Quaternion.identity);
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);
        unitSelf_._animator.SetTrigger("triggerAttack");
        yield return new WaitForSeconds(0.6f / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Magic");
            unitOnTarget_i_._GainBuff("SP", skill_._parameter._buffValue[0]);
        }

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Flying(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        unitSelf_._parameter._unitTypesThroughable = Globals.ALL_UNIT_TYPES;
    }

    public static IEnumerator Force(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._refSkill._parameter._descriptiveName != "Basic Attack") yield break;

        Globals.bonusDamageOnCalcDamage += (unitSelf_._parameter._mdApplied * 0.15f).ToInt();
    }

    public static IEnumerator GainBarrier(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        int barrierValue_ = skill_._parameter._barrierValueBase + (unitSelf_._parameter._arApplied * skill_._parameter._arRatio).ToInt();

        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Divine Barrier"), unitSelf_._posCenter, Quaternion.identity);
        Effect_.transform.SetParent(unitSelf_._goComps.transform);

        yield return new WaitForSeconds(0.6f / Globals.Instance.gameSpeed);
        unitSelf_._BarrierValue += barrierValue_;

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator GainStealth(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "GainStealth"), unitOnTarget_i_.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f / Globals.Instance.gameSpeed);
            unitOnTarget_i_._GainStatus("Stealth", 2);
        }

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator GhostStep(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;

        float dashRange_ = (skill_._parameter._baseValue + skill_._parameter._spRatio * unitSelf_._parameter._sp) / 10;
        Vector3 posTarget_ = unitSelf_.transform.position + (Globals.posOnCursorAtGround - unitSelf_._modelTransform.position).ClampByLength(dashRange_);

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_ == unitSelf_) continue;

            if (unit_i_._IsOverlapColliderArea(unitSelf_, posTarget_))
            {
                Battle.SetInactiveSkillSuggestion(unitSelf_);
                yield break;
            }
        }

        OnEntryCastSkill(unitSelf_, skill_);

        unitSelf_._modelTransform.LookAt(posTarget_);
        unitSelf_._animator.SetBool("isWalk", true);
        yield return General.Instance.StartCoroutine(General.MoveTowards(unitSelf_.gameObject, posTarget_, Globals.UNIT_MOVE_PER_SEC * 2));
        unitSelf_._animator.SetBool("isWalk", false);

        if (unitsOnTargetList_.Count == 1)
        {
            int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
            unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

            unitSelf_._animator.SetTrigger("triggerUseItem00");
            yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

            foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
            {
                int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
                unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
            }
        }

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator GiantSlayer(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_?.Count != 1) yield break;
        if (unitsOnTargetList_[0]._parameter._hp <= unitSelf_._parameter._hp) yield break;

        Globals.damageCoefOnCalcDamage *= 1.2f;
    }

    public static IEnumerator HealingExplosion(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Explosion_Heal"), unitSelf_.transform.position, Quaternion.identity);
        Effect_.transform.SetParent(Prefabs.Instance.transform);
        Effect_.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);
        unitSelf_.gameObject.SetActive(false);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            if (unitOnTarget_i_._parameter._unitType == "Object") continue;

            unitSelf_._Heal(unitOnTarget_i_, skill_._parameter._restoreValueBase, skill_);
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Magic");
        }

        yield return new WaitForSeconds(1.2f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator HeavyStrike(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count == 0) yield break;
        if (unitsOnTargetList_[0] == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
            unitOnTarget_i_._GainStatus("Stun", 1);
        }

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_);

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator HeroBuff_Target(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count != 1) yield break;

        OnEntryCastSkill(unitSelf_, skill_);

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToResolveEffect / Globals.Instance.gameSpeed);

        GameObject Effect = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_" + skill_._parameter._effectColor), unitsOnTargetList_[0]._goComps.transform);
        Effect.transform.localScale = skill_._parameter._effectSize.ToVector3();

        for (int i = 0; i < unitsOnTargetList_.Count; i++)
        {
            for (int j = 0; j < skill_._parameter._buffType.Count; j++)
            {
                unitsOnTargetList_[i]._GainBuff(skill_._parameter._buffType[j], skill_._parameter._buffValue[j]);
            }
        }

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_);

        yield return new WaitForSeconds(1f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator HeroDebuff(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count != 1) yield break;

        OnEntryCastSkill(unitSelf_, skill_);

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToResolveEffect / Globals.Instance.gameSpeed);

        GameObject Effect = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Debuff_" + skill_._parameter._effectColor), unitsOnTargetList_[0]._goComps.transform);
        Effect.transform.localScale = skill_._parameter._effectSize.ToVector3();

        for (int i = 0; i < unitsOnTargetList_.Count; i++)
        {
            for (int j = 0; j < skill_._parameter._buffType.Count; j++)
            {
                unitsOnTargetList_[i]._GainBuff(skill_._parameter._buffType[j], skill_._parameter._buffValue[j]);
            }
        }

        yield return new WaitForSeconds(1f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Harvest(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        Globals.Instance.FoodMax += 1;
        Globals.Instance.food += 1;
        Globals.Instance.Food = Globals.Instance.Food;
    }

    public static IEnumerator Hunger(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        unitSelf_._parameter._adApplied = unitSelf_._parameter._adApplied / 2;
        unitSelf_._parameter._arApplied = unitSelf_._parameter._arApplied / 2;
        unitSelf_._parameter._mdApplied = unitSelf_._parameter._mdApplied / 2;
        unitSelf_._parameter._mrApplied = unitSelf_._parameter._mrApplied / 2;
        unitSelf_._parameter._spApplied = unitSelf_._parameter._spApplied / 2;
    }

    public static IEnumerator HunterAttack(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        unitSelf_._modelTransform.LookAt(unitOnTarget_.transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToLaunchBullet / Globals.Instance.gameSpeed);

        Vector3 vector_ = unitOnTarget_._posCenter - unitSelf_._posCenter;
        Vector3 posStart_ = unitSelf_._posCenter + vector_.normalized * Globals.ARROW_LENGTH;
        Vector3 posEnd_ = unitOnTarget_._posCenter - vector_.normalized * Globals.ARROW_LENGTH;

        GameObject Arrow_ = UnityEngine.Object.Instantiate(Prefabs.goInstances.Find(m => m.name == "Arrow"), posStart_, Quaternion.identity);
        Arrow_.transform.SetParent(unitOnTarget_._goBody.transform, true);
        Arrow_.transform.LookAt(posEnd_);

        yield return unitOnTarget_.StartCoroutine(General.MoveTowards(Arrow_, posEnd_, Globals.PRJ_MOVE_PER_SEC));

        UnityEngine.Object.Destroy(Arrow_, 1f / Globals.Instance.gameSpeed);

        int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_, skill_, damageBase_, unitOnTarget_._parameter._arApplied, "AD");
        unitSelf_._DealDamage(unitOnTarget_, damage_, skill_, "Physical");

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator IceShard(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);

        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();
        bool isShatterd_ = false;
        Vector3 posStart_ = unitSelf_._posCenter + (Globals.posOnCursorAtGround - unitSelf_.transform.position).normalized * 1.0f;
        Vector3 posTarget_ = unitSelf_._posCenter + (Globals.posOnCursorAtGround - unitSelf_.transform.position).normalized * skill_._parameter._targetRange;
        List<RaycastHit> hits = Physics.RaycastAll(unitSelf_.transform.position, Globals.posOnCursorAtGround - unitSelf_.transform.position, skill_._parameter._targetRange, /*layer = Unit*/ 1 << 10).ToList();
        hits.Sort((a, b) => a.distance.CompareTo(b.distance));
        unitSelf_._modelTransform.LookAt(posTarget_);

        foreach (RaycastHit hit in hits)
        {
            _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsHitable() == false) continue;

            posTarget_ = hit.point + new Vector3(0, unit_i_._charaHeight / 2, 0);
            isShatterd_ = true;
            break;
        }

        GameObject Projectile_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicSphere_Ice"), posStart_, Quaternion.identity);
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);
        unitSelf_._animator.SetTrigger("triggerAttack");
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        yield return General.MoveTowards(Projectile_, posTarget_, Globals.PRJ_MOVE_PER_SEC * 0.8f, true);

        if (isShatterd_ == true)
        {
            GameObject Explosion_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicExplosion_IceArc"), posTarget_, Quaternion.identity);
            Explosion_.transform.LookAt(Globals.posOnCursorAtGround - unitSelf_.transform.position);
        }

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Magic");
        }

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Injured(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ != null) yield break;

        foreach (int index_i_ in skill_._parameter._targetHeroIndex)
        {
            _Unit.DealDamage(null, Globals.heroList[index_i_], skill_._parameter._sdDamageBase, skill_, "Static");
        }

        Globals.Instance.globalEffectList.Remove(skill_);
        UI.ConfigureGlobalEffectUI();
    }

    public static IEnumerator Inspire(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_?.Count == 0) yield break;

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);
        OnEntryCastSkill(unitSelf_, skill_);
        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

        GameObject Effect = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_Orange"), unitsOnTargetList_[0]._goComps.transform);
        Effect.transform.localScale = skill_._parameter._effectSize.ToVector3();

        for (int i = 0; i < skill_._parameter._buffType.Count; i++)
        {
            unitsOnTargetList_[0]._GainBuff(skill_._parameter._buffType[i], skill_._parameter._buffValue[i]);
        }

        yield return new WaitForSeconds(1.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Intimidation(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        skill_._parameter._iCount++;
        UI.ConfigureGlobalEffectUI();

        if (skill_._parameter._iCount < 4) yield break;
        skill_._parameter._iCount = 0;
        UI.ConfigureGlobalEffectUI();

        foreach (_Unit unit_i_ in Globals.enemyList)
        {
            unit_i_._LoseHp(unit_i_._parameter._hp / 4, skill_);
        }
    }

    public static IEnumerator KnockbackShot(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);

        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        Vector3 posArrowStart_ = unitSelf_._posCenter + (unitOnTarget_.transform.position - unitSelf_.transform.position).normalized * 1.0f;

        unitSelf_._modelTransform.LookAt(unitOnTarget_.transform.position);
        yield return new WaitForSeconds(0.55f / Globals.Instance.gameSpeed);

        GameObject Arrow_ = UnityEngine.Object.Instantiate(Prefabs.goInstances.Find(m => m.name == "Arrow"), posArrowStart_, Quaternion.identity);
        Arrow_.transform.SetParent(unitOnTarget_._goBody.transform, true);
        Arrow_.transform.LookAt(unitOnTarget_._posCenter);

        yield return unitOnTarget_.StartCoroutine(General.MoveTowards(Arrow_, unitOnTarget_._posCenter, Globals.PRJ_MOVE_PER_SEC));
        UnityEngine.Object.Destroy(Arrow_, 1f / Globals.Instance.gameSpeed);

        int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_, skill_, damageBase_, unitOnTarget_._parameter._arApplied, "AD");
        unitSelf_._DealDamage(unitOnTarget_, damage_, skill_, "Physical");

        Vector3 vectorTarget_ = (unitOnTarget_.transform.position - unitSelf_.transform.position).normalized * skill_._parameter._knockbackRange;
        Vector3 posToMove_ = unitOnTarget_.transform.position + vectorTarget_;
        float margin_ = 0.05f;
        bool isCollide_ = false;

        if (Physics.SphereCast(unitOnTarget_.transform.position, unitOnTarget_._parameter._colliderRange, vectorTarget_, out RaycastHit hit, vectorTarget_.magnitude, 1 << 10))
        {
            posToMove_ = unitOnTarget_.transform.position + vectorTarget_.normalized * (hit.distance - margin_);
            isCollide_ = true;
        }

        posToMove_ = posToMove_.ClampByVector3(Globals.Instance.fieldPosMin, Globals.Instance.fieldPosMax);

        yield return unitOnTarget_.StartCoroutine(General.MoveTowards(unitOnTarget_.gameObject, posToMove_, Globals.PRJ_MOVE_PER_SEC));
        UnityEngine.Object.Destroy(Arrow_, 1f / Globals.Instance.gameSpeed);

        if (isCollide_)
        {
            UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "DamageEffect"), unitOnTarget_._goComps_Y5.transform, false);
            unitOnTarget_._GainStatus("Stun", 1);
        }

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator MageAttack(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        unitSelf_._modelTransform.LookAt(unitOnTarget_.transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToLaunchBullet / Globals.Instance.gameSpeed);

        Vector3 vector_ = unitOnTarget_._posCenter - unitSelf_._posCenter;
        Vector3 posStart_ = unitSelf_._posCenter + vector_.normalized * Globals.ARROW_LENGTH;
        Vector3 posEnd_ = unitOnTarget_._posCenter;

        GameObject Projectile_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MageAttackProjectile"), posStart_, Quaternion.identity, unitOnTarget_._goBody.transform);
        Projectile_.transform.LookAt(posEnd_);

        yield return unitOnTarget_.StartCoroutine(General.MoveTowards(Projectile_, posEnd_, Globals.PRJ_MOVE_PER_SEC * 0.8f));

        UnityEngine.Object.Destroy(Projectile_, 0.5f / Globals.Instance.gameSpeed);

        int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_, skill_, damageBase_, unitOnTarget_._parameter._arApplied, "AD");
        unitSelf_._DealDamage(unitOnTarget_, damage_, skill_, "Physical");

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator MasteryOfMagic(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        Debug.Log(0 + ", " + unitSelf_._parameter._skills[1]._parameter._cooldownDuration);
        if (skill_ == null) yield break;

        if (unitSelf_._parameter._md >= 100)
        {
            foreach (_Skill skill_i_ in unitSelf_._parameter._skills)
            {
                skill_i_._parameter._cooldownDuration = (skill_i_._parameter._cooldownDuration - 1).Clamp(0, 100);
            }
        }
        Debug.Log(1 + ", " + unitSelf_._parameter._skills[1]._parameter._cooldownDuration);
    }

    public static IEnumerator MightyGuard(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (skill_._refTiming  == "StartOfBattle")
        {
            skill_._parameter._isShowICount = true;
            skill_._parameter._iCount = 0;
        }

        if (skill_._refTiming == "StartOfTurn")
        {
            if (++skill_._parameter._iCount == 3)
            {
                foreach (_Unit unit_i_ in Globals.heroList)
                {
                    unit_i_._BarrierValue += 50;
                    skill_._parameter._isShowICount = false;
                }
            }
            UI.ConfigureGlobalEffectUI();
        }
    }

    public static IEnumerator Offering(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count != 1) yield break;

        OnEntryCastSkill(unitSelf_, skill_);

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToResolveEffect / Globals.Instance.gameSpeed);

        GameObject Effect = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_" + skill_._parameter._effectColor), unitsOnTargetList_[0]._goComps.transform);
        Effect.transform.localScale = skill_._parameter._effectSize.ToVector3();

        unitSelf_._LoseHp((unitSelf_._parameter._hp * 0.3f).ToInt().Clamp(0, unitSelf_._parameter._hp - 1), skill_);

        for (int i = 0; i < unitsOnTargetList_.Count; i++)
        {
            for (int j = 0; j < skill_._parameter._buffType.Count; j++)
            {
                unitsOnTargetList_[i]._GainBuff(skill_._parameter._buffType[j], skill_._parameter._buffValue[j]);
            }
        }

        yield return new WaitForSeconds(1f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Outrage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (unitSelf_._parameter._hp <= unitSelf_._parameter._hpMax / 2)
        {
            unitSelf_._parameter._adApplied += 30;
        }
    }

    public static IEnumerator Passive_Hunter (_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._parameter._cooldownRemaining > 0) yield break;
        if (unitsOnTargetList_[0]._parameter._unitType == "Object") yield break;

        _Unit unitOnTarget_ = unitsOnTargetList_[0];

        Vector3 forward_ = unitOnTarget_._modelTransform.TransformDirection(Vector3.forward);
        Vector3 vector_ = unitSelf_.transform.position - unitOnTarget_.transform.position;
        float degree_ = Vector3.Angle(forward_, vector_);

        if (degree_ > 90)
            Globals.damageCoefOnCalcDamage *= skill_._parameter._damageCoef;
    }

    //public static IEnumerator Passive_Hunter(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    //{
    //    if (skill_._parameter._cooldownRemaining > 0) yield break;

    //    if (skill_._refTiming == "OnKillEnemy")
    //    {
    //        skill_._parameter._stack = (skill_._parameter._stack + 1).Clamp(0, 10);
    //    }
    //    else if (skill_._refTiming == "OnCalculateDealDamage")
    //    {
    //        Globals.damageCoefOnCalcDamage *= (10 + skill_._parameter._stack) / 10f;
    //    }
    //}

    public static IEnumerator Passive_Mage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._parameter._cooldownRemaining > 0) yield break;
        if (skill_._refSkill._parameter._descriptiveName == "Basic Attack") yield break;

        if (skill_._refTiming == "AfterCastSkill")
        {
            //if (unitSelf_._skillOnActive._parameter._descriptiveName != "Basic Attack")
            skill_._parameter._cooldownRemaining = skill_._parameter._cooldownDuration;
        }
        else if (skill_._refTiming == "OnCalculateDealDamage")
        {
            Globals.damageCoefOnCalcDamage *= skill_._parameter._damageCoef;
        }
    }

    public static IEnumerator Passive_Demon(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        Debug.Log(0);
        if (unitSelf_._IsAlive() == false) yield break;

        int percent_ = (unitSelf_._parameter._hpMax - unitSelf_._parameter._hp) * 100 / unitSelf_._parameter._hpMax;
        unitSelf_._parameter._adApplied = (unitSelf_._parameter._adApplied * (100 + percent_) / 100f).ToInt();

        //if (unitSelf_ == null) yield break;

        //foreach (_Unit unit_i_ in unitsOnTargetList_)
        //{
        //    for (int i = 0; i < skill_._parameter._buffType.Count; i++)
        //        unit_i_._GainBuff(skill_._parameter._buffType[i], skill_._parameter._buffValue[i]);
        //}
    }

    public static IEnumerator Passive_Slime(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._parameter._hp > unitSelf_._parameter._hpMax / 2) yield break;

        Globals.damageCoefOnCalcDamage *= skill_._parameter._damageCoef;
    }

    public static IEnumerator Passive_Rat(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == true) yield break;

        foreach (_Unit unit_i_ in Globals.enemyList)
        {
            if (unit_i_._parameter._class != "Rat") continue;
            if (unit_i_._IsAlive() == false) continue;

            unit_i_._animator.SetTrigger("triggerCastSkill00");
            unit_i_._GainBuff("AD", 20);
        }
    }

    public static IEnumerator Passive_Wolf(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;

        int count_ = 0;
        foreach (_Unit unit_i_ in Globals.enemyList)
        {
            if (unit_i_._parameter._class != "Wolf") continue;
            if (unit_i_ == unitSelf_) continue;

            count_++;
        }
        unitSelf_._parameter._adApplied = (unitSelf_._parameter._adApplied * (10 + count_) / 10f).ToInt();
    }

    public static IEnumerator Passive_Golem(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        unitSelf_._animator.SetTrigger("triggerCastBuff00");
        yield return new WaitForSeconds(1f / Globals.Instance.gameSpeed);

        unitSelf_._BarrierValue += 30;
        yield return new WaitForSeconds(1f / Globals.Instance.gameSpeed);

        //unitSelf_._GainBuff("AR", 30);
        //unitSelf_._GainBuff("MR", 30);

        //if (skill_._refSkill._parameter._descriptiveName != "Howling") yield break;

        //foreach (_Unit unit_i_ in Globals.enemyList)
        //{
        //    if (unit_i_._parameter._class != "Wolf") continue;
        //    if (unit_i_ == unitSelf_) continue;

        //    unit_i_._GainBuff("AD", 10);
        //}
    }

    public static IEnumerator Passive_Faerie(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;
        OnEntryActivatePassive(unitSelf_, skill_);

        unitSelf_._outline.enabled = true;
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        unitSelf_._animator.SetTrigger("triggerCastSkill00");
        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicArea_Heal"), unitSelf_.transform.position, Quaternion.identity);
        Effect_.transform.localScale = Vector3.one * 0.2f * skill_._parameter._hitRange;

        yield return new WaitForSeconds(0.5f);
        foreach (_Unit unit_i_ in unitsOnTargetList_)
        {
            if (unit_i_ == unitSelf_) continue;

            unitSelf_._Heal(unit_i_, skill_._parameter._restoreValueBase, skill_);
        }
        yield return new WaitForSeconds(0.5f);
;
        yield return new WaitForSeconds(1.4f / Globals.Instance.gameSpeed);
        unitSelf_._outline.enabled = false;

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator Passive_Fungusa(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._parameter._hp <= skill_._refValue) yield break;

        //yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        OnEntryActivatePassive(unitSelf_, skill_);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToResolveEffect / Globals.Instance.gameSpeed);

        _Enemy enemyNew_ = _Unit.CloneFromString("Fungee") as _Enemy;
        enemyNew_._posSOB = unitSelf_.transform.position;
        enemyNew_._posDistSOB = new Vector3(+3, +0, +3);
        enemyNew_._PlaceRandomlyOrDestroy(out bool isDestroy_);
        enemyNew_._ApplyBuff();

        Vector3 posStart_ = unitSelf_._posCenter;
        Vector3 posEnd_ = enemyNew_.transform.position;
        Vector3 posMiddle_ = (posStart_ + posEnd_) / 2 + new Vector3(0, 6, 0);

        for (float timeSum_ = 0, timeMax_ = 0.6f; timeSum_ < timeMax_; timeSum_ += Globals.timeDeltaFixed)
        {
            float p = (timeSum_ / timeMax_);
            enemyNew_.transform.position = Library.BezierQuadratic(posStart_, posMiddle_, posEnd_, p);

            yield return null;
        }
        enemyNew_.transform.position = posEnd_;

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator Passive_Fungee(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryActivatePassive(unitSelf_, skill_);

        unitSelf_._outline.enabled = true;
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        unitSelf_._DealDamage(unitSelf_, (unitSelf_._parameter._hpMax * 0.2f).ToInt(), skill_, "Static");

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);
        unitSelf_._outline.enabled = false;

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator Passive_Treant(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;

        int percent_ = (unitSelf_._parameter._hpMax - unitSelf_._parameter._hp) * 100 / unitSelf_._parameter._hpMax;

        unitSelf_._parameter._arApplied = (unitSelf_._parameter._arApplied * (100 + percent_) / 100f).ToInt();
        unitSelf_._parameter._mrApplied = (unitSelf_._parameter._mrApplied * (100 + percent_) / 100f).ToInt();
    }

    public static IEnumerator Phantasmagoria(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        unitSelf_._parameter._md = (unitSelf_._parameter._md * skill_._parameter._fValue).ToInt();
    }

    public static IEnumerator Photosynthesis(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Photosynthesis"), unitSelf_._posCenter + new Vector3(0, 3f, 0), Quaternion.identity);
        Effect_.transform.SetParent(unitSelf_._goComps.transform);
        Vector3 posStart_ = unitSelf_._posCenter + new Vector3(0, 3f, 0);
        Vector3 posEnd_ = unitSelf_._posCenter;

        for (float timeSum_ = 0, timeMax_ = 1f; timeSum_ < timeMax_; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum_ / timeMax_);
            Effect_.transform.position = Library.BezierLiner(posStart_, posEnd_, p_);

            yield return null;
        }

        unitSelf_._Heal(unitSelf_, skill_._parameter._restoreValueBase, skill_);

        unitSelf_._parameter._hpBuff = (unitSelf_._parameter._hpBuff).Clamp(0, 100);
        unitSelf_._parameter._adBuff = (unitSelf_._parameter._adBuff).Clamp(0, 100);
        unitSelf_._parameter._arBuff = (unitSelf_._parameter._arBuff).Clamp(0, 100);
        unitSelf_._parameter._mdBuff = (unitSelf_._parameter._mdBuff).Clamp(0, 100);
        unitSelf_._parameter._mrBuff = (unitSelf_._parameter._mrBuff).Clamp(0, 100);
        unitSelf_._parameter._spBuff = (unitSelf_._parameter._spBuff).Clamp(0, 100);
        unitSelf_._ApplyBuff();

        //yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

        //unitOnTarget_._BarrierValue += skill_._parameter._barrierValueBase;

        //yield return new WaitForSeconds(1.0f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Pierce(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._refSkill._parameter._descriptiveName != "Basic Attack") yield break;

        Globals.resistCoefOnCalcDamage *= 0.5f;
    }

    public static IEnumerator PiercingShot(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        Vector3 vector_ = Globals.posOnCursorAtGround - unitSelf_.transform.position;
        float margin_ = 0.4f;
        float movePerSec_ = Globals.PRJ_MOVE_PER_SEC * 0.8f;
        Vector3 posStart_ = unitSelf_._posCenter + vector_.normalized * margin_;
        Vector3 posEnd_ = posStart_ + vector_.normalized * (skill_._parameter._hitRange - margin_);
        int killCount_ = 0;

        unitSelf_._modelTransform.LookAt(Globals.posOnCursorAtGround);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToLaunchBullet / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            float distance_ = (unitOnTarget_i_.transform.position - unitSelf_.transform.position).magnitude - margin_;
            float timeDelay_ = (distance_) / (movePerSec_ * Globals.Instance.gameSpeed);
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");

            General.Instance.StartCoroutine(General.DelayForSeconds(timeDelay_, () =>
            {
                unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
                if (unitOnTarget_i_._parameter._hp < 1)
                    killCount_++;
            }));
        }

        GameObject MagicTrail_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "PiercingShot"), posStart_, Quaternion.identity, unitSelf_.transform);
        GameObject Arrow_ = UnityEngine.Object.Instantiate(Prefabs.goInstances.Find(m => m.name == "Arrow"), MagicTrail_.transform, false);
        MagicTrail_.transform.LookAt(posEnd_);

        yield return unitSelf_.StartCoroutine(General.MoveTowards(MagicTrail_, posEnd_, Globals.PRJ_MOVE_PER_SEC * 1.0f));

        MagicTrail_.GetComponent<_Particles>().ConfigureMaxParticles(0);
        UnityEngine.Object.Destroy(Arrow_);
        UnityEngine.Object.Destroy(MagicTrail_, 1f);

        yield return new WaitForSeconds(0.3f);
        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_, killCount_:killCount_);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Predation(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        _Unit.Heal(null, unitSelf_, 20, skill_);
    }

    public static IEnumerator Protect(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_?.Count == 0) yield break;
        _Unit unitOnTarget_ = unitsOnTargetList_[0];

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);
        OnEntryCastSkill(unitSelf_, skill_);
        unitSelf_._modelTransform.LookAt(unitOnTarget_.transform.position);
        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Divine Barrier"), unitOnTarget_._posCenter, Quaternion.identity);
        Effect_.transform.SetParent(unitOnTarget_._goComps.transform);
        yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

        unitOnTarget_._BarrierValue += skill_._parameter._barrierValueBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        yield return new WaitForSeconds(1.0f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Purify(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Quicksilver"), unitSelf_.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);

        unitSelf_._animator.SetTrigger("triggerUseItem00");
        yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

        foreach (_Unit._Parameter._StatusCondition statusCondition_i_ in unitSelf_._parameter._statusConditions)
        {
            if (statusCondition_i_._type == "Bad")
            {
                statusCondition_i_._count = 0;
            }
        }
        //unitSelf_._parameter._stunCount = 0;
        //unitSelf_._parameter._snareCount = 0;
        //unitSelf_._parameter._blindCount = 0;
        unitSelf_._parameter._hpBuff = (unitSelf_._parameter._hpBuff).Clamp(0, 100);
        unitSelf_._parameter._adBuff = (unitSelf_._parameter._adBuff).Clamp(0, 100);
        unitSelf_._parameter._mdBuff = (unitSelf_._parameter._mdBuff).Clamp(0, 100);
        unitSelf_._parameter._arBuff = (unitSelf_._parameter._arBuff).Clamp(0, 100);
        unitSelf_._parameter._mrBuff = (unitSelf_._parameter._mrBuff).Clamp(0, 100);
        unitSelf_._parameter._spBuff = (unitSelf_._parameter._spBuff).Clamp(0, 100);
        unitSelf_._SetAnimatorCondition();
        unitSelf_._ApplyBuff();

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator QuickMove(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (skill_._refTiming == "StartOfBattle")
        {
            skill_._parameter._iCount = 0;
        }
        else if (skill_._refTiming == "StartOfTurn")
        {
            skill_._parameter._iCount++;
        }
        else if (skill_._refTiming == "OnApplyBuff")
        {
            if (skill_._parameter._iCount == 1)
                unitSelf_._parameter._spApplied = (unitSelf_._parameter._spApplied * 1.5f).ToInt();
        }
    }

    public static IEnumerator Rage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        unitSelf_._GainStatus("Fury", 1);
    }

    public static IEnumerator RainingArrow(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        List<Coroutine> runningCoroutine_ = new List<Coroutine>();
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        Vector3 posTarget_ = unitSelf_.transform.position + (Globals.posOnCursorAtGround - unitSelf_.transform.position).ClampByLength(skill_._parameter._targetRange);

        unitSelf_._modelTransform.LookAt(Globals.posOnCursorAtGround);
        unitSelf_._animator.SetTrigger("triggerCastSkill01");

        yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

        for (int i = 0; i < 20; i++)
            runningCoroutine_.Add(General.Instance.StartCoroutine(FireArrow()));

        foreach (Coroutine c in runningCoroutine_)
            yield return c;

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
            unitOnTarget_i_._GainBuff("SP", skill_._parameter._baseValue);
        }

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);

        IEnumerator FireArrow()
        {
            float range_ = skill_._parameter._hitRange;
            GameObject Arrow_ = UnityEngine.Object.Instantiate(Prefabs.goInstances.Find(m => m.name == "Arrow"));
            Vector3 posStart_ = unitSelf_._posCenter + (posTarget_ - unitSelf_._posCenter).normalized.MulVector(new Vector3(0.5f, 0, 0.5f)) + new Vector3(0, 0.5f, 0);
            Vector3 posEnd_ = posTarget_ + new Vector3(new _Random().Gaussian(range_ / 2).Clamp(-range_, range_), -0f, new _Random().Gaussian(range_ / 2).Clamp(-range_, range_));
            Vector3 posMiddle_ = (posStart_ + posEnd_) / 2 + new Vector3(0, 20, 0);

            Arrow_.transform.localScale = 1.5f.ToVector3();
            Arrow_.transform.position = posStart_;

            for (float timeSum_ = 0.05f, timeMax_ = 0.75f; timeSum_ < timeMax_; timeSum_ += Globals.timeDeltaFixed)
            {
                float p_ = (timeSum_ / timeMax_).Clamp(0, 1);
                Arrow_.transform.LookAt(Library.BezierQuadratic(posStart_, posMiddle_, posEnd_, p_));
                Arrow_.transform.position = Library.BezierQuadratic(posStart_, posMiddle_, posEnd_, p_);

                yield return null;
            }
            UnityEngine.Object.Destroy(Arrow_, 2f / Globals.Instance.gameSpeed);
        }
    }

    public static IEnumerator Rampage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count == 0) yield break;
        if (unitsOnTargetList_[0] == null) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        damageBase_ = (damageBase_ * (1 + (float)(unitSelf_._parameter._hpMax - unitSelf_._parameter._hp) / unitSelf_._parameter._hpMax)).ToInt();

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
        }

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Resurrection(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == true) yield break;

        OnEntryActivatePassive(unitSelf_, skill_);

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);
        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Resurrection"), unitSelf_.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f / Globals.Instance.gameSpeed);

        unitSelf_._parameter._hp = 1;
        unitSelf_._Heal(unitSelf_, unitSelf_._parameter._hpMax / 2, skill_);
        unitSelf_._parameter._hp -= 1;
        unitSelf_._animator.SetBool("isDead", false);

        yield return new WaitForSeconds(1.0f / Globals.Instance.gameSpeed);

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator ReturnToOrigin(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count != 1) yield break;

        OnEntryCastSkill(unitSelf_, skill_);

        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        unitSelf_._modelTransform.LookAt(unitOnTarget_.transform.position);

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "ReturnToOrigin"), unitOnTarget_._goComps.transform);
        General.Instance.StartCoroutine(General.SetLightIntensity(0.3f, 0.7f));

        yield return new WaitForSeconds(1.5f / Globals.Instance.gameSpeed);
        unitSelf_._animator.SetTrigger("triggerUseItem00");
        yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

        unitOnTarget_._animator.SetTrigger("triggerGetHit");

        int temp_ = unitOnTarget_._parameter._hp;
        unitOnTarget_._CopyStatusFromOriginal();
        unitOnTarget_._ApplyLevel();
        unitOnTarget_._ApplyEquips();
        unitOnTarget_._parameter._additionalPassives.Clear();
        unitOnTarget_._ApplyPassive_Static();
        unitOnTarget_._RemoveDisable();
        unitOnTarget_._DecayBuffStatus(100);
        unitOnTarget_._parameter._hp = temp_;
        unitOnTarget_._BarrierValue = 0;

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);
        General.Instance.StartCoroutine(General.SetLightIntensity(1f, 0.5f));

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator SavageSmash(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null) yield break;
        if (unitsOnTargetList_.Count == 0) yield break;
        if (unitsOnTargetList_[0] == null) yield break;

        skill_._functionDisplaySkillArea(unitSelf_, unitsOnTargetList_[0].transform.position, skill_);
        yield return new WaitForSeconds(0.6f / Globals.Instance.gameSpeed);

        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);
        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
            unitOnTarget_i_._GainStatus("Stun", 1);
        }

        yield return new WaitForSeconds(0.8f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Skullcrusher(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (skill_._refTiming == "StartOfBattle")
        {
            skill_._parameter._iCount = 1;
            skill_._parameter._isShowAsIcon = true;
        }
        if (skill_._refTiming == "OnDealDamage")
        {
            if (skill_._refSkill?._parameter._descriptiveName == "Basic Attack" && skill_._parameter._iCount == 1)
            {
                skill_._parameter._iCount = 0;
                skill_._parameter._isShowAsIcon = false;
                unitsOnTargetList_[0]._GainStatus("Stun", 1);
            }
        }
    }

    public static IEnumerator ShatteringSmash(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._parameter._unitType != "Hero") yield break;
        if (unitsOnTargetList_.Count != 1) yield break;

        if (unitsOnTargetList_[0]._parameter._statusConditions.Find(m => m._name == "Stun")._count > 0)
            Globals.damageCoefOnCalcDamage *= 1.5f;
    }

    public static IEnumerator StealthStrike(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._parameter._unitType != "Hero") yield break;
        if (unitSelf_._parameter._statusConditions.Find(m => m._name == "Stealth")._count <= 0) yield break;

        Globals.damageCoefOnCalcDamage *= 1.5f;
    }

    public static IEnumerator ShieldStaff(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._parameter._unitType != "Hero") yield break;
        if (unitSelf_._parameter._statusConditions.Find(m => m._name == "Stealth")._count <= 0) yield break;

        Globals.damageCoefOnCalcDamage *= 1.5f;
    }

    public static IEnumerator ShellAttack(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_?.Count == 0) yield break;

        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);
        yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);

        unitSelf_._animator.SetTrigger("triggerCastSkill00");
        yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);

        //foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        //{
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitsOnTargetList_[0], skill_, damageBase_, unitsOnTargetList_[0]._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitsOnTargetList_[0], damage_, skill_, "Physical");
        //}

        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Spark(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        else if (skill_._refTiming == "OnCalculateDealDamage")
        {
            Globals.damageCoefOnCalcDamage *= (10 + skill_._parameter._iCount) / 10f;
        }
        else if (skill_._refTiming == "AfterCastSkill")
        {
            if (skill_._refSkill._parameter._descriptiveName == "Basic Attack") yield break;

            skill_._parameter._iCount = 0;
        }
        else if (skill_._refTiming == "EndOfYourTurn")
        {
            skill_._parameter._iCount = (skill_._parameter._iCount + 1).Clamp(0, 10);
        }
    }

    public static IEnumerator Spellblade(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_._refSkill._parameter._descriptiveName == "Basic Attack") yield break;

        foreach (_Skill passive_i_ in unitSelf_._parameter._additionalPassives)
        {
            if (passive_i_._parameter._name == "Basic Attack Bonus Spellblade")
                yield break;
        }

        unitSelf_._parameter._additionalPassives.Add(_Skill.OriginalSkillList.Find(m => m._parameter._name == "Basic Attack Bonus Spellblade"));
    }

    public static IEnumerator StartOfBattle_BuffHeroes(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        List<Coroutine> runningCoroutineList_ = new List<Coroutine>();

        foreach (int index_i_ in skill_._parameter._targetHeroIndex)
        {
            for (int j = 0; j < skill_._parameter._buffValue.Count; j++)
            {
                runningCoroutineList_.Add(General.Instance.StartCoroutine(StartOfBattle_BuffHeros_ExecuteBuff(Globals.heroList[index_i_], skill_._parameter._buffType[j], skill_._parameter._buffValue[j])));
                yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);
            }
        }

        foreach (Coroutine c in runningCoroutineList_)
            yield return c;

        yield return new WaitForSeconds(1 / Globals.Instance.gameSpeed);

        IEnumerator StartOfBattle_BuffHeros_ExecuteBuff(_Unit unit_i_, string buffType_, int buffValue_)
        {
            GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_" + skill_._parameter._effectColor), unit_i_._goComps.transform);
            Effect_.transform.localScale = skill_._parameter._effectSize.ToVector3();

            yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

            unit_i_._GainBuff(buffType_, buffValue_);
        }
    }

    public static IEnumerator StealLife(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_ == null || unitsOnTargetList_.Count < 1) yield break;
        if (unitsOnTargetList_[0]._parameter._unitType == "Object") yield break;
        
        OnEntryActivatePassive(unitSelf_, skill_);

        yield return StealLife_Effect(unitsOnTargetList_[0]._posCenter, unitSelf_._posCenter, 0.65f);

        if (unitSelf_._parameter._hp < 1)
        {
            unitSelf_._parameter._hp = 1;
            unitSelf_._animator.Rebind();
        }

        unitSelf_._Heal(unitSelf_, (skill_._refValue * (skill_._parameter._iValue / 100f)).ToInt(), skill_);

        OnExitActivatePassive(unitSelf_, skill_);

        IEnumerator StealLife_Effect(Vector3 posStart_, Vector3 posEnd_, float timeMax_)
        {
            List<GameObject> Effects = new List<GameObject>();
            List<Vector3> posMiddle_ = new List<Vector3>();

            for (int i = 0; i < UnityEngine.Random.Range(12, 15); i++)
            {
                Effects.Add(UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "StealLife")));
                Effects[i].transform.SetParent(unitSelf_._modelTransform);
                posMiddle_.Add((posStart_ * 0.8f + posEnd_ * 0.2f) + new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-3, 3)));
            }
            for (float timeSum_ = 0; timeSum_ < timeMax_; timeSum_ += Globals.timeDeltaFixed)
            {
                float p_ = (timeSum_ / timeMax_);

                for (int i = 0; i < Effects.Count; i++)
                {
                    Effects[i].transform.position = Library.BezierQuadratic(posStart_, posMiddle_[i], posEnd_, p_);
                }
                yield return null;
            }
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(Effects[i]);
            }
        }
    }

    public static IEnumerator Stigma(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;
        OnEntryActivatePassive(unitSelf_, skill_);

        unitSelf_._BarrierValue += 40;

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator SuddenAttack(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (skill_._refTiming == "StartOfBattle")
        {
            skill_._parameter._iCount = 1;
        }
        else if (skill_._refTiming == "OnCalculateDealDamage")
        {
            if (skill_._parameter._iCount == 1 && unitSelf_._parameter._unitType == "Hero")
                Globals.damageCoefOnCalcDamage *= 1.5f;
        }
        else if (skill_._refTiming == "AfterCastSkill")
        {
            if (unitSelf_._parameter._unitType == "Hero")
            {
                skill_._parameter._iCount--;
            }
        }
    }

    public static IEnumerator Swift(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        if (skill_._refTiming == "StartOfBattle")
        {
            skill_._parameter._iCount = 1;
            skill_._parameter._isShowAsIcon = true;
        }
        if (skill_._refTiming == "OnApplyBuff")
        {
            if (skill_._parameter._iCount == 1)
            {
                unitSelf_._parameter._spApplied += 15;
            }
        }
        if (skill_._refTiming == "OnTakenDamage")
        {
            skill_._parameter._iCount--;
            skill_._parameter._isShowAsIcon = false;
        }
    }

    public static IEnumerator Thorn(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitsOnTargetList_[0]._IsAlive() == false) yield break;

        OnEntryActivatePassive(unitSelf_, skill_);

        //yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        unitSelf_._DealDamage(unitsOnTargetList_[0], (skill_._refValue * 0.2f).ToInt(), skill_, "Static");

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator ThunderStrike(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();
        int killCount_ = 0;

        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        unitSelf_._modelTransform.LookAt(unitOnTarget_.transform.position);

        General.Instance.StartCoroutine(General.SetLightIntensity(0.2f, 0.5f));
        GameObject Effect00_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicCircle_Purple"), unitSelf_._goComps.transform);
        yield return new WaitForSeconds(0.7f / Globals.Instance.gameSpeed);
        GameObject Effect01_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "MagicCircle_Yellow"), unitOnTarget_._goComps.transform);

        yield return new WaitForSeconds(0.7f / Globals.Instance.gameSpeed);
        unitSelf_._animator.SetTrigger("triggerUseItem00");
        yield return new WaitForSeconds(0.6f / Globals.Instance.gameSpeed);
        GameObject Thunder_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "ThunderStrike"), unitOnTarget_._goComps.transform.position, Quaternion.Euler(-90, 0, 0));

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Magic");
            unitOnTarget_i_._GainStatus("Stun", 1);
            if (unitOnTarget_i_._parameter._hp < 1)
                killCount_++;
        }

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_, killCount_: killCount_);

        yield return new WaitForSeconds(0.7f / Globals.Instance.gameSpeed);
        General.Instance.StartCoroutine(General.SetLightIntensity(1f, 0.5f));

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Tumble(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;

        float dashRange_ = skill_._parameter._moveRange + (skill_._parameter._spRatio * unitSelf_._parameter._sp) / 10;
        Vector3 posTarget_ = unitSelf_.transform.position + (Globals.posOnCursorAtGround - unitSelf_._modelTransform.position).ClampByLength(dashRange_);
        bool isAddPassive_ = true;

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_ == unitSelf_) continue;

            if (unit_i_._IsOverlapColliderArea(unitSelf_, posTarget_))
            {
                Battle.SetInactiveSkillSuggestion(unitSelf_);
                yield break;
            }
        }

        OnEntryCastSkill(unitSelf_, skill_);

        unitSelf_._modelTransform.LookAt(posTarget_);
        unitSelf_._animator.SetTrigger("triggerRoll");

        yield return new WaitForSeconds(0.1f);
        yield return General.Instance.StartCoroutine(General.MoveTowards(unitSelf_.gameObject, posTarget_, Globals.UNIT_MOVE_PER_SEC * 2));

        foreach (_Skill passive_i_ in unitSelf_._parameter._additionalPassives)
        {
            if (passive_i_._parameter._name == "Basic Attack Bonus Tumble")
                isAddPassive_ = false;
        }

        if (isAddPassive_)
        {
            _Skill passive_ = _Skill.OriginalSkillList.Find(m => m._parameter._name == "Basic Attack Bonus Tumble");
            passive_._parameter._baseValue = skill_._parameter._baseValue;

            unitSelf_._parameter._additionalPassives.Add(passive_);
        }

        foreach (_SkillAbility ability_i_ in skill_._parameter._skillAbilities)
            ability_i_._functionEffect?.Invoke(unitSelf_, skill_, ability_i_, "AfterCastSkill", unitsOnTargetList_);

        OnExitCastSkill(unitSelf_, skill_);
    }

    public static IEnumerator Twilight(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (skill_ == null) yield break;

        unitSelf_._GainStatus("Artifact", 1);
    }

    public static IEnumerator UndyingRage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._IsAlive() == false) yield break;

        OnEntryActivatePassive(unitSelf_, skill_);

        unitSelf_._outline.enabled = true;
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        unitSelf_._Heal(unitSelf_, unitSelf_._parameter._hpMax / 20, skill_);
        yield return new WaitForSeconds(1.4f / Globals.Instance.gameSpeed);
        unitSelf_._outline.enabled = false;

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator Vitality(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_ == null) yield break;

        OnEntryActivatePassive(unitSelf_, skill_);

        unitSelf_._parameter._spBuff = unitSelf_._parameter._spBuff.Clamp(0, 100);
        unitSelf_._parameter._statusConditions.Find(m => m._name == "Stun")._count = 0;
        unitSelf_._parameter._statusConditions.Find(m => m._name == "Snare")._count = 0;

        OnExitActivatePassive(unitSelf_, skill_);
    }

    public static IEnumerator Volley(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        Vector3 posTarget_ = Globals.posOnCursorAtGround;
        List<Coroutine> runningCoroutine_ = new List<Coroutine>();
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        unitSelf_._modelTransform.LookAt(posTarget_);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToLaunchBullet / Globals.Instance.gameSpeed);

        Vector3 vector_ = posTarget_ - unitSelf_.transform.position;
        Vector3 posStart_ = unitSelf_._posCenter + vector_.normalized * Globals.ARROW_LENGTH;

        for (int i = 0; i < skill_._parameter._iCount; i++)
        {
            float angle_ = (skill_._parameter._angle / (skill_._parameter._iCount - 1)) * (-(skill_._parameter._iCount - 1) / 2 + i);
            runningCoroutine_.Add(General.Instance.StartCoroutine(Volley_ShootArrow(vector_.RotateThisByRadian(0, angle_.ToRadian(), 0))));
        }

        foreach (_Unit unit_i_ in unitsOnTargetList_)
        {
            runningCoroutine_.Add(General.Instance.StartCoroutine(Volley_DealDamage(unit_i_)));
        }

        foreach (Coroutine c in runningCoroutine_)
            yield return c;

        OnExitCastSkill(unitSelf_, skill_);

        IEnumerator Volley_ShootArrow(Vector3 vector_i_)
        {
            _Unit targetUnit_ = null;
            List<RaycastHit> hits_ = Physics.RaycastAll(unitSelf_.transform.position, vector_i_, skill_._parameter._hitRange, /*layer = Unit*/ 1 << 10).ToList();
            hits_.Sort((a, b) => a.distance.CompareTo(b.distance));
            float length_ = skill_._parameter._hitRange;

            foreach (RaycastHit hit_i_ in hits_)
            {
                _Unit unit_i_ = hit_i_.transform.parent.parent.GetComponent<_Unit>();

                if (unit_i_ == null) continue;
                if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;

                targetUnit_ = unit_i_;
                length_ = hit_i_.distance;
                break;
            }

            if (targetUnit_ == null)
            {
                GameObject Arrow_ = UnityEngine.Object.Instantiate(Prefabs.goInstances.Find(m => m.name == "Arrow"), posStart_, Quaternion.identity);
                Arrow_.transform.LookAt(unitSelf_._posCenter + vector_i_);
                yield return General.MoveTowards(Arrow_, unitSelf_._posCenter + vector_i_, Globals.PRJ_MOVE_PER_SEC * 0.8f, true);
            }
            else
            {
                GameObject Arrow_ = UnityEngine.Object.Instantiate(Prefabs.goInstances.Find(m => m.name == "Arrow"), posStart_, Quaternion.identity, targetUnit_._goBody.transform);
                Arrow_.transform.LookAt(unitSelf_._posCenter + vector_i_);
                yield return General.MoveTowards(Arrow_, unitSelf_._posCenter + vector_i_.normalized * length_, Globals.PRJ_MOVE_PER_SEC * 0.8f);
                UnityEngine.Object.Destroy(Arrow_, 2f / Globals.Instance.gameSpeed);
            }
        }

        IEnumerator Volley_DealDamage(_Unit unit_i_)
        {
            float distance_ = (unit_i_.transform.position - unitSelf_.transform.position).magnitude;
            yield return new WaitForSeconds((distance_ -1) / (Globals.PRJ_MOVE_PER_SEC * 0.8f * Globals.Instance.gameSpeed));

            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unit_i_, skill_, damageBase_, unit_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unit_i_, damage_, skill_, "Physical");
        }
    }

    public static IEnumerator WarriorAttack(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        unitSelf_._modelTransform.LookAt(unitsOnTargetList_[0].transform.position);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
        }

        OnExitCastSkill(unitSelf_, skill_);
    }
    public static IEnumerator Whirlwind(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);
        int damageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        unitSelf_._modelTransform.LookAt(Globals.posOnCursorAtGround);

        yield return new WaitForSeconds(skill_._parameter._delayTimeToDealDamage / Globals.Instance.gameSpeed);

        GameObject Whirlwind = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Whirlwind"), unitSelf_._posCenter, Quaternion.identity);
        Whirlwind.transform.SetParent(unitSelf_._modelTransform);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            int damage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, damageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitSelf_._DealDamage(unitOnTarget_i_, damage_, skill_, "Physical");
        }

        OnExitCastSkill(unitSelf_, skill_);
    }
    public static IEnumerator WindBlast(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        OnEntryCastSkill(unitSelf_, skill_);

        GameObject Effect_ = UnityEngine.Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_Orange"), unitSelf_._goComps.transform);
        Effect_.transform.localScale = skill_._parameter._effectSize.ToVector3();

        yield return new WaitForSeconds(0.5f / Globals.Instance.gameSpeed);

        unitSelf_._GainBuff(skill_._parameter._buffType[0], skill_._parameter._buffValue[0]);

        OnExitCastSkill(unitSelf_, skill_);
    }
}
