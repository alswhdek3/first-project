using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Realtime;
using Photon.Pun;

using UnityEngine;

public class ItemManager : MonoBehaviourPun
{
    protected PhotonView pv;

    private Dictionary<string,GameObject> itemprefabpool = new Dictionary<string, GameObject>();

    protected event EventHandler EnableEventHandler;
    protected event EventHandler DisableEventHandler;
    protected void InitObjectPoolSetting<T>(ref Dictionary<string,List<T>> _itempool,string _path,int _count) where T : Component
    {
        GameObject[] prefabs = Resources.LoadAll(_path) as GameObject[]; // 프리팹들을 리소스 경로를 통해 가져온다
        for(int i=0; i<prefabs.Length; i++)
        {
            List<T> itemList = new List<T>();
            string key = prefabs[i].transform.name;
            for (int j=0; j<_count; i++)
            {
                var obj = Instantiate(prefabs[i]);
                obj.transform.name = prefabs[i].transform.name;
                SetObjectParentChild(gameObject, obj); // ItemManager 자식으로 프리팹을 넣는다
                T item = obj.GetComponent<T>(); // 프리팹 연결
                item.gameObject.SetActive(false); // 비활성화

                // List Item Add
                if (!itemList.Contains(item))
                    itemList.Add(item);
            }
            // ItemPool Input Item
            if (!_itempool.ContainsKey(key))
                _itempool.Add(key, itemList);

            // ItemPrefabPool Input Item
            if (!itemprefabpool.ContainsKey(key))
                itemprefabpool.Add(key, prefabs[i]);
        }
    }
    public T GetItem<T>(ref Dictionary<string, List<T>> _itempool,string _key) where T : Component
    {
        if(_itempool.ContainsKey(_key))
        {
            List<T> itemlist = _itempool[_key];
            T item = null;
            if(itemlist.Count > 0) // 리스트에 여유분 아이템이 있으면 꺼내서 사용후 삭제
            {
                item = itemlist[0];
                itemlist.Remove(item);
                return item;
            }
            else // 리스트에 여유분 아이템이 없으면 새로 생성
            {
                if (!itemprefabpool.ContainsKey(_key)) // Key 검사
                    return null;

                item = GetCreateNewGameObject(_key).GetComponent<T>();
                item.gameObject.SetActive(false);
                return item;
            }
        }
        return null;
    }
    private GameObject GetCreateNewGameObject(string _key)
    {
        var prefab = Instantiate(itemprefabpool[_key]);
        SetObjectParentChild(gameObject, prefab);
        return prefab.gameObject;
    }
    public void RemoveObject<T>(ref Dictionary<string, List<T>> _itempool, T _item) where T : Component
    {
        _item.gameObject.SetActive(false); // 아이템 비활성화

        if (_itempool.ContainsKey(_item.transform.name))
            _itempool[_item.transform.name].Add(_item); // 재활용
    }

    public void SetObjectParentChild(GameObject _parent, GameObject _obj)
    {
        _obj.transform.SetParent(_parent.transform);
        _obj.transform.localScale = Vector3.one;
        _obj.transform.localPosition = Vector3.zero;
    }

    protected virtual void Awake()
    {
        pv = photonView;
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
}
