using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRecoverState : IState
{
    private Zombie zombie;
    public ZombieRecoverState(Zombie _zombie)
    {
        zombie = _zombie;
    }
    public void OperatorEnter()
    {
        // �̺�Ʈ ���
        zombie.UnitDisableEventHandler += (_sender, e) => //��Ȱ��ȭ �̺�Ʈ ���
        {
            // �÷��̾� Ȱ��ȭ
            ZombiePlayer prevlocalplayer = zombie.PlayerList.Find(player => player.ActorNumber == zombie.ActorNumber);
            prevlocalplayer.gameObject.SetActive(true);
            Debug.Log($"Enable Player ActorNumber : {prevlocalplayer.ActorNumber}");
        };
        zombie.RecoverEventHandler += (_sender, prevlocalplayer) => //ȸ�� �̺�Ʈ ���
        {
            ScoreManager.Instance.TargetScoreCardColorChange(prevlocalplayer.ActorNumber, Color.green);//���� ī�� ���� ����
            prevlocalplayer.SetIsZombie(false); //isZombie bool�� ���� false ������ ����
            zombie.gameObject.SetActive(false); // ���� ��Ȱ��ȭ
        };
    }

    public void OperatorExit()
    {
        zombie.gameObject.SetActive(false);
    }

    public void OperatorUpdate()
    {
        zombie.SetState(UnitState.Idle); // ��� ���·� ����
    }
}
