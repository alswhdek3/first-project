using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    public delegate T CreateFuncDel();
    CreateFuncDel createFunc;
    Queue<T> objectQueue = new Queue<T>();
    int count;
    public GameObjectPool(int _count, CreateFuncDel _createFunc)
    {
        count = _count;
        createFunc = _createFunc;
        Allocation();
    }
    void Allocation()
    {
        for (int i = 0; i < count; i++)
        {
            var obj = createFunc();
            objectQueue.Enqueue(obj);
        }
    }
    public T Get()
    {
        if(objectQueue.Count > 0)
        {
            return objectQueue.Dequeue();
        }
        else
        {
            var obj = createFunc();
            return obj;
        }
    }
    public void Set(T _obj)
    {
        objectQueue.Enqueue(_obj);
    }
}
