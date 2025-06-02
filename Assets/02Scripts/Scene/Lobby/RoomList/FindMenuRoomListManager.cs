using Photon.Realtime;
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

        public Dictionary<string, FindRoomMenuJoinBtn> m_roomListDict { get; private set; } = new Dictionary<string, FindRoomMenuJoinBtn>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }
        public void InitializeAtStart()
        {
        }
        // 방생성메뉴UI에서 생성 버튼

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
            //else UpdateFindRoomMenuButtonList(roomName);
        }

        // 네트워크에서 방 업데이트 시 마다 리스트들 새로 생성
        // TODO : 전부 새로하기보다는 차후 관리를 통해 일부만 생성 삭제 되도록
        public void UpdateFindRoomMenuButtonList(List<RoomInfo> createRoomList)
        {
            // 전체 삭제
            foreach(var item in m_roomListDict.Keys)
            {
                Destroy(m_roomListDict[item].gameObject);
            }
            m_roomListDict.Clear();

            // 새로 리스트 생성
            foreach(var roomInfo in createRoomList)
            {
                GameObject _roomList = Instantiate(m_joinRoomBtnItemPrefab, m_roomListScrollViewContent);
                FindRoomMenuJoinBtn _joinBtn = _roomList.GetComponent<FindRoomMenuJoinBtn>();
                m_roomListDict.Add(roomInfo.Name, _joinBtn);
            }
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


