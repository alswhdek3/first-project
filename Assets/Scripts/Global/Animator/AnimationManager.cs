using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public float GetAnimationClipLength(Animator _animtor)
    {
        return _animtor.GetCurrentAnimatorStateInfo(0).length;
    }

    public bool IsCurrentAnimation(Animator _animtor , string _name)
    {
        if(_animtor.GetCurrentAnimatorStateInfo(0).IsName(_name))
        {
            return true;
        }
        return false;
    }

    public void PlayBoolAnimation(Animator _animtor , string _parameter , bool _istrue)
    {
        _animtor.SetBool(_parameter, _istrue);
    }

    public void PlayTriggerAnimation(Animator _animtor, string _parameter)
    {
        _animtor.SetTrigger(_parameter);
    }
}
