using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IbaseObjectPool<T>
{
    public Dictionary<string, List<T>> GetGameObjectPool(string[] _keys, GameObject[] _obj, GameObject _parent);
}
