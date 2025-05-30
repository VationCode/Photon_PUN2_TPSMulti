using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] TMP_Text m_playerNameText;

    private void Start()
    {
        m_playerNameText = GetComponentInChildren<TMP_Text>();
    }

    public string GetPlayerName()
    {
        return m_playerNameText.text;
    }

    public void SetPlayerListText(string name)
    {
        m_playerNameText.text = name;
    }
}
