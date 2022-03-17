using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTable : ScriptableObject
{
    [SerializeField]
    protected TextAsset dataTextAsset;
    protected string path;

    [ContextMenu("InitTableSetting")]
    protected virtual void InitTableSetting()
    {        
        dataTextAsset = Resources.Load<TextAsset>(path);
    }

    protected virtual void Awake()
    {
        InitTableSetting();
    }
}
