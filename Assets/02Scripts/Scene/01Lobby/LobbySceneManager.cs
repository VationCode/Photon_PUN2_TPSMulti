using System;
using System.Collections.Generic;
using DUS.AssetLoad;
using DUS.Network;
using DUS.scene;
using DUS.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour
{
    public static LobbySceneManager Instance { get; private set; } // 싱글톤 인스턴스

    [SerializeField]
    GameObject UICanvas;

    public NextSceneRequireData m_nextSceneRequireData;
    // =========================== 모듈
    private LobbyMenuUIManager m_lobbyMenuUIManager;
    private RoomListManager m_roomListManager;
    private PlayerListUIManager m_playerListManager;

    public event Action InitializeAtStart;
    
    private INetworkService m_networkService;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject); // 중복 인스턴스 방지
            return;
        }

        m_lobbyMenuUIManager = UICanvas.GetComponent<LobbyMenuUIManager>();
        m_roomListManager = UICanvas.GetComponent<RoomListManager>();
        m_playerListManager = UICanvas.GetComponent<PlayerListUIManager>();

        m_networkService = NetworkManager.Service;
    }


    private void Start()
    {
        InitializeAtStart?.Invoke();

        m_networkService.UpdateRoomList += UpdateFindMenuRoomList;
        m_networkService.JoinedPlayerList += JoinedPlayerList;
        m_networkService.EntertedPlayer += EntertedPlayer;
        m_networkService.LeftPlayer += LeftPlayer;
        m_networkService.ActivateStartInGame += ActivateStartInGame;
    }
    public void StartInGame(int sceneNum)
    {
        SceneLoadManager.Instance.LoadNextScene_Boot(SceneType.InGame, m_nextSceneRequireData);
    }

    #region ======================================================================= Button 클릭
    // 메뉴 이동
    public void GoMenuUI(GoMenuUIType mainMenuType)
    {
        m_lobbyMenuUIManager.GoMenuUI(mainMenuType);
    }

    // 방 생성
    // TODO : 방제 중복 생성 방지 필요
    public void CreateRoom()
    {
        // 1. 방 이름과 내 닉네임 가져오기
        string _roomName = m_lobbyMenuUIManager.GetInputFieldRoomName();
        string _playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();

        // 2. 논리적인 네트워크 방 생성(같은 방 있을 경우 만들어지지 않음)
        m_networkService.CreateRoom(_roomName, _playerName);

        // 3. 방 생성 되었는지 체크. 안되었을 시 3초정도에도 안만들어지면 로그
        float time = 0;
        while (!m_networkService.CheckCreateRoom())
        {
            time += Time.deltaTime;
            if(time >= 3)
            {
                Debug.LogError("CreateRoomFailed");
            }
            m_networkService.CreateRoom(_roomName, _playerName);
        }

        // 4 룸 메뉴UI로 이동
        GoMenuUI(GoMenuUIType.RoomMenuUI);

        // 5. 룸 메뉴 입장 후 설정
        // 5.1 룸 메뉴의 방 이름 설정 위에서 이름 받아올 때 없으면 랜덤 처리로 자동 완료        
        // 5.2 플레이어 리스트 생성은 네트워크 매니저의 JoinedPlayerList를 통해 자동 생성

        // 6 FindMenu에 방 조인 버튼 생성
        m_roomListManager.CreateJoinRoomBtn(_roomName);

        // 7 인게임스타트 버튼 활성화
        ActivateStartInGame(true);  //StartBtn은 방장만 켜지도록
    }

    public void ActivateStartInGame(bool isActivate)
    {
        m_lobbyMenuUIManager.ActivateStartInGameBtn(isActivate);
    }
    // 방 조인
    public void OnJoinRoom(string roomName)
    {
        // 1. Join할 방 가져오고, 플레이어 이름
        string _roomName = roomName;
        string _playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();

        // 2. 네트워크 연결
        m_networkService.JoinRoom(_roomName, _playerName);

        // 3. 방 이름 설정
        m_lobbyMenuUIManager.SetRoomMenuRoomName(_roomName);

        ActivateStartInGame(false);
    }

    public void JoinLobby()
    {
        m_networkService.JoinLobby();
    }

    public void LeaveRoom()
    {
        // TODO : 플레이어 리스트들 전부 초기화필요

        // 1. 플레이어 리스트 제거
        m_playerListManager.AllRemove();

        m_networkService.LeaveRoom();
    }
    #endregion ======================================================================= /Button 클릭

    // FindMenu 방 리스트 업데이트
    public void UpdateFindMenuRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) m_roomListManager.RemoveRoomList(roomInfo.Name);
            else m_roomListManager.CreateJoinRoomBtn(roomInfo.Name);
        }
    }

    // CreateRoom + JoinRoom 시 호출 (나한테만 적용, 방에 있던 사람은 내가들어온지 모르니 나에 대해 상대방한테 알려줘야한다 EntertedPlayer)
    public void JoinedPlayerList(Player[] players)
    {
        m_playerListManager.CreatePlayerListItem(players);
    }

    // 내가 방에있고 새로 들어온 플레이어 인식
    public void EntertedPlayer(Player entertedPlayer)
    {
        m_playerListManager.AddPlayerListItem(entertedPlayer);
    }

    // Leave로 떠난 나에 대해 남아있는 유저들에게 떠난 나의 정보 전달 
    public void LeftPlayer(Player player)
    {
        m_playerListManager.RemovePlayer(player);
    }
}