using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected ZombieGameItemManager itemmanager;
    protected ZombieGameItemType type;

    [SerializeField]
    protected float rotatespeed = 200f;

    // 이벤트
    public event EventHandler<Unit> ItemBuffEventHandler; // 버프 이벤트
    public event EventHandler EnableEventHandler; //활성화 이벤트    
    public event EventHandler DisableEventHandler;// 비활성화 이벤트

    public ZombieGameItemType Type { get { return type; } }

    public void SetItem(ZombieGameItemManager _itemmanager,ZombieGameItemType _type)
    {
        itemmanager = _itemmanager;
        type = _type;

        // 이벤트 등록
        if(ItemBuffEventHandler == null)
            ItemBuffEventHandler += ItemBuffEvent;
        if (EnableEventHandler == null)
            EnableEventHandler += EnableEvent;
        if (DisableEventHandler == null)
            DisableEventHandler += DisableEvent;
    }
    public abstract void ItemBuffEvent(object _sender, Unit _unit);

    /// <summary>
    /// 아이템 오브젝트풀 재활용
    /// </summary>
    /// <param name="_sender"></param>
    /// <param name="_e"></param>
    protected virtual void DisableEvent(object _sender, EventArgs _e)
    {
        itemmanager.AllResetSpawnPoint();
    }

    protected virtual void EnableEvent(object _sender, EventArgs _e)
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotatespeed, 0f), Space.World);
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnEnable()
    {
        EnableEventHandler?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void OnDisable()
    {
        DisableEventHandler?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void Update()
    {
        EnableEventHandler?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            ItemBuffEventHandler?.Invoke(this,other.GetComponent<Unit>());
    }
}
