using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected int Level { get; set; }
    protected int Hp { get; set; }

    protected int Index { get; set; }

    protected SpriteRenderer spriteRenderer;

    protected Rigidbody2D Rb;

    protected Animator animtor;

    public virtual void SetUnit(int level , int hp , int index)
    {
        Level = level;
        Hp = hp;
        Index = index;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animtor = GetComponent<Animator>();
    }

    protected void PlayBoolAnimation(string _name , bool _istrue)
    {
        animtor.SetBool(_name, _istrue);
    }
    
    protected void PlayTriggerAnimation(string _name)
    {
        animtor.SetTrigger(_name);
    }

    protected bool IsCurrentAnimation(string _name)
    {
        if (animtor.GetCurrentAnimatorStateInfo(0).IsName(_name))
        {
            return true;
        }
        return false;
    }

    public abstract void OnDmage(int _damage);

    public abstract void OnDie(Action<int> _action);
}
