using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBatchPoint
{
    Player MyPlayer { get; set; }
    GameObject GetSelectBatchPoint(Vector3 _touchpos);

    bool GetEmptyPoint(int _index);
    void SetBatchPoint(int _index , bool _isbatch);
}
