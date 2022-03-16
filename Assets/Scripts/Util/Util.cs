using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;

public class Util : MonoBehaviourPun 
{
    public static Transform GetLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
                return player.transform;
        }
        return null;
    }

    public static Transform GetTargetPlayer<T>(List<T> _playerList , int _actorNumber) where T : Component
    {
        foreach(T player in _playerList)
        {
            if(_actorNumber == player.GetComponent<PhotonView>().OwnerActorNr)
                return player.transform;
        }
        return null;
    }
}
