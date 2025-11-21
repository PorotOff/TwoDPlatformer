using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(DamageableDetector))]
public class Player : MonoBehaviour, ICollectibleVisitor, IDamageable, IAttacker
{
    [Header("Movement settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 3f;
    [Header("Attack settings")]
    [SerializeField] private int _damage;

    private Rigidbody2D _rigidbody;
    private GroundDetector _groundDetector;
    private PlayerAnimator _playerAnimator;
    private DamageableDetector _attacker;

    private InputSystem _inputSystem;
    private Flipper _flipper;
    private Mover _mover;
    private Health _health;
    private Wallet _wallet;

    public event Action Died;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _attacker = GetComponent<DamageableDetector>();

        _inputSystem = new InputSystem();
        _flipper = new Flipper();
        _mover = new Mover(_rigidbody, _speed, _jumpForce);
        _health = new Health();
        _wallet = new Wallet();

        _inputSystem.Game.Enable();
    }

    private void OnEnable()
    {
        _inputSystem.Game.Jump.performed += OnJump;
        _inputSystem.Game.Movement.started += OnMovementStarted;
        _inputSystem.Game.Movement.canceled += OnMovementCanceled;
        _groundDetector.Grounded += OnGrounded;
        _health.BecameZero += OnHealthZero;
        _attacker.Attacked += Attack;
    }

    private void OnDisable()
    {
        _inputSystem.Game.Jump.performed -= OnJump;
        _inputSystem.Game.Movement.started -= OnMovementStarted;
        _inputSystem.Game.Movement.canceled -= OnMovementCanceled;
        _groundDetector.Grounded -= OnGrounded;
        _health.BecameZero -= OnHealthZero;
        _attacker.Attacked -= Attack;
    }

    private void FixedUpdate()
        => _mover.Move(_inputSystem.Game.Movement.ReadValue<Vector2>().x);

    public void Visit(Coin coin)
        => _wallet.TakeCoins(coin.Value);

    public void Visit(HealthPotion healthPotion)
        => _health.Heal(healthPotion.HealthAmount);

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_mover.CanJump)
        {
            _mover.Jump();
            _playerAnimator.OnJump();
        }
    }

    private void OnGrounded(bool isGrounded)
    {
        _mover.OnGrounded(isGrounded);
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

    public void TakeDamage(int damage)
        => _health.TakeDamage(damage);

    private void OnHealthZero()
    {
        Died?.Invoke();
        Debug.Log("Died");
    }

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);
}