using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitObjectPool
{
    void RemoveGameObject(string _key, GameObject _object);
}
