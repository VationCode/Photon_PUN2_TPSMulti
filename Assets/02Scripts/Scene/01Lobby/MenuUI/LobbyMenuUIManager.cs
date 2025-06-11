using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DUS.UI
{
    public enum GoMenuUIType
    {
        MainMenuUI,
        CreateMenuUI,
        FindRoomMenuUI,
        RoomMenuUI,
        SettingsMenuUI,
    }

    public class LobbyMenuUIManager : MonoBehaviour, IInitializeAtStart
    {

        [Header("[Get CrateMenuUI]")]
        [SerializeField] TMP_InputField m_inpuFieldRoomName;
        [Header("[Get MainMenu]"), SerializeField]
        TMP_InputField m_inputFieldPlayerName;

        [Header("[Get RoomMenu]")]
        [SerializeField] TMP_Text m_roomMenuName;

        [Header("[Get RoomMenu]")]
        [SerializeField] Button m_startBtn;

        private List<LobbyMenuTypeInfo> m_lobbyMenuList; // 메뉴들 리스트
        private void Awake()
        {
            m_lobbyMenuList = new List<LobbyMenuTypeInfo>();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.GetComponent<LobbyMenuTypeInfo>() != null)
                    m_lobbyMenuList.Add(transform.GetChild(i).gameObject.GetComponent<LobbyMenuTypeInfo>());

                transform.GetChild(i).gameObject.SetActive(false); // 메뉴아닌 ui들도 있을것이기에
            }
        }
        void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }
        public void InitializeAtStart()
        {
            GoMenuUI(GoMenuUIType.MainMenuUI);
        }

        public void GoMenuUI(GoMenuUIType menuType)
        {
            foreach (var menu in m_lobbyMenuList)
            {
                menu.gameObject.SetActive(menu.m_menuType == menuType);
            }
        }

        public string GetInputFieldPlayerName()
        {
            string playerName = m_inputFieldPlayerName.text;
            if (string.IsNullOrEmpty(playerName))
            {
                playerName = "PlayerCore" + Random.Range(0, 1000);
            }
            SetPlayerName(playerName);
            return playerName;
        }

        public void SetPlayerName(string playerName)
        {
            m_inputFieldPlayerName.text = playerName;
        }

        public string GetInputFieldRoomName()
        {
            string roomName = m_inpuFieldRoomName.text;
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room" + UnityEngine.Random.Range(0, 1000);
                m_inpuFieldRoomName.text = roomName;   
            }
            SetRoomMenuRoomName(roomName);
            return roomName;
        }

        public void SetRoomMenuRoomName(string roomName)
        {
            m_roomMenuName.text = roomName;
        }

        public void ActivateStartInGameBtn(bool isBool)
        {
            m_startBtn.gameObject.SetActive(isBool);
        }
    }
}
