using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

public abstract class BaseCard : MonoBehaviour
{
    protected int score;
    protected int rank;

    protected Image rankImage;
    protected Sprite rankerSprite; // 1~3위 까지 이미지

    public abstract void SetCard(int _score, int _rank, Sprite _rankerSprite = null);

    protected virtual void Start()
    {
        rankImage = GetComponent<Image>();
    }
}
