using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace DUS.AssetLoad
{
    public class AssetLoader : MonoBehaviour
    {
        public static AssetLoader Instance { get; private set; }
        public List<string> m_NextNeedAssetLabelList { get; private set; } = new List<string>();        // 현재는 어드레서블
        public List<string> m_SaveAddressKeyList { get; private set; } = new List<string>();             // Label단위로는 Addressables.InstantiateAsync에서 못 뽑아내기에 실제 리스트들 저장
        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        // 현재 씬에서 다음 씬의 필요 에셋리스트 저장
        public void SetNextSceneNeedAddressable(List<string> nextSceneNeedAddressableList)
        {
            m_NextNeedAssetLabelList.Clear();
            m_NextNeedAssetLabelList = nextSceneNeedAddressableList;
        }

        // 다운로드 함수 따로 씬 셋팅 따로
        public async Task<bool> DownloadDependenciesAsync(Action<float> onProgress = null)
        {
            int _total = m_NextNeedAssetLabelList.Count;

            //데이터 없는 경우 한번 더 체크
            if (_total == 0)                 
            {
                onProgress?.Invoke(1f);
                return true;
            }

            int _currentProgress = 0;
            foreach (string label in m_NextNeedAssetLabelList)
            {
                AsyncOperationHandle _handle = Addressables.DownloadDependenciesAsync(label);

                while (!_handle.IsDone)
                {
                    float progress = (_currentProgress + _handle.PercentComplete) / _total;
                    //onProgress?.Invoke(progress);
                    await Task.Yield();
                }
                
                if(_handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to download dependencies for label: {label}");
                    Addressables.Release(_handle);
                    return false;
                }
                _currentProgress++;
                Addressables.Release(_handle); // 성공해도 반드시 해제
            }
            onProgress?.Invoke(1f); // 완전히 1로 채워줌
            return true;
        }

        // 다운로드된 에셋을 메모리에 로드(즉, 해당 씬에 로드)
        public async Task<bool> LoadAssetsIntoMemoryAsync(Action<float> onProgress = null)
        {
            m_SaveAddressKeyList.Clear(); // 메모리에 로드된 에셋 리스트 초기화

            int _total = m_NextNeedAssetLabelList.Count;

            //데이터 없는 경우 한번 더 체크
            if (_total == 0)
            {
                onProgress?.Invoke(1f);
                return true;
            }

            int _currentProgress = 0;

            foreach (string label in m_NextNeedAssetLabelList)
            {
                AsyncOperationHandle<IList<GameObject>> _handle = Addressables.LoadAssetsAsync<GameObject>(label, null, false);// 자동 릴리스 여부 (false면 직접 Release 필요)

                while (!_handle.IsDone)
                {
                    float progress = (_currentProgress + _handle.PercentComplete) / _total;
                    //onProgress?.Invoke(_totalProgress);
                    await Task.Yield();
                }

                if (_handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load asset for label: {label}");
                    return false;
                }

                var locHandle = Addressables.LoadResourceLocationsAsync(label, typeof(GameObject));

                // 로드된 에셋의 주소 키 가져오기
                foreach (var item in locHandle.Result)
                {
                    string addressKey = item.PrimaryKey;
                    m_SaveAddressKeyList.Add(addressKey);
                }
                
                Addressables.Release(_handle);
                _currentProgress++;
            }
            //onProgress?.Invoke(1f); // 완전히 1로 채워줌
            return true;
        }

        public async Task<long> CheckDownloadSizeAsync(string label)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(label);
            await sizeHandle.Task;

            if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[Check] Download size for {label}: {sizeHandle.Result} bytes");
                return sizeHandle.Result;
            }
            else
            {
                Debug.LogError($"[Check] Failed to get download size for {label}");
                return -1;
            }
        }

        // 메모리에 올라온 에셋을 실제로 Scene에 생성
        public void CreateAddressableAsset(string assetObj, Transform transform = null, Transform parent = null)
        {
            Transform tr = transform;
            if (tr == null) tr = new GameObject(assetObj).transform;

            tr.position = new Vector3(UnityEngine.Random.Range(-2f,2f),0,0);
            Addressables.InstantiateAsync(assetObj, tr, true);
            
        }
    }
}