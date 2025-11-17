using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundChecker))]
public class Player : Entity, ICollectibleVisitor
{
    [SerializeField] private float _jumpForce = 3f;

    private Rigidbody2D _rigidbody;
    private GroundChecker _groundChecker;

    private InputSystem _inputSystem;
    private Flipper _flipper;

    private bool _canJump = false;
    private int _coins;

    public event Action MovementStarted;
    public event Action MovementCanceled;
    public event Action Jumped;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();

        _inputSystem = new InputSystem();
        _flipper = new Flipper();

        _inputSystem.Game.Enable();
    }

    private void OnEnable()
    {
        _inputSystem.Game.Jump.performed += Jump;
        _inputSystem.Game.Movement.started += OnMovementStarted;
        _inputSystem.Game.Movement.canceled += OnMovementCanceled;
        _groundChecker.Grounded += SetJumpAbility;
    }

    private void OnDisable()
    {
        _inputSystem.Game.Jump.performed -= Jump;
        _inputSystem.Game.Movement.started -= OnMovementStarted;
        _inputSystem.Game.Movement.canceled -= OnMovementCanceled;
        _groundChecker.Grounded -= SetJumpAbility;
    }

    public override void Move()
    {
        Vector2 input = _inputSystem.Game.Movement.ReadValue<Vector2>();
        Vector2 velocity = new Vector2(input.x * Speed, _rigidbody.velocity.y);

        _rigidbody.velocity = velocity;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (_canJump)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
            Jumped?.Invoke();
        }
    }

    private void SetJumpAbility(bool canJump)
        => _canJump = canJump;

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        Vector2 input = _inputSystem.Game.Movement.ReadValue<Vector2>();
        _flipper.Flip(transform, input);
        MovementStarted?.Invoke();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
        => MovementCanceled?.Invoke();    

    public void Visit(Coin coin)
        => _coins += coin.Value;
}