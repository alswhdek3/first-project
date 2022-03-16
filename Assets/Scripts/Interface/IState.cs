using System.Collections;
using System.Collections.Generic;

using Photon.Realtime;
using Photon.Pun;

using UnityEngine;

public interface IState
{
    void OperatorEnter();
    void OperatorExit();
    void OperatorUpdate();
}
