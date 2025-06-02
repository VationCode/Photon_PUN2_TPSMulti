// 본인의 플레이어리스트를 받아오는 단계 - 생성과 조인할때 일텐데

// 0529 조인한 유저의 플레이어리스트 생성이안됨, 방 나갔을 시 나간 사람 아직 작업x

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
    private FindMenuRoomListManager m_findMenuRoomListManager;
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
        m_findMenuRoomListManager = UICanvas.GetComponent<FindMenuRoomListManager>();
        m_playerListManager = UICanvas.GetComponent<PlayerListUIManager>();

        m_networkService = NetworkManager.Service;
    }


    private void Start()
    {
        InitializeAtStart?.Invoke();

        m_networkService.UpdateRoomList += UpdateFindMenuRoomList;
        m_networkService.JoinedPlayerList += JoinedPlayerList;
    }
    public void StartInGame(int sceneNum)
    {
        /*m_networkManager.SaveInfo();
        SaveNetworkInfo.Instance.SaveInfo(m_networkManager.m_PlayerName, m_networkManager.m_CurrentRoomName, m_networkManager.m_CurrentJoinPlayers);*/
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
        string roomName = m_lobbyMenuUIManager.GetInputFieldRoomName();
        string playerName = m_lobbyMenuUIManager.GetInputFieldPlayerName();

        // 2. 논리적인 네트워크 방 생성
        m_networkService.CreateRoom(roomName, playerName);

        // 3. 방 생성 되었는지 체크. 안되었을 시 3초정도에도 안만들어지면 로그
        float time = 0;
        while (!m_networkService.CheckCreateRoom())
        {
            time += Time.deltaTime;
            if(time >= 3)
            {
                Debug.LogError("CreateRoomFailed");
            }
            m_networkService.CreateRoom(roomName, playerName);
        }

        // 4 룸 메뉴UI로 이동
        GoMenuUI(GoMenuUIType.RoomMenuUI);

        // 5. 룸 메뉴 입장 후 설정
        // 5.1 룸 메뉴의 방 이름 설정 위에서 이름 받아올 때 없으면 랜덤 처리로 자동 완료
        // 5.2 플레이어 리스트 생성은 네트워크 매니저의 JoinedPlayerList를 통해 자동 생성

        // 5.3 인게임스타트 버튼 활성화
        m_lobbyMenuUIManager.ActivateStartInGameBtn(true);  //StartBtn은 방장만 켜지도록
    }

    // 방 조인



    // 방 나가기

    #endregion ======================================================================= /Button 클릭

    // FindMenu 방 리스트 업데이트
    public void UpdateFindMenuRoomList(List<RoomInfo> roomList)
    {
        m_findMenuRoomListManager.UpdateFindRoomMenuButtonList(roomList);
    }

    // 내가 방 만들었을 때 + 남이 들어왔을 때
    public void JoinedPlayerList(Player[] players)
    {
        m_playerListManager.CreatePlayerListItem(players);
    }

    // 내가 들어간 상황
    public void OnJoinRoom(string roomName)
    {
    }

    public void JoinLobby()
    {
        // TODO : 플레이어 리스트들 전부 초기화필요
        m_playerListManager.AllRemove();
        m_networkService.JoinLobby();
    }


}