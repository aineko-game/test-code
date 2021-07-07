using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Library
{
    public static Vector3 BezierLiner(Vector3 posStart, Vector3 posEnd, float p)
    {
        float x = (1 - p) * posStart.x + p * posEnd.x;
        float y = (1 - p) * posStart.y + p * posEnd.y;
        float z = (1 - p) * posStart.z + p * posEnd.z;

        return new Vector3(x, y, z);
    }

    public static Vector3 BezierQuadratic(Vector3 posStart, Vector3 posMiddle, Vector3 posEnd, float p)
    {
        float x = (1 - p) * (1 - p) * posStart.x + 2 * p * (1 - p) * posMiddle.x + p * p * posEnd.x;
        float y = (1 - p) * (1 - p) * posStart.y + 2 * p * (1 - p) * posMiddle.y + p * p * posEnd.y;
        float z = (1 - p) * (1 - p) * posStart.z + 2 * p * (1 - p) * posMiddle.z + p * p * posEnd.z;

        return new Vector3(x, y, z);
    }

    public static Vector3 ComputeClosestPos_LineAndPoint(Vector3 posLineBegin, Vector3 vectorLine, Vector3 posPoint)
    {
        Vector3 vectorLineNormalized = vectorLine.normalized;

        return posLineBegin + vectorLineNormalized * (posPoint - posLineBegin).Dot(vectorLineNormalized);
    }
}
