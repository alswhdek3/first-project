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

            // ������ ��ġ ����
            newitem.transform.position = spawnpoints[i].transform.position;
            isEmptySpawnPoints[i] = false;

            // ������ Ȱ��ȭ
            newitem.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// ����ߴ� ������ ���� ���°� true�� �ʱ�ȭ
    /// </summary>
    public void AllResetSpawnPoint()
    {
        for (int i = 0; i < isEmptySpawnPoints.Length; i++)
            isEmptySpawnPoints[i] = true;
    }
    private void ItemSetting(ZombieGameItem _item,ref ZombieGameItemType _type,int _spawnPointIndex)
    {
        _item.transform.position = spawnpoints[_spawnPointIndex].transform.position; // ������ ��ġ����
        _item.SetItem(this, _type); // ������ ����
        _item.gameObject.SetActive(true); // ������ Ȱ��ȭ
        isEmptySpawnPoints[_spawnPointIndex] = false; // ������ ������ ��ġ ���� false�� ����

        _type = ZombieGameItemType.None; // ������ Ÿ�� �ʱ�ȭ
    }
    #region Events
    private void ItemManagerEnableEvent(object sender, EventArgs e)
    {
        StartCoroutine(nameof(Coroutine_AutoCreateItem));
        IEnumerator Coroutine_AutoCreateItem()
        {
            float time = 0f;
            ZombieGameItemType newItemType = ZombieGameItemType.None;

            while (!gamemanager.IsGameOver) // ���� ���� �Ǳ������� ������ �ð����� ������ �ݺ� ����
            {     
               time = Time.deltaTime;

                // ������ ������ Ÿ�� ����ȭ
                if(PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    // ������ Ÿ�� ����
                    if (time % coinItemCreateTime == 0)
                        newItemType = ZombieGameItemType.Coin;
                    if (time % recoverItemCreateTime == 0)
                        newItemType = ZombieGameItemType.Recovery;

                    pv.RPC(nameof(CreateItemRPC), RpcTarget.All, newItemType); // ������ ����
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

        AllResetSpawnPoint(); // ������ ��������Ʈ ���� �ʱ�ȭ
    }
    #endregion

    #region RPC
    [PunRPC]
    private void CreateItemRPC(ZombieGameItemType type)
    {
        // ������ ����
        switch (type)
        {
            case ZombieGameItemType.Coin: //���� ������ �� �ڸ� ������ ����
                for (int i = 0; i < spawnpoints.Length; i++)
                {
                    if (!isEmptySpawnPoints[i])
                        continue;

                    ZombieGameItem coinitem = GetItem(ref itempool, type.ToString()); // ������ ����
                    ItemSetting(coinitem,ref type,i); // ������ ����
                }
                break;
            case ZombieGameItemType.Recovery: // ȸ�� ������ 1�� ����
                int spawnPointIndex = GetIsEmptySpawnPoint();
                if (spawnPointIndex != -1)
                {
                    ZombieGameItem recoveritem = GetItem(ref itempool, type.ToString()); // ������ ����
                    ItemSetting(recoveritem,ref type,spawnPointIndex); // ������ ����
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

        // ������ ��������Ʈ �ʱ� ����
        isEmptySpawnPoints = new bool[spawnpoints.Length];
        AllResetSpawnPoint();

        // �ʱ� ������ ����
        InitItemCrate();
    }
    protected override void Start()
    {
        // �̺�Ʈ ���
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
