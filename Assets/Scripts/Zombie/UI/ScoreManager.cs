﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

using System.Linq;

using Photon.Pun;
using Photon.Realtime;
using System;

public class ScoreManager : SingtonMonoBehaviour<ScoreManager>
{
    [SerializeField]
    private ZombieManager gamemanager;

    public GameObject scorecardobj;

    public Sprite[] rankSprites;

    [Header("점수")]
    [SerializeField]
    private GameObject scoreListParent;
    private ScoreCard[] scorecards;
    [SerializeField]
    private GameObject myScore;
    [SerializeField]
    private Text myScoreText;

    // EventHandler
    public event Action<int , GameObject , GameObject , ScoreCard[]> CreateScoreListEventHandler;
    public event Action<ScoreCard[]> AllScoreCardRemoveEventHandler;

    public void ShowScoreCardList(bool _isShow)
    {
        for (int i = 0; i < scorecards.Length; i++)
            scorecards[i].gameObject.SetActive(_isShow);
    }

    public void ShowScoreUI(int _actornumber, int _score , int _rank)
    {
        // 점수가 오른 플레이어 점수 리스트 UI에 적용
        ZombiePlayer player = gamemanager.PlayerList.Find(p => p.GetComponent<PhotonView>().OwnerActorNr == _actornumber);

        for(int i=0; i< scorecards.Length; i++)
        {
            string[] split = scorecards[i].transform.name.Split('_'); // ex) ScoreCard_2
            int actornumber = int.Parse(split[1]);
            if(_actornumber == actornumber)
            {
                scorecards[i].SetCard(_score, _rank ,_rank <= 3 ? rankSprites[_rank-1] : null);
            }
        }

        // LocalPlayer 점수만 UI에 반영(오른쪽 상단 점수)
        if (player.GetIsLocalPlayer())
            myScoreText.text = $"{_score}점";
    }

    private void OnEnable()
    {
        CreateScoreListEventHandler?.Invoke(PhotonNetwork.CurrentRoom.PlayerCount , scorecardobj , scoreListParent , scorecards);
    }
    private void OnDisable()
    {
        AllScoreCardRemoveEventHandler?.Invoke(scorecards);
    }
}