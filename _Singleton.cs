using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class _Singleton<T> : MonoBehaviour where T : _Singleton<T>
{
    public static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);
                instance = (T)Resources.FindObjectsOfTypeAll<T>()[0];
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        KeepSingleton();
    }

    protected void KeepSingleton()
    {
        if (instance == null)
        {
            instance = this as T;
            return;
        }
        else if (instance == this)
        {
            return;
        }

        Debug.LogError("_Singleton <" + typeof(T) + "> was instantiated twice. >" + name);
        Destroy(this);
    }
}