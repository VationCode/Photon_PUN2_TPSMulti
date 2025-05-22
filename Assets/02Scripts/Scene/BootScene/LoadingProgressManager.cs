using UnityEngine;
using UnityEngine.UI;

namespace DUS.Scene.Boot
{
    public class LoadingProgressManager : MonoBehaviour
    {
        [SerializeField]
        Image m_progressBar;

        public void Progressing(float fillAmount)
        {
            fillAmount += Time.deltaTime * 0.1f;
            m_progressBar.fillAmount = fillAmount;
            if (fillAmount >= 1f)
            {
                fillAmount = 0f;
                m_progressBar.fillAmount = 0f;
            }
        }
    }
}
