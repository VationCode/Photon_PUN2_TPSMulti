using Photon.Realtime;
using UnityEngine;

public class SaveNetworkInfo : MonoBehaviour
{
    public static SaveNetworkInfo Instance;
    public string m_PlayerName;
    public string m_CurrentRoomName;

    public Player[] m_CurrentJoinPlayers;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveInfo(string playerName, string currentRoomName, Player[] players)
    {
        m_PlayerName = playerName;
        m_CurrentRoomName = currentRoomName;
        m_CurrentJoinPlayers = players;
    }
}
