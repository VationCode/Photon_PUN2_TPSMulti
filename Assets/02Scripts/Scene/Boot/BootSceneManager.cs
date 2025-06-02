// 씬 전환 시 필요 에셋들을 미리 다운로드하고 로드하는 씬 매니저
// 로드하는동안의 로딩바를 보여주고, 로드가 끝나면 다음 씬으로 전환
using UnityEngine;
using UnityEngine.AddressableAssets;
using DUS.AssetLoad;
using DUS.scene;
using System.Threading.Tasks;
using Photon.Pun;
using DUS.Network;
using DUS.Manager;
using System;

namespace DUS.Scene
{
    public class BootSceneManager : MonoBehaviour
    {
        [SerializeField]
        LoadingEffectController m_loadingEffectController;

        // 결합도 낮추기위해 인터페이스를 활용하여 함수들 적용
        private INetworkService m_networkService;
        private IAssetLoadService m_assetLoadService;
        private ISceneLoadService m_sceneLoadService;
        private void Awake()
        {
            m_networkService = NetworkManager.Service;
            m_assetLoadService = AssetLoadManager.assetLoadService;
            m_sceneLoadService = SceneLoadManager.sceneLoadService;
        }
        private async void Start()
        {
            // 1.동기화 초기화

            await AssetLoadManager.Instance.InitializeAsync();
            m_networkService?.ConnectNetwork();

            // 네트워크 연결될때까지
            while (!m_networkService.CheckConnected()) await Task.Delay(100);


            await m_assetLoadService?.DownLoadAndSceneUpload(m_loadingEffectController.m_onProgress, SceneLoadManager.Instance.m_nextSceneRequireData);

            await Task.Delay(1000);

            if (SceneLoadManager.m_NextScene == SceneType.Lobby) m_networkService.JoinLobby();

            m_sceneLoadService?.LoadNextScene();
        }
    }
}

/*
           await Addressables.InitializeAsync().Task;

           // 2. 다음 씬 필요 에셋들 서버에서 다운로드
           bool downloadSuccess = await AssetLoadManager.Instance.DownloadAsset(m_loadingEffectController.m_onProgress);

           if(!downloadSuccess)
           {
               Debug.LogError("Download failed");
               return;
           }

           // 3. 다운 받은 에셋들 메모리에 로드(실제 씬에 올리는 것은 아님)
           bool _isInMemoryAsset = await AssetLoadManager.Instance.LoadAssetsIntoMemoryAsync(m_loadingEffectController.m_onProgress);
           if(!_isInMemoryAsset)
           {
               return;
           }

           // 4. 다음씬 이동
           SceneLoadManager.Instance.LoadNextScene_Boot(SceneLoadManager.m_NextScene);

           // 5. 다운받은 에셋들 인스턴스화는 다음 씬 매니저에서 Start부분에서 작성*/