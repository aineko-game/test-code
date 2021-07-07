//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Field : MonoBehaviour
//{
//    private static LineRenderer _lrFieldBoarder;

//    private void Awake()
//    {
//        _lrFieldBoarder = GetComponent<LineRenderer>();
//    }

//    public static void ConstructFiaeld(Vector3 posCenter)
//    {
//        Globals.fieldPosCenter = posCenter;
//        Globals.fieldPosMin = posCenter + new Vector3(-Globals.FIELD_WIDTH / 2, -Globals.FIELD_HEIGHT / 2, -Globals.FIELD_LENGTH);
//        Globals.fieldPosMax = posCenter + new Vector3(+Globals.FIELD_WIDTH / 2, +Globals.FIELD_HEIGHT / 2, +Globals.FIELD_LENGTH);
//        Globals.fieldCubicPointsArray = Globals.fieldPosCenter.ToCubicPoints(Globals.FIELD_WIDTH, Globals.FIELD_HEIGHT, Globals.FIELD_LENGTH);

//        LineFieldArea();
//    }

//    public static void LineFieldArea()
//    {
//        Vector3[] positions_ = Globals.fieldCubicPointsArray.CubicPointsToUpperXZ();
        
//        _lrFieldBoarder.SetPositions(positions_);
//    }
//}
