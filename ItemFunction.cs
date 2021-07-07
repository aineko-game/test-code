using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ItemFunction
{
    public static void OnEnterItemActivate()
    {
        _Item item_ = Globals.itemOnActive;

        if(--item_._stackCount < 1)
        {
            Globals.itemsInBagList[Globals.itemsInBagList.IndexOf(item_)] = default;
        }

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            unit_i_._damagePopup = 0;
            unit_i_._healingPopup = 0;
        }

        Globals.triggerStateBasedAction = true;
        Globals.InputState = "HeroOnActive";
        Globals.inputStopperCount++;
        Globals.Instance.paramsOnStackList.Clear();
        Globals.Instance.itemsOnStackList.Clear();

        Battle.SetInactiveItemSuggestion();
        UI.ConfigureItemsUI();
        //Prefabs.goSkillSuggestion.SetActive(false);
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public static void OnExitItemActivate()
    {
        Globals.inputStopperCount--;
    }

    public static IEnumerator ThrowPotion(_Item item_, Vector3 posOnCursor_)
    {
        GameObject Potion_ = new GameObject("Potion", typeof(SpriteRenderer));
        Potion_.transform.SetParent(Prefabs.Instance.transform);
        Potion_.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(item_._pathIcon);
        Potion_.transform.localRotation = Camera.main.transform.localRotation;
        Potion_.transform.localScale = 0.6f.ToVector3();

        Vector3 posStart_;
        Vector3 posMiddle_;
        Vector3 posEnd_ = posOnCursor_;
        if (Globals.posOnCursorAtGround.x > 0)
        {
            posStart_ = Globals.posOnCursorAtGround + new Vector3(-3.0f, 0.0f, 0.0f);
            posMiddle_ = Globals.posOnCursorAtGround + new Vector3(-1.5f, 4f, 0.0f);
        }
        else
        {
            posStart_ = Globals.posOnCursorAtGround + new Vector3(+3.0f, 0.0f, 0.0f);
            posMiddle_ = Globals.posOnCursorAtGround + new Vector3(+1.5f, 4f, 0.0f);
        }

        for (float timeSum_ = 0.15f, timeMax_ = 0.5f; timeSum_ < timeMax_ + Globals.timeDeltaFixed; timeSum_ += Globals.timeDeltaFixed)
        {
            float p_ = (timeSum_ / timeMax_).Clamp(0, 1);

            Potion_.transform.position = Library.BezierQuadratic(posStart_, posMiddle_, posEnd_, p_);

            yield return null;
        }

        Object.Destroy(Potion_);
    }

    public static IEnumerator Activate_ApplyBuff(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) yield break;
        if (item_._DetectUnits(item_, Globals.posOnCursorAtGround).Count < 1) yield break;

        _Unit unitOnTarget_ = item_._DetectUnits(item_, posOnTarget_)[0];

        OnEnterItemActivate();

        yield return General.Instance.StartCoroutine(ThrowPotion(item_, posOnTarget_));

        GameObject Effect = Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "Buff_" + item_._effectColor), unitOnTarget_._goComps.transform);
        Effect.transform.localScale = item_._effectSize.ToVector3();

        for (int i = 0; i < item_._buffTypes.Count; i++)
        {
            unitOnTarget_._GainBuff(item_._buffTypes[i], item_._buffValues[i]);
        }

        OnExitItemActivate();
    }

    public static IEnumerator Activate_BlockPotion(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) yield break;
        if (item_._DetectUnits(item_, Globals.posOnCursorAtGround).Count < 1) yield break;

        _Unit unitOnTarget_ = item_._DetectUnits(item_, posOnTarget_)[0];

        OnEnterItemActivate();

        yield return General.Instance.StartCoroutine(ThrowPotion(item_, posOnTarget_));

        int powerCoef_ = (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Potion Mastery") == null) ? 1 : 2;
        unitOnTarget_._BarrierValue += item_._iValue00 * powerCoef_;
        //_Unit.DealDamage(null, unitOnTarget_, item_._sdDamage, null, "Static");

        OnExitItemActivate();
    }

    public static IEnumerator Activate_ExplosivePotion(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) yield break;
        
        OnEnterItemActivate();

        yield return General.Instance.StartCoroutine(ThrowPotion(item_, posOnTarget_));

        GameObject Effect_ = Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "ExplosivePotion"), posOnTarget_, Quaternion.identity);
        Effect_.transform.SetParent(Prefabs.Instance.transform);
        Effect_.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.10f / Globals.Instance.gameSpeed);

        int powerCoef_ = (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Potion Mastery") == null) ? 1 : 2;
        foreach (_Unit unit_i_ in item_._DetectUnits(item_, posOnTarget_))
        {
            //int damage_ = Battle.ComputeAppliedDamage(null, unit_i_, null, item_._mdDamage, unit_i_._parameter._mrApplied);
            int damage_ = item_._sdDamage;
            _Unit.DealDamage(null, unit_i_, damage_ * powerCoef_, null, "Static");
        }

        OnExitItemActivate();
    }

    public static IEnumerator Activate_FirePotion(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) yield break;
        if (item_._DetectUnits(item_, Globals.posOnCursorAtGround).Count < 1) yield break;

        _Unit unitOnTarget_ = item_._DetectUnits(item_, posOnTarget_)[0];

        OnEnterItemActivate();

        yield return General.Instance.StartCoroutine(ThrowPotion(item_, posOnTarget_));

        int powerCoef_ = (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Potion Mastery") == null) ? 1 : 2;
        _Unit.DealDamage(null, unitOnTarget_, item_._sdDamage * powerCoef_, null, "Static");

        OnExitItemActivate();
    }

    public static IEnumerator Activate_HealingPotion(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) yield break;
        if (item_._DetectUnits(item_, Globals.posOnCursorAtGround).Count < 1) yield break;

        _Unit unitOnTarget_ = item_._DetectUnits(item_, posOnTarget_)[0];

        OnEnterItemActivate();

        yield return General.Instance.StartCoroutine(ThrowPotion(item_, posOnTarget_));

        int powerCoef_ = (Globals.Instance.globalEffectList.Find(m => m._parameter._name == "Potion Mastery") == null) ? 1 : 2;
        _Unit.Heal(null, unitOnTarget_, item_._restoreValue * powerCoef_, null);

        OnExitItemActivate();
    }

    public static IEnumerator Activate_ScrollOfBlessing(_Item item_, Vector3 posOnTarget_)
    {
        if (item_?._DetectUnits(item_, Globals.posOnCursorAtGround).Count < 1) yield break;

        OnEnterItemActivate();

        List<_Unit> unitsOnTargetList_ = new List<_Unit>(Globals.heroList);

        foreach (_Unit unit_i_ in unitsOnTargetList_)
        {
            General.Instance.StartCoroutine(Activate_ScrollOfProtection_GainProtection(unit_i_));
            yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);
        }

        OnExitItemActivate();

        IEnumerator Activate_ScrollOfProtection_GainProtection(_Unit unit_)
        {
            Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "GainProtection"), unit_._goComps.transform);
            yield return new WaitForSeconds(0.4f);
            unit_._BarrierValue += 80;
        }
    }

    public static IEnumerator Activate_ScrollOfFury(_Item item_, Vector3 posOnTarget_)
    {
        if (item_?._DetectUnits(item_, Globals.posOnCursorAtGround).Count < 1) yield break;

        OnEnterItemActivate();

        List<_Unit> unitsOnTargetList_ = new List<_Unit>(Globals.heroList);

        foreach (_Unit unit_i_ in unitsOnTargetList_)
        {
            General.Instance.StartCoroutine(Activate_ScrollOfProtection_GainProtection(unit_i_));
            yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);
        }

        OnExitItemActivate();

        IEnumerator Activate_ScrollOfProtection_GainProtection(_Unit unit_)
        {
            Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "GainFury"), unit_._goComps.transform);
            yield return new WaitForSeconds(0.4f);
            unit_._GainStatus("Fury", 1);
        }
    }

    public static IEnumerator Activate_ScrollOfProtection(_Item item_, Vector3 posOnTarget_)
    {
        if (item_?._DetectUnits(item_, Globals.posOnCursorAtGround).Count < 1) yield break;

        OnEnterItemActivate();

        List<_Unit> unitsOnTargetList_ = new List<_Unit>(Globals.heroList);

        foreach (_Unit unit_i_ in unitsOnTargetList_)
        {
            General.Instance.StartCoroutine(Activate_ScrollOfProtection_GainProtection(unit_i_));
            yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);
        }

        OnExitItemActivate();

        IEnumerator Activate_ScrollOfProtection_GainProtection(_Unit unit_)
        {
            Object.Instantiate(Prefabs.goParticles.Find(m => m.name == "GainProtection"), unit_._goComps.transform);
            yield return new WaitForSeconds(0.4f);
            unit_._GainStatus("Protection", 1);
        }
    }

    public static List<_Unit> DetectUnits_MouseCircle(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) return default;
        
        return Battle.DetectUnitsByCircle(posOnTarget_, item_._hitRange, item_._unitTypesTargetableList, /*isOnlyAlive = */true);
    }

    public static void DisplayArea_None(_Item item_, Vector3 posOnTarget_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        //Prefabs.imTargetAreaCircle.gameObject.SetActive(false);
        //Prefabs.imHitAreaCircle.gameObject.SetActive(false);
        //Prefabs.imHitAreaArc.gameObject.SetActive(false);
        //Prefabs.imHitAreaLine.gameObject.SetActive(false);
    }

    public static void DisplayArea_AllHeroes(_Item item_, Vector3 posOnTarget_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (Globals.unitOnMouseover?._parameter._unitType == "Hero")
        {
            foreach (_Unit unit_i_ in Globals.heroList)
            {
                unit_i_._srColliderArea.color = Globals.SR_COLLIDER_AREA_RED;
            }
            Globals.triggerResetColliderColor = true;
        }
    }

    public static void DisplayArea_MouseCircle(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null)
        {
            Battle.SetInactiveItemSuggestion();
            return;
        }

        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }

        Prefabs.imTargetAreaCircle.gameObject.SetActive(true);
        //Prefabs.imHitAreaCircle.gameObject.SetActive(false);
        //Prefabs.imHitAreaArc.gameObject.SetActive(false);
        //Prefabs.imHitAreaLine.gameObject.SetActive(false);
        
        Prefabs.imTargetAreaCircle.transform.position = Globals.posOnCursorAtGround;
        Prefabs.imTargetAreaCircle.transform.localScale = new Vector3(item_._hitRange * 2, item_._hitRange * 2, 1);
    }

    public static void SimulateEffect_NormalDamage(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) return;
        
        List<_Unit> unitsList = item_._DetectUnits(item_, Globals.posOnCursorAtGround);

        foreach (_Unit unit_i_ in unitsList)
        {
            int adDamage_ = Battle.ComputeAppliedDamage(null, unit_i_, null, item_._adDamage, unit_i_._parameter._arApplied, "AD");
            int mdDamage_ = Battle.ComputeAppliedDamage(null, unit_i_, null, item_._mdDamage, unit_i_._parameter._mrApplied, "MD");
            int sdDamage_ = item_._sdDamage;
            unit_i_._parameter._hpToLose = (adDamage_ + mdDamage_ + sdDamage_).Clamp(0, unit_i_._parameter._hpOnView);
            unit_i_._canvas.sortingOrder = 1;
        }
        Globals.triggerResetCanvasOrder = true;
    }

    public static void SimulateEffect_NormalRestore(_Item item_, Vector3 posOnTarget_)
    {
        if (item_ == null) return;
        
        List<_Unit> unitsList = item_._DetectUnits(item_, Globals.posOnCursorAtGround);

        foreach (_Unit unit_i_ in unitsList)
        {
            unit_i_._parameter._hpToRestore = item_._restoreValue;
            unit_i_._canvas.sortingOrder = 1;
        }
        Globals.triggerResetCanvasOrder = true;
    }

    public static void SimulateEffect_None(_Item item_, Vector3 posOnTarget)
    {

    }
}
