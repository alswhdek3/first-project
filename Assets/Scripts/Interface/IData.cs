using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData<T>
{
    Dictionary<string, T> ItemTable { get; set; }
}
