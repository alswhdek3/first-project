using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePlayerIdleState : IState
{
    private ZombiePlayer player;
    public ZombiePlayerIdleState(ZombiePlayer _player)
    {
        player = _player;
    }
    public void OperatorEnter()
    {
        player.UnitAnimator.SetBool(UnitState.Run.ToString(), false);
    }

    public void OperatorExit()
    {
        
    }

    public void OperatorUpdate()
    {
        //대기 애니메이션 동기화
        player.AnimationShare(UnitState.Run.ToString(), false);

        // 대기 상태중 IsDrag이면 달리기 상태로 변경 
        if (player.MovePadController.IsDrag)
            player.SetState(UnitState.Run);
    }
}
