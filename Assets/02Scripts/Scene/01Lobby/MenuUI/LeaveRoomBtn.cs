using UnityEngine;
using UnityEngine.UI;

public class LeaveRoomBtn : MonoBehaviour
{
    Button m_btn;

    private void Awake()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(LeaveRoom);
    }

    private void LeaveRoom()
    {
        LobbySceneManager.Instance.LeaveRoom();
    }
}
