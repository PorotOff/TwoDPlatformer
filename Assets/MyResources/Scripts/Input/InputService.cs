using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour
{
    private InputSystem _inputSystem;

    public event Action Jumped;
    public event Action MovementStarted;
    public event Action MovementCanceled;

    public Vector2 HorizontalAxesValue => _inputSystem.Game.Movement.ReadValue<Vector2>();

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Game.Enable();
    }

    private void OnEnable()
    {
        _inputSystem.Game.Jump.performed += OnJump;
        _inputSystem.Game.Movement.started += OnMovementStarted;
        _inputSystem.Game.Movement.canceled += OnMovementCanceled;
    }

    private void OnJump(InputAction.CallbackContext context)
        => Jumped?.Invoke();

    private void OnMovementStarted(InputAction.CallbackContext context)
        => MovementStarted?.Invoke();
    
    private void OnMovementCanceled(InputAction.CallbackContext context)
        => MovementCanceled?.Invoke();
}