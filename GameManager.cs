using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : _Singleton<GameManager>
{
    private static EventSystem eventSystem;

    protected override void Awake()
    {
        base.Awake();

        eventSystem = GetComponent<EventSystem>();
        
        Application.targetFrameRate = 60;
        MainMenu.Instance.gameObject.SetActive(true);
    }

    private void Update()
    {
        Globals.Instance.frameCount++;

        Globals.canvasScale = UI.Instance.GetComponent<RectTransform>().localScale.x;
        Globals.timeDeltaFixed = Time.deltaTime * Globals.Instance.gameSpeed;
        Globals.Instance.timeCount += Globals.timeDeltaFixed;
        
        if (Mouse.current.position.ReadValue().IsInThisRect(Vector2.zero, new Vector2(Screen.width, Screen.height)))
        {
            Globals.posOnCursorAtScreen = Mouse.current.position.ReadValue();
            Globals.posOnCursorAtGround = Mouse.current.position.ReadValue().ScreenToGroundPoint();
        }

        //if (Globals.Instance.frameCount == 1) //To delete someday
        //    General.StartAdventure();

        if (Globals.inputStopperCount == 0 && Globals.runningAnimationCount == 0)
        {
            UI.goInputProof.SetActive(false);
            InputModule.imControls.Enable();
        }
        else
        {
            UI.goInputProof.SetActive(true);
            InputModule.imControls.Disable();
            Globals.timeCountToShowTooltip = 0;
        }

        if (Globals.Instance.sceneState == "Battle" && Globals.Instance.turnState == "Hero")
        {
            if(Globals.triggerSaveData && Globals.isAutoSave)
            {
                Globals.triggerSaveData = false;
                //General.SaveDataAll();
            }

            Battle.UpdateOnBattle();
        }
    }

    private void LateUpdate()
    {
        General.LateUpdateCameraPosition();
        Prefabs.goPlane_Y5.SetActive(true);

        if (Globals.unitOnMouseover != null)
        {
            Globals.unitOnMouseover._canvas.sortingOrder = 2;
            Globals.triggerResetCanvasOrder = true;
        }

        foreach (_Unit unit_i_ in Globals.unitList)
        {
            unit_i_._LateUpdate();
        }

        Prefabs.goPlane_Y5.SetActive(false);
    }
}
