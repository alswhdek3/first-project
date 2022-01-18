﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonster
{
    void SetMonster(int _spawnPointIndex , Transform[] _waypoints , int _monsterindex);
}

public class Monster : Unit , IMonster
{
    private int currentWaypoint;

    private float speed;

    private int waypointslength;

    public int CurrentWayPoint => currentWaypoint;

    public float Speed => speed;
    private IEnumerator Coroutine_Move(Transform[] _waypoints)
    {
        int waypointsLength = _waypoints.Length;
        while (true)
        {
            Vector2 distance = _waypoints[currentWaypoint].transform.position - transform.position;
            //float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            if(distance.magnitude <= 0.05f)
            {
                // 목적지 도착시 잠시 멈춘다
                Rb.velocity = Vector2.zero;
                animtor.SetBool("isWalk", false);
                yield return new WaitForSeconds(0.2f);

                // 다음 도착지 선정
                currentWaypoint += 1;
                if(currentWaypoint > waypointsLength-1)
                {
                    //계단을 내려가는 기능이므로 end -> start
                    Vector2 start = _waypoints[waypointsLength - 1].transform.GetChild(0).transform.position;
                    Vector2 end = _waypoints[waypointsLength - 1].transform.GetChild(1).transform.position;
                    yield return Coroutine_StairsPlay(start, end);
                    animtor.SetTrigger("Die");
                    yield break;
                }
                Vector2 newdirection = _waypoints[currentWaypoint].transform.position - transform.position;
                Rb.velocity = newdirection.normalized * speed;
                animtor.SetBool("isWalk", true);
            }

            yield return null;
        }
    }

    private IEnumerator Coroutine_StairsPlay(Vector2 _start, Vector2 _end)
    {
        float currenttime = 0f;
        float percent = 0f;
        float playtime = Vector2.Distance(_start, _end);

        while(percent < 1f)
        {
            currenttime += Time.deltaTime;
            percent = currenttime / playtime;
            transform.position = Vector2.Lerp(_start, _end, percent);
            yield return null;
        }
    }

    /// <summary>
    /// 애니메이션 클립 지정한곳에 이벤트 발생
    /// </summary>
    protected override void AnimEvent_Die()
    {
        OnDie((currentWaypoint) =>
        {
            // 플레이어 HP  감소
            if (currentWaypoint > waypointslength - 1)
            {
                Debug.Log("플레이어 HP 감소");
            }
            // 플레이어 경험치 AND 골드 증가
            else
            {
                Debug.Log("플레이어 경험치 AND 골드 증가 ");
            }
            currentWaypoint = 0;
        });
    }

    public override void SetUnit(int _level, int _hp)
    {
        base.SetUnit(_level, _hp);       
    }

    public void SetMonster(int spawnPointIndex , Transform[] _waypoints , int _monsterindex)
    {
        // Monster Name Change
        transform.name = $"Monster_{_monsterindex}";

        currentWaypoint = 0;
        speed = Level * 1.5f;

        // 왼쪽에서 몬스터가 스폰되면 Sprite Renderer FlipX false
        if (spawnPointIndex == 2 || spawnPointIndex == 3)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        Rb = gameObject.GetComponent<Rigidbody2D>();
        Rb.velocity = (_waypoints[0].transform.position - transform.position).normalized * speed;

        animtor = gameObject.GetComponent<Animator>();
        animtor.SetBool("isWalk", true);

        waypointslength = _waypoints.Length;
        StartCoroutine(Coroutine_Move(_waypoints));
    }
    public override void OnDmage(int _damage)
    {
        
    }

    /// <summary>
    /// Unit이 죽은 시점이 마지막 WayPoint Index인지 그전 WayPoint Index인지 체크후 다른 이벤트 실행
    /// </summary>
    /// <param name="_dieAction"></param>
    public override void OnDie(Action<int> _callback)
    {
        _callback?.Invoke(waypointslength);
        UnitManager.Instance.RemoveGameObject(transform.name , gameObject);
    }
}