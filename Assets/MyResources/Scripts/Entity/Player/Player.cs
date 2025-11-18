using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(PlayerAnimatorController))]
public class Player : Entity, ICollectibleVisitor
{
    [SerializeField] private float _jumpForce = 3f;

    private Rigidbody2D _rigidbody;
    private GroundChecker _groundChecker;

    private InputSystem _inputSystem;
    private Flipper _flipper;
    private PlayerAnimatorController _playerAnimatorController;

    private bool _canJump = false;
    private int _coins;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
        _playerAnimatorController = GetComponent<PlayerAnimatorController>();

        _inputSystem = new InputSystem();
        _flipper = new Flipper();

        _inputSystem.Game.Enable();
    }

    private void OnEnable()
    {
        _inputSystem.Game.Jump.performed += Jump;
        _inputSystem.Game.Movement.started += OnMovementStarted;
        _inputSystem.Game.Movement.canceled += OnMovementCanceled;
        _groundChecker.Grounded += OnGrounded;
    }

    private void OnDisable()
    {
        _inputSystem.Game.Jump.performed -= Jump;
        _inputSystem.Game.Movement.started -= OnMovementStarted;
        _inputSystem.Game.Movement.canceled -= OnMovementCanceled;
        _groundChecker.Grounded -= OnGrounded;
    }

    public override void Move()
    {
        Vector2 input = _inputSystem.Game.Movement.ReadValue<Vector2>();
        Vector2 velocity = new Vector2(input.x * Speed, _rigidbody.velocity.y);

        _rigidbody.velocity = velocity;
    }

    public void Visit(Coin coin)
        => _coins += coin.Value;

    private void Jump(InputAction.CallbackContext context)
    {
        if (_canJump)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
            _playerAnimatorController.OnJump();
        }
    }

    private void OnGrounded(bool isGrounded)
    {
        _canJump = isGrounded;
        _playerAnimatorController.OnGrounded(isGrounded);
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        Vector2 input = _inputSystem.Game.Movement.ReadValue<Vector2>();

        _flipper.Flip(transform, input);
        _playerAnimatorController.OnMovementStarted();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
        => _playerAnimatorController.OnMovementCanceled();
}