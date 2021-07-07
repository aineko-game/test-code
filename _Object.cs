using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Object : _Unit
{
    protected override void Awake()
    {
        base.Awake();

        Globals.objectList.Add(this);

        _parameter._unitType = "Object";

        _imHpSliderMain.color = new Color32(000, 255, 255, 255);
        _imHpSliderToLose.color = new Color32(255, 180, 000, 255);
        _imHpSliderToRestore.color = new Color32(000, 255, 255, 255);
        _outline.OutlineColor = new Color32(000, 255, 255, 255);

        _SetClassParameter();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        Globals.objectList.Remove(this);
    }
}
