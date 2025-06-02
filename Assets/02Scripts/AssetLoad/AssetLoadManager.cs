// 순서 : 서버에서 다운로드 -> 메모리 업로드 -> 씬 업로드

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DUS.AssetLoad
{
    public class AssetLoadManager : MonoBehaviour, IAssetLoadService
    {
        public static AssetLoadManager Instance { get; private set; }
        public static IAssetLoadService assetLoadService => Instance;

        // 다운 및 메모리 업로드 와료된 키값들 저장
        public List<string> m_downLoadedAddressables { get; private set; } = new List<string>();

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public async Task InitializeAsync()
        {
            try
            {
                await Addressables.InitializeAsync().Task;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Addressables] 초기화 실패 - 무시하고 진행합니다: {e.Message}");
            }
        }

        // TODO : 차후 DownloadAssets와 LoadAssetsIntoMemoryAsync의 중복 리팩토링
        int currentProgress = 0;
        public async Task DownLoadAndSceneUpload(Action<float> onProgress, NextSceneRequireData nextSceneRequireData = null)
        {
            if (nextSceneRequireData == null) return;
            currentProgress = 0;
            m_downLoadedAddressables.Clear(); // 메모리에 로드된 에셋 리스트 초기화

            List<string> _labelUnityKeyList = nextSceneRequireData.m_RequiredAddressableLabelKeyList;
            List<string> _keyList = nextSceneRequireData.m_RequiredAddressableKeyList;

            int _total = _labelUnityKeyList.Count + _keyList.Count;

            // 라벨 단위
            if(_labelUnityKeyList.Count > 0)
            {
                await DownloadAssets(_total, currentProgress, onProgress, _labelUnityKeyList);               // 서버에서 다운로드
                await LoadAssetsIntoMemoryAsync(_total, currentProgress, onProgress, _labelUnityKeyList);     // 다운받은 에셋 메모리에 업로드
            }

            if(_keyList.Count > 0)
            {
                await DownloadAssets(_total, currentProgress, onProgress, _keyList);
                await LoadAssetsIntoMemoryAsync(_total, currentProgress, onProgress, _keyList);
            }

            onProgress?.Invoke(1f); // 완전히 1로 채워줌

            // 실제 씬에 생성
            await CreateAddressableAsset(m_downLoadedAddressables);
        }

        private async Task DownloadAssets(int total, int currentProgress,Action<float> onProgress, List<string> AssetKeyList) //isLabel은 Memory에서만 체크
        {
            foreach (string label in AssetKeyList)
            {
                AsyncOperationHandle handle= Addressables.DownloadDependenciesAsync(label);
                
                while (!handle.IsDone)
                {
                    float progress = (currentProgress + handle.PercentComplete) / total;
                    onProgress?.Invoke(progress);
                    await Task.Yield();
                }

                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load assets for label: {label}");
                    Addressables.Release(handle);
                    return;
                }
                Addressables.Release(handle);
                currentProgress++;
            }
        }

        public async Task LoadAssetsIntoMemoryAsync(int total,int currentProgress,Action<float> onProgress, List<string> AssetKeyList)
        {
            foreach (string label in AssetKeyList)
            {
                AsyncOperationHandle<IList<GameObject>> _handle = Addressables.LoadAssetsAsync<GameObject>(label, null, false);// 자동 릴리스 여부 (false면 직접 Release 필요)
                
                while (!_handle.IsDone)
                {
                    float progress = (currentProgress + _handle.PercentComplete) / total;
                    onProgress?.Invoke(progress);
                    await Task.Yield();
                }

                if (_handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load asset for label: {label}");
                    return;
                }

                var locHandle = Addressables.LoadResourceLocationsAsync(label, typeof(GameObject));
                await locHandle.Task;

                // 로드된 에셋의 주소 키 가져오기
                if (locHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var item in locHandle.Result)
                    {
                        string addressKey = item.PrimaryKey;
                        m_downLoadedAddressables.Add(addressKey);
                    }
                }
                Addressables.Release(_handle);
                currentProgress++;
            }
        }

        // 메모리에 올라온 에셋을 실제로 Scene에 생성
        public async Task CreateAddressableAsset(List<string> assetObj)
        {
            foreach (var asset in assetObj)
            {
                AsyncOperationHandle<GameObject> _handle = Addressables.InstantiateAsync(asset);
                GameObject obj = await _handle.Task; // 비동기 완료까지 대기 후 GameObject 획득
            }
        }
    }
}