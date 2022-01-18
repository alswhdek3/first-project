using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    IEnumerator Coroutine_TargetBatchPointMove(GameObject _batchPoint, int _index)
    {
        while(true)
        {
            Vector2 distance = _batchPoint.transform.position - transform.position;

            // 클릭한 BatchPoint에 도착시 index번째 배치 포인트 false 상태로 변경
            if(distance.magnitude < 0.1f)
            {
                Rb.velocity = Vector2.zero;
                BatchManager.Instance.SetBatchPoint(_index, false);
                yield break;
            }

            yield return null;
        }
    }

    public void TargetBatchPointMove(GameObject _batchPoint , int _index)
    {
        Vector2 distance = _batchPoint.transform.position - transform.position;

        if(distance.normalized.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        Rb.velocity = distance.normalized * 1.5f;

        StartCoroutine(Coroutine_TargetBatchPointMove(_batchPoint , _index));
    }

    public override void OnDie(Action<int> _action)
    {
        
    }

    public override void OnDmage(int _damage)
    {
        
    }

    protected override void AnimEvent_Die()
    {
        
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();
        animtor = GetComponent<Animator>();
    }
}
