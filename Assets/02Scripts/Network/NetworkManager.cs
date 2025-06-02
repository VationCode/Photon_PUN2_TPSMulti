using Photon.Pun;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace DUS.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks, INetworkService
    {
        public static NetworkManager Instance { get; private set; }
        public static INetworkService Service => Instance; //해당 서비스에 있는 함수들을 전달하는 방식으로 결합도 낮춤 싱글톤 직접 접근이 아닌

        public event Action<List<RoomInfo>> UpdateRoomList;
        public event Action<Player[]> JoinedPlayerList;
        // 본인 정보
        public Player m_player { get; private set; }

        private bool m_isConnected;
        private bool m_isCreatedRoom;

        private List<string> m_roomList = new List<string>();
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #region ====================================================== BootScene에서 첫 시작시
        public void ConnectNetwork()
        {
            if (PhotonNetwork.IsConnected) return;
            PhotonNetwork.AutomaticallySyncScene = true; // 마스터 클라이언트가 씬을 변경하면 다른 클라이언트들도 자동으로 동기화됨
            PhotonNetwork.ConnectUsingSettings();
        }

        public bool CheckConnected() => m_isConnected;

        public override void OnConnected()
        {
            Debug.Log("OnConnected");
            m_isConnected = true;
        }

        public override void OnConnectedToMaster()
        {
            m_player = PhotonNetwork.LocalPlayer;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            PhotonNetwork.ConnectUsingSettings();
            m_isConnected = false;
        }
        #endregion ====================================================== /BootScene에서 첫 시작시

        public void JoinLobby()
        {
            if(PhotonNetwork.InRoom)
            {
                Debug.Log("InRoom && LeaveRoom");
                PhotonNetwork.LeaveRoom();
            }
            PhotonNetwork.JoinLobby();
            Debug.Log("JoinLobby");
        }

        #region ====================================================== LobbyScene에서 시작
        // 방 생성
        public void CreateRoom(string roomName, string playerName)
        {
            Debug.Log("CreateRoom");
            PhotonNetwork.CreateRoom(roomName);
            m_player.NickName = playerName;
            m_isCreatedRoom = true;
        }

        public bool CheckCreateRoom() => m_isCreatedRoom;

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            m_isCreatedRoom = false;
        }

        // 방 조인

        // 방 떠났을 때

        // 방 플레이어들 관리

        // 방 목록 관리, 방 리스트가 업데이트될 때 마다 불려지는 포톤 함수
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
            UpdateRoomList?.Invoke(roomList);
        }

        public void JoinRoom(string roomNmae)
        {
            PhotonNetwork.JoinRoom(roomNmae);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoomNewPlayer");
            Player[] players = PhotonNetwork.PlayerList;

            JoinedPlayerList?.Invoke(players);
        }

        #endregion ====================================================== /LobbyScene에서 시작

    }
}
