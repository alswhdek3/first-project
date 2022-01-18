using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : Singleton<AnimatorManager>
{
    public float GetAnimationLength(AnimationClip _clip)
    {
        return _clip.length;
    }

    public bool IsPlayAnimation(Animator _animator , string _clipname)
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName(_clipname))
        {
            return true;
        }
        return false;
    }

    public void PlaySetBool(Animator _animator, string _clipname, bool _is)
    {
        _animator.SetBool(_clipname, _is);
    }

    public void PlaySetTrigger(Animator _animator, string _clipname)
    {
        _animator.SetTrigger(_clipname);
    }
}
