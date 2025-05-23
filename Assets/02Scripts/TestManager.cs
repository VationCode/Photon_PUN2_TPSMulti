using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DUS.AssetLoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TestManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<string> m_NextNeedAssetLabelList = new List<string>();
    private async void Start()
    {
        m_NextNeedAssetLabelList.Add("PlayerInfoList");
        AssetLoader.Instance.SetNextSceneNeedAddressable(m_NextNeedAssetLabelList);
        
        bool isload = await AssetLoader.Instance.DownloadDependenciesAsync();

        if (!isload) return;
        
        Debug.Log("[isLoad] Download 성공");
        
        bool isMemory = await AssetLoader.Instance.LoadAssetsIntoMemoryAsync();

        if (!isMemory) return;
        Debug.Log("[isMemory] Download 성공");
        foreach(var item in AssetLoader.Instance.m_SaveAddressKeyList)
        {
            AssetLoader.Instance.CreateAddressableAsset(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
