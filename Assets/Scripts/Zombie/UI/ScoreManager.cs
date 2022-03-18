using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

using System.Linq;

using Photon.Pun;
using Photon.Realtime;
using System;

public class ScoreManager : SingtonMonoBehaviour<ScoreManager>
{
    private PhotonView pv;

    [Header("ZombieGameManager")]
    [SerializeField]
    private ZombieManager gamemanager;

    public Sprite[] rankSprites;

    [Header("점수관련오브젝트들")]
    [SerializeField]
    private GameObject scoreListParent, scorecardobj;
    private ScoreCard[] scorecards;
    [SerializeField]
    private GameObject myScore;
    [SerializeField]
    private Text myScoreText;

    // EventHandler
    public event Action<int,GameObject,GameObject,ScoreCard[]> CreateScoreListEventHandler;
    public event Action<ScoreCard[]> AllScoreCardRemoveEventHandler;

    public void ShowScoreCardList(bool _isShow)
    {
        for (int i = 0; i < scorecards.Length; i++)
            scorecards[i].gameObject.SetActive(_isShow);
    }

    public void AddScore(int _actornumber, int _amount)
    {
        pv.RPC(nameof(TargetPlayerAddScore), RpcTarget.All, _actornumber, _amount); // 점수 동기화
    }

    public void TargetScoreCardColorChange(int _actornumber , Color _color)
    {
        for (int i = 0; i < scorecards.Length; i++)
        {
            int actornumber = Util.GetActorNumber(scorecards[i].transform.name);

            if (_actornumber == actornumber)
                scorecards[i].GetComponent<Image>().color = _color;
        }
    }
    #region RPC
    [PunRPC]
    private void TargetPlayerAddScore(int _actornumber,int _amount)
    {
        ZombiePlayer player = gamemanager.PlayerList.Find(p => _actornumber == p.GetComponent<PhotonView>().OwnerActorNr);
        player.AddScore(_amount);

        // LocalPlayer 점수만 UI에 반영(오른쪽 상단 점수)
        if (player.GetIsLocalPlayer())
            myScoreText.text = $"{player.Score}점";

        //// 전체 플레이어 점수를 비교해 랭킹 계산
        //for (int i = 0; i < scorecards.Length; i++)
        //{
        //    string[] split = scorecards[i].transform.name.Split('_'); // ex) ScoreCard_2
        //    int scorecardnumber = int.Parse(split[1]);
        //    if (_actornumber == scorecardnumber)
        //        scorecards[i].SetCard(player.Score,_rank, _rank <= 3 ? rankSprites[_rank - 1] : null);
        //}       
    }
    #endregion
    protected override void OnAwake()
    {
        pv = photonView;
    }
    private void OnEnable()
    {
        // 모든 플레이어 점수 카드 생성
        CreateScoreListEventHandler?.Invoke(PhotonNetwork.CurrentRoom.PlayerCount,scorecardobj,scoreListParent,scorecards);
    }
    private void OnDisable()
    {
        // 모든 플레이어 점수 카드 삭제(재활용)
        AllScoreCardRemoveEventHandler?.Invoke(scorecards);
    }
}
