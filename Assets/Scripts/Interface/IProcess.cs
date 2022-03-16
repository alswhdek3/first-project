using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameProcess
{
    void SetGameOver(bool _isGameOver);
    bool GetIsGameOver();
}
