using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected ZombieGameItemType type;

    // 이벤트
    public event EventHandler<Unit> ItemBuffEventHandler; // 버프 이벤트
    public event EventHandler EnableEventHandler; //활성화 이벤트    
    public event EventHandler DisableEventHandler;// 비활성화 이벤트

    public ZombieGameItemType Type { get { return type; } }

    public abstract void SetItem(ZombieGameItemType _type);

    protected virtual void OnEnable()
    {
        EnableEventHandler?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void OnDisable()
    {
        DisableEventHandler?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            ItemBuffEventHandler?.Invoke(this,other.GetComponent<Unit>());
    }
}
