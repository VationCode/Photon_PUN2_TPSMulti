// ============================== 250418
//하이어라키 구조
//PlayerCore
//├── target_FollowCam (View 높이)

//CameraRig (CameraRigManager, target_FollowCam와 글로벌 위치 동일하게)
//├── Pivot(Empty, 어깨와의 거리)
//│   └── MainCamera(실제 카메라, Pivot과의 거리 유지)
//============================== 
using UnityEngine;

public class CameraRigManager : MonoBehaviour
{
    [SerializeField] private PlayerCore m_PlayerCore;
    private Transform m_playerTr;


    [Header("[ CameraRig의 추적 대상 ]")]
    [SerializeField] private Transform m_target_FollowCamTr;        // 카메라의 특정 위치 (Player의 하위객체, 계산 시 LocalPos 사용)
    [Tooltip("뷰가 높이"), SerializeField] float m_followTargetHeight = 1.4f;

    [Header("[ 에임 설정 ]")]
    [SerializeField] private Transform m_target_AimTr;              // 애니메이션 리깅을 위한 타겟
    [SerializeField] float m_maxAimDis = 10f;
    [SerializeField] private LayerMask m_aimMask;
    [SerializeField] float m_aimBaseFov = 60f; // 에임 시 카메라의 FOV 값
    [SerializeField] float m_aimMinFov = 45f; // 에임 시 카메라의 FOV 값

    [Header("[ 카메라 설정 ]")] // 구조 : pivot_maincamera
    [SerializeField] Transform m_cameraRigTr;                       // m_cameraRigTrManager
    [SerializeField] private Transform m_pivotTr;                   // CameraRig 하위 객체(높이는 CameraRig가 설정, 피벗은 어깨와의 거리만 판별 필요)
    [SerializeField] float m_pivotOffsetX = 0.5f;                   // pivot의 위치 (어깨와의 옆 거리)

    [SerializeField] private Camera m_mainCamera;                   //pivot 하위 객체 (pivot와의 거리만 판별 필요)
    [SerializeField] float m_minCameraDis = 0.5f;
    [SerializeField] float m_maxCameraDis = 2.5f;                   // Pivot과 카메라 사이의 기본 거리이자 최대 거리

    [SerializeField] float m_rotationSensitivity = 10f;             // 마우스 회전 감도, SetCurrentSpeed
    [SerializeField] float m_downAngle = 60f;                       // 수직 회전 최소 각도 (계산 시 -로)
    [SerializeField] float m_upAngle = 35f;                         // 수직 회전 최대 각도

    [SerializeField] private LayerMask m_cameraMask;

    [SerializeField] float aimPosSpeed = 20;

    public float m_MouseX { get; private set; }
    public float m_MouseY { get; private set; }

    private void Awake()
    {
        m_mainCamera = GetComponentInChildren<Camera>();
        m_cameraRigTr = GetComponent<Transform>();
    }

    /// <summary>
    ///  m_target_FollowCamTr 로컬 위치 = Zero + m_followTargetHeight
    /// m_CmaeraRigTr 글로벌로 위치 = m_target_FollowCamTr
    /// m_pivotTr 로컬 위치 = Zero + m_pivotOffsetX (어깨와의 거리)
    /// m_mainCamera 로컬 위치 = Zero + m_maxCameraDis (Pivot과의 거리)
    /// </summary>
    private void InitializeCameraPos()
    {
        //1. m_target_FollowCamTr 위치 초기화
        m_target_FollowCamTr.localPosition = new Vector3(0f, m_followTargetHeight, 0f); // 카메라의 높이

        //2. CameraRig의 위치 초기화
        m_cameraRigTr.position = m_target_FollowCamTr.position; // CameraRig의 위치는 FollowTarget과 동일

        //3. Pivot의 위치 초기화
        m_pivotTr.localPosition = new Vector3(m_pivotOffsetX, 0, 0f); // 카메라의 높이

        //4. MainCamera의 위치 초기화
        m_mainCamera.transform.localPosition = new Vector3(0f, 0f, -m_maxCameraDis); // 카메라의 높이

        //5. Aim의 위치 초기화
        m_target_AimTr.transform.position = m_pivotTr.position + m_pivotTr.forward * m_maxAimDis; // 카메라의 높이
        UpdateAimTargetPos();
    }

