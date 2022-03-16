using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieGameItemType
{
    None = -1,
    Coin,
    Slow,
    Recovery,
    Max
}
public class ZombieGameItemData
{
    public ZombieGameItemData(ZombieGameItemType _type, int _amount, float _duration = -1)
    {
        type = _type;
        amount = _amount;
        duration = _duration;
    }

    private ZombieGameItemType type; // 아이템 타입
    private int amount; // 증가값
    private float duration; // 지속시간
}

public class ZombieGameItemManager : SingtonMonoBehaviour<ZombieGameItemManager> , IData<ZombieGameItemData>
{
    public Dictionary<string, ZombieGameItemData> ItemTable { get; set; }

    private void InitItemData()
    {
        for(int i=0; i<(int)ZombieGameItemType.Max; i++)
        {
            ZombieGameItemData newItemData = null;
            switch((ZombieGameItemType)i)
            {

            }
        }
        //ItemTable = CommonData<ZombieGameItemData>.GetDataTable()
    }

    protected override void OnAwake()
    {
        
    }
}
