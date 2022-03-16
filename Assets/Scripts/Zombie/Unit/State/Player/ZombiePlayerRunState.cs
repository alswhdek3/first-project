using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePlayerRunState : IState
{
    private ZombiePlayer player;
    public ZombiePlayerRunState(ZombiePlayer _player)
    {
        player = _player;
    }
    public void OperatorEnter()
    {
        player.UnitAnimator.SetBool(UnitState.Run.ToString(), true);
    }

    public void OperatorExit()
    {
        //달리기 상태 종료시 대기 상태로 변경
        player.UnitAnimator.SetBool(UnitState.Run.ToString(), false);
        player.AnimationShare(UnitState.Run.ToString(), false);
    }

    public void OperatorUpdate()
    {
        //달리기 애니메이션 동기화
        player.AnimationShare(UnitState.Run.ToString(), true);

        // 달리기 상태중 !IsDrag이면 대기 상태로 변경 
        if (!player.MovePadController.IsDrag)
            player.SetState(UnitState.Idle);
    }
}
