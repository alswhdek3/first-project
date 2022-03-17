using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Realtime;
using Photon.Pun;

using UnityEngine;

public class ZombieGameItemManager : ItemManager
{
    [Header("Manager")]
    [SerializeField]
    private ZombieManager gamemanager;

    [Header("SpawnPoints")]
    [SerializeField]
    private Transform[] spawnpoints;
    [SerializeField]
    private bool[] isEmptySpawnPoints;

    [Header("ItemCreateTime")]
    public float coinItemCreateTime = 10f, recoverItemCreateTime = 15f;

    // ItemPool
    public Dictionary<string,List<ZombieGameItem>> itempool;

    public int GetIsEmptySpawnPoint()
    {
        for(int i=0; i< isEmptySpawnPoints.Length; i++)
        {
            if (isEmptySpawnPoints[i])
                return i;
        }
        return -1;
    }

    private void InitItemCrate()
    {
        isEmptySpawnPoints = new bool[spawnpoints.Length];

        for (int i=0; i< spawnpoints.Length; i++)
        {
            ZombieGameItem newitem = GetItem(ref itempool, ZombieGameItemType.Coin.ToString());
            newitem.SetItem(this, ZombieGameItemType.Coin);

            // 아이템 위치 지정
            newitem.transform.position = spawnpoints[i].transform.position;
            isEmptySpawnPoints[i] = false;

            // 아이템 활성화
            newitem.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 사용했던 아이템 스폰 상태값 true로 초기화
    /// </summary>
    public void AllResetSpawnPoint()
    {
        for (int i = 0; i < isEmptySpawnPoints.Length; i++)
            isEmptySpawnPoints[i] = true;
    }
    private void ItemSetting(ZombieGameItem _item,ref ZombieGameItemType _type,int _spawnPointIndex)
    {
        _item.transform.position = spawnpoints[_spawnPointIndex].transform.position; // 아이템 위치지정
        _item.SetItem(this, _type); // 아이템 세팅
        _item.gameObject.SetActive(true); // 아이템 활성화
        isEmptySpawnPoints[_spawnPointIndex] = false; // 아이템 생성된 위치 상태 false로 변경

        _type = ZombieGameItemType.None; // 아이템 타입 초기화
    }
    #region Events
    private void ItemManagerEnableEvent(object sender, EventArgs e)
    {
        StartCoroutine(nameof(Coroutine_AutoCreateItem));
        IEnumerator Coroutine_AutoCreateItem()
        {
            float time = 0f;
            ZombieGameItemType newItemType = ZombieGameItemType.None;

            while (!gamemanager.IsGameOver) // 게임 종료 되기전까지 지정한 시간마다 아이템 반복 생성
            {     
               time = Time.deltaTime;

                // 방장이 아이템 타입 동기화
                if(PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    // 아이템 타입 지정
                    if (time % coinItemCreateTime == 0)
                        newItemType = ZombieGameItemType.Coin;
                    if (time % recoverItemCreateTime == 0)
                        newItemType = ZombieGameItemType.Recovery;

                    pv.RPC(nameof(CreateItemRPC), RpcTarget.All, newItemType); // 아이템 생성
                }
                yield return null;
            }
            gameObject.SetActive(false); // ItemManager GameObject Disable
        }
    }
    private void ItemManagerDisableEvent(object sender, EventArgs e)
    {
        ZombieGameItem[] items = gamemanager.GetComponentsInChildren<ZombieGameItem>();
        foreach (ZombieGameItem item in items)
            RemoveObject(ref itempool, item);

        AllResetSpawnPoint(); // 아이템 스폰포인트 상태 초기화
    }
    #endregion

    #region RPC
    [PunRPC]
    private void CreateItemRPC(ZombieGameItemType type)
    {
        // 아이템 생성
        switch (type)
        {
            case ZombieGameItemType.Coin: //코인 아이템 빈 자리 모든곳에 생성
                for (int i = 0; i < spawnpoints.Length; i++)
                {
                    if (!isEmptySpawnPoints[i])
                        continue;

                    ZombieGameItem coinitem = GetItem(ref itempool, type.ToString()); // 아이템 생성
                    ItemSetting(coinitem,ref type,i); // 아이템 세팅
                }
                break;
            case ZombieGameItemType.Recovery: // 회복 아이템 1개 생성
                int spawnPointIndex = GetIsEmptySpawnPoint();
                if (spawnPointIndex != -1)
                {
                    ZombieGameItem recoveritem = GetItem(ref itempool, type.ToString()); // 아이템 생성
                    ItemSetting(recoveritem,ref type,spawnPointIndex); // 아이템 세팅
                }
                break;
            default:
                break;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        string path = "Prefabs/ZombieGame/Item";
        InitObjectPoolSetting(ref itempool, path, 5);

        // 아이템 스폰포인트 초기 세팅
        isEmptySpawnPoints = new bool[spawnpoints.Length];
        AllResetSpawnPoint();

        // 초기 아이템 생성
        InitItemCrate();
    }
    protected override void Start()
    {
        // 이벤트 등록
        EnableEventHandler += ItemManagerEnableEvent;
        DisableEventHandler += ItemManagerDisableEvent;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();       
    }
}
