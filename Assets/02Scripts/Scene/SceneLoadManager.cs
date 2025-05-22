using System.Collections;
using DUS.Addressable;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

namespace DUS.Scene
{
    public enum SceneType
    {
        Boot,
        Loaby,
        InGame,
    }
    public class SceneLoadManager : MonoBehaviour
    {
        public static SceneLoadManager Instance { get; private set; }
        public static SceneType m_NextScene;

        private Coroutine m_asyncLoadCR;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void Start()
        {
            m_NextScene = SceneType.Loaby;
            List<string> startAddressableList = new List<string>();
            startAddressableList.Add("Player");
            AddressableAssetLoad.Instance.SetNextSceneNeedAddressable(startAddressableList);

            LoadGameScene(m_NextScene);
        }

        public void SetNextScene()
        {

        }

        public void LoadGameScene(SceneType nextScene)
        {
            if(m_asyncLoadCR != null)
            {
                StopCoroutine(m_asyncLoadCR);
            }
            m_asyncLoadCR = StartCoroutine(AsyncLoadSceneCoroutine());
        }

        private IEnumerator AsyncLoadSceneCoroutine()
        {
            //yield return AddressableAssetLoad.Instance.DownLoadNextAddressableList();

            //var handle = Addressables.LoadAssetAsync<GameObject>();

            yield return null;
        }
    }
}