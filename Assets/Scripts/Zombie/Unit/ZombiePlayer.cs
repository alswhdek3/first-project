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

    public event EventHandler<ZombieGameTeleport> TeleportEventHandler; // 텔레포트 이벤트
    public event Action<int> AutoAddScoreEventHandler; // 점수 자동 증가 이벤트

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
           
    }

    protected override void OnDisable()
    {
        
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

        // 일회성 버프이기때문에 버프 실행후 삭제
        ItemBuffEvent -= ItemEvent;
    }

    protected override void InitStateTableAdd()
    {
        stateTable.Add(UnitState.Idle, new ZombiePlayerIdleState(this));
        stateTable.Add(UnitState.Run, new ZombiePlayerIdleState(this));
        stateTable.Add(UnitState.Die, new ZombiePlayerIdleState(this));
    }
}
