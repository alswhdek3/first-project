using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;

public class Zombie : Unit , IZombiePlayerSpawn
{
    private ZombieManager gamemanager;

    private NavMeshAgent nvAgent;
    private List<ZombiePlayer> playerList;

    private ZombiePlayer targetplayer;

    private float distance;

    [Header("추적 대상 플레이어 방향으로 회전 완료시간")]
    public float turnduration = 1.2f;

    public event EventHandler<ZombiePlayer> TargetingEventHandler; // (AI 좀비) 플레이어 추적 이벤트
    public event EventHandler<ZombiePlayer> KillEventHandler; // (AI가 아닌 좀비) // 플레이어 킬 이벤트

    public int killscore = 100;

    private bool isAI;

    #region 프로퍼티
    public bool IsAI { get { return isAI; } }
    public ZombiePlayer Targetplayer { get { return targetplayer; } }
    #endregion

    private IEnumerator Coroutine_TracePatten()
    {
        // 초기 대기상태로 전환
        SetState(UnitState.Idle);
        // 1명 이상의 플레이어가 존재할때 좀비는 계속 플레이어를 추적
        while (playerList.Count > 0)
        {
            yield return new WaitUntil(() => targetplayer != null);
            for(int i=0; i<playerList.Count; i++)
            {
                if(Vector3.Distance(transform.position,playerList[i].transform.position) <= distance)
                {
                    distance = Vector3.Distance(transform.position, playerList[i].transform.position);
                    targetplayer = playerList[i];

                    // 대기 상태일때만 추적대상 방향으로 회전
                    if(targetplayer != null)
                    {
                        Vector3 dist = targetplayer.transform.position - transform.position;

                        // 추적대상 플레이어와의 거리가 0.1f 이하이면 플레이어 공격(공격 당한 플레이어는 움직임을 멈추고 좀비로 변한다)
                        if(dist.magnitude <= 0.1f)
                        {
                            // 공격당한 캐릭터 이동을 멈추고(이동불가상태) 비활성화 후 좀비로 변한다(좀비를 비활성화된 플레이어 자리에 생성)
                            SetState(UnitState.Attack);
                        }                       
                    }                   
                }
            }
        }
    }

    #region FSM
    private void AttackState()
    {
        animator.SetTrigger("Attack");
    }
    #endregion

    #region ZombiePlayer Spawn Interface
    public void Enable(Vector3 _position, ICamera camera)
    {
        gameObject.SetActive(true);
        transform.position = _position;

        camera.SetCameraTarget(transform);
    }

    public bool GetIsPrevSpawn()
    {
        if (gamemanager.ZombieList.Contains(this))
            return true;

        return false;
    }
    #endregion

    public void SetPlayerList(List<ZombiePlayer> _playerList)
    {
        // GameManager로 부터 게임에 참여중인 플레이어 리스트를 가져온다
        playerList = _playerList;

        // 첫번째 캐릭터를 타겟으로 지정
        targetplayer = playerList[0];
    }
    
    public void SetIsAI(bool _isAI)
    {
        isAI = _isAI;
    }

    public void Turn()
    {
        StartCoroutine(nameof(Coroutine_Turn));

        IEnumerator Coroutine_Turn()
        {
            Vector3 distance = targetplayer.transform.position - transform.position;
            float dot = Vector3.Dot(transform.forward, distance.normalized);
            // Zombie transform forward 기준으로 추적대상 플레이어가 앞에있는지 뒤에있는지 dot값 양수,음수로 판단
            if (dot < 0)
            {
                // 뒤에있으므로 플레이어 방향으로 회전
                Quaternion endrotation = Quaternion.LookRotation(distance.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, endrotation, turnduration);

                // 회전이 끝날때까지 대기
                yield return new WaitUntil(() => dot >= 0f);
                // 회전이 끝나면 추적 상태로 변경
                SetState(UnitState.Run);
            }
            // 추적대상이 앞에있으면 회전하지않고 추적상태로 변경
            else
                SetState(UnitState.Run);
        }       
    }

    #region EventHandler
    private void KillEvent(object _sender, EventArgs e)
    {
        gamemanager.GetLocalPlayer().SetScore(killscore);
    }
    #endregion

    #region 재정의 메서드
    public override void SetState(UnitState _state)
    {
        base.SetState(_state);
    }

    protected override void Awake()
    {
        base.Awake();
        
        nvAgent = GetComponent<NavMeshAgent>();
        distance = Mathf.Infinity;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(!isAI)
        {
            // Item Buff Event
            base.OnTriggerEnter(other);

            // Kill Event
            if (other.gameObject.CompareTag("Player"))
                KillEventHandler?.Invoke(this, other.GetComponent<ZombiePlayer>());
        }        
    }

    public override void SetUnit<T>(int _actornumber, float _speed, T _manager)
    {
        ActorNumber = _actornumber;

        // 좀비 스피드 적용
        Speed = _speed;
        nvAgent.speed = _speed;

        gamemanager = _manager as ZombieManager;
    }

    /// <summary>
    /// AI가 아닌 좀비 이벤트 등록
    /// </summary>
    public override void InitEventAdd()
    {
        // ItemEvent
        ItemBuffEvent += (_sender, item) =>
        {
            switch(item.Type)
            {
                case ZombieGameItemType.Recovery:
                    // 좀비 비활성화
                    gameObject.SetActive(false);

                    // 이전에 조작했던 캐릭터 활성화
                    foreach(ZombiePlayer player in gamemanager.GetPlayerList())
                    {
                        if(player.GetIsLocalPlayer())
                        {
                            if(player.GetIsPrevSpawn())
                            {
                                // 플레이어 활성화 : 카메라 타겟 지정
                                player.Enable(transform.position , gamemanager);
                            }
                        }
                    }
                    break;
            }
        };

        // KillEvent(AI 좀비 제외)
        KillEventHandler += (_sender, player) =>
        {
            Vector3 prevposition = player.transform.position;
            int actornumber = player.ActorNumber;

            foreach(ZombiePlayer p in gamemanager.GetPlayerList())
            {
                if (p.ActorNumber == actornumber)
                {
                    p.Enable(prevposition, gamemanager);
                    return;
                }
                // 킬에 성공한 좀비 점수 증가(AI 좀비 제외)
                p.DieEventHandler += KillEvent;
            }
            player.gameObject.SetActive(false);
        };
    }
    #endregion
}
