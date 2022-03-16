using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class RoomManager : BasePhoton,IPhotonButton
{
    [SerializeField]
    private GameObject intro , inRoom;

    [Header("게임시작 인원관련 변수")]
    [SerializeField]
    private Text playerCount;
    [SerializeField]
    private Text gameStartCountText;
    [SerializeField]
    private int gameStartMinCount = 2;

    #region ButtonOnClickEvent
    public void OnClickStart()
    {
        // 이벤트 등록
        photonServerEventHandler += (_sender, gameversion, isServerConnect) => // 서버접속 성공 여부 이벤트
        {
            Debug.Log($"====== ZombieGame ======\n" +
                $"PhotonNetwork Server Version : {gameversion} / IsServerConnect : {isServerConnect}");
        };     
        photonRoomEventHandler += (_sender, _e) => // 방 입장 이벤트
        {
            Debug.Log($"Enter Room Name : {_e.RoomName},EnterRoomPlayerCount : {_e.CurrentRoomPlayerCount}, IsServerConnect : {_e.IsServerConnect}\n" +
                $"PlayerNickName:{PhotonNetwork.NickName} / PlayerActorNumber:{PhotonNetwork.LocalPlayer.ActorNumber}");
        };
        roomEnterEvent += (_currentPlayerCount) => // 방 입장 이벤트2
        {
            // 인트로 UI 비활성화 -> InRoomUI 활성화
            intro.gameObject.SetActive(false);
            inRoom.gameObject.SetActive(true);

            // 현재 방에 들어와있는 인원 Text UI 반영(현재인원 / 최대인원)
            playerCount.text = $"{_currentPlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}"; // ex) 1/6

            // 방에있는 플레이어 수가 최대인원이면 더이상 다른 플레이어가 들어오지 못하도록 방음 잠그고 게임이 시작된다(게임시작 카운트시 방을 못나간다)  
            if (_currentPlayerCount == gameStartMinCount)
            {
                // 방장이 방을 잠근다
                if(PhotonNetwork.LocalPlayer.IsMasterClient)
                    PhotonNetwork.CurrentRoom.IsOpen = false;

                cancelButton.interactable = false; // 게임 시작 카운트 시 방을 못나가게 설정
                pv.RPC(nameof(GameStartCountRPC), RpcTarget.AllBuffered);
            }
        };

        // 서버 접속
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }
    public void OnClickExit()
    {
       
    }

    public void OnClickGameCancel()
    {
        leftRoomEvent += () =>
        {
            inRoom.gameObject.SetActive(false);
            intro.gameObject.SetActive(true);

            // 서버 연결 종료(방 퇴장시 서버연결을 종료시켜 다시 서버 접속부터 하게끔 작성)
            PhotonNetwork.Disconnect();
        };
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    [PunRPC]
    private void GameStartCountRPC()
    {
        int count = 6;
        StartCoroutine(Coroutine_GameStartCount());
        IEnumerator Coroutine_GameStartCount()
        {
            while(count >= 0)
            {
                yield return new WaitForSeconds(1f);
                count -= 1;
                gameStartCountText.text = $"{count}초 후 시작합니다..";
            }
            inRoom.gameObject.SetActive(false);
            intro.gameObject.SetActive(false);
            isGameStart = true;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if(gameStartCountText != null)
            gameStartCountText.text = "프렌즈가 모이길 기다리는 중...";

        // Unity Button Action Add
        startButton.onClick.AddListener(OnClickStart);
        cancelButton.onClick.AddListener(OnClickGameCancel);
    }
}
