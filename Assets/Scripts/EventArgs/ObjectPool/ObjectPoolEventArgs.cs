using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolEventArgs<T> : EventArgs where T : class
{
    public delegate void ObjectPoolEventHandler(ObjectPoolEventArgs<T> _e);

    public ObjectPoolEventArgs(Dictionary<string, List<T>> _pool)
    {
        Pool = _pool;
    }

    public Dictionary<string,List<T>> Pool
    {
        get;
        private set;
    }

    public string Key
    {
        get;
        private set;
    }

    public List<T> ObjectList
    {
        get
        {
            return Pool[Key];
        }
    }
}
