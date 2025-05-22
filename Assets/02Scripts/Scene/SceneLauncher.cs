using System.Collections.Generic;
using DUS.Addressable;
using DUS.Scene;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneLauncher : MonoBehaviour
{
    [SerializeField] SceneType m_currentSceneType;
    [SerializeField] List<AssetReference> AddressableList = new List<AssetReference>();

    List<string> m_nextSceneNeedAddressableList = new List<string>();

    public void GetNextNeedSceneInfo()
    {
        SceneLoadManager.m_NextScene = m_currentSceneType;
        foreach (var item in AddressableList)
        {
            m_nextSceneNeedAddressableList.Add(item.RuntimeKey.ToString());
        }
        AddressableAssetLoad.Instance.SetNextSceneNeedAddressable(m_nextSceneNeedAddressableList);
    }
}
