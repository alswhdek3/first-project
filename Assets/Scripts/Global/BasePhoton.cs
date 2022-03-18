using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;
using UnityEngine;

using System;

[RequireComponent(typeof(PhotonView))]
public class BasePhoton : MonoBehaviourPunCallbacks
{
    protected PhotonView pv;

    protected string gameVerstion = "SG_1.2";

    [SerializeField]
    protected Button startButton, cancelButton, exitButton;

    [SerializeField]
    protected GameObject playersparent;

    [SerializeField]
    protected CameraController_MiniGame cameracontroller;

    protected string path;
    protected static bool isGameStart;

    // 이벤트
    protected event Action<string,bool> PhotonServerConnectEventHandler; //서버 연결 성공 여부 이벤트
    protected event Action PhotonRoomEnterEventHandler; //방 입장 성공 여부 이벤트  
    protected event Action PhotonLeftRoomEventHandler; //방 퇴장 이벤트  

    #region PhotonNetwork Callback Methods
    public override void OnConnectedToMaster()
    {
        // Server Connect Success EventHandler Show 
    
        // Lobby Joined
        if(PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
            PhotonNetwork.JoinLobby();
    }
    /// <summary>
    /// 로비 입장 후 방 입장
    /// </summary>
    public override void OnJoinedLobby()
    {
        // 방 입장
        if(PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
            PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Server Connect Fail(End) EventHandler Show 
        
        //StopAllCoroutines();
    } 
    public override void OnJoinedRoom()
    {
        // 방 입장 후 게임 시작 카운트다운
        PhotonRoomEnterEventHandler?.Invoke();
        StartCoroutine(Coroutine_IsGameStartTrue());
        IEnumerator Coroutine_IsGameStartTrue()
        {
            while (!isGameStart)
            {
                yield return null;
            }            
        }
    }
    /// <summary>
    /// 방 퇴장시 호출되는 콜백
    /// </summary>
    public override void OnLeftRoom()
    {
        PhotonLeftRoomEventHandler?.Invoke();
    }
    /// <summary>
    /// 랜덤 방 입장 실패시 본인이 직접 방을 생성
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if(PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            Debug.LogWarning($"Random Room Enter Fail try to enter the other attempt !!");
            PhotonNetwork.CreateRoom($"Room_{UnityEngine.Random.Range(0, 101)}", new RoomOptions { MaxPlayers = 4, IsOpen = true });
        }
    }     
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
    #endregion

    #region Events                                                                                 
    protected virtual void PhotonServerConnectEvent(string _version, bool _isServerConnect)
    {
        Debug.Log($"====== ZombieGame ======\n" +
                $"PhotonNetwork Server Version : {_version} / IsServerConnect : {_isServerConnect}");
    }
    protected virtual void PhotonRoomEnterEvent()
    {
        Debug.Log($"Enter Room Name : {PhotonNetwork.CurrentRoom.Name},EnterRoomPlayerCount : {PhotonNetwork.CurrentRoom.PlayerCount}, \n" +
            $"PlayerNickName:{PhotonNetwork.NickName} / PlayerActorNumber:{PhotonNetwork.LocalPlayer.ActorNumber}");
    }
    protected virtual void LeftRoomEvent()
    {
        // 서버 연결 종료(방 퇴장시 서버연결을 종료시켜 다시 서버 접속부터 하게끔 작성)
        PhotonNetwork.Disconnect();
    }
    #endregion
    protected virtual void Awake()
    {
        pv = photonView;
        isGameStart = false;
        PhotonNetwork.GameVersion = gameVerstion;
    }
    protected virtual void Start()
    {
        // 이벤트 추가
        PhotonServerConnectEventHandler += PhotonServerConnectEvent;
        PhotonRoomEnterEventHandler += PhotonRoomEnterEvent;
        PhotonLeftRoomEventHandler += LeftRoomEvent;
    }
}
