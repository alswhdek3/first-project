using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Events;

public class ZombiePlayer : Unit , IZombiePlayerSpawn
{
    private bool isZombie;

    private int score;

    private ZombieManager gamemanager;

    public event EventHandler<ZombieGameTeleport> TeleportEventHandler; // 텔레포트 이벤트
    public event EventHandler DieEventHandler; // 좀비에게 잡혔을때 이벤트
    public event EventHandler<Zombie> RecoveryEventHandler; // 좀비에서 회복되었을때 이벤트

    public int Score { get { return score; } }
    public bool IsZombie { get { return isZombie; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (movePadController == null)
            return;

        // 좀비 상태가 아닐때 MovePad 컨트롤 가능
        if(!isZombie)
        {
            if (pv.IsMine)
            {
                // 애니메이션 동기화
                pv.RPC(nameof(AnimationShareRPC), RpcTarget.All, UnitState.Run.ToString(), movePadController.IsDrag);
            }
        }
    }

    protected override void OnEnable()
    {
        if(pv.IsMine)
        {
            // 추적중인 플레이어가 좀비에서 캐릭터로 회복되면 타겟팅 재 지정 이벤트도 등록
            Zombie prevlocalzombie = gamemanager.ZombieList.Find(localzombie => localzombie.GetComponent<PhotonView>().IsMine && !localzombie.IsAI);
            RecoveryEventHandler?.Invoke(this, prevlocalzombie);
        }        
    }

    protected override void OnDisable()
    {
        if(pv.IsMine)
        {
            // MovePad로 조작가능한 좀비 생성
            DieEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }

    #region IZombiePlayerSpawn
    public void Enable(Vector3 _position , ICamera camera)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = _position;

        camera.SetCameraTarget(transform);
    }

    public bool GetIsPrevSpawn()
    {
        if (gamemanager.GetPlayerList().Contains(this))
            return true;

        return false;
    }
    #endregion

    #region EventHandler
    private void ItemEvent(object _sender , ZombieGameItem _item)
    {
        switch (_item.Type)
        {
            case ZombieGameItemType.Coin:
                // 50점 증가
                score += 50;
                break;
            // 좀비 이동속도 5초동안 /2
            case ZombieGameItemType.Slow:
                List<Zombie> zombieList = gamemanager.ZombieList;
                foreach (Zombie zombie in zombieList)
                {
                    zombie.Speed = zombie.Speed / 2f;

                    StartCoroutine(nameof(Coroutine_SpeedSlowEnd));
                    IEnumerator Coroutine_SpeedSlowEnd()
                    {
                        yield return new WaitForSeconds(5f);
                        zombie.Speed = gamemanager.moveSpeed * 1.5f;
                    }
                }
                break;
        }
    }
    private void DieEvent(object _sender , EventArgs _e)
    {
        int randomindex = UnityEngine.Random.Range(0, gamemanager.ZombieSpawnPointLength);
        gamemanager.CreateZombie(randomindex, transform.position, false);
    }
    private void RecoveryEvent(object _sender , EventArgs _e)
    {

    }
    #endregion

    public void SetScore(int _amount)
    {
        score += _amount;
        // LocalPlayer ScoreText UI 적용

    }

    public override void SetUnit<T>(int _actornumber, float _speed, T _manager)
    {
        ActorNumber = _actornumber;
        Speed = _speed;
        gamemanager = _manager as ZombieManager;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // 일회성 버프이기때문에 버프 실행후 삭제
        ItemBuffEvent -= ItemEvent;
    }

    public override void InitEventAdd()
    {
        ItemBuffEvent += ItemEvent; // 아이템 버프 이벤트 등록
        DieEventHandler += DieEvent; // 좀비에게 잡혔을때 이벤트 등록
    }   
}
