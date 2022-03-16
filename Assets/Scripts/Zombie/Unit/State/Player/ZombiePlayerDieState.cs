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
        // 비활성화 이벤트 등록
        player.UnitDisableEventHandler += (_sender,_e) =>
        {
            ScoreManager.Instance.TargetScoreCardColorChange(player.ActorNumber, Color.green); // 점수 카드 보라색으로 변경
            if(player.GetIsLocalPlayer())
                player.GameManager.CreateZombie(Random.Range(0, player.GameManager.ZombieKindCount), player.transform.position, true); // 좀비 생성

            // 카메라 타겟 변경
            CameraController_MiniGame.Instance.MyLocalPlayerTarget = player.transform; 
        };
    }

    public void OperatorExit()
    {
        
    }

    public void OperatorUpdate()
    {
        
    }
}
