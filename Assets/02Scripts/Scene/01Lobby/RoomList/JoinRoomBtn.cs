using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomBtn : MonoBehaviour
{
    [SerializeField] Button m_btn;
    [SerializeField] TMP_Text m_RoomName;
    private void Awake()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(JoinRoom);
    }

    public void SetBtnRoomName(string name)
    {
        m_RoomName.text = name;
    }

    private void JoinRoom()
    {
        LobbySceneManager.Instance.OnJoinRoom(m_RoomName.text);
    }
}
