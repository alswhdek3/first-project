using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager> , IUnitObjectPool
{
    [SerializeField]
    private CharacterCreateEffect characterCreateEffect;

    private GameObject unitPrefab;
 
    private Dictionary<string, List<GameObject>> objectPoolTable = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, GameObject> objectPoolGameObjectTable = new Dictionary<string, GameObject>();

    // EventHandlers
    public event UnitObjectPoolEventHandler UnitObjectPoolEventHandler;
    public void SetResetPosition(GameObject _parent , GameObject _object)
    {
        _object.transform.SetParent(_parent.transform);
        _object.transform.localScale = Vector3.one;
        _object.transform.localPosition = Vector3.zero;
    }

    #region 프로퍼티
    public Dictionary<string, List<GameObject>> ObjectPoolTable => objectPoolTable;
    public int ObjectPoolCount => objectPoolTable.Count;
    #endregion

    #region EventHandlers
    // EventHandler Methods
    public void UnitObjectPoolLogEvent(object _sender, UnitObjectPoolEventArgs e)
    {
        Debug.Log($"ObjectPool Key : {e.Key} / PoolListCount : {e.PoolListCount}");
    }
    #endregion

    private void InitObjectSetting()
    {
        // 몬스터 종류만큼 오브젝트풀 세팅(추후 일반화 형식 형태로 수정)
        GameObject[] objects = Resources.LoadAll<GameObject>("Prefabs/Unit/Monsters");
        for(int i=0; i< objects.Length; i++)
        {
            unitPrefab = Resources.Load($"Prefabs/Unit/Monsters/Monster_{i}") as GameObject;
            string key = unitPrefab.transform.name;
            // 각 종류 오브젝트당 4개씩 오브젝트 리스트에 추가
            List<GameObject> objectList = new List<GameObject>();
            objectList.Clear();
            for(int j=0; j<4; j++)
            {
                var obj = Instantiate(unitPrefab);
                SetResetPosition(gameObject, obj.gameObject);
                obj.gameObject.SetActive(false);
                if(!objectList.Contains(obj))
                {
                    objectList.Add(obj);
                }
            }
            if(!objectPoolTable.ContainsKey(key))
            {
                objectPoolTable.Add(key, objectList);
            }

            // objectPoolGameObject Add
            if (!objectPoolGameObjectTable.ContainsKey(key))
            {
                var prefab = Instantiate(unitPrefab);
                SetResetPosition(gameObject, prefab.gameObject);
                prefab.gameObject.SetActive(false);
                objectPoolGameObjectTable.Add(key, prefab);
            }
        }
    }

    public GameObject GetCreateGameObject(string _key)
    {
        if(!objectPoolTable.ContainsKey(_key))
        {
            Debug.LogError($"{_key}의 ObjectPoolTable이 존재하지않습니다");
            return null;
        }

        string[] keysplit = _key.Split('_'); // ex) Monster_2

        List<GameObject> objectList = objectPoolTable[_key];
        GameObject obj = null;

        if(objectList.Count > 0)
        {
            obj = objectList[0];
            objectList.Remove(obj);

            // 캐릭터 생성 애니메이션 재생 완료후 캐릭터 활성화를 위한 미리 프리팹 비활성화
            obj.gameObject.SetActive(keysplit[0].Equals("Monster") ? true : false);
            if(keysplit[0] == "Character")
            {
                characterCreateEffect.CreateObject(obj.gameObject, (length) =>
                {
                    StartCoroutine(Coroutine_CreateCharacter(length));

                    IEnumerator Coroutine_CreateCharacter(float _length)
                    {
                        yield return new WaitForSeconds(length);
                        obj.gameObject.SetActive(true);
                        //Debug.Log($"CreateCharacter Level : {} Name : {} / offense power : {}");
                    }
                });
            }

            UnitObjectPoolEventHandler(this, new UnitObjectPoolEventArgs(_key, objectList.Count));
            return obj;
        }

        else
        {
            if(!objectPoolGameObjectTable.ContainsKey(_key))
            {
                Debug.LogError($"{_key}의 ObjectPoolGameObjectTable이 존재하지않습니다");
                return null;
            }

            obj = GetCreateGameObjectUnit(_key);

            // 캐릭터 생성 애니메이션 재생 완료후 캐릭터 활성화를 위한 미리 프리팹 비활성화
            obj.gameObject.SetActive(keysplit[0].Equals("Monster") ? true : false);
            if (keysplit[0] == "Character")
            {
                characterCreateEffect.CreateObject(obj.gameObject, (length) =>
                {
                    StartCoroutine(Coroutine_CreateCharacter(length));

                    IEnumerator Coroutine_CreateCharacter(float _length)
                    {
                        yield return new WaitForSeconds(length);
                        obj.gameObject.SetActive(true);
                        //Debug.Log($"CreateCharacter Level : {} Name : {} / offense power : {}");
                    }
                });
            }

            UnitObjectPoolEventHandler(this, new UnitObjectPoolEventArgs(_key, objectList.Count));
            return obj;
        }
    }
    private GameObject GetCreateGameObjectUnit(string _key)
    {
        var prefab = Instantiate(objectPoolGameObjectTable[_key]);
        SetObjectParentIn(prefab.gameObject);
        return prefab;
    }

    public void RemoveGameObject(string _key , GameObject _object)
    {
        if (!objectPoolTable.ContainsKey(_key))
        {
            Debug.LogError($"{_key}의 ObjectPoolTable이 존재하지않습니다");
            return;
        }

        _object.gameObject.SetActive(false);
        List<GameObject> objectList = objectPoolTable[_key];
        objectList.Add(_object);
    }

    public void SetObjectParentIn(GameObject _object)
    {
        _object.transform.SetParent(transform);
        _object.transform.localScale = Vector3.one;
        _object.transform.localPosition = Vector3.zero;
    }

    protected override void OnStart()
    {
        // ComPonet Connect
        characterCreateEffect = GameObject.FindGameObjectWithTag("CreateEffect").GetComponent<CharacterCreateEffect>();

        // EventHandlers Add
        UnitObjectPoolEventHandler += UnitObjectPoolLogEvent;

        InitObjectSetting();
    }
}
