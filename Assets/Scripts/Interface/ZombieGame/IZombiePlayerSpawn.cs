using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZombiePlayerSpawn
{
    void Enable(Vector3 _position, ICamera camera);

    /// <summary>
    /// 이전에 스폰된 이력이 있는지 확인
    /// 이전에 스폰된 이력이있으면 새로 생성하지않고 기존 리스트에서 활성화
    /// </summary>
    /// <returns></returns>
    bool GetIsPrevSpawn();
}
