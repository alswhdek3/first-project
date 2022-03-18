using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;
using UnityEngine;

public class BaseGameManager<T> : BasePhoton,IPlayerList<T>,IGameProcess,ICamera where T : Component
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

    protected event Action GameStartEventHandler;
    protected event Action GameObjectCreateEventHandler;
    protected event Action GamePlayEventHandler;
    protected event Action GameOverEventEventHandler;

    protected string UIPath { get; set; }
    public List<T> PlayerList { get { return playerList; } }
    public MovePadController MovePadController { get { return movepadctr; } }
    public bool IsGameOver { get; private set; }

    protected void SetObjectParent(GameObject _parent , GameObject _children)
    {
        _children.transform.SetParent(_parent.transform);
    }

    private void InitEventAdd()
    {
        GameStartEventHandler += GameStartEvent;
        GameObjectCreateEventHandler += GameObjectCreateEventHandler;
        GamePlayEventHandler += GamePlayEvent;
        GameOverEventEventHandler += GameOverEvent;
    }

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

    #region Events
    protected virtual void GameStartEvent()
    {
        GameObjectCreateEventHandler?.Invoke();
    }
    protected virtual void GameObjectCreateEvent()
    {
        GamePlayEventHandler?.Invoke();
    }
    protected virtual void GamePlayEvent()
    {
        GameOverEventEventHandler?.Invoke();
    }
    protected virtual void GameOverEvent()
    {

    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        IsGameOver = true;
        InitEventAdd();
    }
    protected override void Start()
    {
        base.Start();        
    }
    public override void OnEnable()
    {
        GameStartEventHandler?.Invoke();
    }
}
