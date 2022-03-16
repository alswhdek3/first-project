using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRunState : IState
{
    private Zombie zombie;
    public ZombieRunState(Zombie _zombie)
    {
        zombie = _zombie;
    }
    public void OperatorEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OperatorExit()
    {
        throw new System.NotImplementedException();
    }

    public void OperatorUpdate()
    {
        throw new System.NotImplementedException();
    }
}
