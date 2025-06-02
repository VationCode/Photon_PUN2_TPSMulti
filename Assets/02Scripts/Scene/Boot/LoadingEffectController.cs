using System;
using UnityEngine;
using UnityEngine.UI;

namespace DUS.AssetLoad
{
    public class LoadingEffectController : MonoBehaviour
    {
        [SerializeField]
        Image m_progressBar;

       public Action<float> m_onProgress = null;

        private void OnEnable()
        {
            
        }

        private void Awake()
        {
            m_onProgress += OnProgressing;
        }

        private void Start()
        {
            m_progressBar.fillAmount = 0f;
            m_onProgress?.Invoke(0f);
        }

        public void OnProgressing(float progress)
        {
            progress += Time.deltaTime * 0.1f;
            m_progressBar.fillAmount = Mathf.Clamp01(progress);
        }
    }
}
