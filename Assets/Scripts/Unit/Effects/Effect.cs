using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    protected Animator animator;

    protected string key;

    protected float animtioncliplength;

    protected IEnumerator Coroutine_EffectDisable()
    {
        yield return new WaitForSeconds(animtioncliplength);
        ObjectPool<Effect>.RemoveObject(key, gameObject);
    }

    public virtual void SetEffect(string _key)
    {
        animator = GetComponent<Animator>();

        key = _key;

        animtioncliplength = animator.runtimeAnimatorController.animationClips[0].length;

        StartCoroutine(Coroutine_EffectDisable());
    }
}
