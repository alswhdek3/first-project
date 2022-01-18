using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Waypoint")]
    [SerializeField]
    private GameObject[] waypointsObject;

    [Header("SpawnPoint")]
    [SerializeField]
    private Transform[] spawnpoints;

    // Stage
    private int currentStage;
    private bool isGameover;

    public int CurrentStage => currentStage;
    public bool IsGameover => isGameover;

    private IEnumerator Coroutine_GamePatten()
    {
        Action CreateMonster = () =>
        {
            // 몬스터 생성
            int monsterindex = UnityEngine.Random.Range(0, currentStage + 1);
            var unit = UnitManager.Instance.GetCreateGameObject($"Monster_{monsterindex}");
            var newMonster = unit.GetComponent<Monster>();

            // 몬스터 스폰 위치 설정
            int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnpoints.Length);
            newMonster.transform.position = spawnpoints[randomSpawnPointIndex].transform.position;

            // WayPoints 지정
            Transform[] targetSpawnPointTransforms = new Transform[waypointsObject[randomSpawnPointIndex].transform.childCount];
            for (int i = 0; i < targetSpawnPointTransforms.Length; i++)
                targetSpawnPointTransforms[i] = waypointsObject[randomSpawnPointIndex].transform.GetChild(i).GetComponent<Transform>();

            // 현재 스테이지에 해당하는 몬스터 스프라이트 반환
            newMonster.SetUnit(currentStage, currentStage * 100);            
            newMonster.SetMonster(randomSpawnPointIndex, targetSpawnPointTransforms , monsterindex);
        };

        Func<bool> IsUnitPool = () =>
        {
            if (UnitManager.Instance.ObjectPoolTable != null)
            {
                return true;
            }
            return false;
        };

        while(true)
        {
            yield return new WaitUntil(IsUnitPool);
            // 임시로 1초 마다 생성
            CreateMonster?.Invoke();
            yield return new WaitForSeconds(5f);
        }
    }

    private void InitSetting()
    {
        currentStage = 1;
        isGameover = false;
    }

    protected override void OnAwake()
    {
        InitSetting();

        StartCoroutine(Coroutine_GamePatten());
    }
}
