using UnityEngine;


namespace DUS.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            // TODO : 시작 시 필요한 주요 리소스들은 미리 다운

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
