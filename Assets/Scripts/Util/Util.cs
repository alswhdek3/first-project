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
    public static int GetActorNumber(string _name)
    {
        string[] split = _name.Split('_');
        return int.Parse(split[1]);
    }
}
