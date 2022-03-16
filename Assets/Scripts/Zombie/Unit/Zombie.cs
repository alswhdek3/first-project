using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;

public class Zombie : Unit , IZombiePlayerSpawn
{
    private NavMeshAgent nvAgent;
    private List<ZombiePlayer> playerList;

    private ZombiePlayer targetplayer;

    private float distance;

    [Header("추적 대상 플레이어 방향으로 회전 완료시간")]
    public float turnduration = 1.2f;

    public event EventHandler<ZombiePlayer> KillEventHandler; // 킬 이벤트
    public event EventHandler<ZombiePlayer> RecoverEventHandler; // 회복 이벤트

    public int killscore = 100;
    public float attackdistance = 1f;

    private bool isAI;

    #region 프로퍼티
    public List<ZombiePlayer> PlayerList { get { return playerList; } }
    public ZombiePlayer Targetplayer { get { return targetplayer; } }
    public bool IsAI { get { return isAI; } }
    #endregion

    #region AnimationEvent
    private void AnimEvent_Attack()
    {
        KillEventHandler?.Invoke(this, targetplayer);
    }
    #endregion

    public void TargetSearch()
    {
        StartCoroutine(nameof(Coroutine_TargetSearch));
        
        IEnumerator Coroutine_TargetSearch()
        {
            while(!gamemanager.IsGameOver)
            {
                // 1명 이상의 플레이어가 존재할때 좀비는 계속 플레이어를 추적
                yield return new WaitUntil(() => playerList.Count > 0);
                
                // 플레이어와 좀비의 거리 계산
                for (int i = 0; i < playerList.Count; i++)
                {
                    if (Vector3.Distance(transform.position, playerList[i].transform.position) <= distance)
                    {
                        distance = Vector3.Distance(transform.position, playerList[i].transform.position);
                        targetplayer = playerList[i];

                        if (targetplayer != null)
                        {
                            Vector3 dist = targetplayer.transform.position - transform.position;

                            // 추적대상 플레이어와의 거리가 attackdistance 이하이면 플레이어 공격(공격 당한 플레이어는 움직임을 멈추고 좀비로 변한다)
                            if (dist.magnitude <= attackdistance)
                            {
                                // 공격당한 캐릭터 이동을 멈추고(이동불가상태) 비활성화 후 좀비로 변한다(좀비를 비활성화된 플레이어 자리에 생성)
                                SetState(UnitState.Attack);
                            }
                        }
                    }
                }
            }           
        }
    }

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
    }
    
    public void SetIsAI(bool _isAI)
    {
        isAI = _isAI;
    }

    public void ResetTarget()
    {
        targetplayer = null;
        distance = Mathf.Infinity;
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
    #endregion

    #region 재정의 메서드
    public override void SetState(UnitState _state)
    {
        base.SetState(_state);
        stateTable[currentState].OperatorUpdate(); // 현재 상태 실행
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

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();        
    }

    protected override void Update()
    {
        base.Update();        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);            
    }

    public override void SetUnit(int _actornumber, float _speed, ZombieManager _manager)
    {
        base.SetUnit(_actornumber, _speed, _manager);
        nvAgent.speed = Speed;

        // 활성화 이벤트 등록
        UnitEnableEventHandler += (_sender, e) =>
        {
            // AI
            if(isAI)
            {
                SetState(UnitState.Idle); // 대기 상태로 초기화
                TargetSearch(); // 타겟 검색 메서드 호출
            }
            // Player
            else
            {
                CameraController_MiniGame.Instance.MyLocalPlayerTarget = transform; //카메라 추적 플레이어 지정
                gamemanager.MovePadController.SetMyPlayer(transform,Speed); // 패드로 조작할 플레이어 지정                                                                   
            }
        };
    }

    protected override void InitStateTableAdd()
    {
        stateTable.Add(UnitState.Idle, new ZombieIdleState(this));
        stateTable.Add(UnitState.Run, new ZombieIdleState(this));
        stateTable.Add(UnitState.Attack, new ZombieIdleState(this));
    }
    #endregion
}
