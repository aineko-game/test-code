using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Battle
{
    public static IEnumerator ActivatePassive(_Unit unit_, _Skill skill_, List<_Unit> unitsOnTarget_ = default, string refTiming_ = "", int refValue_ = 0, _Skill refSkill_ = null)
    {
        if (skill_._parameter._cooldownRemaining > 0) yield break;
        if (unitsOnTarget_ == default && skill_._functionDetectUnits != null)
            unitsOnTarget_ = skill_._functionDetectUnits(unit_.transform.position, Globals.posOnCursorAtGround, skill_);

        skill_._refValue = refValue_;
        skill_._refTiming = refTiming_;
        skill_._refSkill = refSkill_;

        yield return skill_._functionCastSkill(unit_, unitsOnTarget_, skill_);

        if (refTiming_.IsNullOrEmpty())
            Debug.LogError("Invalid timing : " + refTiming_);
    }

    public static IEnumerator AnimateGameOver()
    {
        Globals.inputStopperCount++;
        Globals.InputState = "AnimatingUI";
        //Globals.Instance.isGameOver = true;
        SetUnitOnInactive(Globals.InputState);
        UI.goGameOver.SetActive(true);
        InputModule.imControls.Battle.Disable();

        UI.ConfigureGraphicsAlpha(UI.graphicsGameOver, 0f);

        for (float timeSum = 0, timeMax = 1.5f; timeSum < timeMax + Globals.timeDeltaFixed; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum / timeMax).Clamp(0, 1);
            UI.ConfigureGraphicsAlpha(UI.graphicsGameOver, p_);
            yield return null;
        }

        Globals.inputStopperCount--;
        Globals.InputState = "GameOver";

        SaveData.Remove("Globals");
        //SaveData.Clear();
        SaveData.Save();
    }

    public static void ConfigureField(Vector3 posCenter)
    {
        Globals.Instance.fieldPosCenter = posCenter;
        Globals.Instance.fieldPosMin = posCenter + new Vector3(-Globals.FIELD_WIDTH / 2, -Globals.FIELD_HEIGHT / 2, -Globals.FIELD_LENGTH / 2);
        Globals.Instance.fieldPosMax = posCenter + new Vector3(+Globals.FIELD_WIDTH / 2, +Globals.FIELD_HEIGHT / 2, +Globals.FIELD_LENGTH / 2);
        Globals.Instance.fieldCubicPointsArray = Globals.Instance.fieldPosCenter.ToCubicPoints(Globals.FIELD_WIDTH, Globals.FIELD_HEIGHT, Globals.FIELD_LENGTH);

        Vector3[] positions_ = Globals.Instance.fieldCubicPointsArray.CubicPointsToUpperXZ();
        Prefabs.lrFieldOutline.SetPositions(positions_);
    }

    public static int ComputeAppliedDamage(_Unit unitSelf_, _Unit unitOnTarget_, _Skill skill_, int damage_, int resistance_, string type_)
    {
        Globals.bonusDamageOnCalcDamage = 0;
        Globals.damageCoefOnCalcDamage = 1;
        Globals.resistCoefOnCalcDamage = 1;

        if (unitSelf_?._parameter._statusConditions.Find(m => m._name == "Blind")._count > 0 && skill_._parameter._descriptiveName == "Basic Attack")
            return 0;

        if (unitSelf_ != null && skill_ != null)
        {
            foreach (_Skill passive_i_ in unitSelf_._parameter._classPassives)
            {
                if (passive_i_._parameter._triggerTiming?.Contains("OnCalculateDealDamage") == false) continue;

                passive_i_._refType =  type_;
                General.Instance.StartCoroutine(ActivatePassive(unitSelf_, passive_i_, new List<_Unit> { unitOnTarget_ }, refTiming_: "OnCalculateDealDamage", refSkill_: skill_));
            }
            foreach (_Skill passive_i_ in unitSelf_._parameter._additionalPassives)
            {
                if (passive_i_._parameter._triggerTiming?.Contains("OnCalculateDealDamage") == false) continue;

                passive_i_._refType = type_;
                General.Instance.StartCoroutine(ActivatePassive(unitSelf_, passive_i_, new List<_Unit> { unitOnTarget_ }, refTiming_: "OnCalculateDealDamage", refSkill_: skill_));
            }
        }

        if (unitOnTarget_ != null && skill_ != null)
        {
            foreach (_Skill passive_i_ in unitOnTarget_._parameter._classPassives)
            {
                if (passive_i_._parameter._triggerTiming?.Contains("OnCalculateTakenDamage") == false) continue;

                passive_i_._refType = type_;
                General.Instance.StartCoroutine(ActivatePassive(unitOnTarget_, passive_i_, refTiming_: "OnCalculateTakenDamage", refSkill_: skill_));
            }
            foreach (_Skill passive_i_ in unitOnTarget_._parameter._additionalPassives)
            {
                if (passive_i_._parameter._triggerTiming?.Contains("OnCalculateTakenDamage") == false) continue;

                passive_i_._refType = type_;
                General.Instance.StartCoroutine(ActivatePassive(unitOnTarget_, passive_i_, refTiming_: "OnCalculateTakenDamage", refSkill_: skill_));
            }
        }

        foreach (_Skill passive_i_ in new List<_Skill>(Globals.Instance.globalEffectList))
        {
            if (passive_i_._parameter._triggerTiming?.Contains("OnCalculateDealDamage") == false) continue;

            General.Instance.StartCoroutine(ActivatePassive(unitSelf_, passive_i_, new List<_Unit> { unitOnTarget_ }, refTiming_: "OnCalculateDealDamage", refSkill_: skill_));
        }

        if (unitOnTarget_?._parameter._statusConditions.Find(m => m._name == "Protection")._count > 0)
            Globals.damageCoefOnCalcDamage *= 0.0f;
        if (unitSelf_?._parameter._statusConditions.Find(m => m._name == "Fury")._count > 0)
            Globals.damageCoefOnCalcDamage *= 1.5f;

        damage_ = ((damage_ + Globals.bonusDamageOnCalcDamage) * Globals.damageCoefOnCalcDamage).ToInt();
        resistance_ = (resistance_ * Globals.resistCoefOnCalcDamage).ToInt();

        if (resistance_ > 0)
            return (damage_ * 100f / (100f + resistance_)).ToInt();
        else
            return (damage_ * (100f + - resistance_) / 100f).ToInt();
    }

    public static int ComputeAppliedResotoreValue(_Unit unit_, _Skill skill_)
    {
        return skill_._parameter._restoreValueBase + (unit_._parameter._hpMax * skill_._parameter._hpRatio).ToInt() + (unit_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();
    }

    public static List<_Unit> DetectUnitsByCircle(Vector3 posCenter, float radius_, List<string> unitTypeArray_, bool isOnlyAlive)
    {
        List<_Unit> unitsList_ = new List<_Unit>();

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_._parameter._hp < 1 && isOnlyAlive) continue;
            if (unitTypeArray_.Contains(unit_i_._parameter._unitType) == false) continue;

            if ((unit_i_.transform.position - posCenter).magnitude < (radius_ + unit_i_._parameter._colliderRange))
            {
                unitsList_.Add(unit_i_);
            }
        }
        return unitsList_;
    }

    public static List<_Unit> DetectUnitsBySphereCast(Vector3 posStart_, Vector3 vector_, float radius_)
    {
        List<_Unit> unitsList_ = new List<_Unit>();

        foreach (RaycastHit hits_ in Physics.SphereCastAll(posStart_, radius_, vector_, vector_.magnitude, /*layer = Unit*/ 1 << 10))
        {
            if (hits_.collider.transform.parent.parent.GetComponent<_Unit>() == true)
            {
                _Unit unit_i_ = hits_.collider.GetComponentInParent<_Unit>();

                if (unit_i_._parameter._hp < 1) continue;

                unitsList_.Add(unit_i_);
            }
        }
        return unitsList_;
    }

    public static List<_Unit> DetectUnitsByRaycast(Vector3 posStart_, Vector3 vector_)
    {
        List<_Unit> unitsList_ = new List<_Unit>();

        foreach (RaycastHit hits_ in Physics.RaycastAll(posStart_, vector_, vector_.magnitude, /*layer = Unit*/ 1 << 10))
        {
            if (hits_.collider.transform.parent.parent.GetComponent<_Unit>() == true)
            {
                _Unit unit_i_ = hits_.collider.GetComponentInParent<_Unit>();

                if (unit_i_._parameter._hp < 1) continue;

                unitsList_.Add(unit_i_);
            }
        }
        return unitsList_;
    }

    public static IEnumerator EndHeroTurn()
    {
        if (Globals.InputState.Slice(0, 4) != "Hero") yield break;

        SetUnitOnInactive(/*inputStateNew = */ Globals.InputState);

        foreach (_Hero hero_i_ in Globals.heroList)
        {
            if (hero_i_._IsAlive() == false) continue;

            for (int j = 0; j < hero_i_._parameter._classPassives.Count; j++)
            {
                if (hero_i_._parameter._classPassives[j]._parameter._triggerTiming?.Contains("EndOfYourTurn") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(hero_i_, hero_i_._parameter._classPassives[j], refTiming_: "EndOfYourTurn");
            }
            for (int j = 0; j < hero_i_._parameter._additionalPassives.Count; j++)
            {
                if (hero_i_._parameter._additionalPassives[j]._parameter._triggerTiming?.Contains("EndOfYourTurn") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(hero_i_, hero_i_._parameter._additionalPassives[j], refTiming_: "EndOfYourTurn");
            }

            foreach (_Skill skill_j_ in new List<_Skill>(Globals.Instance.globalEffectList))
            {
                if (skill_j_._parameter._triggerTiming?.Contains("EndOfYourTurn") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0) yield return null;

                yield return ActivatePassive(hero_i_, skill_j_, refTiming_: "EndOfYourTurn");
            }

            hero_i_._DecayDisable(1);
            hero_i_._DisplayBuffAndStatusIcon();
        }

        while (Globals.inputStopperCount > 0 || Globals.triggerStateBasedAction == true)
            yield return null;

        foreach (_Enemy enemy_i_ in Globals.enemyList)
        {
            if (enemy_i_._IsAlive() == false)
            {
                Globals.Instance.enemyParametersKilledInBattle.Add(enemy_i_._parameter.DeepCopy());
                Object.Destroy(enemy_i_.gameObject);
            }
        }

        foreach (_Object object_i_ in new List<_Object>(Globals.objectList))
        {
            if (object_i_._IsAlive() == false)
            {
                Object.Destroy(object_i_.gameObject);
                continue;
            }

            for (int j = 0; j < object_i_._parameter._classPassives.Count; j++)
            {
                if (object_i_._parameter._classPassives[j]._parameter._triggerTiming?.Contains("EndOfHeroTurn") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(object_i_, object_i_._parameter._classPassives[j], refTiming_: "EndOfHeroTurn");
            }
            for (int j = 0; j < object_i_._parameter._additionalPassives.Count; j++)
            {
                if (object_i_._parameter._additionalPassives[j]._parameter._triggerTiming?.Contains("EndOfHeroTurn") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(object_i_, object_i_._parameter._additionalPassives[j], refTiming_: "EndOfHeroTurn");
            }
        }

        if (IsGameOver() == false && IsWinBattle() == false)
            General.Instance.StartCoroutine(StartEnemyTurn());
    }

    public static IEnumerator EndEnemyTurn()
    {
        foreach (_Enemy enemy_i_ in Globals.enemyList)
        {
            if (enemy_i_._IsAlive() == false)
            {
                Object.Destroy(enemy_i_.gameObject);
                continue;
            }

            for (int j = 0; j < enemy_i_._parameter._classPassives.Count; j++)
            {
                if (enemy_i_._parameter._classPassives[j]._parameter._triggerTiming?.Contains("EndOfYourTurn") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(enemy_i_, enemy_i_._parameter._classPassives[j], refTiming_: "EndOfYourTurn");
            }

            for (int j = 0; j < enemy_i_._parameter._additionalPassives.Count; j++)
            {
                if (enemy_i_._parameter._additionalPassives[j]._parameter._triggerTiming?.Contains("EndOfYourTurn") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(enemy_i_, enemy_i_._parameter._additionalPassives[j], refTiming_: "EndOfYourTurn");
            }

            enemy_i_._DecayDisable(1);
            enemy_i_._DisplayBuffAndStatusIcon();

            //enemy_i_._DecayBuffStatus();
            //enemy_i_._DecayDisable();
            //enemy_i_._DisplayBuffAndStatusIcon();
        }

        General.Instance.StartCoroutine(StartHeroTurn());
    }

    public static IEnumerator ExecuteStateBaseAction()
    {
        if (Globals.isRunningSBA) yield break;
        string inputStateSave_ = Globals.InputState;
        Globals.InputState = "ExecutingSBA";
        Globals.inputStopperCount++;
        Globals.isRunningSBA = true;

        foreach (_Enemy enemy_i_ in Globals.enemyList)
        {
            yield return ExecuteStateBaseAction_TriggerPassive_OnDeath(enemy_i_);
            if (enemy_i_._IsAlive() == false)
                enemy_i_.gameObject.SetActive(false);
        }

        foreach (_Object object_i_ in Globals.objectList)
        {
            yield return ExecuteStateBaseAction_TriggerPassive_OnDeath(object_i_);
            if (object_i_._IsAlive() == false)
                object_i_.gameObject.SetActive(false);
        }

        foreach (_Hero hero_i_ in Globals.heroList)
        {
            yield return ExecuteStateBaseAction_TriggerPassive_OnDeath(hero_i_);
        }

        if (Globals.triggerStateBasedAction == false)
        {
            if (IsGameOver())
            {
                Globals.InputState = inputStateSave_;
                Globals.inputStopperCount--;
                Globals.isRunningSBA = false;
                General.Instance.StartCoroutine(AnimateGameOver());
                yield break;
            }
            
            if (IsDefeatBoss())
            {
                yield return General.Instance.StartCoroutine(MakeMinionsRunaway());
                Globals.bossList.Clear();
                Globals.InputState = inputStateSave_;
                Globals.inputStopperCount--;
                Globals.isRunningSBA = false;
                Globals.triggerStateBasedAction = true;
                yield break;
            }

            if (IsWinBattle())
            {
                Globals.InputState = inputStateSave_;
                Globals.inputStopperCount--;
                Globals.isRunningSBA = false;
                General.Instance.StartCoroutine(WinBattle_Coroutine());
                yield break;
            }
        }

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            unit_i_._ApplyBuff();
        }

        UI.ConfigureItemsUI();
        UI.ConfigureHeroesUI();
        UI.ConfigureUnitUI(Globals.unitOnActive);

        Globals.InputState = inputStateSave_;
        Globals.inputStopperCount--;
        Globals.triggerSaveData = true;
        Globals.isRunningSBA = false;

        IEnumerator ExecuteStateBaseAction_TriggerPassive_OnDeath(_Unit unit_i_)
        {
            for (int j = 0; j < unit_i_._parameter._classPassives.Count; j++)
            {
                if (unit_i_._IsAlive() == true) break;
                if (unit_i_._parameter._classPassives[j]._parameter._triggerTiming?.Contains("OnDeath") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(unit_i_, unit_i_._parameter._classPassives[j], refTiming_: "OnDeath");
                unit_i_._parameter._classPassives.Remove(unit_i_._parameter._classPassives[j--]);
            }

            for (int j = 0; j < unit_i_._parameter._additionalPassives.Count; j++)
            {
                if (unit_i_._IsAlive() == true) break;
                if (unit_i_._parameter._additionalPassives[j]._parameter._triggerTiming?.Contains("OnDeath") == false) continue;

                while (Globals.inputStopperCount > 1 || Globals.runningAnimationCount > 0)
                    yield return null;

                yield return ActivatePassive(unit_i_, unit_i_._parameter._additionalPassives[j], refTiming_: "OnDeath");
                unit_i_._parameter._additionalPassives.Remove(unit_i_._parameter._additionalPassives[j--]);
            }
        }
    }

    public static void InitializeUnitsComps()
    {
        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (Globals.triggerResetColliderColor)
                unit_i_._srColliderArea.color = Globals.SR_COLLIDER_AREA_WHITE;

            if (Globals.triggerResetHpBar)
            {
                unit_i_._parameter._hpToLose = 0;
                unit_i_._parameter._hpToRestore = 0;            }

            if (Globals.triggerResetCanvasOrder)
                unit_i_._canvas.sortingOrder = 0;
        }

        Globals.triggerResetHpBar = false;
        Globals.triggerResetColliderColor = false;
        Globals.triggerResetCanvasOrder = false;
    }

    public static bool IsDefeatBoss()
    {
        if (IsGameOver() == true) return false;

        foreach (_Enemy boss_i_ in Globals.bossList)
        {
            if (boss_i_._IsAlive() == true) return false;

            if (boss_i_ == Globals.bossList.Last()) return true;
        }

        return false;
    }

    public static bool IsWinBattle()
    {
        if (IsGameOver() == true) return false;

        foreach (_Enemy enemy_i_ in Globals.enemyList)
        {
            if (enemy_i_._IsAlive() == true) return false;
        }

        return true;
    }

    public static bool IsGameOver()
    {
        foreach (_Hero hero_i_ in Globals.heroList)
        {
            if (hero_i_._IsAlive() == true) return false;
        }

        return true;
    }

    public static IEnumerator MakeMinionsRunaway()
    {
        List<Coroutine> coroutines_ = new List<Coroutine>();

        foreach (_Enemy enemy_i_ in Globals.enemyList)
        {
            if (enemy_i_._IsAlive() == false) continue;

            coroutines_.Add(General.Instance.StartCoroutine(enemy_i_._MoveHeroTo(new Vector3(18, 0, enemy_i_.transform.position.z))));
            enemy_i_._parameter._hp = 0;
        }

        foreach (Coroutine c_ in coroutines_)
            yield return c_;
    }

    public static void SetActiveMoveSuggestion(_Unit unit_)
    {
        if (unit_ == null) return;
        if (unit_._parameter._hp < 1) return;
        if (unit_._IsMovable() == false) return;

        SetUnitOnActive(unit_);

        Globals.InputState = "HeroOnMove";
        Prefabs.goMoveSuggestion.SetActive(true);
        Prefabs.imColliderArea.transform.localScale = new Vector3(unit_._parameter._colliderRange * 2, unit_._parameter._colliderRange * 2, 1);
        unit_._srMovableArea.gameObject.SetActive(true);
        //Prefabs.SetCursorActive(Resources.Load<Sprite>("UI/Move"));
    }

    public static void SetInactiveMoveSuggestion(_Unit unit_)
    {
        if (unit_ == null) return;
        //if (unit_._hp < 1) return;

        Globals.InputState = "HeroOnActive";
        Prefabs.goMoveSuggestion.SetActive(false);
        unit_._srMovableArea.gameObject.SetActive(false);
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Prefabs.SetCursorInactive();
    }

    public static void SetActiveItemSuggestion(int itemIndex_)
    {
        if (Globals.Instance.turnState != "Hero") return;
        if (Globals.Instance.sceneState != "Battle") return;
        if (itemIndex_.IsBetween(0, Globals.itemsInBagList.Count - 1) == false) return;
        if (Globals.itemsInBagList[itemIndex_] == null) return;
        if (IsGameOver()) return;
        if (IsWinBattle()) return;

        SetUnitOnInactive();

        _Item item_ = Globals.itemsInBagList[itemIndex_];
        Globals.itemOnActive = item_;

        Prefabs.goSkillSuggestion.SetActive(true);

        //if (Resources.Load<Texture2D>(item_._pathIcon) is Texture2D tex_)
        //    Cursor.SetCursor(tex_, new Vector2(tex_.width / 2, tex_.height / 2), CursorMode.Auto);
        Prefabs.SetCursorActive(Resources.Load<Sprite>(item_._pathIcon));

        Globals.InputState = "HeroOnUseItem";
    }

    public static void SetInactiveItemSuggestion()
    {
        Globals.itemOnActive = null;
        Globals.InputState = "HeroOnActive";
        Prefabs.goSkillSuggestion.SetActive(false);
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Prefabs.SetCursorInactive();
    }

    public static void SetActiveSkillSuggestion(_Unit unit_, int skillIndex_)
    {
        if (unit_ == null) return;
        if ((unit_ is _Hero) == false) return;
        if (unit_._IsAlive() == false) return;
        if (unit_._parameter._statusConditions.Find(m => m._name == "Stun")._count > 0) return;
        if (unit_._parameter._skills[skillIndex_] == null) return;
        if (unit_._parameter._skills[skillIndex_]._parameter == null) return;
        if (unit_._parameter._skills[skillIndex_]._parameter._name.IsNullOrEmpty()) return;
        if (unit_._parameter._skills[skillIndex_]._IsCastable() == false) return;
        if (unit_._IsCastableThisSkill(unit_._parameter._skills[skillIndex_]) == false) return;

        _Skill skill_ = unit_._parameter._skills[skillIndex_];
        unit_._skillOnActive = skill_;

        SetInactiveMoveSuggestion(unit_);

        Globals.InputState = "HeroOnCastSkill";
        Prefabs.goSkillSuggestion.SetActive(true);
        UI.ConfigureUnitUI(unit_);
        //if (Resources.Load<Texture2D>("UI/Attack") is Texture2D tex_)
        //    Cursor.SetCursor(tex_, new Vector2(tex_.width / 2, tex_.height / 2), CursorMode.Auto);
        Prefabs.SetCursorActive(Resources.Load<Sprite>("UI/Attack"));
    }

    public static void SetInactiveSkillSuggestion(_Unit unit_)
    {
        if (unit_ == null) return;
        if (unit_._IsAlive() == false) return;

        unit_._skillOnActive = default;

        if (Globals.InputState.Slice(0, 4) == "Hero")
            Globals.InputState = "HeroOnActive";
        Prefabs.goSkillSuggestion.SetActive(false);
        Prefabs.goMoveSuggestion.SetActive(false);
        unit_._srMovableArea.gameObject.SetActive(false);
        UI.ConfigureUnitUI(unit_);
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Prefabs.SetCursorInactive();
    }

    public static void SetActiveEquipSuggestion(_Unit unit_, int equipIndex_)
    {
        if (unit_ == null) return;
        if (unit_._IsAlive() == false) return;
        if (unit_._parameter._equips[equipIndex_] == null) return;
        if (unit_._parameter._equips[equipIndex_]._castableLimitCount < 1) return;
        if (unit_._parameter._equips[equipIndex_]._castableStacks < 1) return;
        if (unit_._parameter._equips[equipIndex_]._cooldownRemaining > 0) return;
        if (unit_._parameter._statusConditions.Find(m => m._name == "Stun")._count > 0 && unit_._parameter._equips[equipIndex_]._active._parameter._isCastableWhileDisabled == false) return;

        _Skill skill_ = unit_._parameter._equips[equipIndex_]._active;
        unit_._skillOnActive = skill_;

        SetInactiveMoveSuggestion(unit_);

        Globals.equipOnActive = unit_._parameter._equips[equipIndex_];
        Globals.InputState = "HeroOnCastSkill";
        Prefabs.goSkillSuggestion.SetActive(true);
        UI.ConfigureUnitUI(unit_);
        //if (Resources.Load<Texture2D>("UI/Attack") is Texture2D tex_)
        //    Cursor.SetCursor(tex_, new Vector2(tex_.width / 2, tex_.height / 2), CursorMode.Auto);
        Prefabs.SetCursorActive(Resources.Load<Sprite>("UI/Attack"));
    }

    public static void SetUnitOnActive(_Unit unit_)
    {
        if (unit_ == null) return;
        if (unit_._IsAlive() == false) return;

        SetUnitOnInactive(/*inputStateNew = */ Globals.InputState);
        Globals.unitOnActive = unit_;
        Globals.InputState = "HeroOnActive";
        UI.goUnit.SetActive(true);
        UI.ConfigureHeroesUI();
        UI.ConfigureUnitUI(unit_);

        unit_._outline.enabled = true;
    }

    public static void SetUnitOnInactive(string inputStateNew_ = "HeroMain")
    {
        if (Globals.unitOnActive != null)
        {
            SetInactiveMoveSuggestion(Globals.unitOnActive);
            SetInactiveSkillSuggestion(Globals.unitOnActive);
            Globals.unitOnActive._srMovableArea.gameObject.SetActive(false);
            Globals.unitOnActive._outline.enabled = false;
            Globals.unitOnActive = default;
        }

        SetInactiveItemSuggestion();
        UI.ConfigureHeroesUI();
        UI.goUnit.SetActive(false);
        Globals.equipOnActive = null;
        Globals.InputState = inputStateNew_;
    }

    public static IEnumerator StartBattle(int seed_, string battleType_, string enemyName_ = "", string objectName_ = "")
    {
        if (Globals.InputState.Slice(0, 5) == "Enemy") yield break;
        Globals.inputStopperCount++;

        Sprite transitionSprite_ = (battleType_ == "Boss") ? Prefabs.Instance.transitionSprites[3] :
                                   /*else              */   Prefabs.Instance.transitionSprites[4];

        yield return General.Instance.StartCoroutine(General.TransitionScreen(Prefabs.Instance.transitionIn, transitionSprite_, 0.6f));
        General.ChangeScene("Battle");

        Globals.Instance.randomOnBattle = new _Random((uint)seed_);
        Globals.Instance.cameraPosOffset = new Vector3(0f, 16, -7.9f);
        Globals.Instance.enemyParametersKilledInBattle.Clear();
        Globals.Instance.turnCount = 0;
        InputModule.imControls.Battle.Enable();
        UI.goGameOver.SetActive(false);
        UI.goWinBattle.SetActive(false);
        UI.ConfigureHeroesUI();
        UI.ConfigureItemsUI();
        ConfigureField(Vector3.zero);

        string[] heroTableKey_ = new string[3] { "Hero 00", "Hero 01", "Hero 02" };
        string objectTableKey_ = "None";
        string enemyTableKey_ = "Enemy Table";

        if (Cheat.Instance?.transform.Find("Canvas").Find("DebugCommand").Find("NewBattle"))
        {
            TMP_Dropdown[] dropdowns_ = Cheat.Instance.transform.Find("Canvas").Find("DebugCommand").Find("NewBattle").GetComponentsInChildren<TMP_Dropdown>();
            if (dropdowns_ != null && dropdowns_.Length == 4)
            {
                heroTableKey_ = new string[] { dropdowns_[0].Text(), dropdowns_[1].Text(), dropdowns_[2].Text() };
                enemyTableKey_ = dropdowns_[3].Text();
            }
        }

        if (objectName_.IsNullOrEmpty())
            objectTableKey_ = Table.ObjectTable[new System.Tuple<int, string>(Globals.Instance.stageCount, "Normal")].GetRandom(Globals.Instance.randomOnBattle);
        else
            objectTableKey_ = objectName_;

        if (battleType_ == "Boss")
            enemyTableKey_ = Globals.Instance.bossName;
        if (enemyTableKey_ == "Random Enemy")
        {
            if (Globals.Instance.spotCurrent._distanceFromStart < 4)
                enemyTableKey_ = Table.EnemyTable[new System.Tuple<int, string>(Globals.Instance.stageCount, "EnemyWeak")].GetRandom(Globals.Instance.randomOnBattle);
            else
                enemyTableKey_ = Table.EnemyTable[new System.Tuple<int, string>(Globals.Instance.stageCount, "EnemyStrong")].GetRandom(Globals.Instance.randomOnBattle);
        }

        if (enemyName_.IsNullOrEmpty() == false)
            enemyTableKey_ = enemyName_;

        StartBattle_CreateUnits(heroTableKey_, objectTableKey_, enemyTableKey_);

        yield return StartBattle_StageEffect(battleType_);

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            foreach (_Skill skill_i_ in new List<_Skill>(unit_i_._parameter._classPassives))
            {
                if (skill_i_._parameter._triggerTiming.Contains("StartOfBattle") == false) continue;

                skill_i_._refTiming = "StartOfBattle";
                yield return skill_i_._functionCastSkill?.Invoke(unit_i_, null, skill_i_);
            }
            foreach (_Skill skill_i_ in new List<_Skill>(unit_i_._parameter._additionalPassives))
            {
                if (skill_i_._parameter._triggerTiming.Contains("StartOfBattle") == false) continue;

                skill_i_._refTiming = "StartOfBattle";
                yield return skill_i_._functionCastSkill?.Invoke(unit_i_, null, skill_i_);
            }
        }

        //List<_Skill> triggeredGlobalEffects_ = new List<_Skill>(Globals.Instance.globalEffectList);
        foreach (_Skill skill_i_ in new List<_Skill>(Globals.Instance.globalEffectList))
        {
            if (skill_i_._parameter._triggerTiming.Contains("StartOfBattle") == false) continue;

            skill_i_._refTiming = "StartOfBattle";
            yield return skill_i_._functionCastSkill?.Invoke(null, null, skill_i_);
        }

        yield return General.Instance.StartCoroutine(StartHeroTurn());

        Globals.inputStopperCount--;
    }

    public static void StartBattle_CreateUnits(string[] heroTableKeys_, string objectTableKey_, string enemyTableKey_)
    {
        Object.Destroy(_Unit._GoUnits.Find("TreasureChest")?.gameObject);

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            unit_i_.transform.position = new Vector3(-100, 0, 0);
        }

        for (int i_ = 0; i_ < Globals.heroList.Count; i_++)
        {
            _Hero hero_i_ = Globals.heroList[i_];

            if (i_ < heroTableKeys_.Length && heroTableKeys_[i_] != "Hero " + i_.ToString("D2"))
            {
                Object.Destroy(hero_i_.gameObject);
                Globals.unitList.Remove(hero_i_);
                Globals.heroList.Remove(hero_i_);

                hero_i_ = _Unit.CloneFromString(heroTableKeys_[i_]) as _Hero;
                Globals.unitList.Remove(hero_i_);
                Globals.heroList.Remove(hero_i_);
                Globals.unitList.Insert(i_, hero_i_);
                Globals.heroList.Insert(i_, hero_i_);
            }

            hero_i_._animator.Rebind();
            //hero_i_._PlaceHeroRandomly(i_.Mod(3));
        }

        foreach (_Object object_i_ in _Unit._GoUnits.GetComponentsInChildren<_Object>())
        {
            Object.Destroy(object_i_.gameObject);
        }
        foreach (_Enemy enemy_i_ in _Unit._GoUnits.GetComponentsInChildrenOnlyInactive<_Enemy>())
        {
            Object.Destroy(enemy_i_.gameObject);
        }
        if (enemyTableKey_ == "Enemy Table")
        {
            Globals.objectList.Clear();
            Globals.bossList.Clear();
            Globals.enemyList = new List<_Enemy>(_Unit._GoUnits.GetComponentsInChildren<_Enemy>(false));
            Globals.unitList = new List<_Unit>(Globals.heroList);
            Globals.unitList.AddRange(Globals.enemyList);
        }
        else
        {
            Globals.objectList.Clear();
            Globals.bossList.Clear();
            Globals.enemyList.Clear();
            foreach (_Enemy enemy_i_ in _Unit._GoUnits.GetComponentsInChildren<_Enemy>(true))
            {
                Object.Destroy(enemy_i_.gameObject);
                Globals.unitList.Remove(enemy_i_);
            }
        }

        string[][] objectsData_ = Table.ObjectWaveData.GetValue(objectTableKey_);
        foreach (string[] objectData_i_ in objectsData_)
        {
            _Object objectNew_i_ = _Unit.CloneFromTableData(objectData_i_) as _Object;
            objectNew_i_._PlaceRandomlyOrDestroy(out bool isDestroy_);
        }

        foreach (_Unit unit_i_ in Globals.heroList)
        {
            unit_i_._PlaceHeroRandomly();
        }

        if (Globals.enemyList.Count == 0)
        {
            if (Table.EnemyWaveData.TryGetValue(enemyTableKey_, out string[][] enemiesData_) == false)
            {
                if (Globals.Instance.spotCurrent._distanceFromStart < 4)
                    enemyTableKey_ = Table.EnemyTable[new System.Tuple<int, string>(Globals.Instance.stageCount, "EnemyWeak")].GetRandom(Globals.Instance.randomOnBattle);
                else
                    enemyTableKey_ = Table.EnemyTable[new System.Tuple<int, string>(Globals.Instance.stageCount, "EnemyStrong")].GetRandom(Globals.Instance.randomOnBattle);
                enemiesData_ = Table.EnemyWaveData.GetValue(enemyTableKey_);
            }

            foreach (string[] enemyData_i_ in enemiesData_)
            {
                _Enemy enemyNew_i_ = _Unit.CloneFromTableData(enemyData_i_) as _Enemy;
                enemyNew_i_._PlaceRandomlyOrDestroy(out bool isDestroy_);
            }
        }

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            for (int j_ = 0; j_ < unit_i_._parameter._skills.Length; j_++)
            {
                if (unit_i_._parameter._skills[j_] is _Skill skill_j_ && skill_j_._originalSkill is _Skill)
                {
                    skill_j_._parameter._cooldownRemaining = 0;
                }
            }

            for (int j_ = 0; j_ < unit_i_._parameter._equips.Length; j_++)
            {
                if (unit_i_._parameter._equips[j_] is _Equip equip_j_ && equip_j_._name.IsNullOrEmpty() == false)
                {
                    unit_i_._parameter._equips[j_] = _Equip.CloneFromString(equip_j_._name);
                }
            }

            unit_i_._CopyStatusFromOriginal();
            unit_i_._ApplyLevel();
            unit_i_._ApplyEquips();
            unit_i_._ApplyPassive_Static();
            unit_i_._ApplyBuff();
            unit_i_._RecomputeComponents();
            unit_i_._parameter._hp = unit_i_._parameter._hpMax;
            unit_i_._parameter._hpOnView = unit_i_._parameter._hp;
            unit_i_._BarrierValue = 0;
            unit_i_._SetAnimatorCondition();
            unit_i_._DisplayBuffAndStatusIcon();
        }
    }

    public static IEnumerator StartBattle_StageEffect(string battleType_)
    {
        Sprite transitionSprite_ = (battleType_ == "Boss") ? Prefabs.Instance.transitionSprites[3] :
                                   /*else              */   Prefabs.Instance.transitionSprites[4];

        List<Vector3> posSave_ = new List<Vector3>();
        foreach (_Unit unit_i_ in Globals.unitList)
            posSave_.Add(unit_i_.transform.position);

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_._parameter._entranceType == "Stay") continue;
            else if (unit_i_._parameter._entranceType == "FromLeft")
            {
                unit_i_.transform.position = new Vector3(-18, 0, unit_i_.transform.position.z);
            }
            else if (unit_i_._parameter._entranceType == "FromRight")
            {
                unit_i_.transform.position = new Vector3(+18, 0, unit_i_.transform.position.z);
            }
        }

        yield return new WaitForSeconds(0.2f / Globals.Instance.gameSpeed);
        yield return General.Instance.StartCoroutine(General.TransitionScreen(Prefabs.Instance.transitionOut, transitionSprite_, 0.6f));
        yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

        if (battleType_ == "Boss")
            yield return General.Instance.StartCoroutine(UI.ShowBossWarning());
        else
            yield return General.Instance.StartCoroutine(UI.ShowChapter("StartBattle"));

        for (int i = 0; i < Globals.unitList.Count; i++)
        {
            _Unit unit_i_ = Globals.unitList[i];
            //unit_i_._modelTransform.localRotation = unit_i_._qrtSOB;

            if (unit_i_._parameter._entranceType == "Stay") continue;

            if (unit_i_._parameter._entranceType == "FromRight")
            {
                yield return unit_i_._MoveHeroTo(posSave_[i]);
            }
            if (unit_i_._parameter._entranceType == "FromLeft")
            {
                yield return unit_i_._MoveHeroTo(posSave_[i]);
            }
        }
    }

    public static IEnumerator StartHeroTurn()
    {
        Globals.Instance.paramsOnStackList.Clear();
        Globals.Instance.paramsAtStartOfTurn.Clear();
        Globals.Instance.itemsOnStackList.Clear();
        Globals.Instance.itemsAtStartOfTurn.Clear();
        Globals.Instance.turnState = "Hero";
        Globals.Instance.turnCount++;
        
        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_ is _Hero hero_i_)
            {
                hero_i_._parameter._movableCount = hero_i_._parameter._movableCountMax;
                hero_i_._parameter._actableCount = hero_i_._parameter._actableCountMax;

                if (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Guardian Shield") == null)
                    hero_i_._BarrierValue = 0;

                hero_i_._parameter._statusConditions.Find(m => m._name == "Stealth")._count = (hero_i_._parameter._statusConditions.Find(m => m._name == "Stealth")._count - 1);

                if (Globals.Instance.turnCount > 1)
                    hero_i_._DecayBuffStatus();
                hero_i_._DisplayBuffAndStatusIcon();
                hero_i_._SetAnimatorCondition();

                //unit_i_._parameter._classPassive._parameter._cooldownRemaining = (unit_i_._parameter._classPassive._parameter._cooldownRemaining - 1).Clamp(0, 99);
                foreach (_Skill skill_j_ in unit_i_._parameter._skills)
                {
                    if (skill_j_ == null) continue;
                    skill_j_._parameter._cooldownRemaining = (skill_j_._parameter._cooldownRemaining - 1).Clamp(0, 99);
                }
                foreach (_Skill passive_j_ in unit_i_._parameter._classPassives)
                {
                    //if (skill_j_ == null) continue;
                    passive_j_._parameter._cooldownRemaining = (passive_j_._parameter._cooldownRemaining - 1).Clamp(0, 99);
                }
                foreach (_Equip equip_j_ in unit_i_._parameter._equips)
                {
                    if (equip_j_ == null || equip_j_._active == null) continue;
                    equip_j_._cooldownRemaining = (equip_j_._cooldownRemaining - 1).Clamp(0, 9);
                }
            }
            else if (unit_i_ is _Enemy enemy_)
            {
                enemy_._parameter._movableCount = 0;
                enemy_._parameter._actableCount = 0;
            }

            yield return null;
            Globals.Instance.paramsAtStartOfTurn.Add(unit_i_._SaveParameter());
            yield return null;
        }

        foreach (_Skill skill_i_ in new List<_Skill>(Globals.Instance.globalEffectList))
        {
            if (skill_i_._parameter._triggerTiming?.Contains("StartOfTurn") == true)
                General.Instance.StartCoroutine(Battle.ActivatePassive(null, skill_i_, new List<_Unit>() { null }, "StartOfTurn"));
        }

        Globals.Instance.itemsAtStartOfTurn = Globals.itemsInBagList.DeepCopy();

        yield return General.Instance.StartCoroutine(UI.ShowChapter("Hero"));

        //General.SaveDataAll();
        Globals.InputState = "HeroMain";
        Globals.triggerStateBasedAction = true;
    }

    public static IEnumerator StartEnemyTurn()
    {
        Globals.InputState = "AnimatingUI";
        Globals.Instance.turnState = "Enemy";
        Globals.inputStopperCount++;

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unit_i_ is _Hero hero_)
            {

            }
            else if (unit_i_ is _Enemy enemy_i_)
            {
                enemy_i_._parameter._movableCount = enemy_i_._parameter._movableCountMax;
                enemy_i_._parameter._actableCount = enemy_i_._parameter._actableCountMax;
                enemy_i_._BarrierValue = 0;

                enemy_i_._DecayBuffStatus();
                //enemy_i_._DecayDisable();
                enemy_i_._DisplayBuffAndStatusIcon();

                foreach (_Skill skill_j_ in enemy_i_._parameter._skills)
                {
                    if (skill_j_ == null) continue;
                    skill_j_._parameter._cooldownRemaining = (skill_j_._parameter._cooldownRemaining - 1).Clamp(0, 9);
                }
                foreach (_Equip equip_j_ in enemy_i_._parameter._equips)
                {
                    if (equip_j_ == null || equip_j_._active == null) continue;
                    equip_j_._cooldownRemaining = (equip_j_._cooldownRemaining - 1).Clamp(0, 9);
                }
            }
        }

        yield return General.Instance.StartCoroutine(UI.ShowChapter("Enemy"));
        
        General.Instance.StartCoroutine(_Enemy.AnimateEneiesAction());
        Globals.inputStopperCount--;
    }

    public static bool TryCastSkill(_Unit unitSelf_, _Unit unitOnCursor_, _Skill  skill_)
    {
        if (unitSelf_ == null) return false;
        if (unitSelf_._IsAlive() == false) return false;
        if (skill_ == null) return false;
        if (unitSelf_._IsCastableThisSkill(skill_) == false) return false;

        List<_Unit> unitsOnTargetList_ = skill_._functionDetectUnits(Globals.unitOnActive.transform.position, Globals.posOnCursorAtGround, skill_);

        if (skill_._parameter._castType == "Target")
        {
            unitsOnTargetList_ = unitsOnTargetList_.ExcludeStealth("Hero");
            if (unitOnCursor_ == null) return false;
            if (unitOnCursor_._parameter._hp < 1) return false;
            if (unitsOnTargetList_.Contains(unitOnCursor_) == false) return false;

            General.Instance.StartCoroutine(skill_._functionCastSkill(unitSelf_, new List<_Unit>() { unitOnCursor_ }, skill_));
        }
        else
        {
            General.Instance.StartCoroutine(skill_._functionCastSkill(unitSelf_, unitsOnTargetList_, skill_));
        }
        return true;
    }

    public static bool TryUseItem(_Unit unitOnCursor_)
    {
        if (Globals.itemOnActive == null) return false;

        _Item item_ = Globals.itemOnActive;

        if (item_._castType == "Target")
        {
            if (unitOnCursor_ == null) return false;
            if (unitOnCursor_._parameter._hp < 1) return false;
            if (item_._DetectUnits(item_, Globals.posOnCursorAtGround).Count == 0) return false;

            General.Instance.StartCoroutine(item_._Activate(item_, Globals.posOnCursorAtGround));
        }
        else
        {
            General.Instance.StartCoroutine(item_._Activate(item_, Globals.posOnCursorAtGround));
        }
        return true;
    }

    public static bool TryMove(_Unit unit_)
    {
        if (unit_ == null) return false;
        if (unit_._parameter._hp < 1) return false;
        if (unit_._parameter._movableCount < 1) return false;

        Vector3 posToMove_ = unit_.transform.position + (Globals.posOnCursorAtGround - unit_.transform.position).ClampByLength(unit_._parameter._movableRange);
        if (unit_._IsMovableToThisPos(posToMove_, unit_._parameter._unitTypesThroughable) == false) return false;

        unit_.StartCoroutine(unit_._MoveHeroTo(posToMove_));
        return true;
    }

    public static bool TryResetTurn()
    {
        if (Globals.InputState.Slice(0, 4) != "Hero") return false;

        for (int i_ = 0; i_ < Globals.unitList.Count; i_++)
        {
            _Unit unit_i_ = Globals.unitList[i_];
            unit_i_._LoadParameter(Globals.Instance.paramsAtStartOfTurn[i_]);

            unit_i_._ApplyBuff();
            unit_i_._RecomputeComponents();
            unit_i_._DisplayBuffAndStatusIcon();
        }

        for (int i_ = 0; i_ < Globals.itemsInBagList.Count; i_++)
        {
            _Item temp_ = Globals.Instance.itemsAtStartOfTurn[i_];

            if (temp_ == null || temp_._name.IsNullOrEmpty())
            {
                Globals.itemsInBagList[i_] = null;
            }
            else
            {
                Globals.itemsInBagList[i_] = _Item.CloneFromString(temp_._name);
                Globals.itemsInBagList[i_]._stackCount = temp_._stackCount;
            }
        }

        SetUnitOnInactive(/*inputStateNew = */ "HeroMain");

        return true;
    }

    public static bool TryUndoUnitMove()
    {
        if (Globals.InputState.Slice(0, 4) != "Hero") return false;
        if (Globals.Instance.paramsOnStackList.Count == 0) return false;

        List<_Unit._Parameter> parameterList_ = Globals.Instance.paramsOnStackList.Last();
        for (int i_ = 0; i_ < Globals.unitList.Count; i_++)
        {
            _Unit unit_i_ = Globals.unitList[i_];

            unit_i_._LoadParameter(parameterList_[i_]);

            unit_i_._ApplyBuff();
            unit_i_._RecomputeComponents();
            unit_i_._DisplayBuffAndStatusIcon();
        }
        Globals.Instance.paramsOnStackList.Remove(Globals.Instance.paramsOnStackList.Last());

        for (int i_ = 0; i_ < Globals.itemsInBagList.Count; i_++)
        {
            _Item temp_ = Globals.Instance.itemsOnStackList.Last()[i_];

            if (temp_ == null || temp_._name.IsNullOrEmpty()) continue;
            
            Globals.itemsInBagList[i_] = _Item.CloneFromString(temp_._name);
            Globals.itemsInBagList[i_]._stackCount = temp_._stackCount;
        }
        Globals.Instance.itemsOnStackList.Remove(Globals.Instance.itemsOnStackList.Last());

        SetUnitOnInactive(/*inputStateNew = */ "HeroMain");
        return true;
    }

    public static void UpdateOnBattle()
    {
        _Unit unitSelf_ = Globals.unitOnActive;
        InitializeUnitsComps();

        if (Globals.InputState == "ExecutingSBA")
        {
            return;
        }
        
        if (Globals.triggerStateBasedAction == true)
        {
            if (Globals.runningAnimationCount == 0 && Globals.inputStopperCount == 0)
            {
                Globals.triggerStateBasedAction = false;
                General.Instance.StartCoroutine(ExecuteStateBaseAction());
            }
        }

        if (Globals.InputState == "HeroMain")
        {

        }
        else if (Globals.InputState == "HeroOnActive")
        {

        }
        else if (Globals.InputState == "HeroOnMove")
        {
            UpdateMoveSuggestion(unitSelf_, Globals.posOnCursorAtGround, unitSelf_._parameter._movableRange);
        }
        else if (Globals.InputState == "HeroOnCastSkill")
        {
            UpdateSkillSuggestion(unitSelf_, Globals.posOnCursorAtGround);
        }
        else if (Globals.InputState == "HeroOnUseItem")
        {
            UpdateItemSuggestion(Globals.posOnCursorAtGround);
        }
    }

    public static void UpdateMoveSuggestion(_Unit unitSelf_, Vector3 posOnCursor_, float movableRange_, bool isDash_ = false)
    {
        if (unitSelf_ == null) return;
        if (unitSelf_._parameter._hp < 1) return;
        //if (unitSelf_._parameter._movableCount < 1) return;
        if (posOnCursor_ == Vector3.down) return;
        if (Globals.posOnCursorAtScreen == Vector2.zero) return;

        Vector3 posStart_ = unitSelf_.transform.position;
        Vector3 posToMove_ = posStart_ + (posOnCursor_ - posStart_).ClampByLength(movableRange_);
        List<string> unitTypesThroughable_ = (isDash_) ? Globals.ALL_UNIT_TYPES : unitSelf_._parameter._unitTypesThroughable;
        bool isMovableToPoint_ = unitSelf_._IsMovableToThisPos(posToMove_, unitTypesThroughable_);

        Prefabs.goMoveSuggestion.transform.position = posStart_;
        Prefabs.goMoveSuggestion.transform.LookAt(posToMove_);
        Prefabs.rtMoveArrow.sizeDelta = new Vector2((posToMove_ - posStart_).magnitude + 0.65f, 5.12f);
        Prefabs.imMoveArrow.sprite = (isMovableToPoint_) ? Prefabs.Instance.spMoveArrowGreen : Prefabs.Instance.spMoveArrowRed;
        Prefabs.imColliderArea.transform.position = posToMove_;
        Prefabs.imColliderArea.color = (isMovableToPoint_) ? new Color32(000, 255, 000, 255) : new Color32(255, 000, 000, 255);
        
        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (unitSelf_ == unit_i_) continue;
            if (unit_i_._parameter._hp < 1) continue;

            if ((unit_i_._IsOverlapColliderArea(unitSelf_, posToMove_)) ||
                (unit_i_._IsOnPath(posStart_, posToMove_) && unitTypesThroughable_.Contains(unit_i_._parameter._unitType) == false) && unit_i_._parameter._tags.Contains("Throughable") == false)
            {
                unit_i_._srColliderArea.color = Globals.SR_COLLIDER_AREA_RED;
                Globals.triggerResetColliderColor = true;
            }
        }
    }

    public static void UpdateSkillSuggestion(_Unit unitSelf_, Vector3 posOnCursor_)
    {
        if (unitSelf_ == null) return;
        if (unitSelf_ ._IsAlive() == false) return;
        if (unitSelf_._skillOnActive == null) return;
        if (unitSelf_._IsCastableThisSkill(unitSelf_._skillOnActive) == false) return;
        if (posOnCursor_ == Vector3.down) return;
        if (unitSelf_._skillOnActive == null) return;
        if (unitSelf_._skillOnActive._parameter == null) return;
        if (unitSelf_._skillOnActive._parameter._name.IsNullOrEmpty()) return;

        _Skill  skill_ = unitSelf_._skillOnActive;
        List<_Unit> unitsOnTargetList_ = skill_._functionDetectUnits(unitSelf_.transform.position, posOnCursor_, skill_);
        skill_._functionDisplaySkillArea(unitSelf_, posOnCursor_, skill_);

        if (skill_._parameter._castType == "Target")
            unitsOnTargetList_ = unitsOnTargetList_.ExcludeStealth("Hero");

        if (unitsOnTargetList_ != null)
        {
            foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
            {
                unitOnTarget_i_._srColliderArea.color = Globals.SR_COLLIDER_AREA_RED;
                Globals.triggerResetColliderColor = true;

                if (skill_._parameter._castType == "Target")
                {
                    _Unit unitOnCursor_ = Globals.unitOnMouseover;
                    if (unitOnCursor_ == null) continue;
                    if (unitOnCursor_._parameter._hp < 1) continue;
                    if (unitsOnTargetList_.Contains(unitOnCursor_) == false) continue;

                    skill_._functionSimulateEffect?.Invoke(unitSelf_, new List<_Unit>() { unitOnCursor_ }, skill_);
                    Globals.triggerResetHpBar = true;
                }
                else
                {
                    skill_._functionSimulateEffect?.Invoke(unitSelf_, unitsOnTargetList_, skill_);
                    Globals.triggerResetHpBar = true;
                }
            }
        }
    }

    public static void UpdateItemSuggestion(Vector3 posOnCursor_)
    {
        if (Globals.itemOnActive == null) return;

        _Item item_ = Globals.itemOnActive;
        
        item_._DisplayArea(item_, Globals.posOnCursorAtGround);
        List<_Unit> unitsOnTargetList_ = item_._DetectUnits(item_, Globals.posOnCursorAtGround);

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            unitOnTarget_i_._srColliderArea.color = Globals.SR_COLLIDER_AREA_RED;
            Globals.triggerResetColliderColor = true;

            if (item_._castType == "Target")
            {
                _Unit unitOnCursor_ = Globals.unitOnMouseover;
                if (unitOnCursor_ == null) continue;
                if (unitOnCursor_._parameter._hp < 1) continue;
                if (unitsOnTargetList_.Contains(unitOnCursor_) == false) continue;

                item_._SimulateEffect(item_, Globals.posOnCursorAtGround);
                Globals.triggerResetHpBar = true;
            }
            else
            {
                item_._SimulateEffect(item_, Globals.posOnCursorAtGround);
                Globals.triggerResetHpBar = true;
            }
        }
    }

    public static void UpdateKnockbackSuggestion(_Unit unitSelf_, Vector3 vectorTarget_, _Skill skill_, bool isDetectCollide_)
    {
        if (unitSelf_ == null) return;
        if (unitSelf_._IsAlive() == false) return;
        if (Globals.posOnCursorAtScreen == Vector2.zero) return;

        Vector3 posStart_ = unitSelf_.transform.position;
        float length_ = vectorTarget_.magnitude;
        float margin_ = 0.05f;
        bool isCollide_ = false;

        if (Physics.SphereCast(posStart_, unitSelf_._parameter._colliderRange, vectorTarget_, out RaycastHit hit, vectorTarget_.magnitude, 1 << 10))
        {
            length_ = hit.distance;
            if (isDetectCollide_)
                isCollide_ = true;
        }

        Vector3 posToMove_ = posStart_ + vectorTarget_.normalized * (length_ - margin_).Clamp(0, length_);
        posToMove_ = posToMove_.ClampByVector3(Globals.Instance.fieldPosMin, Globals.Instance.fieldPosMax);

        Prefabs.goMoveSuggestion.transform.position = posStart_;
        Prefabs.goMoveSuggestion.transform.LookAt(posToMove_);
        Prefabs.rtMoveArrow.sizeDelta = new Vector2((posToMove_ - posStart_).magnitude + 0.65f, 5.12f);
        Prefabs.imMoveArrow.sprite = (isCollide_) ? Prefabs.Instance.spMoveArrowRed : Prefabs.Instance.spMoveArrowGreen;
        Prefabs.imColliderArea.transform.position = posToMove_;
        Prefabs.imColliderArea.color = (isCollide_) ? Globals.SR_COLLIDER_AREA_RED : Globals.SR_COLLIDER_AREA_WHITE;
    }

    public static IEnumerator WinBattle_Coroutine()
    {
        Globals.inputStopperCount++;
        Globals.InputState = "AnimatingUI";
        SetUnitOnInactive(Globals.InputState);
        UI.goWinBattle.SetActive(true);
        InputModule.imControls.Battle.Disable();

        foreach (_Enemy enemy_i_ in Globals.enemyList)
        {
            if (enemy_i_._parameter._hp < 1)
            {
                Globals.Instance.enemyParametersKilledInBattle.Add(enemy_i_._parameter.DeepCopy());
                Object.Destroy(enemy_i_.gameObject);
                Object.Destroy(enemy_i_);
            }
        }
        foreach (_Object object_i_ in Globals.objectList)
        {
            Object.Destroy(object_i_.gameObject);
            Object.Destroy(object_i_);
        }

        List<_Skill> triggeredGlobalEffects_ = new List<_Skill>(Globals.Instance.globalEffectList);
        foreach (_Skill skill_i_ in triggeredGlobalEffects_)
        {
            if (skill_i_._parameter._triggerTiming.Contains("EndOfBattle") == false) continue;
            General.Instance.StartCoroutine(ActivatePassive(null, skill_i_, null, "EndOfBattle"));
        }

        if (Globals.Instance.spotCurrent._type == "Boss")
        {
            Globals.Instance.spotCurrent._isActive = false;
            GameObject goSpot_ = Map.goSpotsList[Globals.Instance.spotCurrent._index];
            goSpot_.transform.Find("Enemy").gameObject.SetActive(false);
            Map.HeroToken.transform.position = goSpot_.transform.position;
            Map.HeroToken.transform.LookAt(Map.HeroToken.transform.position + Vector3.right);
        }
        else /*if (Globals.Instance.spotCurrent._type == "Battle")*/
        {
            Globals.Instance.spotCurrent._isActive = false;
            WinBattle_CreateLoot();
        }

        UI.ConfigureGraphicsAlpha(UI.graphicsWinBattle, 0f);
        for (float timeSum = 0, timeMax = 0.4f; timeSum < timeMax + Globals.timeDeltaFixed; timeSum += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum / timeMax).Clamp(0, 1);
            UI.ConfigureGraphicsAlpha(UI.graphicsWinBattle, p_);
            yield return null;
        }

        //General.SaveDataAll();
        Globals.inputStopperCount--;
        Globals.InputState = "WinBattle";
    }

    public static void WinBattle_CreateLoot()
    {
        if (Globals.Instance.globalEffectList.Find(m => m._parameter._tags.Contains("Hunger")) != null) return;
        //if (Globals.Instance.Food < 1) return;

        int goldTotal_ = 0;
        int expTotal_ = 0;
        
        foreach (_Unit._Parameter parameter_i_ in Globals.Instance.enemyParametersKilledInBattle)
        {
            goldTotal_ += parameter_i_._goldEnemyHave;
            expTotal_ += parameter_i_._expEnemyHave;
        }
        Globals.Instance.enemyParametersKilledInBattle.Clear();

        goldTotal_ = (goldTotal_ * Globals.Instance.randomOnBattle.Range(0.9f, 1.1f)).ToInt();
        expTotal_ = (expTotal_ * Globals.Instance.randomOnBattle.Range(0.9f, 1.1f)).ToInt();

        if (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Amulet Coint") != null)
            goldTotal_ = (goldTotal_ * 1.2f).ToInt();

        if (goldTotal_ > 0)
            Globals.Instance.spotCurrent._lootsList.Add(new Map._Spot._Loot("Gold", goldTotal_));
        if (expTotal_ > 0)
            Globals.Instance.spotCurrent._lootsList.Add(new Map._Spot._Loot("Exp", expTotal_));

        UI.ConfigureWinBattle();
    }

    public static void WinBattle_TakeLoot(int index_)
    {
        if (index_ >= Globals.Instance.spotCurrent._lootsList.Count) return;

        List<Map._Spot._Loot> lootsList_ = Globals.Instance.spotCurrent._lootsList;

        if (lootsList_[index_]._type == "Gold")
        {
            Globals.Instance.Gold += lootsList_[index_]._value;
            lootsList_.RemoveAt(index_);
            UI.ConfigureWinBattle();
        }
        else if (lootsList_[index_]._type == "Exp")
        {
            List<int> levelAtStartList = new List<int>();
            foreach (_Hero hero_i_ in Globals.heroList)
            {
                levelAtStartList.Add(hero_i_._parameter._lv);
                hero_i_._GainExp(lootsList_[index_]._value);
            }
            UI.WinBattle_AnimateExpBar(lootsList_[index_]._value, levelAtStartList);
            lootsList_.RemoveAt(index_);
            UI.ConfigureWinBattle(false);
        }
    }
}
