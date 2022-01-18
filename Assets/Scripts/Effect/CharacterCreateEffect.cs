using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreateEffect : BaseEffect
{
    public override void CreateObject(GameObject _object, Action<float> _callback)
    {
        _object.gameObject.SetActive(false);
        float cliplength = animtor.GetCurrentAnimatorClipInfo(0).Length;

        _callback?.Invoke(cliplength);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animtor = GetComponent<Animator>();    
    }
}
