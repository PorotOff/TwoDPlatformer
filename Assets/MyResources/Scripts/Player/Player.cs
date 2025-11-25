using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(CollectibleDetector))]
[RequireComponent(typeof(DamageableDetector))]
[RequireComponent(typeof(AbyssDetector))]
public class Player : MonoBehaviour, ICollectibleVisitor, IDamageable, IAttacker
{
    [Header("Movement settings")]
    [SerializeField] private InputService _inputService;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 3f;
    [Header("Attack settings")]
    [SerializeField] private int _damage;

    private Rigidbody2D _rigidbody;
    private PlayerAnimator _playerAnimator;
    private GroundDetector _groundDetector;
    private CollectibleDetector _collectibleDetector;
    private DamageableDetector _damageableDetector;
    private AbyssDetector _abyssDetector;

    private Mover _mover;
    private Jumper _jumper;
    private Flipper _flipper;
    private Health _health;
    private Wallet _wallet;

    public event Action Died;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _groundDetector = GetComponent<GroundDetector>();
        _collectibleDetector = GetComponent<CollectibleDetector>();
        _damageableDetector = GetComponent<DamageableDetector>();
        _abyssDetector = GetComponent<AbyssDetector>();
        
        _mover = new Mover(_rigidbody, _speed);
        _jumper = new Jumper(_rigidbody, _jumpForce);
        _flipper = new Flipper();
        _wallet = new Wallet();

        Initialize();
    }

    private void OnEnable()
    {
        _inputService.Jumped += OnJump;
        _inputService.MovementStarted += OnMovementStarted;
        _inputService.MovementCanceled += OnMovementCanceled;

        _health.BecameZero += OnHealthZero;

        _groundDetector.Detected += OnGroundDetected;
        _collectibleDetector.Detected += OnCollectibleDetected;
        _damageableDetector.Detected += Attack;
        _abyssDetector.Detected += OnAbyssDetected;
    }

    private void OnDisable()
    {
        _inputService.Jumped -= OnJump;
        _inputService.MovementStarted -= OnMovementStarted;
        _inputService.MovementCanceled -= OnMovementCanceled;

        _health.BecameZero -= OnHealthZero;

        _groundDetector.Detected -= OnGroundDetected;
        _collectibleDetector.Detected -= OnCollectibleDetected;
        _damageableDetector.Detected -= Attack;
        _abyssDetector.Detected -= OnAbyssDetected;
    }

    private void FixedUpdate()
        => _mover.Move(_inputService.HorizontalAxesValue.x);

    public void Initialize()
        => _health = new Health();

    public void Visit(Coin collectible)
        => _wallet.TakeCoins(collectible.Value);

    public void Visit(HealthPotion healthPotion)
        => _health.Heal(healthPotion.HealthAmount);

    public void TakeDamage(int damage)
        => _health.TakeDamage(damage);

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);

    private void OnJump()
    {
        if (_jumper.CanJump)
        {
            _jumper.Jump();
            _playerAnimator.OnJump();
        }
    }

    private void OnGroundDetected(bool isGrounded)
    {
        _jumper.OnGrounded(isGrounded);
        _playerAnimator.OnGrounded(isGrounded);
    }

    private void OnCollectibleDetected(ICollectible collectible)
        => collectible.Accept(this);

    private void OnMovementStarted()
    {
        _flipper.Flip(transform, _inputService.HorizontalAxesValue);
        _playerAnimator.OnMovementStarted();
    }

    private void OnMovementCanceled()
        => _playerAnimator.OnMovementCanceled();

    private void OnHealthZero()
        => Died?.Invoke();

    private void OnAbyssDetected()
        => _health.Die();
}