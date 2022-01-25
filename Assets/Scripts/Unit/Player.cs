using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit , IAnimation
{
    public float attacktime = 2f;

    public float range = 2f;

    private Transform shootingTransform;

    public Monster target;

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
        yield return Coroutine_Targeting();
    }

    private IEnumerator Coroutine_Targeting()
    {
        float magnitude = Mathf.Infinity;

        float time = attacktime;

        // 추후 true를 !isDie 변수로 변경
        while(true)
        {
            // 충돌체 반경에 들어온 몬스터 갯수가 1개이상일때부터 거리계산 시작
            yield return new WaitUntil(() => GameManager.Instance.MonsterList.Count > 0);

            for (int i=0; i< GameManager.Instance.MonsterList.Count; i++)
            {
                // 사정거리 영역에 있으면
                if(Vector2.Distance(transform.position, GameManager.Instance.MonsterList[i].transform.position) < range)
                {
                    // 더 가까운 몬스터 검출
                    if (Vector2.Distance(transform.position, GameManager.Instance.MonsterList[i].transform.position) < magnitude)
                    {
                        magnitude = Vector2.Distance(transform.position, GameManager.Instance.MonsterList[i].transform.position);
                        target = GameManager.Instance.MonsterList[i];
                    }
                }
            }

            if(target != null)
            {
                // 타겟에 있는 방향으로 캐릭터 회전(z좌표 회전)
                Vector2 distance = target.transform.position - transform.position;
                float angle = angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;

                // 타겟과 플레이어의 거리중 x좌표가 음수이면 Player Scale -1로 변경
                if (distance.x > 0f)
                {
                    transform.localScale = Vector3.one;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    // 음수이면 구한각도에서 180도를 플러스
                    transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
                }

                if (Vector2.Distance(transform.position, target.transform.position) > range)
                {
                    magnitude = Mathf.Infinity;
                    target = null;
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                }
            }

            if (target == null)
                magnitude = Mathf.Infinity;
            

            yield return null;
        }
    }

    private IEnumerator Coroutine_Attack()
    {
        while(true)
        {
            if (target != null)
            {
                // 공격 : 추후 수정
                string key = "Bullet_0";
                Bullet newbullet = ShootingManager.Instance.GetBullet(key, shootingTransform.transform.position);
                if (newbullet != null)
                {
                    newbullet.SetBullet(UnityEngine.Random.Range(1,11), key, target.gameObject);
                    Vector2 distance = target.transform.position - transform.position;
                    newbullet.GetComponent<Rigidbody2D>().velocity = distance.normalized * 5f;
                }

                yield return new WaitForSeconds(0.5f);
            }
            else
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

        Rb.velocity = distance.normalized * 2f;

        StartCoroutine(Coroutine_TargetBatchPointMove(_batchPoint , _index));
    }

    public override void OnDie(Action<int> _action)
    {
        
    }

    public override void OnDamge(int _damage)
    {
        
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();
        animtor = GetComponent<Animator>();

        shootingTransform = transform.GetChild(2).GetComponent<Transform>();

        StartCoroutine(Coroutine_Attack());
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
}
