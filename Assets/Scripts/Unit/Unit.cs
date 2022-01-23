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

    protected BoxCollider2D boxCollider;

    protected Canvas canvas;

    protected Transform damageTextDistance;

    public virtual void SetUnit(int level , int hp , int index)
    {
        Level = level;
        Hp = hp;
        Index = index;
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        damageTextDistance = transform.GetChild(1).GetComponent<Transform>();

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

    public virtual void OnDamge(int _damage)
    {
        if (_damage < 0)
        {
            Debug.LogError($"{_damage}은 잘못된 데미지 수치 입니다");
            return;
        }
        // DamageText UI
        DamageText dmgtext = UIManager.Instance.GetCreateDamageText();
        dmgtext.gameObject.SetActive(true);
        dmgtext.transform.SetParent(canvas.transform);
        dmgtext.transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + (boxCollider.bounds.max.y + 0.1f));
        dmgtext.SetText(_damage, damageTextDistance);

        Hp -= _damage;
    }

    public abstract void OnDie(Action<int> _action);
}
