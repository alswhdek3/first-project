using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit , IAnimation
{
    private List<Monster> monsterList = new List<Monster>();

    IEnumerator Coroutine_TargetBatchPointMove(GameObject _batchPoint, int _index)
    {
        Vector2 distance = _batchPoint.transform.position - transform.position;
        while (distance.magnitude > 0.1f)
        {
            distance = _batchPoint.transform.position - transform.position;
            yield return null;
        }
        // 클릭한 BatchPoint에 도착시 index번째 배치 포인트 false 상태로 변경
        Rb.velocity = Vector2.zero;
        BatchManager.Instance.SetBatchPoint(_index, false);

        // 공격 시작
        yield return Coroutine_Attack();
    }

    private IEnumerator Coroutine_Attack()
    {
        float magnitude = Mathf.Infinity;
        Monster target = null;

        // 추후 true를 !isDie 변수로 변경
        while(true)
        {
            // 충돌체 반경에 들어온 몬스터 갯수가 1개이상일때부터 거리계산 시작
            yield return new WaitUntil(() => monsterList.Count > 0);

            for(int i=0; i<monsterList.Count; i++)
            {
                if(Vector2.Distance(transform.position, monsterList[i].transform.position) < magnitude)
                {
                    magnitude = Vector2.Distance(transform.position, monsterList[i].transform.position);
                    target = monsterList[i];
                }
            }

            // 타겟에 있는 방향으로 캐릭터 회전(z좌표 회전)
            Vector2 distance = target.transform.position - transform.position;
            float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 공격 애니메이션 재생
            if (!IsCurrentAnimation("Attack")) // 현재 공격 상태가 아닌지 확인
            {
                PlayTriggerAnimation("Attack");
            }

            yield return null;
        }
    }

    public void TargetBatchPointMove(GameObject _batchPoint , int _index)
    {
        Vector2 distance = _batchPoint.transform.position - transform.position;

        if(distance.normalized.x > 0f)
            transform.localScale = Vector3.one;
        else
            transform.localScale = new Vector3(-1f, 1f, 1f);

        Rb.velocity = distance.normalized * 1.5f;

        StartCoroutine(Coroutine_TargetBatchPointMove(_batchPoint , _index));
    }

    public override void OnDie(Action<int> _action)
    {
        
    }

    public override void OnDmage(int _damage)
    {
        
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();
        animtor = GetComponent<Animator>();
    }

    public void AnimEvent_Attack()
    {
        
    }

    public void AnimEvent_Hit()
    {
       
    }

    public void AnimEvent_Die()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            Monster newmonster = collision.gameObject.GetComponent<Monster>();
            if(!monsterList.Contains(newmonster))
            {
                monsterList.Add(newmonster);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Monster newmonster = collision.gameObject.GetComponent<Monster>();
            if (monsterList.Contains(newmonster))
            {
                monsterList.Remove(newmonster);
            }
        }
    }
}
