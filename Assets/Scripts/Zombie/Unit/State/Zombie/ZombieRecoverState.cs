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
        // 이벤트 등록
        zombie.UnitDisableEventHandler += (_sender, e) => //비활성화 이벤트 등록
        {
            // 플레이어 활성화
            ZombiePlayer prevlocalplayer = zombie.PlayerList.Find(player => player.ActorNumber == zombie.ActorNumber);
            prevlocalplayer.gameObject.SetActive(true);
            Debug.Log($"Enable Player ActorNumber : {prevlocalplayer.ActorNumber}");
        };
        zombie.RecoverEventHandler += (_sender, prevlocalplayer) => //회복 이벤트 등록
        {
            ScoreManager.Instance.TargetScoreCardColorChange(prevlocalplayer.ActorNumber, Color.green);//점수 카드 색상 변경
            prevlocalplayer.SetIsZombie(false); //isZombie bool형 변수 false 값으로 변경
            zombie.gameObject.SetActive(false); // 좀비 비활성화
        };
    }

    public void OperatorExit()
    {
        zombie.gameObject.SetActive(false);
    }

    public void OperatorUpdate()
    {
        zombie.SetState(UnitState.Idle); // 대기 상태로 변경
    }
}
