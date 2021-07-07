using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class _Hero : _Unit
{ 
    protected override void Awake()
    {
        base.Awake();
        
        Globals.heroList.Add(this);
        tag = "Unit";
        _parameter._unitType = "Hero";
        _parameter._unitTypesThroughable = new List<string> { "Hero" };

        _imHpSliderMain.color = new Color32(000, 255, 000, 255);
        _imHpSliderToLose.color = new Color32(255, 180, 000, 255);
        _imHpSliderToRestore.color = new Color32(000, 255, 255, 255);
        _outline.OutlineColor = new Color32(000, 255, 000, 255);

        _SetClassParameter();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        Globals.heroList.Remove(this);
    }

}
