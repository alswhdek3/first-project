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

    protected event PhotonServerEventHandler photonServerEventHandler;
    protected event PhotonRoomEventHandler photonRoomEventHandler;

    protected event Action<int> roomEnterEvent; // 방 입장 
    protected event Action leftRoomEvent; // 방 퇴장

    protected event Action InitGameEvent; // 게임 시작시 초기 이벤트
    
    public override void OnConnectedToMaster()
    {       
        // Server Connect Success EventHandler Show 
        photonServerEventHandler?.Invoke(this, gameVerstion, true);

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
        photonServerEventHandler?.Invoke(this, gameVerstion, false);
        //StopAllCoroutines();
    }

    #region 방 입장 , 방 입장 실패 , 방 퇴장
    public override void OnJoinedRoom()
    {
        //photonRoomEventHandler?.Invoke(this, new PhotonEventArgs(gameVerstion, PhotonNetwork.CurrentRoom.Name,
        //    PhotonNetwork.CurrentRoom.PlayerCount, true));

        // 방 입장 후 게임 시작 카운트다운
        roomEnterEvent?.Invoke(PhotonNetwork.CurrentRoom.PlayerCount);
        StartCoroutine(Coroutine_IsGameStartTrue());
        IEnumerator Coroutine_IsGameStartTrue()
        {
            while (!isGameStart)
            {
                yield return null;
            }
            InitGameEvent?.Invoke();
        }
    }
    /// <summary>
    /// 방 퇴장시 호출되는 콜백
    /// </summary>
    public override void OnLeftRoom()
    {
        leftRoomEvent?.Invoke();
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
    #endregion

    #region 방 입장 And 퇴장
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
    #endregion

    public void ShowTargetCameraInfo(int _actionNumber)
    {
        Debug.Log($"Camera Target Player ActorNumber : {_actionNumber}");
    }

    protected virtual void Awake()
    {
        pv = photonView;
        isGameStart = false;
        PhotonNetwork.GameVersion = gameVerstion;
    }

    protected virtual void Start()
    {
        
    }
}
