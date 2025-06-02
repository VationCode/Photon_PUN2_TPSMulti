using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    /*[SerializeField] LobbySceneManager m_lobbySceneManager;

    [Header("[Get RoomMenu]")]
    [SerializeField] Button m_startBtn;

    int m_gameVersion = 1;
    public bool m_isRoomCreated { get; private set; } = false; // 방 생성 여부, 방 나갈 때 초기화 하기

    public string m_PlayerName { get; private set; }
    public string m_CurrentRoomName { get; private set; }
    public Player[] m_CurrentJoinPlayers { get; private set; }

    private void Awake()
    {
        m_lobbySceneManager = GetComponent<LobbySceneManager>();
        InitializeAtStart();
    }

    private void Start()
    {
        //LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
    }

    public void InitializeAtStart()
    {
        // 게임 버전 설정
        PhotonNetwork.GameVersion = m_gameVersion.ToString();
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 서버에 연결되었을 때 호출되는 메서드
    *//*public override void OnConnectedToMaster()
    {
        // 부트씬에서 처리하기로
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true; // 마스터 클라이언트가 씬을 변경하면 다른 클라이언트들도 자동으로 동기화됨
    }*//*

    // 서버와 연결이 끊어졌을 때 호출되는 메서드
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Server: " + cause.ToString());
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnRoomCreate(string roomName, string playerName)
    {
        PhotonNetwork.CreateRoom(roomName);
        PhotonNetwork.NickName = playerName;
        
        m_isRoomCreated = true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
        m_isRoomCreated = false;
    }

    public bool CheckRoomCreated(string name)
    {
        return m_isRoomCreated; // 방 생성 성공 여부 반환
    }

    // 방 생성 혹은 JoinLobby 시 호출
    public override void OnJoinedRoom()
    {
        // 현재 방에 접속해 있는 본인 포함 모든 플레이어들
        // 생성시에는 이미 리스트에 들어가 있기에 다시 생성하진 않음
        Player[] players = PhotonNetwork.PlayerList;
        
        if(players.Length != 0 || players != null)
        {
            foreach (Player player in players)
            {
                m_lobbySceneManager.OnJoinedRoom(player.NickName);
            }
        }

        // 방 이름 받아오기
        m_startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnJoinRoom(string roomName, string playerName)
    {
        PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.NickName = playerName;
    }

    // 로비에서 방 목록이 업데이트되었을 때 호출되는 메서드
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            // 제거된 상태인지
            m_lobbySceneManager.OnRoomListUpdate(roomInfo.RemovedFromList, roomInfo.Name);
        }
    }

    // 플레이어가 방에 입장했을 때 상대방만 호출
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 들어온 플레이어정보 상대방에게 전달
        m_lobbySceneManager.OnPlayerEnterted(newPlayer.NickName);
    }

    public void OnRoomLeave()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
        }
    }

    // 상대방이 방을 떠났을 때 호출
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        m_lobbySceneManager.OnPlayerLeftRoom(otherPlayer.NickName);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        m_startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void SaveInfo()
    {
        m_PlayerName = PhotonNetwork.NickName;
        m_CurrentRoomName = PhotonNetwork.CurrentRoom.Name;
        m_CurrentJoinPlayers = PhotonNetwork.PlayerList;
    }*/
}
