using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera m_mainCamera; // 메인 카메라
    
    [SerializeField] private Transform m_target; // 카메라가 따라갈 대상

    public Vector3 m_Dir { get; private set; } // 카메라 방향
    private Vector3 m_currentVelocity;
    [SerializeField] private float smoothSpeed = 0.125f;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;

    PlayerCore m_playerCore; // 플레이어 코어 스크립트
    float mouseX;
    float mouseY;

    private void Awake()
    {
        if(!m_playerCore.m_photonView.IsMine)
        {
            m_mainCamera.GetComponent<AudioListener>().enabled = false;
            //Destroy(this.gameObject); // 다른 플레이어의 카메라는 제거
        }
    }

    public void Initialize(PlayerCore core)
    {
        this.m_playerCore = core;
    }

    private void Start()
    {
        transform.parent = null; // 카메라가 다른 오브젝트의 자식이 되지 않도록 설정
    }
    private void LateUpdate()
    {
        if (!m_playerCore.m_photonView.IsMine) return;
        FollowCam();
    }

    private void FollowCam()
    {
        if (m_target == null) return;
        Vector3 desiredPosition = m_target.position;
        desiredPosition = Vector3.SmoothDamp(desiredPosition, m_target.position, ref m_currentVelocity, smoothSpeed); // 카메라가 따라갈 위치
        transform.position = desiredPosition;
    }

    public void OnCameraRotation(Vector2 lookInput)
    {
        mouseX += lookInput.x * rotationSpeed * Time.deltaTime;
        mouseY -= lookInput.y * rotationSpeed * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        // 카메라 회전

        Vector3 mouseDir = new Vector3(mouseY, mouseX, 0);
        transform.rotation = Quaternion.Euler(mouseDir);

        m_Dir = new Vector3(0, mouseX, 0);

    }
}
