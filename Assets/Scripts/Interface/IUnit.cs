using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    int ActorNumber { get; set; }
    float Speed { get; set; }

    bool GetIsLocalPlayer();
}
