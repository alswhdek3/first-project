using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TableType
{
    None=-1,
    MissionTable,
    Max
}
public class TableManager : SingtonMonoBehaviour<TableManager>
{
    public event EventHandler TableSaveEventHandler;
    public T GetTable<T>(TableType type) where T : class
    {
        T table = Resources.Load($"Table/{type}") as T;
        if(table == null)
        {
            Debug.LogError($"{type}Table�� �������� �ʽ��ϴ�");
            return null;
        }
        return table;
    }
    private void OnApplicationQuit()
    {
        TableSaveEventHandler?.Invoke(this, EventArgs.Empty);
    }
}
