using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData<T>
{
    private static Dictionary<string, T> dataTable;

    public static Dictionary<string,T> GetDataTable(string[] _key , T[] _value)
    {
        dataTable = new Dictionary<string, T>();
        for(int i=0; i<_key.Length; i++)
        {
            if (dataTable.ContainsKey(_key[i]))
                continue;

            dataTable.Add(_key[i], _value[i]);
        }

        return dataTable;
    }
}
