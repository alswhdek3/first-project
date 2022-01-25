using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField]
    private GameObject[] objects;

    public Dictionary<string, List<Monster>> MonsterTable
    {
        get;
        private set;
    }

    public void InitSetting()
    {
        // Prefab 적용
        objects = Resources.LoadAll("Prefabs/Units/Monsters") as GameObject[];

        // Keys 값들을 가져온다
        string[] keys = new string[objects.Length];
        for(int i=0; i<keys.Length; i++)
            keys[i] = objects[i].transform.name; // Monster_0(Clone)형태로 이름이 설정되기때문에 코드 수정 예정

        // DictionaryObjectPool 적용
        //MonsterTable = ObjectPool<Monster>.GetGameObjectPool(keys, objects, gameObject);
        // ObjectPoolEventHandler Call
    }

    public Monster GetMonster(string key)
    {
        return ObjectPool<Monster>.GetObject(key, gameObject);
    }

    #region 

    //private void MonsterManager_ObjectPoolEventHandler(object _sender, ObjectPoolEventArgs<Monster> _e)
    //{
    //    Dictionary<string, List<Monster>> objectpool = _e.ObjectPool;
    //    string message = string.Empty;
    //    Debug.Log("====== Show MonsterObjectPool ======");
    //    foreach(KeyValuePair<string,List<Monster>> pool in objectpool)
    //    {
    //        message += $"Key : {pool.Key} / MonsterListCount : {pool.Value.Count}\n";
    //    }
    //    Debug.Log($"{message}\n" +
    //        $"MonsterObjectPool Count : {objectpool.Count}");
    //}

    protected override void OnAwake()
    {
        InitSetting();
    }

    #endregion
}
