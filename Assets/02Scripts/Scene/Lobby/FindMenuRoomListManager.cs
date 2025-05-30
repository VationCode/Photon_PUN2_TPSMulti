using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DUS.UI
{
    // 실상적으로는 FindMenu에 있는 조인룸버튼 리스트들 관리
    public class FindMenuRoomListManager : MonoBehaviour, IInitializeAtStart
    {
        [SerializeField]
        GameObject m_joinRoomBtnItemPrefab;
        [Header("[Get FindRoomMenu]")]
        [SerializeField] Transform m_roomListScrollViewContent;

        public string m_getRoomName { get; private set; }

        public Dictionary<string, FindRoomMenuRoomButtonItem> m_roomListDict { get; private set; } = new Dictionary<string, FindRoomMenuRoomButtonItem>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }
        public void InitializeAtStart()
        {
        }
        // 방생성메뉴UI에서 생성 버튼
        public string OnRoomCreate(string roomName)
        {
            m_getRoomName = roomName;
            if (string.IsNullOrEmpty(m_getRoomName))
            {
                m_getRoomName = "Room" + Random.Range(0, 1000);
            }

            CreateRoomButtonItem(m_getRoomName);
            return m_getRoomName;
        }

        public void FailedRoomCrate()
        {
            AllRemoveRoomList();
        }

        public void OnRoomLeave()
        {
            AllRemoveRoomList();
        }

        public void OnRoomUpdateList(bool isRemove, string roomName)
        {
            if (isRemove) RemoveRoomList(roomName);
            else CreateRoomButtonItem(roomName);
        }
        // 유저들이 방 생성 시마다 FindMenuUI에 방리스트(버튼) 생성
        private void CreateRoomButtonItem(string createRoomName)
        {
            if (m_roomListDict.ContainsKey(createRoomName)) return; // 이미 존재하는 방 이름이면 생성하지 않음

            GameObject roomList = Instantiate(m_joinRoomBtnItemPrefab, m_roomListScrollViewContent);
            FindRoomMenuRoomButtonItem joinBtnInfo = roomList.GetComponent<FindRoomMenuRoomButtonItem>();

            joinBtnInfo.SetRoomListInfo(createRoomName);
            m_roomListDict.Add(createRoomName, joinBtnInfo);
        }

        private void RemoveRoomList(string createRoomName)
        {
            if (!m_roomListDict.ContainsKey(createRoomName)) return;
            Destroy(m_roomListDict[createRoomName].gameObject);
            m_roomListDict.Remove(createRoomName);
        }

        private void AllRemoveRoomList()
        {
            foreach (var roomName in m_roomListDict.Keys)
            {
                Destroy(m_roomListDict[roomName].gameObject);
            }
            m_roomListDict.Clear();
        }
    }
}


