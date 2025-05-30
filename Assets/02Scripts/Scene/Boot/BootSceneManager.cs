// 씬 전환 시 필요 에셋들을 미리 다운로드하고 로드하는 씬 매니저
// 로드하는동안의 로딩바를 보여주고, 로드가 끝나면 다음 씬으로 전환
using UnityEngine;
using UnityEngine.AddressableAssets;
using DUS.AssetLoad;
using UnityEngine.SceneManagement;
using DUS.Scene;
using System.Threading.Tasks;
using Photon.Pun;

public class BootSceneManager : MonoBehaviour
{
    [SerializeField]
    LoadingProgress m_loadingProgress;

    private async void Start()
    {
        // 시작시 서버 동기화 시켜줘야하기에 로비씬의 네트워크가 아닌 여기서 설정
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true; // 마스터 클라이언트가 씬을 변경하면 다른 클라이언트들도 자동으로 동기화됨
        }

        // Boot 씬부터 시작하므로
        if (GameManager.Instance.m_StartCheck == -1)
        {
            SceneLoadManager.Instance.PushNextInfoToBootSceneAndLoadBootScene(SceneType.Lobby);
            GameManager.Instance.SetStartCheck();
            SceneLoadManager.Instance.LoadNextScene();
        }

        await Task.Delay(2000); // 2초 대기
        SceneLoadManager.Instance.LoadNextScene();


        /*// 1.어드레서블 초기화
        await Addressables.InitializeAsync().Task;

        // 2. 다음 씬 필요 에셋들 서버에서 다운로드
        bool downloadSuccess = await AssetLoadManager.Instance.DownloadDependenciesAsync(m_loadingProgress.m_onProgress);

        if(!downloadSuccess)
        {
            Debug.LogError("Download failed");
            return;
        }

        // 3. 다운 받은 에셋들 메모리에 로드(실제 씬에 올리는 것은 아님)
        bool _isInMemoryAsset = await AssetLoadManager.Instance.LoadAssetsIntoMemoryAsync(m_loadingProgress.m_onProgress);
        if(!_isInMemoryAsset)
        {
            return;
        }

        // 4. 다음씬 이동
        SceneLoadManager.Instance.LoadNextScene(SceneLoadManager.m_NextScene);

        // 5. 다운받은 에셋들 인스턴스화는 다음 씬 매니저에서 Start부분에서 작성*/
    }
}
