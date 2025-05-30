using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DUS.UI;

public class GoMenuUIBtn : MonoBehaviour
{
    [SerializeField] GoMenuUIType m_btnType;
    public bool isRoomMenu;
    public bool isCreateRoom;
    Button m_btn;
    private void Awake()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(OnClickMainButtons);
    }

    private void OnClickMainButtons()
    {
        // 자기 자신을 제외한 나머지 메뉴 비활성화
        LobbySceneManager.Instance.GoMenuUI(m_btnType);

        if(m_btnType == GoMenuUIType.MainMenuUI && isRoomMenu)
        {
            LobbySceneManager.Instance.OnRoomLeave();
        }
        if(m_btnType == GoMenuUIType.RoomMenuUI && isCreateRoom)
        {
            LobbySceneManager.Instance.OnRoomCreate();
        }
    }
}
