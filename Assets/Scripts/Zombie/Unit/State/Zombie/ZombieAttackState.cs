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
        zombie.KillEventHandler += (_sender, player) =>
        {
            if (!zombie.IsAI) // 킬에 성공한 좀비 점수 증가(AI가 아닌 좀비)
                ScoreManager.Instance.AddScore(zombie.ActorNumber, zombie.killscore);

            zombie.ResetTarget(); // 타겟 리셋
            player.gameObject.SetActive(false); // 공격당한 플레이어 비활성화
        };
    }

    public void OperatorUpdate()
    {        
        zombie.ResetState(); // 공격 재생시간이 종료되고 대기 상태로 초기화
    }
}
