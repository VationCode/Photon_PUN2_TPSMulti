using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyBtn : MonoBehaviour
{
    Button m_btn;

    private void Awake()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(JoinLobby);
    }

    private void JoinLobby()
    {
        LobbySceneManager.Instance.JoinLobby();
    }
}
