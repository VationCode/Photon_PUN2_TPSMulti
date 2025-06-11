using Photon.Pun;
using System.IO;
using UnityEngine;

public class InGameSceneManager : MonoBehaviour
{
    [SerializeField]SpwanPointManager m_spwanPointManager;

    private void Awake()
    {
        PhotonNetwork.SendRate = 30;            // 초당 몇 번 데이터를 보낼지 (기본 10)
        PhotonNetwork.SerializationRate = 20;   // 초당 몇 번 동기화할지 (기본 10)
        CreatePlayer();
    }
    private void Start()
    {

    }

    private void CreatePlayer()
    {
        int random = Random.Range(0, m_spwanPointManager.m_SpwanPoints.Length-1);

        Vector3 pos = m_spwanPointManager.m_SpwanPoints[random].transform.position;
        Quaternion rot = m_spwanPointManager.m_SpwanPoints[random].transform.rotation;
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs/InGamePlayer"), pos, rot);

        //m_photonView = m_player.GetComponent<PhotonView>();

    }
}
