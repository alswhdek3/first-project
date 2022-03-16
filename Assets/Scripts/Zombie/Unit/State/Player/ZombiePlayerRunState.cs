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
        //�޸��� ���� ����� ��� ���·� ����
        player.UnitAnimator.SetBool(UnitState.Run.ToString(), false);
        player.AnimationShare(UnitState.Run.ToString(), false);
    }

    public void OperatorUpdate()
    {
        //�޸��� �ִϸ��̼� ����ȭ
        player.AnimationShare(UnitState.Run.ToString(), true);

        // �޸��� ������ !IsDrag�̸� ��� ���·� ���� 
        if (!player.MovePadController.IsDrag)
            player.SetState(UnitState.Idle);
    }
}
