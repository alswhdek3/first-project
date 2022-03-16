using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerList<T> where T : Component
{
    void AddPlayer(T _newplayer);
    void RemovePlayer(T _player);
    List<T> GetPlayerList();
    T GetLocalPlayer();
    T GetTargetPlayer(int _actornumber);
}
