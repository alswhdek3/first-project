using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    public delegate T CreateDel();

    private int count;
    private CreateDel createDel;
    private Queue<T> queueList = new Queue<T>();

    public int QueueCount => queueList.Count;

    public GameObjectPool(int _count , CreateDel _createDel)
    {
        count = _count;
        createDel = _createDel;
        AllSetting();
    }

    public void AllSetting()
    {
        for(int i=0; i<count; i++)
        {
            var obj = createDel();
            queueList.Enqueue(obj);
        }
    }

    public T Get()
    {
        if(queueList.Count > 0)
        {
            return queueList.Dequeue();
        }
        else
        {
            var obj = createDel();
            return obj;
        }
    }

    public void Set(T _obj)
    {
        queueList.Enqueue(_obj);
    }
}
