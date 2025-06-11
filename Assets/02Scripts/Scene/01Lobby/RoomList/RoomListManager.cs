using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DUS.UI
{
    // 실상적으로는 FindMenu에 있는 조인룸버튼 리스트들 관리
    public class RoomListManager : MonoBehaviour, IInitializeAtStart
    {
        [SerializeField]
        GameObject m_joinRoomBtnPrefab;
        [Header("[Get FindRoomMenu]")]
        [SerializeField] Transform m_roomListScrollViewContent;

        public string m_getRoomName { get; private set; }

        public Dictionary<string, JoinRoomBtn> m_roomListDict { get; private set; } = new Dictionary<string, JoinRoomBtn>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }
        public void InitializeAtStart()
        {
        }
        // 방생성메뉴UI에서 생성 버튼

        public void CreateJoinRoomBtn(string roomName)
        {
            if (m_roomListDict.ContainsKey(roomName)) return;
            GameObject _roomList = Instantiate(m_joinRoomBtnPrefab, m_roomListScrollViewContent);
            JoinRoomBtn _joinBtn = _roomList.GetComponent<JoinRoomBtn>();
            _joinBtn.SetBtnRoomName(roomName);
            m_roomListDict.Add(roomName, _joinBtn);
        }

        // 네트워크에서 방 업데이트(변화 발생) 시 마다 버튼 리스트들 전부 새로 생성
        // TODO : 전부 새로하기보다는 차후 관리를 통해 일부만 생성 삭제 되도록
        /*public void UpdateJoinButtonList(List<RoomInfo> createRoomList)
        {
            // 전체 삭제
            AllRemoveRoomList();

            // 새로 리스트 생성
            foreach (var roomInfo in createRoomList)
            {
                // 방은 생성되고 삭제되는게 아니라 켜졌다 껴졌다하는것이기에 이를 판단해서 리스트 생성
                if(roomInfo.IsVisible)
                {
                    GameObject _roomList = Instantiate(m_joinRoomBtnPrefab, m_roomListScrollViewContent);
                    JoinRoomBtn _joinBtn = _roomList.GetComponent<JoinRoomBtn>();
                    _joinBtn.SetBtnRoomName(roomInfo.Name);
                    m_roomListDict.Add(roomInfo.Name, _joinBtn);
                }
            }
        }*/

        public void RemoveRoomList(string createRoomName)
        {
            if (!m_roomListDict.ContainsKey(createRoomName)) return;
            Destroy(m_roomListDict[createRoomName].gameObject);
            m_roomListDict.Remove(createRoomName);
        }

        public void AllRemoveRoomList()
        {
            foreach (var roomName in m_roomListDict.Keys)
            {
                Destroy(m_roomListDict[roomName].gameObject);
            }
            m_roomListDict.Clear();
        }
    }
}


