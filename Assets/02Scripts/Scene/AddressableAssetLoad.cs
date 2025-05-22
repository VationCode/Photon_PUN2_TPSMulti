using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DUS.Addressable
{
    public class AddressableAssetLoad : MonoBehaviour
    {
        public static AddressableAssetLoad Instance { get; private set; }
        private List<string> m_nextSceneNeedAddressableList = new List<string>();

        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }
        
        public void SetNextSceneNeedAddressable(List<string> nextSceneNeedAddressableList)
        {
            m_nextSceneNeedAddressableList.Clear();
            m_nextSceneNeedAddressableList = nextSceneNeedAddressableList;
        }

        public AsyncOperationHandle DownLoadNextAddressableList()
        {
            return Addressables.DownloadDependenciesAsync(m_nextSceneNeedAddressableList);
        }

        public AsyncOperationHandle LoadMemoryAddressableList()
        {
            return Addressables.LoadAssetAsync<GameObject>(m_nextSceneNeedAddressableList);
        }
    }
}