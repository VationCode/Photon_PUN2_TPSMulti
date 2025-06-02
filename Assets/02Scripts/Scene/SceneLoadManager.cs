using System.Collections;
using DUS.AssetLoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Photon.Pun;

namespace DUS.scene
{
    public enum SceneType
    {
        Boot = 0,
        Lobby = 1,
        InGame = 2,
        InGame2 = 3,
        Test = 4
    }
    public class SceneLoadManager : MonoBehaviour, ISceneLoadService
    {
        public static SceneLoadManager Instance { get; private set; }
        public static SceneType m_NextScene;
        public static ISceneLoadService sceneLoadService => Instance;
        public SceneType m_CurrentScene;

        [HideInInspector]
        public NextSceneRequireData m_nextSceneRequireData;

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
            m_NextScene = SceneType.Lobby;
        }

        // 다음 씬으로 넘어가는 과정에서 진행
        public void LoadNextScene_Boot(SceneType nextScene, NextSceneRequireData nextSceneRequireData = null)
        {
            m_nextSceneRequireData = new NextSceneRequireData();
            if(nextSceneRequireData != null)
            m_nextSceneRequireData = nextSceneRequireData;

            m_NextScene = nextScene;

            SceneManager.LoadSceneAsync((int)SceneType.Boot);
        }

        public void LoadNextScene()
        {
            Debug.Log(m_NextScene.ToString() + "Scene");
            m_CurrentScene = m_NextScene;
            PhotonNetwork.LoadLevel((int)m_NextScene);
        }
    }
}