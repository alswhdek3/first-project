using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    protected Animator animtor;

    public abstract void CreateObject(GameObject _object , Action<float> _callback);
}
