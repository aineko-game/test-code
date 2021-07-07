using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class _Random
{
    public uint x, y, z, w;

    public _Random() : this((uint)DateTime.Now.Ticks) { }

    public _Random(uint seed)
    {
        SetSeed(seed);
    }

    public void SetSeed(uint seed)
    {
        x = seed; y = x * 3266489917U + 1; z = y * 3266489917U + 1; w = z * 3266489917U + 1;
    }

    public uint GetNext()
    {
        uint t = x ^ (x << 11);
        x = y;
        y = z;
        z = w;
        w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
        return w;
    }

    public int Range(int min, int max)
    {
        UnityEngine.Random.InitState((int)GetNext());
        return UnityEngine.Random.Range(min, max);
    }

    public float Range(float min, float max)
    {
        UnityEngine.Random.InitState((int)GetNext());
        return UnityEngine.Random.Range(min, max);
    }

    public float Gaussian(float var = 1)
    {
        float n0 = UnityEngine.Random.Range(0f, 1f);
        float n1 = UnityEngine.Random.Range(0f, 1f);

        float r = Mathf.Sqrt(-2 * Mathf.Log(n0)) * Mathf.Cos(2 * Mathf.PI * n1);
        return Mathf.Sqrt(var) * r;
    }
}
