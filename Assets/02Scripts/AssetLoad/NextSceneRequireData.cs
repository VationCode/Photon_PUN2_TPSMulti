// 다음 씬으로의 요청할 데이터

using DUS.scene;
using System.Collections.Generic;
using UnityEngine;


namespace DUS.AssetLoad
{
    
    public class NextSceneRequireData : ScriptableObject
    {
        public SceneType m_SceneType;
        public List<string> m_RequiredAddressableLabelKeyList = new List<string>(); // 라벨단위로 생성
        public List<string> m_RequiredAddressableKeyList = new List<string>();      // 일반 단위로 생성
    }
}
