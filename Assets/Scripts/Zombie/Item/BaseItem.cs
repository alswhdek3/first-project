using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected ZombieGameItemType type;

    public ZombieGameItemType Type { get { return type; } }

    public abstract void SetItem(ZombieGameItemType _type);

    protected virtual void OnDisable()
    {
        // 오브젝트풀 다시 넣어준다
    }
}
