using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using System.Threading.Tasks;

using UnityEngine.UI;
using UnityEngine;

using System;
using System.Linq;

public class ZombieManager : BaseGameManager<ZombiePlayer>
{
    private ZombiePlayer localplayer;
    private List<Zombie> zombieList = new List<Zombie>();

    [Header("ZombieGameItemManager")]
    [SerializeField]
    private ZombieGameItemManager itemmanager;

    [Header("좀비 이동속도는 캐릭터 이동속도 * 1.5")]
    public float moveSpeed = 10f;

    [Header("좀비 종류 갯수")]
    [SerializeField]
    private int zombiekindcount = 4; // 추후 리소스 코드로 접근해서 수정 예정

    [Header("생성된 좀비는 zombieparent 자식으로 넣는다)")]
    [SerializeField]
    private GameObject zombieparent;

    [Header("PlayerSpawnPoint 관련 변수들")]
    [SerializeField]
    private GameObject spawnPointTable;
    public float spawnPointDistance = 4f;

    [Header("Zombie SpawnPoint 관련 변수")]
    [SerializeField]
    private GameObject zombieSpawnPoint;
    private Transform[] zombieSpawnPoints;

    [Header("GameCountImage")]
    [Tooltip("3,2,1 시작 순으로 UI 표시")]
    [SerializeField]
    private Image gameCountImage;

    public int ZombieSpawnPointLength { get { return zombieSpawnPoints.Length; } }
    public int ZombieKindCount { get { return zombiekindcount; } }
    public List<Zombie> ZombieList { get { return zombieList; } }

    private void CreateZombie()
    {
        // GameCount Image Disable
        StartCoroutine(Coroutine_DelayGameCountImageDisable());
        IEnumerator Coroutine_DelayGameCountImageDisable()
        {
            yield return new WaitForSeconds(1f);
            gameCountImage.sprite = null;
            gameCountImage.gameObject.SetActive(false);

            // 시간 , 점수 UI 활성화
            gameTimer.gameObject.SetActive(true);
            timeDurationText.text = timeDuration.ToString();
        }

        // Create Zombie
        for (int i=0; i<4; i++)
            CreateZombie(i , zombieSpawnPoints[i].transform.position , true);
    }

    public void CreateZombie(int _index , Vector3 _position , bool _isAI)
    {
        string zombiepath = $"Zombie/Zombie_{_index}";
        Zombie newzombie = PhotonNetwork.Instantiate(zombiepath, _position, Quaternion.identity).GetComponent<Zombie>();
        SetObjectParent(zombieparent, newzombie.gameObject);

        newzombie.SetIsAI(_isAI);
        newzombie.SetUnit(newzombie.GetComponent<PhotonView>().OwnerActorNr, moveSpeed * 1.5f, this);

        newzombie.SetPlayerList(playerList);
    }
    #region Game Process Event
    protected override void GameObjectCreateEvent()
    {
        // 생성 위치 지정
        float angle = 360f / PhotonNetwork.CurrentRoom.PlayerCount;
        Vector3[] position = new Vector3[PhotonNetwork.CurrentRoom.PlayerCount];
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            position[i] = new Vector3(spawnPointTable.transform.position.x + Mathf.Cos((angle * i) * (Mathf.PI / 180f)) * spawnPointDistance, spawnPointTable.transform.position.y,
               spawnPointTable.transform.position.z + Mathf.Sin((angle * i) * (Mathf.PI / 180f)) * spawnPointDistance);
        }

