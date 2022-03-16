using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : IState
{
    private Zombie zombie;
    public ZombieAttackState(Zombie _zombie)
    {
        zombie = _zombie;
    }
    public void OperatorEnter()
    {
        // 공격 애니메이션 재생
        zombie.UnitAnimator.SetTrigger(UnitState.Attack.ToString());
    }

    public void OperatorExit()
    {
        // 공격 종료시 공격 대상이었던 플레이어 좀비로 변경

    }

    public void OperatorUpdate()
    {
        // 공격 재생시간이 종료되고 대기 상태로 초기화
        zombie.ResetState();
    }
}
