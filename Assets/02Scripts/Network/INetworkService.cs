using NUnit.Framework;
using NUnit.Framework.Constraints;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkService
{
    // ========================= BootScene
    void ConnectNetwork();
    bool CheckConnected();

    // ======================== °ø¿ë
    void JoinLobby();

    // ======================== Lobby
    void CreateRoom(string roomName, string playerName);
    bool CheckCreateRoom();

    event Action<List<RoomInfo>> UpdateRoomList;

    event Action<Player[]> JoinedPlayerList;
    // ======================== InGame
}
