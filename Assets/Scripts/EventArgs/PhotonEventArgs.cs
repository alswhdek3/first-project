using System.Collections;
using System.Collections.Generic;

using System;

using UnityEngine;


public delegate void PhotonServerEventHandler(object _sender, string _verstion , bool _isServerConnect);
public delegate void PhotonRoomEventHandler(object _sender, PhotonEventArgs _e);
public class PhotonEventArgs : EventArgs
{
    public PhotonEventArgs(string _version , string _roomname , int _playercount , bool _isServerConnect)
    {
        Version = _version;
        RoomName = _roomname;
        CurrentRoomPlayerCount = _playercount;
        IsServerConnect = _isServerConnect;
    }
    public string Version
    {
        get;
        private set;
    }

    public bool IsServerConnect
    {
        get;
        private set;
    }
    
    public string RoomName
    {
        get;
        private set;
    }
    public int CurrentRoomPlayerCount
    {
        get;
        private set;
    }
}