    private void Start()
    {
        if (m_PlayerCore == null)
        {
            m_PlayerCore = FindObjectOfType<PlayerCore>();
            m_playerTr = m_PlayerCore.transform;
            m_target_FollowCamTr = m_PlayerCore.m_TargetFollowCam;
        }
        // 카메라의 초기 회전값을 타겟과 동일하게 설정
        InitializeCameraPos();
        m_MouseX = m_playerTr.eulerAngles.y;
        m_MouseY = m_playerTr.eulerAngles.x;
    }

    private void Update()
    {
        if (!m_PlayerCore.m_photonView.IsMine) return;
        UpdateChangeShoulderView();
        UpdateRotation();
    }

    private void LateUpdate()
    {
        if (!m_PlayerCore.m_photonView.IsMine) return;
        if (m_playerTr == null) return;
        UpdatePosition();
        UpdateAimTargetPos();
        AimFov();
    }

    private void UpdateChangeShoulderView()
    {
        float xOffset = m_PlayerCore.m_InputManager.m_IsChangeLeftView ? -m_pivotOffsetX : m_pivotOffsetX;
        m_pivotTr.localPosition = new Vector3(xOffset, 0f, 0f);
    }

    /// <summary>
    /// 위치 업데이트 및 충돌 감지시의 카메라 위치 조정
    /// </summary>
    private void UpdatePosition()
    {
        // 1. m_cameraRigTr를 m_target_FollowCamTr의 위치 추적 설정
        m_cameraRigTr.position = m_target_FollowCamTr.position;

        // 2. 충돌 감지
        // 프레임마다 카메라 위치가 변경됨에 따라 카메라의 포지션을 바로 옮기게 되면 튀는 현상이나 떨림 발생
        // 따라서 계산으로 미리 위치 선정
        Vector3 desiredCameraPos = m_pivotTr.position - (m_pivotTr.forward * m_maxCameraDis); //피벗에서 -m_maxCameraDis만큼의 카메라 글로벌 위치

        // 3. 피벗->카메라 방향
        Vector3 dir = desiredCameraPos - m_pivotTr.transform.position;

        // 4. 카메라와 Pivot사이의 장애물 체크
        float currentDistance = m_maxCameraDis; //기본 거리 저장
        RaycastHit hit;
        if (Physics.Raycast(m_pivotTr.position, dir.normalized, out hit, m_maxCameraDis, m_cameraMask))
        {
            currentDistance = Mathf.Clamp(hit.distance - 0.2f, m_minCameraDis, m_maxCameraDis); //Hit한 거리가 최소나 최대값이 넘지 않다록
        }

        // 5. 카메라 위치 보정
        Vector3 finalCameraPos = m_pivotTr.position - m_pivotTr.forward * currentDistance;
        m_mainCamera.transform.position = Vector3.Lerp(m_mainCamera.transform.position, finalCameraPos, Time.deltaTime * 10f);

        //Quaternion.LookRotation가 LookAt보다 더 직관적일 때가 있음
        m_mainCamera.transform.rotation = Quaternion.LookRotation(-dir.normalized, Vector3.up);
    }

    private void UpdateRotation()
    {
        //m_MouseX : 수평 회전(좌우)
        //m_MouseY : 수직 회전(상하)
        m_MouseX += m_PlayerCore.m_InputManager.m_LookInput_LocoM.x * m_rotationSensitivity * Time.deltaTime;
        m_MouseY += m_PlayerCore.m_InputManager.m_LookInput_LocoM.y * m_rotationSensitivity * Time.deltaTime;

        // 수직 회전 제한
        m_MouseY = Mathf.Clamp(m_MouseY, -m_downAngle, m_upAngle); //아래 각도는 음수전환이기에 -
        m_cameraRigTr.rotation = Quaternion.Euler(-m_MouseY, m_MouseX, 0f);
    }

    public Vector3 UpdateAimTargetPos()
    {
        float distance = m_maxAimDis;

        if (Physics.Raycast(m_pivotTr.position, m_pivotTr.forward, out RaycastHit hit, m_maxAimDis, m_aimMask))
        {
            distance = Mathf.Clamp(hit.distance, m_minCameraDis, m_maxAimDis);
        }

        Vector3 finalAimPos = m_pivotTr.position + m_pivotTr.forward * distance;
        m_target_AimTr.position = Vector3.Lerp(m_target_AimTr.position, finalAimPos, Time.deltaTime * aimPosSpeed);

        return m_target_AimTr.position;
    }

    private void AimFov()
    {
        m_mainCamera.fieldOfView = Mathf.Lerp(m_mainCamera.fieldOfView, m_PlayerCore.m_InputManager.m_IsAim ? m_aimMinFov : m_aimBaseFov, Time.deltaTime * 10f);
    }
}