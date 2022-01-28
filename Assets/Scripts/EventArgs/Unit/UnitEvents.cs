using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;
using UnityEngine;

public class UnitEvents : MonoBehaviour
{
    [System.Serializable]
    public class UnitAttackEvent : UnityEvent<int, float> { }
}
