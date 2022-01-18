using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected int Level { get; set; }
    protected int Hp { get; set; }

    protected SpriteRenderer spriteRenderer;

    protected Rigidbody2D Rb;

    protected Animator animtor;

    public virtual void SetUnit(int level , int hp)
    {
        Level = level;
        Hp = hp;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animtor = GetComponent<Animator>();
    }
    public abstract void OnDmage(int _damage);

    public abstract void OnDie(Action<int> _action);

    protected abstract void AnimEvent_Die();
}
