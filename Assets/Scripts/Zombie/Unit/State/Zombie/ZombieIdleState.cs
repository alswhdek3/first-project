using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : IState
{
    private Zombie zombie;
    public ZombieIdleState(Zombie _zombie)
    {
        zombie = _zombie;
    }
    public void OperatorEnter()
    {
        zombie.UnitAnimator.SetBool(UnitState.Run.ToString(), false); // 애니메이션 대기 상태로 전환
    }

    public void OperatorExit()
    {
        
    }

    public void OperatorUpdate()
    {
        if(zombie.Targetplayer != null)
            zombie.Turn();
    }
}
