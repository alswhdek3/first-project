using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> , ISetting<Monster>
{
    [Header("Waypoint")]
    [SerializeField]
    private GameObject[] waypointsObject;

    [Header("SpawnPoint")]
    [SerializeField]
    private Transform[] spawnpoints;

    private List<Monster> monsterList = new List<Monster>(); 

    // Stage
    private int currentStage;
    private bool isGameover;

    public int CurrentStage => currentStage;

    public List<Monster> MonsterList => monsterList;
    public bool IsGameover => isGameover;

    private IEnumerator Coroutine_GamePatten()
    {
        Action CreateMonster = () =>
        {
            // 몬스터 생성
            int monsterindex = UnityEngine.Random.Range(0, currentStage + 1);
            Monster newMonster = ObjectPool<Monster>.GetObject($"Monster_{monsterindex}", gameObject);

            //몬스터 스폰 위치 설정
            int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnpoints.Length);
            if(newMonster != null)
                newMonster.transform.position = spawnpoints[randomSpawnPointIndex].transform.position;

            //WayPoints 지정
            Transform[] targetSpawnPointTransforms = new Transform[waypointsObject[randomSpawnPointIndex].transform.childCount];
            for (int i = 0; i < targetSpawnPointTransforms.Length; i++)
                targetSpawnPointTransforms[i] = waypointsObject[randomSpawnPointIndex].transform.GetChild(i).GetComponent<Transform>();

            //현재 스테이지에 해당하는 몬스터 스프라이트 반환
            if(newMonster != null)
            {
                newMonster.gameObject.SetActive(true);
                newMonster.SetUnit(currentStage, currentStage * 100, monsterindex);
                newMonster.SetMonster(randomSpawnPointIndex, targetSpawnPointTransforms, monsterindex);

                // New Monster Add
                monsterList.Add(newMonster);
            }
        };

        Func<bool> IsMonsterPool = () =>
        {
            if (ObjectPool<Monster>.GameObjectPool != null)
            {
                return true;
            }
            return false;
        };

        while(true)
        {
            yield return new WaitUntil(IsMonsterPool);
            // 임시로 1초 마다 생성
            CreateMonster?.Invoke();
            yield return new WaitForSeconds(1f);
        }
    }

    public void RemoveMonster(Monster _monster)
    {
        if (monsterList.Contains(_monster))
            monsterList.Remove(_monster);
    }

    protected override void OnAwake()
    {
        InitSetting();
        StartCoroutine(Coroutine_GamePatten());
    }

    public void InitSetting()
    {
        currentStage = 1;
        isGameover = false;

        // 몬스터 오브젝트풀 세팅
        GameObject[] monstres = ObjectPool<Monster>.GetAllObjects("Prefabs/Units/Monsters");
        string[] keys = new string[monstres.Length];
        for (int i = 0; i < keys.Length; i++)
            keys[i] = monstres[i].transform.name;

        Dictionary<string, List<Monster>> monsterpool = ObjectPool<Monster>.GetGameObjectPool(keys, monstres, gameObject);
        Debug.Log(monsterpool);
    }
}
