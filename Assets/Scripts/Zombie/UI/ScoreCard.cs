using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCard : BaseCard
{
    public override void SetCard(int _score, int _rank, Sprite _rankerSprite = null)
    {
        score = _score;
        rank = _rank;

        // 3위까지 왕관 UI 활성화
        if(_rankerSprite != null)
            rankImage.sprite = _rankerSprite;
    }
}
