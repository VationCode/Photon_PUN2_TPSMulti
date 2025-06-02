using Photon.Realtime;
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

        // 방 나갈 때는 m_playerListDict를 Clear하기에 생성시에만 신경쓰면됨
        public void CreatePlayerListItem(Player[] players)
        {
            // 완전히 제거 후
            AllRemove();

            foreach (var playerItem in players)
            {
                GameObject playerList = Instantiate(m_playerListItemPrefab, m_playerListScrollViewContent);
                PlayerListItem PlayerListItem = playerList.GetComponent<PlayerListItem>();
                m_playerListDict.Add(playerItem.NickName, PlayerListItem);

                PlayerListItem.SetPlayerListText(playerItem.NickName);
            }
        }

        public void AllRemove()
        {
            foreach (var playerItem in m_playerListDict.Keys)
            {
                Destroy(m_playerListDict[playerItem].gameObject);
            }
            m_playerListDict.Clear();
        }


    }
}