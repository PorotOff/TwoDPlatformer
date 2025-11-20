using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(PlayerAnimator))]
public class Player : MonoBehaviour, ICollectibleVisitor
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 3f;

    private Rigidbody2D _rigidbody;
    private GroundChecker _groundChecker;
    private PlayerAnimator _playerAnimator;

    private InputSystem _inputSystem;
    private Flipper _flipper;
    private Mover _mover;

    private bool _canJump = false;
    private int _coins;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
        _playerAnimator = GetComponent<PlayerAnimator>();

        _inputSystem = new InputSystem();
        _flipper = new Flipper();
        _mover = new Mover();

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

    private void FixedUpdate()
        => Move();

    public void Move()
    {
        Vector2 input = _inputSystem.Game.Movement.ReadValue<Vector2>();
        Vector2 velocity = new Vector2(input.x * _speed, _rigidbody.velocity.y);

        _mover.Move(_rigidbody, velocity);
    }

    public void Visit(Coin coin)
        => _coins += coin.Value;

    private void Jump(InputAction.CallbackContext context)
    {
        if (_canJump)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
            _playerAnimator.OnJump();
        }
    }

    private void OnGrounded(bool isGrounded)
    {
        _canJump = isGrounded;
        _playerAnimator.OnGrounded(isGrounded);
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        Vector2 input = _inputSystem.Game.Movement.ReadValue<Vector2>();

        _flipper.Flip(transform, input);
        _playerAnimator.OnMovementStarted();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
        => _playerAnimator.OnMovementCanceled();
}