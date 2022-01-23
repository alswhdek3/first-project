using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectPool<T> : MonoBehaviour where T : Component
{
    //public delegate T CreateObjectDel(string _key, GameObject _obj);

    private static Dictionary<string, List<T>> gameObjectPool;

    private static Dictionary<string, GameObject> prefabPool;

    public static event ObjectPoolEventArgs<T>.ObjectPoolEventHandler ObjectPoolEventHandler;

    public static Dictionary<string, List<T>> GameObjectPool { get { return gameObjectPool; } }

    public static GameObject[] GetAllObjects(string _path)
    {
        return Resources.LoadAll<GameObject>(_path);
    }

    public static Dictionary<string, List<T>> GetGameObjectPool(string[] _keys , GameObject[] _obj , GameObject _parent)
    {
        gameObjectPool = new Dictionary<string, List<T>>();
        prefabPool = new Dictionary<string, GameObject>();

        ObjectPoolEventHandler += (e) =>
        {
            Debug.Log($"ObjectPool({e.GetType()}) Count : {e.Pool.Count}");
            string message = string.Empty;
            foreach(KeyValuePair<string, List<T>> objpool in e.Pool)
            {
                message += $"Key : {objpool.Key} , ValueListCount : {objpool.Value.Count}\n";
            }
            Debug.Log(message);
        };

        try
        {
            int index = 0;
            foreach (string key in _keys)
            {
                List<T> objList = new List<T>();
                if (!gameObjectPool.ContainsKey(key))
                {
                    // 5개씩 미리 생성
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject obj = Instantiate(_obj[index]);
                        SetObjectParentInChildren(obj, _parent);

                        // T Componet Connect
                        T target = obj.GetComponent<T>();
                        obj.gameObject.SetActive(false);

                        // T Object List Add
                        if (!objList.Contains(target))
                        {
                            objList.Add(target);
                        }
                    }
                    gameObjectPool.Add(key, objList);
                }

                GameObject prefab = Instantiate(_obj[index]);
                SetObjectParentInChildren(prefab, _parent);
                prefab.gameObject.SetActive(false);

                if (!prefabPool.ContainsKey(key))
                    prefabPool.Add(key, prefab);

                index += 1;
            }
            if(ObjectPoolEventHandler != null)
            {
                ObjectPoolEventHandler?.Invoke(new ObjectPoolEventArgs<T>(gameObjectPool));
            }
            return gameObjectPool;
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    public static T GetObject(string _key , GameObject _parent)
    {
        try
        {
            List<T> pool = gameObjectPool[_key];
            T obj;
            // 리스트안에 여유분의 오브젝트가 있으면 리스트안에서 꺼내서 사용
            if(pool.Count > 0)
            {
                obj = pool[0];
                // 꺼낸 오브젝트는 리스트에서 삭제
                pool.Remove(obj);

                return obj;
            }
            // 리스트안에 여유분의 오브젝트가 없으면 생성
            else
            {
                obj = GetCreateObjectPoolUnit(_key , _parent);
                return obj;
            }
        }
        catch(KeyNotFoundException e)
        {
            Debug.LogError($"{_key} Null !!\n" +
                $"{e.Message}");

            return null;
        }
    }

    public static void RemoveObject(string _key , GameObject _obj)
    {
        if(!gameObjectPool.ContainsKey(_key))
        {
            Debug.LogError($"GameObjectPool Key({_key} Null !!)");
            return;
        }

        gameObjectPool[_key].Add(_obj.GetComponent<T>());
        _obj.gameObject.SetActive(false);
    }

    private static T GetCreateObjectPoolUnit(string _key , GameObject _parent)
    {
        try
        {
            GameObject obj = Instantiate(prefabPool[_key]);
            SetObjectParentInChildren(obj, _parent);
            obj.gameObject.SetActive(false);

            T target = obj.GetComponent<T>();
            return target;
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError($"{_key} Null !!\n" +
                $"{e.Message}");

            return null;
        }
    }

    public static void SetObjectParentInChildren(GameObject _obj , GameObject _parent)
    {
        _obj.transform.SetParent(_parent.transform);
        _obj.transform.localScale = Vector3.one;
        _obj.transform.localPosition = Vector3.zero;
    }
}
