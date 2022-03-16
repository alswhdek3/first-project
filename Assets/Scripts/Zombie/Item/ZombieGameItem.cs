using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGameItem : BaseItem
{
    public override void SetItem(ZombieGameItemType _type)
    {
        type = _type;

        // 이벤트 등록

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
