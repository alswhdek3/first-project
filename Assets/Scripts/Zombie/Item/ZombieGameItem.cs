using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieGameItemType
{
    None = -1,
    Coin,
    Recovery,
    Max
}
public class ZombieGameItem : BaseItem
{
    #region Event
    public override void ItemBuffEvent(object _sender, Unit unit)
    {
        if (type == ZombieGameItemType.Coin)
            unit.TargetItemBuffPlay(unit.GetComponent<ZombiePlayer>(),type);
        else
            unit.TargetItemBuffPlay(unit.GetComponent<Zombie>(), type);

        gameObject.SetActive(false);
    }    
    protected override void EnableEvent(object _sender, EventArgs _e)
    {
        base.EnableEvent(_sender, _e);
    }
    protected override void DisableEvent(object _sender, EventArgs _e)
    {
        base.DisableEvent(_sender, _e);
        itemmanager.RemoveObject(ref itemmanager.itempool, this);
    }
    #endregion

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // 아이템 버프 이벤트 등록
        base.OnTriggerEnter(other);
    }
}
