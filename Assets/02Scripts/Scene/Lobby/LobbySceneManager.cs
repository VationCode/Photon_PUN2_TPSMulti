// 본인의 플레이어리스트를 받아오는 단계 - 생성과 조인할때 일텐데

// 0529 조인한 유저의 플레이어리스트 생성이안됨, 방 나갔을 시 나간 사람 아직 작업x

using System;
using DUS.Scene;
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

    // =========================== 모듈
    private LobbyMenuUIManager m_lobbyMenuUIManager;
    private FindMenuRoomListManager m_findMenuRoomListManager;
    private PlayerListUIManager m_playerListManager;
    private LobbyNetworkManager m_networkManager;

    public event Action InitializeAtStart;

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
        m_findMenuRoomListManager = UICanvas.GetComponent<FindMenuRoomListManager>();
        m_playerListManager = UICanvas.GetComponent<PlayerListUIManager>();

        m_networkManager = GetComponent<LobbyNetworkManager>();
    }

    private void Start()
    {
        InitializeAtStart?.Invoke();
    }
    public void StartInGame(int sceneNum)
    {
        m_networkManager.SaveInfo();
        SaveNetworkInfo.Instance.SaveInfo(m_networkManager.m_PlayerName, m_networkManager.m_CurrentRoomName, m_networkManager.m_CurrentJoinPlayers);
        SceneLoadManager.Instance.PushNextInfoToBootSceneAndLoadBootScene(SceneType.InGame);
    }

    // 각 버튼들 OnClick에 전달
    public void GoMenuUI(GoMenuUIType mainMenuType)
    {
        m_lobbyMenuUIManager.GoMenuUI(mainMenuType);
    }

    // 방 생성 시
    public void OnRoomCreate()
    {
        string roomName = m_lobbyMenuUIManager.GetInputFieldRoomName();
        string playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();
        m_lobbyMenuUIManager.SetInputFieldPlayerName(playerName);

        m_networkManager.OnRoomCreate(roomName, playerName);

        // 방 생성 실패시
        if (!m_networkManager.m_isRoomCreated)
        {
            return;
            //FailedRoomCrate();
        }
        
        m_playerListManager.OnRoomCreate(playerName);
        m_findMenuRoomListManager.OnRoomCreate(roomName);
    }

    // 내가 들어간 상황
    public void OnJoinRoom(string roomName)
    {
        string playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();
        m_lobbyMenuUIManager.SetRoomMenuNameText(roomName);
        m_networkManager.OnJoinRoom(roomName, playerName);
    }

    // 방에 접속된 플레이어들(나도 포함이지만 실직적으로는 )
    public void OnJoinedRoom(string playerName)
    {
        m_playerListManager.OnJoinedRoom(playerName);
    }

    public void OnRoomLeave()
    {
        m_networkManager.OnRoomLeave();

        m_findMenuRoomListManager.OnRoomLeave();
        m_playerListManager.OnRoomLeave();

        m_lobbyMenuUIManager.GoMenuUI(GoMenuUIType.MainMenuUI);
    }

    public void OnPlayerLeftRoom(string leftName)
    {
        string leftPlayerName = leftName;
        m_playerListManager.OnPlayerLeftRoom(leftPlayerName);
    }

    // 제거와 생성
    public void OnRoomListUpdate(bool isRemove, string roomName)
    {
        m_findMenuRoomListManager.OnRoomUpdateList(isRemove, roomName);
    }

    public void OnPlayerEnterted(string newplayerName)
    {
        m_playerListManager.EntertedPlayer(newplayerName);
    }

    // 플레이어의 닉네임 결정 필요
    // 닉네임 부여되는 시기 결정(방 생성 클릭 시, 조인 버튼 클릭 시)
    /*public void OnRoomCreate(string roomName)
    {
        if (roomName == "" || string.IsNullOrEmpty(roomName))
        {
            roomName = "RoomMenuUI" + UnityEngine.Random.Range(1000, 9999).ToString(); // 방 이름이 없을 경우 랜덤 숫자 생성
        }

        LobbyMenuUIManager.Instance.OnRoomCreate(roomName);

        string playerNickName = LobbyMenuUIManager.Instance.GetPlayerNickNameTextEnterRoom();

        LobbyNetworkManager.Instance.OnRoomCreate(roomName, playerNickName);

    }


    // 본인이 방 나갔을 때
    public void OnRoomLeave()
    {
        LobbyMenuUIManager.Instance.OnRoomLeave(PhotonNetwork.CurrentRoom.roomName);
        LobbyNetworkManager.Instance.OnRoomLeave(PhotonNetwork.CurrentRoom.roomName);
    }

    // 상대방이 방을 떠났을 때 호출
    public void OnPlayerLeftedRoom(string leftPlayerName)
    {
        LobbyMenuUIManager.Instance.OnPlayerListItemRemove(leftPlayerName);
    }
    // LobbyNetworkManager에서 방 생성 혹은 입장시 사용할 메서드
    public void AddPlayerListWhenJoined(string createPlayerList)
    {
        LobbyMenuUIManager.Instance.OnPlayerListItemCreate(createPlayerList);
    }

    public void OnRoomRemove(string removeRoomName)
    {
        LobbyMenuUIManager.Instance.OnRoomRemove(removeRoomName);
    }

    public void FindRoomListCreate(string roomName)
    {
        LobbyMenuUIManager.Instance.HandleFindMenuRoomList(roomName);
    }

    public void OnJoinRoom(string roomName)
    {
        LobbyMenuUIManager.Instance.OnJoinRoom(roomName);

        string playerName = LobbyMenuUIManager.Instance.GetPlayerNickNameTextEnterRoom();
        LobbyNetworkManager.Instance.OnJoinRoom(roomName, playerName);
    }

    // LobbyNetworkManager에서 새로운 플레이어 입장 확인 시
    */
}