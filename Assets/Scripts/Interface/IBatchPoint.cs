using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBatchPoint
{
    BasePlayer MyPlayer { get; set; }
    bool GetEmptyPoint(int _index);
    void SetBatchPoint(int _index , bool _isbatch);
}
