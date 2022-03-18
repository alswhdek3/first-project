using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Events;

public class ZombiePlayer : Unit,IZombiePlayerSpawn,IZombieGameItemBuff
{
    private bool isZombie;

    private int score;

    public event Action<ZombiePlayer,int> TeleportEventHandler; // 텔레포트 이벤트
    public event Action<bool> AutoAddScoreEventHandler; // 점수 자동 증가 이벤트

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

        stateTable[currentState].OperatorUpdate();
    }

    protected override void OnEnable()
    {
        AutoAddScoreEventHandler?.Invoke(isZombie);
    }

    protected override void OnDisable()
    {
        
    }

    #region Interface Methods
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

    public void PlayItemBuff(ZombieGameItemType type)
    {
        switch(type)
        {
            case ZombieGameItemType.Coin: // 10~100점 사이 점수 증가
                if(pv.IsMine)
                    ScoreManager.Instance.AddScore(ActorNumber, UnityEngine.Random.Range(10, 101));
                break;
            default:
                break;
        }
    }
    #endregion

    public void AddScore(int _amount)
    {
        score += _amount;
    }
    public void SetIsZombie(bool _iszombie)
    {
        isZombie = _iszombie;
    }

    public override void SetUnit(int _actornumber, float _speed, ZombieManager _manager)
    {
        base.SetUnit(_actornumber, _speed, _manager);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if(other.gameObject.CompareTag("Teleport"))
        {
            int actornumber = Util.GetActorNumber(other.transform.name);
            TeleportEventHandler?.Invoke(this,actornumber);
        }
    }

    protected override void InitStateTableAdd()
    {
        stateTable.Add(UnitState.Idle, new ZombiePlayerIdleState(this));
        stateTable.Add(UnitState.Run, new ZombiePlayerIdleState(this));
        stateTable.Add(UnitState.Die, new ZombiePlayerIdleState(this));
    }
}
