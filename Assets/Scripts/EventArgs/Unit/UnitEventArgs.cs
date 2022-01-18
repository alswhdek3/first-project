using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void UnitEventHandler(object _sender, UnitEventArgs _e);
public class UnitEventArgs : EventArgs
{
    public UnitEventArgs(int _level , int _hp , string _name)
    {
        Level = _level;
        Hp = _hp;
        Name = _name;
    }

    public int Level { get; private set; }

    public int Hp { get; private set; }

    public string Name { get; private set; }
}
