//

//
using UnityEngine;
using UnityEngine.AddressableAssets;
using DUS.AssetLoad;
using UnityEngine.SceneManagement;
using DUS.Scene;

public class BootSceneManager : MonoBehaviour
{
    [SerializeField]
    LoadingProgress m_loadingProgress;

    private async void Start()
    {
        // 1.어드레서블 초기화
        await Addressables.InitializeAsync().Task;

        // 2. 다음 씬 필요 에셋들 서버에서 다운로드
        bool downloadSuccess = await AssetLoader.Instance.DownloadDependenciesAsync(m_loadingProgress.m_onProgress);

        if(!downloadSuccess)
        {
            Debug.LogError("Download failed");
            return;
        }

        // 3. 다운 받은 에셋들 메모리에 로드(실제 씬에 올리는 것은 아님)
        bool _isInMemoryAsset = await AssetLoader.Instance.LoadAssetsIntoMemoryAsync(m_loadingProgress.m_onProgress);
        if(!_isInMemoryAsset)
        {
            return;
        }

        // 4. 다음씬 이동
        SceneLoadManager.Instance.LoadNextScene(SceneLoadManager.m_NextScene);

        // 5. 다운받은 에셋들 인스턴스화는 다음 씬 매니저에서 Start부분에서 작성
    }
}
