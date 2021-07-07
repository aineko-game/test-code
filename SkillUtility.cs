using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class SkillUtility
{
    public static Vector3 ComputeBestPos_Closest(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill)
    {
        if (unitsOnTargetList_ == null) return unitSelf_.transform.position;
        if (unitsOnTargetList_.Count == 0) return unitSelf_.transform.position;
        if (unitsOnTargetList_[0] == null) return unitSelf_.transform.position;

        List<string> unitTypesAll_ = new List<string> { "Hero", "Enemy", "Object" };
        _Unit unitOnTarget_ = unitsOnTargetList_[0];
        _Unit._Parameter paramSelf_ = unitSelf_._parameter;
        Vector3 posStart_ = unitSelf_.transform.position;
        Vector3 posTarget_ = unitOnTarget_.transform.position;
        Vector3 posResultProv_ = posStart_;
        Vector3 vector_ = (posTarget_ - posStart_);
        float margin_ = 0.1f;
        float distanceMinProv_ = vector_.magnitude;

        for (int i_ = 1, iMax_ = 24; i_ < iMax_; i_++)
        {
            float radian_i_ = -Mathf.PI / 2 + (float)i_ / iMax_ * Mathf.PI;
            Vector3 vector_i_ = vector_.RotateThisByRadian(0f, radian_i_, 0f);
            Vector3 vectorNorm_i_ = vector_i_.normalized;
            Vector3 posResultProv_i_ = posStart_;
            Vector3 vectorClamped_i_ = ClampByFieldAndUnitNotThroughable(vector_i_);
            Vector3 perpendicularPoint_i_ = Library.ComputeClosestPos_LineAndPoint(posStart_, vectorNorm_i_, posTarget_);

            if (vectorClamped_i_.sqrMagnitude <= (posStart_ - perpendicularPoint_i_).sqrMagnitude)
            {
                posResultProv_i_ = ComputeDeployableClosestPoint(posStart_, posStart_ + vectorClamped_i_, i_);
            }
            else
            {
                Vector3 prov00_ = ComputeDeployableClosestPoint(posStart_, perpendicularPoint_i_, i_);
                Vector3 prov01_ = ComputeDeployableClosestPoint(posStart_ + vectorClamped_i_, perpendicularPoint_i_, i_);
                posResultProv_i_ = ((prov00_ - posTarget_).sqrMagnitude + 0.1f < (prov01_ - posTarget_).sqrMagnitude ) ? prov00_ : prov01_;
            }

            if ((posResultProv_i_ - posTarget_).magnitude < distanceMinProv_)
            {
                distanceMinProv_ = (posResultProv_i_ - posTarget_).magnitude;
                posResultProv_ = posResultProv_i_;
            }
        }

        return posResultProv_;


        Vector3 ClampByFieldAndUnitNotThroughable(Vector3 vector_x_)
        {
            Vector3 vectorClamped_ = (vector_x_.normalized) * unitSelf_._parameter._movableRange;
            if (Physics.Raycast(posStart_, vector_x_, out RaycastHit hit, 1000, /*layer = FieldWall*/ 1 << 12))
            {
                vectorClamped_ = vectorClamped_.ClampByLength(hit.distance - margin_);
            }

            foreach (RaycastHit hit_i_ in Physics.RaycastAll(posStart_, vector_x_, paramSelf_._movableRange + paramSelf_._colliderRange, /*layer = Unit */ 1 << 10))
            {
                _Unit unit_i_ = hit_i_.collider.GetComponentInParent<_Unit>();

                if (unitSelf_._parameter._unitTypesThroughable.Contains(unit_i_._parameter._unitType) == false && unit_i_._IsAlive() == true &&
                    unit_i_._parameter._statusConditions.Find(m => m._name == "Stealth")._count == 0 &&
                    unit_i_._parameter._tags.Contains("Throughable") == false)
                {
                    vectorClamped_ = vectorClamped_.ClampByLength(hit_i_.distance - paramSelf_._colliderRange - margin_);
                    break;
                }
            }

            return vectorClamped_;
        }

        Vector3 ComputeDeployableClosestPoint(Vector3 posStart_x_, Vector3 posEnd_x_, int i)
        {
            Vector3 vector_x_ = posEnd_x_ - posStart_x_;

            if (Battle.DetectUnitsByCircle(posEnd_x_, paramSelf_._colliderRange + margin_ / 2, unitTypesAll_, /*isOnlyAlive = */false).Exclude(unitSelf_).Count == 0)
            {
                return posEnd_x_;
            }
            else
            {
                RaycastHit[] hitsArray_ = Physics.SphereCastAll(posStart_x_, paramSelf_._colliderRange + margin_, vector_x_, vector_x_.magnitude, /*layer = Unit */ 1 << 10);
                for (int i_ = hitsArray_.Length - 1; i_ >= 0; i_--)
                {
                    RaycastHit hit_ = hitsArray_[i_];
                    Vector3 pos_x_i_ = posStart_x_ + vector_x_.normalized * hit_.distance;

                    if (hit_.transform.parent.parent == unitSelf_.transform) continue;


                    if (Battle.DetectUnitsByCircle(pos_x_i_, unitSelf_._parameter._colliderRange + margin_ / 2, unitTypesAll_, /*isOnlyAlive = */false).Exclude(unitSelf_).Count == 0)
                    {
                        return pos_x_i_;
                    }
                }
            }
            return posStart_;
        }
    }

    public static List<_Unit> DetectUnits_SelfTargetCircle(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        return Battle.DetectUnitsByCircle(posOnSelf_, skill_._parameter._targetRange, skill_._parameter._unitTypesTargetableList, /*isOnlyAlive = */true);
    }

    public static List<_Unit> DetectUnits_SelfHitCircle(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        return Battle.DetectUnitsByCircle(posOnSelf_, skill_._parameter._hitRange, skill_._parameter._unitTypesTargetableList, /*isOnlyAlive = */true);
    }

    public static List<_Unit> DetectUnits_TargetPoint_HitCircle(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        Vector3 posTarget_ = posOnSelf_ + (posOnCursor_ - posOnSelf_).ClampByLength(skill_._parameter._targetRange);

        return Battle.DetectUnitsByCircle(posTarget_, skill_._parameter._hitRange, skill_._parameter._unitTypesTargetableList, /*isOnlyAlive = */true);
    }

    public static List<_Unit> DetectUnits_SelfHitArc(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>();
        Vector3 vector00 = (posOnCursor_ - posOnSelf_).RotateThisByRadian(0, +skill_._parameter._angle.ToRadian() / 2, 0);
        Vector3 vector01 = (posOnCursor_ - posOnSelf_).RotateThisByRadian(0, -skill_._parameter._angle.ToRadian() / 2, 0);

        foreach (_Unit unit_i_ in Battle.DetectUnitsByCircle(posOnSelf_, skill_._parameter._hitRange, skill_._parameter._unitTypesTargetableList, /*isOnlyAlive = */true))
        {
            float radian_ = (unit_i_.transform.position - posOnSelf_).ToVector2_XZ().GetRadianFrom((posOnCursor_ - posOnSelf_).ToVector2_XZ());

            if (radian_.Abs() <= skill_._parameter._angle.ToRadian() / 2 ||
                unit_i_._sphereCollider.Raycast(new Ray(posOnSelf_, vector00), out RaycastHit hit00, skill_._parameter._hitRange) ||
                unit_i_._sphereCollider.Raycast(new Ray(posOnSelf_, vector01), out RaycastHit hit01, skill_._parameter._hitRange))
            {
                result.Add(unit_i_);
            }
        }
        return result;
    }

    public static List<_Unit> DetectUnits_HitLine_AllUnits(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>();

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsTargetable() == false) continue;

            if (unit_i_._sphereCollider.Raycast(new Ray(posOnSelf_, posOnCursor_ - posOnSelf_), out RaycastHit hit, skill_._parameter._hitRange))
            {
                result.Add(unit_i_);
            }
        }
        return result;
    }

    public static List<_Unit> DetectUnits_TargetLine_ClosestUnit(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>();
        List<RaycastHit> hits = Physics.RaycastAll(posOnSelf_, posOnCursor_ - posOnSelf_, skill_._parameter._targetRange, /*layer = Unit */ 1 << 10).ToList();
        hits.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsHitable() == false) continue;

            result.Add(unit_i_);
            return result;
        }
        return result;
    }

    public static List<_Unit> DetectUnits_Fireball(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>();
        Vector3 posTarget_ = posOnSelf_ + (posOnCursor_ - posOnSelf_).normalized * skill_._parameter._targetRange;
        List<RaycastHit> hits = Physics.RaycastAll(posOnSelf_, posOnCursor_ - posOnSelf_, skill_._parameter._targetRange, /*layer = Unit*/ 1 << 10).ToList();
        hits.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsHitable() == false) continue;

            result.Add(unit_i_);
            posTarget_ = hit.point;
            break;
        }

        if (result.Count == 0) return result;

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (result.Contains(unit_i_)) continue;
            if (unit_i_._IsHitable() == false) continue;
            if ((unit_i_.transform.position - posTarget_).sqrMagnitude > (skill_._parameter._hitRange + unit_i_._parameter._colliderRange).Square()) continue;

            result.Add(unit_i_);
        }
        return result;
    }

    public static List<_Unit> DetectUnits_Closest(Vector3 posSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>(Globals.unitList);

        for (int i = result.Count - 1; i >= 0; i--)
        {
            if (skill_._parameter._unitTypesTargetableList.Contains(result[i]._parameter._unitType) == false)
                result.RemoveAt(i);

            else if (result[i] == Globals.unitOnActive)
                result.RemoveAt(i);

            else if (result[i]._IsAlive() == false)
                result.RemoveAt(i);
        }

        result.Sort((a, b) => (a.transform.position - posSelf_).sqrMagnitude.CompareTo((b.transform.position - posSelf_).sqrMagnitude));

        for (int i = 0; i < result.Count; i++)
        {
            if ((posSelf_ - result[i].transform.position).sqrMagnitude > (result[i]._parameter._colliderRange + skill_._parameter._targetRange).Square())
            {
                result.RemoveRange(i, result.Count - i);
                break;
            }
        }

        return result;
    }

    public static List<_Unit> DetectUnits_Null(Vector3 posSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        return null;
    }

    public static List<_Unit> DetectUnits_Charge(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>();
        Vector3 vector00 = (posOnCursor_ - posOnSelf_).RotateThisByRadian(0, +skill_._parameter._angle.ToRadian() / 2, 0);
        Vector3 vector01 = (posOnCursor_ - posOnSelf_).RotateThisByRadian(0, -skill_._parameter._angle.ToRadian() / 2, 0);
        float dashRange_ = skill_._parameter._moveRange;
        Vector3 posTarget_ = posOnSelf_ + (posOnCursor_ - posOnSelf_).ClampByLength(dashRange_);

        foreach (_Unit unit_i_ in Battle.DetectUnitsByCircle(posTarget_, skill_._parameter._hitRange, skill_._parameter._unitTypesTargetableList, /*isOnlyAlive = */true))
        {
            float radian_ = (unit_i_.transform.position - posTarget_).ToVector2_XZ().GetRadianFrom((posOnCursor_ - posOnSelf_).ToVector2_XZ());

            if (radian_.Abs() <= skill_._parameter._angle.ToRadian() / 2 ||
                unit_i_._sphereCollider.Raycast(new Ray(posTarget_, vector00), out RaycastHit hit00, skill_._parameter._hitRange) ||
                unit_i_._sphereCollider.Raycast(new Ray(posTarget_, vector01), out RaycastHit hit01, skill_._parameter._hitRange))
            {
                result.Add(unit_i_);
            }
        }
        return result;
    }

    public static List<_Unit> DetectUnits_GhostStep(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        float dashRange_ = (skill_._parameter._baseValue + skill_._parameter._spRatio * Globals.unitOnActive._parameter._spApplied) / 10;
        Vector3 posTarget_ = posOnSelf_ + (posOnCursor_ - posOnSelf_).ClampByLength(dashRange_);

        List<_Unit> unitsList_ = Battle.DetectUnitsByCircle(posTarget_, skill_._parameter._targetRange, skill_._parameter._unitTypesTargetableList, /*isOnlyAlive = */true);

        if (unitsList_.Count == 0)
            return unitsList_;

        unitsList_.Sort((a, b) => (a.transform.position - posTarget_).sqrMagnitude.CompareTo((b.transform.position - posTarget_).sqrMagnitude));
        return new List<_Unit>() { unitsList_[0] };
    }

    public static List<_Unit> DetectUnits_IceShard(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>();
        Vector3 posTarget_ = posOnSelf_ + (posOnCursor_ - posOnSelf_).normalized * skill_._parameter._targetRange;
        List<RaycastHit> hits = Physics.RaycastAll(posOnSelf_, posOnCursor_ - posOnSelf_, skill_._parameter._targetRange, /*layer = Unit*/ 1 << 10).ToList();
        hits.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsHitable() == false) continue;

            result.Add(unit_i_);
            posTarget_ = hit.point;
            break;
        }

        if (result.Count == 0) return result;

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (result.Contains(unit_i_)) continue;
            if (unit_i_._IsHitable() == false) continue;
            if ((unit_i_.transform.position - posTarget_).sqrMagnitude > (skill_._parameter._hitRange + unit_i_._parameter._colliderRange).Square()) continue;

            Vector3 vector00 = (posOnCursor_ - posOnSelf_).RotateThisByRadian(0, +skill_._parameter._angle.ToRadian() / 2, 0);
            Vector3 vector01 = (posOnCursor_ - posOnSelf_).RotateThisByRadian(0, -skill_._parameter._angle.ToRadian() / 2, 0);
            float radian_ = (unit_i_.transform.position - posTarget_).ToVector2_XZ().GetRadianFrom((posOnCursor_ - posTarget_).ToVector2_XZ());

            if (radian_.Abs() <= skill_._parameter._angle.ToRadian() / 2 ||
                unit_i_._sphereCollider.Raycast(new Ray(posTarget_, vector00), out RaycastHit hit00, skill_._parameter._hitRange) ||
                unit_i_._sphereCollider.Raycast(new Ray(posTarget_, vector01), out RaycastHit hit01, skill_._parameter._hitRange))
            {
                result.Add(unit_i_);
            }
        }
        return result;
    }

    public static List<_Unit> DetectUnits_Volley(Vector3 posOnSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result = new List<_Unit>();

        for (int i = 0; i < skill_._parameter._iCount; i++)
        {
            float degree_i_ = (skill_._parameter._angle / (skill_._parameter._iCount - 1)) * (-(skill_._parameter._iCount - 1) / 2 + i);
            Vector3 posEnd_i_ = posOnSelf_ + (posOnCursor_ - posOnSelf_).RotateThisByRadian(0, degree_i_.ToRadian(), 0);

            List<RaycastHit> hits = Physics.RaycastAll(posOnSelf_, posEnd_i_ - posOnSelf_, skill_._parameter._hitRange, /*layer = Unit */ 1 << 10).ToList();
            hits.Sort((a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

                if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
                if (unit_i_._IsHitable() == false) continue;

                if (result.Contains(unit_i_) == false)
                    result.Add(unit_i_);
                break;
            }
        }

        return result;

        //List<RaycastHit> hits = Physics.RaycastAll(posOnSelf_, posOnCursor_ - posOnSelf_, skill_._parameter._targetRange, /*layer = Unit */ 1 << 10).ToList();
        //hits.Sort((a, b) => a.distance.CompareTo(b.distance));

        //foreach (RaycastHit hit in hits)
        //{
        //    _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

        //    if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
        //    if (unit_i_._IsHitable() == false) continue;

        //    result.Add(unit_i_);
        //    break;
        //}
        //return result;
    }

    public static void DisplaySkillArea_SelfTargetCircle(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imTargetAreaCircle.gameObject.SetActive(true);

        Prefabs.imTargetAreaCircle.transform.position = unitSelf_.transform.position;
        Prefabs.imTargetAreaCircle.transform.localScale = new Vector3(skill_._parameter._targetRange * 2, skill_._parameter._targetRange * 2, 1);
    }

    public static void DisplaySkillArea_SelfHitCircle(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imHitAreaCircle.gameObject.SetActive(true);

        Prefabs.imHitAreaCircle.transform.position = unitSelf_.transform.position;
        Prefabs.imHitAreaCircle.transform.localScale = new Vector3(skill_._parameter._hitRange * 2, skill_._parameter._hitRange * 2, 1);
    }

    public static void DisplaySkillArea_SelfToMouseHitArc(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imHitAreaArc.gameObject.SetActive(true);

        float degree_ = (posOnTarget_ - unitSelf_.transform.position).ToVector2_XZ().ToAngleFrom(Vector2.right) ;
        Prefabs.imHitAreaArc.transform.position = unitSelf_.transform.position;
        Prefabs.imHitAreaArc.transform.localScale = new Vector3(skill_._parameter._hitRange * 2, skill_._parameter._hitRange * 2, 1);
        //Prefabs.imHitAreaArc.fillAmount = skill_._parameter._angle.ToRadian() / (2 * Mathf.PI);
        //Prefabs.imHitAreaArc.transform.localRotation = Quaternion.Euler(90, 00, degree_ + skill_._parameter._angle.ToRadian().ToDegree() / 2);
        Prefabs.imHitAreaArc.transform.localRotation = Quaternion.Euler(90, 00, degree_);
        Prefabs.imHitAreaArc.gameObject.Find("Arc00").GetComponent<Image>().fillAmount = skill_._parameter._angle.ToRadian().ToDegree() / 2 / 360;
        Prefabs.imHitAreaArc.gameObject.Find("Arc00").transform.localRotation = Quaternion.Euler(00, 00, -skill_._parameter._angle.ToRadian().ToDegree() / 2);
        Prefabs.imHitAreaArc.gameObject.Find("Arc01").GetComponent<Image>().fillAmount = skill_._parameter._angle.ToRadian().ToDegree() / 2 / 360;
        Prefabs.imHitAreaArc.gameObject.Find("Arc01").transform.localRotation = Quaternion.Euler(00, 00, +skill_._parameter._angle.ToRadian().ToDegree() / 2);
    }

    public static void DisplaySkillArea_TargetPoint_HitCircle(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }

        Vector3 posTarget_ = unitSelf_.transform.position + (posOnTarget_ - unitSelf_.transform.position).ClampByLength(skill_._parameter._targetRange);

        Prefabs.imTargetAreaCircle.gameObject.SetActive(true);
        Prefabs.imHitAreaCircle.gameObject.SetActive(true);

        Prefabs.imTargetAreaCircle.transform.position = unitSelf_.transform.position;
        Prefabs.imTargetAreaCircle.transform.localScale = new Vector3(skill_._parameter._targetRange * 2, skill_._parameter._targetRange * 2, 1);
        Prefabs.imHitAreaCircle.transform.position = posTarget_;
        Prefabs.imHitAreaCircle.transform.localScale = new Vector3(skill_._parameter._hitRange * 2, skill_._parameter._hitRange * 2, 1);
    }

    public static void DisplaySkillArea_HitLine_AllUnits(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imHitAreaLines[0].gameObject.SetActive(true);

        Vector3 posStart_ = unitSelf_.transform.position;
        Vector3 posEnd_ = posStart_ + (posOnTarget_ - posStart_).normalized * skill_._parameter._hitRange;
        float degree_ = (posOnTarget_ - unitSelf_.transform.position).ToVector2_XZ().ToAngleFrom(Vector2.right);

        Prefabs.imHitAreaLines[0].transform.position = unitSelf_.transform.position;
        Prefabs.rtHitAreaLines[0].sizeDelta = new Vector2((posEnd_ - posStart_).magnitude, 5.12f);
        Prefabs.imHitAreaLines[0].transform.localRotation = Quaternion.Euler(90, 00, degree_ + skill_._parameter._angle / 2);
    }

    public static void DisplaySkillArea_TargetLine_ClosestUnit(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imHitAreaLines[0].gameObject.SetActive(true);

        List<_Unit> unitsList_ = skill_._functionDetectUnits?.Invoke(unitSelf_.transform.position, Globals.posOnCursorAtGround, skill_);
        float length_ = skill_._parameter._targetRange;
        if (unitsList_ != null && unitsList_.Count > 0)
            length_ = length_.Clamp(0, (unitsList_[0].transform.position - unitSelf_.transform.position).magnitude);


        Vector3 posStart_ = unitSelf_.transform.position;
        Vector3 posEnd_ = posStart_ + (posOnTarget_ - posStart_).normalized * skill_._parameter._targetRange;
        float degree_ = (posOnTarget_ - unitSelf_.transform.position).ToVector2_XZ().ToAngleFrom(Vector2.right);

        Prefabs.imHitAreaLines[0].transform.position = unitSelf_.transform.position;
        Prefabs.rtHitAreaLines[0].sizeDelta = new Vector2(length_, 5.12f);
        Prefabs.imHitAreaLines[0].transform.localRotation = Quaternion.Euler(90, 00, degree_ + skill_._parameter._angle / 2);
    }

    public static void DisplaySkillArea_Fireball(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imHitAreaLines[0].gameObject.SetActive(true);
        //Prefabs.imHitAreaCircle.gameObject.SetActive(true);

        Vector3 posTarget_ = unitSelf_.transform.position + (posOnTarget_ - unitSelf_.transform.position).normalized * skill_._parameter._targetRange;
        List<RaycastHit> hits = Physics.RaycastAll(unitSelf_.transform.position, posOnTarget_ - unitSelf_.transform.position, skill_._parameter._targetRange, /*layer = Unit*/ 1 << 10).ToList();
        hits.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsHitable() == false) continue;

            posTarget_ = hit.point;
            Prefabs.imHitAreaCircle.gameObject.SetActive(true);
            break;
        }

        float length_ = (posTarget_ - unitSelf_.transform.position).magnitude;
        float degree_ = (posTarget_ - unitSelf_.transform.position).ToVector2_XZ().ToAngleFrom(Vector2.right);

        Prefabs.imHitAreaLines[0].transform.position = unitSelf_.transform.position;
        Prefabs.rtHitAreaLines[0].sizeDelta = new Vector2(length_, 5.12f);
        Prefabs.imHitAreaLines[0].transform.localRotation = Quaternion.Euler(90, 00, degree_ + skill_._parameter._angle / 2);
        Prefabs.imHitAreaCircle.transform.position = posTarget_;
        Prefabs.imHitAreaCircle.transform.localScale = new Vector3(skill_._parameter._hitRange * 2, skill_._parameter._hitRange * 2, 1);
    }

    public static void DisplaySkillArea_MoveTargetDirection(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        float dashRange_ = skill_._parameter._moveRange + (skill_._parameter._spRatio * unitSelf_._parameter._sp) / 10f;
        Vector3 posTarget_ = unitSelf_.transform.position + (posOnTarget_ - unitSelf_._modelTransform.position).ClampByLength(dashRange_);

        Prefabs.imTargetAreaCircle.gameObject.SetActive(true);
        Prefabs.imTargetAreaCircle.transform.position = unitSelf_.transform.position;
        Prefabs.imTargetAreaCircle.transform.localScale = new Vector3(dashRange_ * 2, dashRange_ * 2, 1);

        Prefabs.goMoveSuggestion.SetActive(true);
        Prefabs.imColliderArea.transform.localScale = new Vector3(unitSelf_._parameter._colliderRange * 2, unitSelf_._parameter._colliderRange * 2, 1);
        Battle.UpdateMoveSuggestion(unitSelf_, Globals.posOnCursorAtGround, dashRange_, /*isDash_ = */ true);
    }

    public static void DisplaySkillArea_None(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public static void DisplaySkillArea_Charge(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imTargetAreaCircle.gameObject.SetActive(true);
        Prefabs.imHitAreaArc.gameObject.SetActive(true);

        float dashRange_ = skill_._parameter._moveRange;
        Vector3 posTarget_ = unitSelf_.transform.position + (posOnTarget_ - unitSelf_._modelTransform.position).ClampByLength(dashRange_);

        Prefabs.imTargetAreaCircle.transform.position = posTarget_;
        Prefabs.imTargetAreaCircle.transform.localScale = new Vector3(skill_._parameter._targetRange * 2, skill_._parameter._targetRange * 2, 1);

        float degree_ = (posOnTarget_ - unitSelf_.transform.position).ToVector2_XZ().ToAngleFrom(Vector2.right);
        Prefabs.imHitAreaArc.transform.position = posTarget_;
        Prefabs.imHitAreaArc.transform.localScale = new Vector3(skill_._parameter._hitRange * 2, skill_._parameter._hitRange * 2, 1);
        Prefabs.imHitAreaArc.transform.localRotation = Quaternion.Euler(90, 00, degree_);
        Prefabs.imHitAreaArc.gameObject.Find("Arc00").GetComponent<Image>().fillAmount = skill_._parameter._angle.ToRadian().ToDegree() / 2 / 360;
        Prefabs.imHitAreaArc.gameObject.Find("Arc00").transform.localRotation = Quaternion.Euler(00, 00, -skill_._parameter._angle.ToRadian().ToDegree() / 2);
        Prefabs.imHitAreaArc.gameObject.Find("Arc01").GetComponent<Image>().fillAmount = skill_._parameter._angle.ToRadian().ToDegree() / 2 / 360;
        Prefabs.imHitAreaArc.gameObject.Find("Arc01").transform.localRotation = Quaternion.Euler(00, 00, +skill_._parameter._angle.ToRadian().ToDegree() / 2);

        Prefabs.goMoveSuggestion.SetActive(true);
        Prefabs.imColliderArea.transform.localScale = new Vector3(unitSelf_._parameter._colliderRange * 2, unitSelf_._parameter._colliderRange * 2, 1);
        Battle.UpdateMoveSuggestion(unitSelf_, Globals.posOnCursorAtGround, dashRange_, /*isDash_ = */ false);
    }

    public static void DisplaySkillArea_GhostStep(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imTargetAreaCircle.gameObject.SetActive(true);

        float dashRange_ = (skill_._parameter._baseValue + skill_._parameter._spRatio * unitSelf_._parameter._sp) / 10f;
        Vector3 posTarget_ = unitSelf_.transform.position + (posOnTarget_ - unitSelf_._modelTransform.position).ClampByLength(dashRange_);

        Prefabs.imTargetAreaCircle.transform.position = posTarget_;
        Prefabs.imTargetAreaCircle.transform.localScale = new Vector3(skill_._parameter._targetRange * 2, skill_._parameter._targetRange * 2, 1);

        Prefabs.goMoveSuggestion.SetActive(true);
        Prefabs.imColliderArea.transform.localScale = new Vector3(unitSelf_._parameter._colliderRange * 2, unitSelf_._parameter._colliderRange * 2, 1);
        Battle.UpdateMoveSuggestion(unitSelf_, Globals.posOnCursorAtGround, dashRange_, /*isDash_ = */ true);
    }

    public static void DisplaySkillArea_KnockbackShot(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imTargetAreaCircle.gameObject.SetActive(true);
        Prefabs.goMoveSuggestion.SetActive(true);

        //float dashRange_ = (skill_._parameter._baseValue + skill_._parameter._spRatio * unitSelf_._parameter._sp) / 10f;
        //Vector3 posTarget_ = unitSelf_.transform.position + (posOnTarget_ - unitSelf_._modelTransform.position).ClampByLength(dashRange_);

        Prefabs.imTargetAreaCircle.transform.position = unitSelf_.transform.position;
        Prefabs.imTargetAreaCircle.transform.localScale = new Vector3(skill_._parameter._targetRange * 2, skill_._parameter._targetRange * 2, 1);

        Prefabs.imColliderArea.transform.localScale = new Vector3(unitSelf_._parameter._colliderRange * 2, unitSelf_._parameter._colliderRange * 2, 1);
        if (Globals.unitOnMouseover != null)
        {
            Prefabs.goMoveSuggestion.SetActive(true);
            Battle.UpdateKnockbackSuggestion(Globals.unitOnMouseover, (Globals.unitOnMouseover.transform.position - unitSelf_.transform.position).normalized * skill_._parameter._knockbackRange, skill_, true);
        }
        else
        {
            Prefabs.goMoveSuggestion.SetActive(false);
        }
    }

    public static void DisplaySkillArea_IceShard(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }
        Prefabs.imHitAreaLines[0].gameObject.SetActive(true);

        Vector3 posTarget_ = unitSelf_.transform.position + (posOnTarget_ - unitSelf_.transform.position).normalized * skill_._parameter._targetRange;
        bool isShatterd_ = false;
        List<RaycastHit> hits = Physics.RaycastAll(unitSelf_.transform.position, posOnTarget_ - unitSelf_.transform.position, skill_._parameter._targetRange, /*layer = Unit*/ 1 << 10).ToList();
        hits.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            _Unit unit_i_ = hit.transform.parent.parent.GetComponent<_Unit>();

            if (skill_._parameter._unitTypesTargetableList.Contains(unit_i_._parameter._unitType) == false) continue;
            if (unit_i_._IsHitable() == false) continue;

            posTarget_ = hit.point;
            isShatterd_ = true;
            break;
        }

        float length_ = (posTarget_ - unitSelf_.transform.position).magnitude;
        float degree_ = (posTarget_ - unitSelf_.transform.position).ToVector2_XZ().ToAngleFrom(Vector2.right);

        Prefabs.imHitAreaLines[0].transform.position = unitSelf_.transform.position;
        Prefabs.rtHitAreaLines[0].sizeDelta = new Vector2(length_, 5.12f);
        Prefabs.imHitAreaLines[0].transform.localRotation = Quaternion.Euler(90, 00, degree_);

        if (isShatterd_ == true)
        {
            Prefabs.imHitAreaArc.gameObject.SetActive(true);
            Prefabs.imHitAreaArc.transform.position = posTarget_;
            Prefabs.imHitAreaArc.transform.localScale = new Vector3(skill_._parameter._hitRange * 2, skill_._parameter._hitRange * 2, 1);
            Prefabs.imHitAreaArc.fillAmount = skill_._parameter._angle.ToRadian() / (2 * Mathf.PI);
            Prefabs.imHitAreaArc.transform.localRotation = Quaternion.Euler(90, 00, degree_ + skill_._parameter._angle.ToRadian().ToDegree() / 2);
        }
    }

    public static void DisplaySkillArea_Volley(_Unit unitSelf_, Vector3 posOnTarget_, _Skill skill_)
    {
        foreach (Transform child in Prefabs.goSkillSuggestion.transform)
        {
            child.gameObject.SetActive(false);
        }

        Vector3 posStart_ = unitSelf_.transform.position;
        float degree_ = (posOnTarget_ - unitSelf_.transform.position).ToVector2_XZ().ToAngleFrom(Vector2.right);
        float degreeGap_ = skill_._parameter._angle / (skill_._parameter._iCount - 1);
        //float length_ = skill_._parameter._hitRange;

        for (int i = 0; i < skill_._parameter._iCount; i++)
        {
            float degree_i_ = degree_ + degreeGap_ * (-(skill_._parameter._iCount - 1) / 2 + i);
            Vector3 posEnd_i_ = posStart_ + ((posOnTarget_ - posStart_).normalized * skill_._parameter._hitRange).RotateThisByRadian(0, (degree_ - degree_i_).ToRadian(), 0);
            float length_ = skill_._parameter._hitRange;

            List<RaycastHit> hits = Physics.RaycastAll(posStart_, posEnd_i_ - posStart_, skill_._parameter._hitRange, /*layer = Unit*/ 1 << 10).ToList();
            hits.Sort((a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit_j_ in hits)
            {
                _Unit unit_j_ = hit_j_.transform.parent.parent.GetComponent<_Unit>();

                if (unit_j_ == null) continue;
                if (unit_j_._IsHitable() == false) continue;
                if (skill_._parameter._unitTypesTargetableList.Contains(unit_j_._parameter._unitType) == false) continue;

                length_ = length_.Clamp(0, hit_j_.distance);
                break;
            }

            Prefabs.imHitAreaLines[i].gameObject.SetActive(true);
            Prefabs.imHitAreaLines[i].transform.position = unitSelf_.transform.position;
            Prefabs.rtHitAreaLines[i].sizeDelta = new Vector2(length_, 5.12f);
            Prefabs.imHitAreaLines[i].transform.localRotation = Quaternion.Euler(90, 00, degree_i_);
        }
    }

    public static List<_Unit> FindUnits_ClosestExcludeSelf(Vector3 posSelf_, Vector3 posOnCursor_, _Skill  skill_)
    {
        List<_Unit> result = new List<_Unit>(Globals.unitList);

        for (int i = result.Count - 1; i >= 0; i--)
        {
            if (skill_._parameter._unitTypesTargetableList.Contains(result[i]._parameter._unitType) == false)
                result.RemoveAt(i);

            else if (result[i] == Globals.unitOnActive)
                result.RemoveAt(i);

            else if (result[i]._IsAlive() == false)
                result.RemoveAt(i);
        }

        result.Sort((a, b) => (a.transform.position - posSelf_).sqrMagnitude.CompareTo((b.transform.position - posSelf_).sqrMagnitude));
        return result;
    }

    public static List<_Unit> FindUnits_BuffAlly(Vector3 posSelf_, Vector3 posOnCursor_, _Skill skill_)
    {
        List<_Unit> result00 = new List<_Unit>(Globals.unitList);
        List<_Unit> result01 = new List<_Unit>();

        for (int i = result00.Count - 1; i >= 0; i--)
        {
            if (skill_._parameter._unitTypesTargetableList.Contains(result00[i]._parameter._unitType) == false)
                result00.RemoveAt(i);

            else if (result00[i] == Globals.unitOnActive)
                result00.RemoveAt(i);

            else if (result00[i]._parameter._class == Globals.unitOnActive._parameter._class)
            {
                result01.Add(result00[i]);
                result00.RemoveAt(i);
            }

            else if (result00[i]._IsAlive() == false)
                result00.RemoveAt(i);
        }

        result00.Sort((a, b) => (a.transform.position - posSelf_).sqrMagnitude.CompareTo((b.transform.position - posSelf_).sqrMagnitude));
        result01.Sort((a, b) => (a.transform.position - posSelf_).sqrMagnitude.CompareTo((b.transform.position - posSelf_).sqrMagnitude));

        result00.AddRange(result01);
        return result00;
    }

    public static List<_Unit> FindHero_Null(Vector3 posOnUnit_, Vector3 posOnCursor_, _Skill skill_)
    {
        return null;
    }

    public static bool IsCastThisSkill_Photosynthesis(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        if (unitSelf_._parameter._hp < unitSelf_._parameter._hpMax) return true;
        if (unitSelf_._parameter._adBuff < 0) return true;
        if (unitSelf_._parameter._arBuff < 0) return true;
        if (unitSelf_._parameter._mdBuff < 0) return true;
        if (unitSelf_._parameter._mrBuff < 0) return true;
        if (unitSelf_._parameter._spBuff < 0) return true;

        return false;
    }

    public static void SimulateEffect_NormalDamage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        //skill_._ComputeDamage(unitSelf_._parameter._adApplied, unitSelf_._parameter._mdApplied);
        int adDamageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();
        int mdDamageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            unitOnTarget_i_._parameter._hpToLose = 0;
            for (int i = 0; i < skill_._parameter._hitCount; i++)
            {
                int adDamage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, adDamageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
                int mdDamage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, mdDamageBase_, unitOnTarget_i_._parameter._mrApplied, "MD");
                unitOnTarget_i_._parameter._hpToLose += (adDamage_ + mdDamage_).Clamp(0, unitOnTarget_i_._parameter._hpOnView);
            }
            unitOnTarget_i_._canvas.sortingOrder = 1;
        }
        Globals.triggerResetCanvasOrder = true;
    }

    public static void SimulateEffect_NormalHeal(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        //skill_._ComputeDamage(unitSelf_._parameter._ad, unitSelf_._parameter._md);

        foreach (_Unit unit_i_ in unitsOnTargetList_)
        {
            int restoreValue_ = Battle.ComputeAppliedResotoreValue(unit_i_, skill_);
            unit_i_._parameter._hpToRestore = restoreValue_;
            unit_i_._canvas.sortingOrder = 1;
        }
        Globals.triggerResetCanvasOrder = true;
    }

    public static void SimulateEffect_None(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {

    }

    public static void SimulateEffect_ChainLightning(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        int mdDamageBase_ = skill_._parameter._mdDamageBase + (unitSelf_._parameter._mdApplied * skill_._parameter._mdRatio).ToInt();

        for (int i = 0; i < unitsOnTargetList_.Count; i++)
        {
            _Unit unitOnTarget_i_ = unitsOnTargetList_[i];

            foreach (_Unit unit_j_ in Globals.unitList)
            {
                if (unitsOnTargetList_.Contains(unit_j_)) continue;
                if (skill_._parameter._unitTypesTargetableList.Contains(unit_j_._parameter._unitType) == false) continue;
                if ((unit_j_.transform.position - unitOnTarget_i_.transform.position).sqrMagnitude > (skill_._parameter._chainRange + unit_j_._parameter._colliderRange).Square()) continue;

                unitsOnTargetList_.Add(unit_j_);
            }
        }

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            mdDamageBase_ = mdDamageBase_ + ((unitOnTarget_i_._parameter._hpMax - unitOnTarget_i_._parameter._hp) * skill_._parameter._iValue / 100f).ToInt();
            int adDamage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, mdDamageBase_, unitOnTarget_i_._parameter._arApplied, "MD");
            unitOnTarget_i_._parameter._hpToLose = (adDamage_).Clamp(0, unitOnTarget_i_._parameter._hpOnView);
            unitOnTarget_i_._canvas.sortingOrder = 1;
        }
        Globals.triggerResetCanvasOrder = true;
    }

    public static void SimulateEffect_Execution(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        int adDamageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            adDamageBase_ = adDamageBase_ + ((unitOnTarget_i_._parameter._hpMax - unitOnTarget_i_._parameter._hp) * skill_._parameter._iValue / 100f).ToInt();
            int adDamage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, adDamageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitOnTarget_i_._parameter._hpToLose = (adDamage_).Clamp(0, unitOnTarget_i_._parameter._hpOnView);
            unitOnTarget_i_._canvas.sortingOrder = 1;
        }
        Globals.triggerResetCanvasOrder = true;
    }

    public static void SimulateEffect_Rampage(_Unit unitSelf_, List<_Unit> unitsOnTargetList_, _Skill skill_)
    {
        int adDamageBase_ = skill_._parameter._adDamageBase + (unitSelf_._parameter._adApplied * skill_._parameter._adRatio).ToInt();

        foreach (_Unit unitOnTarget_i_ in unitsOnTargetList_)
        {
            adDamageBase_ = (adDamageBase_ * (1 + (float)(unitSelf_._parameter._hpMax - unitSelf_._parameter._hp) / unitSelf_._parameter._hpMax)).ToInt();
            int adDamage_ = Battle.ComputeAppliedDamage(unitSelf_, unitOnTarget_i_, skill_, adDamageBase_, unitOnTarget_i_._parameter._arApplied, "AD");
            unitOnTarget_i_._parameter._hpToLose = (adDamage_).Clamp(0, unitOnTarget_i_._parameter._hpOnView);
            unitOnTarget_i_._canvas.sortingOrder = 1;
        }
        Globals.triggerResetCanvasOrder = true;
    }
}
