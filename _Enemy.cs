using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Enemy : _Unit
{
    protected override void Awake()
    {
        base.Awake();

        Globals.enemyList.Add(this);
        if (_parameter.isBoss)
            Globals.bossList.Add(this);

        _parameter._unitType = "Enemy";
        _parameter._unitTypesThroughable = new List<string> { "Enemy" };

        _imHpSliderMain.color = new Color32(210, 000, 000, 255);
        _imHpSliderToLose.color = new Color32(255, 180, 000, 255);
        _imHpSliderToRestore.color = new Color32(000, 255, 255, 255);
        _outline.OutlineColor = new Color32(255, 000, 000, 255);

        _SetClassParameter();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        Globals.enemyList.Remove(this);
        Globals.bossList.Remove(this);
    }

    public static IEnumerator AnimateEneiesAction()
    {
        Globals.inputStopperCount++;
        Globals.InputState = "EnemyMain";
        List<_Enemy> enemyActList_ = new List<_Enemy>(Globals.enemyList);

        foreach (_Enemy enemy_i_ in enemyActList_)
        {
            while (enemy_i_._parameter._actableCount > 0)
            {
                if (enemy_i_._IsAlive() == false) break;
                if (enemy_i_._parameter._statusConditions.Find(m => m._name == "Stun")._count > 0) break;

                Globals.unitOnActive = enemy_i_;
                _Skill skill_ = enemy_i_.SelectSkill(); ;
                enemy_i_._skillOnActive = skill_;

                enemy_i_._outline.enabled = true;
                yield return new WaitForSeconds(0.4f / Globals.Instance.gameSpeed);

                List<_Unit> unitsOnTargetList_ = skill_._functionFindUnits(enemy_i_.transform.position, Vector3.zero, skill_).ExcludeStealth("Enemy");
                Vector3 posToMove_ = skill_._functionComputeBestPos(enemy_i_, unitsOnTargetList_, skill_);

                if (enemy_i_._IsMovable())
                    yield return General.Instance.StartCoroutine(enemy_i_.MoveEnemyTo(posToMove_, Globals.UNIT_MOVE_PER_SEC * enemy_i_._moveSpeed));

                List<_Unit> unitsToAttackList_ = skill_._functionDetectUnits(enemy_i_.transform.position, Vector3.zero, skill_).ExcludeStealth("Enemy");
                yield return General.Instance.StartCoroutine(skill_._functionCastSkill(enemy_i_, unitsToAttackList_, enemy_i_._skillOnActive));

                if (skill_._parameter._isQuickCast == false)
                {
                    enemy_i_._parameter._movableCount = 0;
                    enemy_i_._parameter._actableCount -= 1;
                    enemy_i_._parameter._statusConditions.Find(m => m._name == "Stealth")._count = 0;
                    //enemy_i_._parameter._stealthCount = 0;
                    enemy_i_._SetAnimatorCondition();
                    enemy_i_._DisplayBuffAndStatusIcon();
                }

                Globals.unitOnActive = null;
                enemy_i_._outline.enabled = false;

                while (Globals.triggerStateBasedAction == true)
                {
                    while (Globals.runningAnimationCount != 0)
                        yield return null;

                    Globals.triggerStateBasedAction = false;
                    yield return Battle.ExecuteStateBaseAction();
                }
                if (Battle.IsGameOver())
                {
                    Globals.inputStopperCount--;
                    yield break;
                }
            }

            while (Globals.triggerStateBasedAction == true)
            {
                while (Globals.runningAnimationCount != 0)
                    yield return null;

                Globals.triggerStateBasedAction = false;
                yield return Battle.ExecuteStateBaseAction();
            }
            if (Battle.IsGameOver())
            {
                Globals.inputStopperCount--;
                yield break;
            }
        }

        yield return new WaitForSeconds(0.3f / Globals.Instance.gameSpeed);
        yield return Battle.EndEnemyTurn();
        Globals.inputStopperCount--;
    }

    public _Skill SelectSkill()
    {
        for (int i = 1; i < _parameter._skills.Length; i++)
        {
            if (_parameter._skills[i]._parameter._name.IsNullOrEmpty() == false && _parameter._skills[i]._IsCastable())
            {
                _Skill skill_i_ = _parameter._skills[i];

                if (skill_i_._functionIsCastThisSkill?.Invoke(this, null, skill_i_) == false) continue;
                if (skill_i_._functionFindUnits?.Invoke(transform.position, Vector3.zero, skill_i_)?.ExcludeStealth("Enemy").Count == 0) continue;
                return _parameter._skills[i];
            }
        }

        //if (_parameter._skills[1]._IsCastable() == true)
        //{
        //    Debug.Log(1);
        //    return _parameter._skills[1];
        //}
        //else if (_parameter._skills[2]._IsCastable() == true)
        //{
        //    Debug.Log(2);
        //    return _parameter._skills[2];
        //}
        //else if (_parameter._skills[3]?._IsCastable() == true)
        //{
        //    Debug.Log(3);
        //    return _parameter._skills[3];
        //}

        //Debug.Log(0);
        return _parameter._skills[0];
    }
}
