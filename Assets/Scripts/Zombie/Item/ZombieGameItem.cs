using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGameItem : BaseItem
{
    public override void SetItem(ZombieGameItemType _type)
    {
        type = _type;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
