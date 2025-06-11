using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour, IInputService
{
    PlayerInputAC m_playerInput;


    public Vector2 m_MoveInput;
    public Vector2 m_LookInput;
    public event Action IsRun;
    public event Action IsCrouch;
    public event Action IsJump;
    public event Action IsDodge;
    public event Action IsSlide;
    public bool m_IsAttack { get; private set; }


    private void Awake()
    {
        m_playerInput = new PlayerInputAC();
        m_playerInput.Player.Enable();
    }


    private void OnEnable()
    {
        m_playerInput.Player.Move.performed += OnMove;
        m_playerInput.Player.Move.canceled += OnMove;
        m_playerInput.Player.Look.performed += OnLook;
        m_playerInput.Player.Look.canceled += OnLook;
        m_playerInput.Player.Run.performed += OnRun;
        //m_playerInput.Player.Run.canceled += OnRun;
        m_playerInput.Player.Crouch.performed += OnCrouch;
        //m_playerInput.Player.Crouch.canceled += OnCrouch;
        m_playerInput.Player.Jump.performed += OnJump;
        m_playerInput.Player.Dodge.performed += OnDodge;
        //m_playerInput.Player.Slide.performed += OnSlide;
        m_playerInput.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        m_playerInput.Player.Move.performed -= OnMove;
        m_playerInput.Player.Move.canceled -= OnMove;
        m_playerInput.Player.Run.performed -= OnRun;
        //m_playerInput.Player.Run.canceled -= OnRun;
        m_playerInput.Player.Crouch.performed -= OnCrouch;
        //m_playerInput.Player.Crouch.canceled -= OnCrouch;
        m_playerInput.Player.Jump.performed -= OnJump;
        m_playerInput.Player.Dodge.performed -= OnDodge;
        //m_playerInput.Player.Slide.performed -= OnSlide;
        m_playerInput.Player.Attack.performed -= OnAttack;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        m_MoveInput = context.ReadValue<Vector2>();
    }
    private void OnLook(InputAction.CallbackContext context)
    {
        m_LookInput = context.ReadValue<Vector2>();
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        IsRun?.Invoke();
    }
    private void OnCrouch(InputAction.CallbackContext context)
    {
        IsCrouch?.Invoke();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        IsJump?.Invoke();
    }
    private void OnDodge(InputAction.CallbackContext context)
    {
        IsDodge?.Invoke();
    }
    /*private void OnSlide(InputAction.CallbackContext context)
    {
        m_IsSlide = context.ReadValueAsButton();
    }*/
    private void OnAttack(InputAction.CallbackContext context)
    {
        m_IsAttack = context.ReadValueAsButton();
    }

}
