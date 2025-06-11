using Photon.Pun;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public static PlayerCore Instance { get; private set; }

    public PlayerInputManager m_inputManager { get; private set; }
    public PlayerAnimationManager m_animationManager { get; private set; }
    public PlayerLocomotion m_locomotion { get; private set; }
    public CameraManager m_cameraManager;

    public PhotonView m_photonView { get; private set; }
    private void Awake()
    {
        if (m_inputManager == null) m_inputManager = GetComponent<PlayerInputManager>();
        if (m_animationManager == null) m_animationManager = GetComponentInChildren<PlayerAnimationManager>();
        if (m_locomotion == null) m_locomotion = GetComponent<PlayerLocomotion>();
        if (m_cameraManager == null) m_cameraManager = GetComponentInChildren<CameraManager>();

        m_photonView = GetComponent<PhotonView>();

        m_locomotion.Initialize(this);
        m_animationManager.Initialize(this);
        m_cameraManager.Initialize(this);
    }
    
    private void Start()
    {
        // 초기화 작업
        // 예: 애니메이션 매니저 초기화, 입력 매니저 설정 등
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // 싱글톤 패턴을 위해 중복 생성 방지
        
    }

    private void FixedUpdate()
    {
        // 물리 기반 동작 처리
        // 예: 이동, 점프, 대시 등
        if (!m_photonView.IsMine) return;
        m_locomotion.HandleMove(m_inputManager.m_MoveInput);
    }

    private void Update()
    {
        

    }

    private void LateUpdate()
    {
        // 애니메이션 업데이트
        // 예: 애니메이션 상태 업데이트, 이동 속도 전달 등
        if (!m_photonView.IsMine) return;
        m_locomotion.HandleRotation(m_cameraManager.m_Dir);
        m_cameraManager.OnCameraRotation(m_inputManager.m_LookInput);
        m_animationManager.HandleAnimation(m_inputManager, m_locomotion);
    }

}
