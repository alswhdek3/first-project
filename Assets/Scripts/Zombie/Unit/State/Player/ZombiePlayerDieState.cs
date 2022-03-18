using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePlayerDieState : IState
{
    private ZombiePlayer player;
    public ZombiePlayerDieState(ZombiePlayer _player)
    {
        player = _player;
    }
    public void OperatorEnter()
    {
        // ��Ȱ��ȭ �̺�Ʈ ���
        player.UnitDisableEventHandler += (_sender,_e) =>
        {
            ScoreManager.Instance.TargetScoreCardColorChange(player.ActorNumber, Color.green); // ���� ī�� ��������� ����
            if(player.GetIsLocalPlayer())
                player.GameManager.CreateZombie(Random.Range(0, player.GameManager.ZombieKindCount), player.transform.position, true); // ���� ����

            // ī�޶� Ÿ�� ����
            CameraController_MiniGame.Instance.MyLocalPlayerTarget = player.transform; 
        };
        player.SetState(UnitState.Idle);
    }

    public void OperatorExit()
    {
        player.gameObject.SetActive(false);
    }

    public void OperatorUpdate()
    {
        
    }
}
