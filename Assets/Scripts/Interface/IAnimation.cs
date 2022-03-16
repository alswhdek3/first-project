using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimation
{
    Dictionary<string, float> AnimationLengthTable { get; set; }

    float GetAnimationLength(string _animationclipname);
}
