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
        //��� �ִϸ��̼� ����ȭ
        player.AnimationShare(UnitState.Run.ToString(), false);

        // ��� ������ IsDrag�̸� �޸��� ���·� ���� 
        if (player.MovePadController.IsDrag)
            player.SetState(UnitState.Run);
    }
}
