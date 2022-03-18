using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGameTeleport : SingtonMonoBehaviour<ZombieGameTeleport>
{
    [Header("Teleport Transform")]
    [SerializeField]
    [Tooltip("")]private Transform[] teleports;
    [Tooltip("텔레포트를 타면 지정한 위치로 이동/Teleport의 Children Object를 string 형태로 검색")] public Vector3[] endpos;

    private List<ZombiePlayer> playerList;
    public List<ZombiePlayer> PlayerList
    {
        get
        {
            return playerList;
        }
        set
        {
            playerList = value;
            foreach(ZombiePlayer player in playerList)
            {
                player.TeleportEventHandler += (player,index) =>
                {
                    if(!player.IsZombie)
                        player.transform.position = endpos[index];
                };
            }
        }
    }
    protected override void OnStart()
    {
        endpos = new Vector3[teleports.Length];
        for(int i=0; i< teleports.Length; i++)
        {
            endpos[i] = teleports[i].Find("EndPosition").position;
            Debug.Log($"Teleport_{i} EndPosition : {endpos}");
        }
    }
}
