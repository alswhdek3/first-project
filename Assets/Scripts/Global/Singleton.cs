using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; } 
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
        OnAwake();
    }

    private void Start()
    {
        if(Instance != null)
        {
            OnStart();
        }
    }

    protected virtual void OnAwake()
    {
        
    }
    protected virtual void OnStart()
    {

    }
}
