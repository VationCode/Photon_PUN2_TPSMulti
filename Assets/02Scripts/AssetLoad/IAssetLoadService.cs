using DUS.AssetLoad;
using System;
using System.Threading.Tasks;
using UnityEngine;

public interface IAssetLoadService
{
    Task DownLoadAndSceneUpload(Action<float> onProgress, NextSceneRequireData data);
}
