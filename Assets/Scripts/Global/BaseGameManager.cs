using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;
using UnityEngine;

public abstract class BaseGameManager<T> : BasePhoton , IPlayerList<T> , IGameProcess , ICamera where T : Component
{
    [SerializeField]
    protected GameObject gamePlayObject;

    [Header("생성된 플레이어는 playerperent 자식으로 넣는다)")]
    [SerializeField]
    protected GameObject playerperent;

    [SerializeField]
    protected MovePadController movepadctr;

    [Header("게임 타이머")]
    [SerializeField]
    protected GameObject gameTimer;
    [SerializeField]
    protected float timeDuration = 60f;
    [SerializeField]
    protected Text timeDurationText;

    [SerializeField]
    protected float speed = 5f;

    protected List<T> playerList = new List<T>();

    protected event Action GameStartEventAction;
    protected event Action GamePlayEventAction;
    protected event Action GameOverEventAction;

    protected string UIPath { get; set; }
    public List<T> PlayerList { get { return playerList; } }
    public MovePadController MovePadController { get { return movepadctr; } }
    public bool IsGameOver { get; private set; }

    protected void SetObjectParent(GameObject _parent , GameObject _children)
    {
        _children.transform.SetParent(_parent.transform);
    }

    protected abstract void InitEventAdd();

    #region PlayerList Interface
    public void AddPlayer(T _newplayer)
    {
        if (!playerList.Contains(_newplayer))
            playerList.Add(_newplayer);
    }

    public void RemovePlayer(T _player)
    {
        if (playerList.Contains(_player))
            playerList.Remove(_player);
    }

    public List<T> GetPlayerList()
    {
        return playerList;
    }
    public T GetLocalPlayer()
    {
        foreach(var player in playerList)
        {
            if (player.GetComponent<PhotonView>().IsMine)
                return player;
        }
        return null;
    }
    public T GetTargetPlayer(int _actornumber)
    {
        foreach (var player in playerList)
        {
            if (_actornumber == player.GetComponent<PhotonView>().OwnerActorNr)
                return player;
        }
        return null;
    }
    #endregion

    #region GameProcess Interface
    public float GameTime { get; set; }
    public void SetGameOver(bool _isGameOver)
    {
        IsGameOver = _isGameOver;
    }

    public bool GetIsGameOver()
    {
        return IsGameOver;
    }
    #endregion

    #region ICamera Interface
    public void SetCameraTarget(Transform _localPlayer)
    {
        cameracontroller.MyLocalPlayerTarget = _localPlayer;
    }
    #endregion

    protected override void Awake()
    {
        IsGameOver = true;
    }
    protected override void Start()
    {
        base.Start();

        GameStartEventAction?.Invoke();
        GamePlayEventAction?.Invoke();
        GameOverEventAction?.Invoke();
    }
}
