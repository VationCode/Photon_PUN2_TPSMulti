using Photon.Pun;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine.XR;
using System.Collections;

namespace DUS.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks, INetworkService
    {
        public static NetworkManager Instance { get; private set; }
        public static INetworkService Service => Instance; //해당 서비스에 있는 함수들을 전달하는 방식으로 결합도 낮춤 싱글톤 직접 접근이 아닌

        public event Action<List<RoomInfo>> UpdateRoomList;
        public event Action<Player[]> JoinedPlayerList;
        public event Action<Player> EntertedPlayer;
        public event Action<Player> LeftPlayer;

        public event Action<bool> ActivateStartInGame;
        // 본인 정보
        public Player m_player { get; private set; }

        private bool m_isConnected;
        private bool m_isCreatedRoom;

        private void Awake()
        {
            if (Instance != null) 
            { 
                Destroy(gameObject); 
                return; 
            }
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
            Debug.Log("OnConnectedToMaster" + PhotonNetwork.IsConnectedAndReady);
            m_player = PhotonNetwork.LocalPlayer;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            //PhotonNetwork.ReconnectAndRejoin();
            //PhotonNetwork.ConnectUsingSettings();
            m_isConnected = false;
        }
        #endregion ====================================================== /BootScene에서 첫 시작시

        // 비동기 처리이기에 LeaveRoom과 JoinLobby룸 동시 처리시 로비로의 입장이 적용안될 수도 있음
        public void JoinLobby()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogWarning("JoinLobby");
                PhotonNetwork.JoinLobby();
            }
            else
            {
                Debug.LogWarning("Photon not ready. Cannot join lobby.");
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("로비 입장 성공");
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
        public void JoinRoom(string roomNmae, string playerName)
        {
            Debug.Log("JoinRoom");
            m_player.NickName = playerName;
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinRoom(roomNmae);
        }

        /*private void Update()
        {
            Debug.Log(PhotonNetwork.IsConnectedAndReady);
        }*/
        // 방 플레이어들 관리
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            Player[] players = PhotonNetwork.PlayerList;

            JoinedPlayerList?.Invoke(players);
        }
        // 방에 들어온 사람에 대한 기존 방에 있던 사람들의 처리
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("EntertedPlayer");
            EntertedPlayer?.Invoke(newPlayer);
        }

        // 상대방이 나갔을 때 호출
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom");
            LeftPlayer?.Invoke(otherPlayer);
        }

        // 방 목록 관리, 방 리스트가 변화가 있으면 호출(변화가 있으면에 주의)
        // 방을 나가서 로비로 돌아왔을 때 방 리스트 변화 없으면 불려지지 않음(즉, 방에 있을땐 업데이트가 이루어지지 않음)
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
            UpdateRoomList?.Invoke(roomList);
        }

        public void LeaveRoom()
        {
            // 방->로비일경우
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("LeaveRoom");
                PhotonNetwork.LeaveRoom();
            }
        }

        // StartCoroutine 사용하여 로비로 가는 시간 주는 이유
        // 비동기 처리이기에 LeaveRoom과 JoinLobby룸 동시 처리시 로비로의 입장이 적용안될 수도 있음
        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
            StartCoroutine(SafeJoinLobby());
        }

        private IEnumerator SafeJoinLobby()
        {
            yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
            Debug.Log("JoinLobby 준비 상태 완료");
            PhotonNetwork.JoinLobby();
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            ActivateStartInGame(PhotonNetwork.IsMasterClient);
        }
        #endregion ====================================================== /LobbyScene에서 시작

        #region ====================================================== InGameScene에서 시작



        #endregion ====================================================== /InGameScene에서 시작

    }
}
