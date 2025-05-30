using System.Collections;
using DUS.AssetLoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Photon.Pun;

namespace DUS.Scene
{
    public enum SceneType
    {
        Boot = 0,
        Lobby = 1,
        InGame = 2,
        InGame2 = 3,
        Test = 4
    }
    public class SceneLoadManager : MonoBehaviour
    {
        public static SceneLoadManager Instance { get; private set; }
        public static SceneType m_NextScene;

        public SceneType m_CurrentScene;
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

        private void Start()
        {
            if(SceneManager.GetActiveScene().buildIndex == (int)SceneType.Boot)
            {
                m_NextScene = SceneType.Lobby;
            }
            
        }

        // 다음 씬으로 넘어가는 과정에서 진행
        public void PushNextInfoToBootSceneAndLoadBootScene(SceneType nextScene, List<string> nextNeedAssetList = null)
        {
            m_NextScene = nextScene;

            // TODO : 에셋들 정보(위치값, 상태 등등) 저장
            if (nextNeedAssetList != null)
            AssetLoadManager.Instance.SetNextSceneNeedAddressable(nextNeedAssetList);

            // Boot 씬으로 이동
            if(SceneManager.GetActiveScene().buildIndex != (int)SceneType.Boot)
            {
                SceneManager.LoadScene((int)SceneType.Boot);
            }
        }

        public void LoadNextScene()
        {
            m_CurrentScene = m_NextScene;
            PhotonNetwork.LoadLevel((int)m_NextScene);
            //Boot Scene 진입
            //SceneManager.LoadScene((int)m_NextScene);
        }
    }
}