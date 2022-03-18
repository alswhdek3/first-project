using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZombiePlayerSpawn
{
    void Enable(Vector3 _position, ICamera camera);
    bool GetIsPrevSpawn();
}
