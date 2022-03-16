using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OperatorEnter();
    void OperatorExit();
    void OperatorUpdate();
}
