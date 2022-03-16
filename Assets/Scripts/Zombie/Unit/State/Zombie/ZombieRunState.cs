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
        zombie.UnitAnimator.SetBool(UnitState.Run.ToString(), true); // 애니메이션 달리기 상태로 전환
    }

    public void OperatorExit()
    {
        zombie.UnitAnimator.SetBool(UnitState.Run.ToString(), false); // 애니메이션 대기 상태로 전환
    }

    public void OperatorUpdate()
    {
        zombie.TargetSearch();
    }
}
