using UnityEngine;
using UnityEngine.UI;

public class ExitBtn : MonoBehaviour
{
    Button m_btn;

    private void Awake()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(ExitApp);
    }

    private void ExitApp()
    {
        Application.Quit();
    }
}
