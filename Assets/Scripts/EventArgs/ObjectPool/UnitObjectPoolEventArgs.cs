using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void UnitObjectPoolEventHandler(object _sender, UnitObjectPoolEventArgs e);
public class UnitObjectPoolEventArgs : EventArgs
{
    public UnitObjectPoolEventArgs(string _key , int _poolListCount)
    {
        Key = _key;
        PoolListCount = _poolListCount;
    }

    public string Key
    {
        get;
        private set;
    }
    
    public int PoolListCount
    {
        get;
        private set;
    }
}
