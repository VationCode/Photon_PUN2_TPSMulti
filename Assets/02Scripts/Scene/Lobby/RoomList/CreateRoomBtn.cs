using UnityEngine;
using UnityEngine.UI;

public class CreateRoomBtn : MonoBehaviour
{
    Button m_btn;

    private void Awake()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(CreateRoom);
    }

    private void CreateRoom()
    {
        LobbySceneManager.Instance.CreateRoom();
    }
}
