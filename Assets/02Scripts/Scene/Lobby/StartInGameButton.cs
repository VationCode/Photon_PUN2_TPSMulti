using DUS.Scene;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartInGameButton : MonoBehaviour
{
    [SerializeField] Button m_btn;

    private void Awake()
    {
        if(m_btn == null)
        {
            m_btn = GetComponent<Button>();
        }
        m_btn.onClick.AddListener(OnClickStartInGame);
    }

    private void OnClickStartInGame()
    {
        LobbySceneManager.Instance.StartInGame((int)SceneType.InGame);
    }
}
