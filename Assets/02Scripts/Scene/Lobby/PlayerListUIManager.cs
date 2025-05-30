using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DUS.UI
{
    public class PlayerListUIManager : MonoBehaviour, IInitializeAtStart
    {
        [SerializeField]
        GameObject m_playerListItemPrefab;

        [Header("[Get RoomMenu]"),SerializeField]
        Transform m_playerListScrollViewContent; // m_playerListItemPrefab 생성 부모
        public string m_minePlayerName { get; private set; }
        public Dictionary<string, PlayerListItem> m_playerListDict { get; private set; } = new Dictionary<string, PlayerListItem>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }

        public void InitializeAtStart(){}

        // 방 Joined 상태에서 플레이어 리스트 생성 및 m_playerListDict로 관리
        public string OnRoomCreate(string playerNameText)
        {
            string playerName = playerNameText;
            m_minePlayerName = playerName;
            // 플레이어 리스트 아이템 생성

            CreatePlayerListItem(playerName);
            return playerName;
        }

        public void FailedRoomCrate()
        {
            AllRemovePlayerList();
        }

        public void OnRoomLeave()
        {
            AllRemovePlayerList();
        }

        public void OnPlayerLeftRoom(string leftPlayerName)
        {
            RemovePlayerList(leftPlayerName);
        }

        public string OnJoinRoom(string playerName)
        {
            m_minePlayerName = playerName;
            // 플레이어 리스트 아이템 생성

            CreatePlayerListItem(playerName);
            return playerName;
        }

        public void OnJoinedRoom(string playerName)
        {
            // 플레이어 리스트 아이템 생성

            CreatePlayerListItem(playerName);
        }

        public void EntertedPlayer(string playerName)
        {
            CreatePlayerListItem(playerName);
        }
        // 방 나갈 때는 m_playerListDict를 Clear하기에 생성시에만 신경쓰면됨
        private void CreatePlayerListItem(string playerName)
        {
            if (m_playerListDict.ContainsKey(playerName)) return;
            GameObject playerList = Instantiate(m_playerListItemPrefab, m_playerListScrollViewContent);
            PlayerListItem PlayerListItem = playerList.GetComponent<PlayerListItem>();

            PlayerListItem.SetPlayerListText(playerName);

            m_playerListDict.Add(playerName, PlayerListItem);
        }

        // 본인 나갔을 시 및 방에 있는 사람
        private void RemovePlayerList(string removeTargetName)
        {
            if (!m_playerListDict.ContainsKey(removeTargetName)) return;

            Destroy(m_playerListDict[removeTargetName].gameObject);
            m_playerListDict.Remove(removeTargetName);
        }

        private void AllRemovePlayerList()
        {
            foreach (var playerName in m_playerListDict.Keys)
            {
                Destroy(m_playerListDict[playerName].gameObject);
            }
            m_playerListDict.Clear();
        }
    }
}