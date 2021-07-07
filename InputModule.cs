using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputModule : _Singleton<InputModule>
{
    public static InputMaster imControls;

    protected override void Awake()
    {
        base.Awake();

        imControls = new InputMaster();
        
        imControls.Battle.SetActiveMove.performed += ctx => Battle.SetActiveMoveSuggestion(Globals.unitOnActive);
        imControls.Battle.SetActiveSkill00.performed += ctx => Battle.SetActiveSkillSuggestion(Globals.unitOnActive, 0);
        imControls.Battle.SetActiveSkill01.performed += ctx => Battle.SetActiveSkillSuggestion(Globals.unitOnActive, 1);
        imControls.Battle.SetActiveSkill02.performed += ctx => Battle.SetActiveSkillSuggestion(Globals.unitOnActive, 2);
        imControls.Battle.SetActiveSkill03.performed += ctx => Battle.SetActiveSkillSuggestion(Globals.unitOnActive, 3);

        imControls.Battle.SetActiveItem01.performed += ctx => Battle.SetActiveItemSuggestion(0);
        imControls.Battle.SetActiveItem02.performed += ctx => Battle.SetActiveItemSuggestion(1);
        imControls.Battle.SetActiveItem03.performed += ctx => Battle.SetActiveItemSuggestion(2);
        imControls.Battle.SetActiveItem04.performed += ctx => Battle.SetActiveItemSuggestion(3);
        imControls.Battle.SetActiveItem05.performed += ctx => Battle.SetActiveItemSuggestion(4);
        imControls.Battle.SetActiveItem06.performed += ctx => Battle.SetActiveItemSuggestion(5);

        imControls.Battle.TryUndoUnitMove.performed += ctx => Battle.TryUndoUnitMove();
        imControls.Battle.TryResetTurn.performed += ctx => Battle.TryResetTurn();
    }

    public void OnEnable()
    {
        imControls.Enable();
    }

    public void OnDisable()
    {
        imControls.Disable();
    }
}