        // 캐릭터 생성
        ZombiePlayer newplayer = PhotonNetwork.Instantiate(path, position[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.identity).GetComponent<ZombiePlayer>();
        newplayer.gameObject.SetActive(false); // 세팅되기전 캐릭터 비활성화
        SetObjectParent(playersparent, newplayer.gameObject); // 생성된 플레이어는 Players Object 자식으로 넣어준다
        newplayer.SetUnit(PhotonNetwork.LocalPlayer.ActorNumber, speed, this); // 플레이어 세팅
        newplayer.AutoAddScoreEventHandler += (isZombie) => // 이벤트 등록
        {
            // 좀비 상태가 아닌경우 1초마다 10점씩 점수증가
            StartCoroutine(nameof(Coroutine_AutoAddScore));
            IEnumerator Coroutine_AutoAddScore()
            {
                while (!IsGameOver)
                {
                    yield return new WaitForSeconds(1f);

                    if (!isZombie)
                        ScoreManager.Instance.AddScore(newplayer.ActorNumber, 10);
                   
                }
            }
        };
        newplayer.gameObject.SetActive(true); // 세팅이 완료되었으므로 활성화
        if (newplayer.GetIsLocalPlayer())
        {
            localplayer = newplayer;
            newplayer.SetMovePadController(movepadctr);
        }

        // 플레이어 리스트에 추가
        AddPlayer(newplayer);
        Transform localPlayer = Util.GetLocalPlayer();
        cameracontroller.MyLocalPlayerTarget = localPlayer; //로컬 플레이어 카메라 타겟 지정
        SetGameOver(false); // 게임 종료 변수 false로 초기화

        base.GameObjectCreateEvent();
    }
    protected override void GameStartEvent()
    {
        UIPath = "Images/ZombieGame/";
        StartCoroutine(Coroutine_GameStartCount());
        IEnumerator Coroutine_GameStartCount()
        {
            int count = 3;
            while (count > 0)
            {
                // 게임이 시작되기까지 대기
                yield return new WaitUntil(() => !GetIsGameOver());
                gamePlayObject.gameObject.SetActive(true);

                gameCountImage.sprite = Resources.Load<Sprite>(UIPath + $"Loading_{count}");
                gameCountImage.SetNativeSize();

                yield return new WaitForSeconds(1f);
                count--;
            }
            gameCountImage.sprite = Resources.Load<Sprite>(UIPath + "Start"); // 좀비가 생성되는순간 Start Image 비활성화
            gameCountImage.SetNativeSize();

            // 방장이 좀비 생성
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
                CreateZombie();
            Transform myplayer = Util.GetLocalPlayer();// MovePadController Target Player Add
            movepadctr.SetMyPlayer(myplayer.transform, speed);

            // 점수 카드 이벤트 등록
            ScoreManager.Instance.CreateScoreListEventHandler += (_playercount, _prefab, parent, cards) =>
            {
                // 점수 카드 생성
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    GameObject obj = Instantiate(_prefab);

                    obj.transform.SetParent(parent.transform);
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;

                    ScoreCard card = obj.GetComponent<ScoreCard>(); // ScoreCard Connect
                    card.SetCard(0, 1, ScoreManager.Instance.rankSprites[0]);
                }

                cards = parent.GetComponentsInChildren<ScoreCard>();
                ScoreManager.Instance.ShowScoreCardList(false); // 초기 점수 리스트 비활성화
            };
            ScoreManager.Instance.gameObject.SetActive(true); // ScoreManager 오브젝트 활성화

            // PlayerList Teleport ActionEvent Add
            ZombieGameTeleport.Instance.PlayerList = playerList;

            base.GameStartEvent();
        }
    }
    protected override void GamePlayEvent()
    {
        float time = timeDuration + 1;
        StartCoroutine(Coroutine_GameTimer());
        IEnumerator Coroutine_GameTimer()
        {
            // 1초씩 감소
            while (time > 0f)
            {               
                yield return new WaitForSeconds(1f);

                time -= 1f;
                timeDurationText.text = time.ToString();               
            }
            base.GamePlayEvent();            
        }       
    }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        path = "Character/Player_0";
        zombieSpawnPoints = new Transform[zombieSpawnPoint.transform.childCount];
        for (int i = 0; i < zombieSpawnPoint.transform.childCount; i++)
            zombieSpawnPoints[i] = zombieSpawnPoint.transform.GetChild(i);
    }
}
