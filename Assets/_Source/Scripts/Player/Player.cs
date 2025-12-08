using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(CollectibleDetector))]
[RequireComponent(typeof(AbyssDetector))]
public class Player : MonoBehaviour, ICollectibleVisitor, IDamageable, IAttacker
{
    [Header("Movement settings")]
    [SerializeField] private InputService _inputService;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 3f;
    [Header("Attack settings")]
    [SerializeField] private LayerMask _attackLayerMask;
    [SerializeField] private float _attackRadius;
    [SerializeField] private int _damage;
    [Header("Animations settings")]
    [SerializeField] private PlayerAnimationEvents _playerAnimationEvents;


    private Rigidbody2D _rigidbody;
    private PlayerAnimator _playerAnimator;
    [SerializeField] private AnimatedBarHealthIndicator _healthIndicator;
    private GroundDetector _groundDetector;
    private CollectibleDetector _collectibleDetector;
    private AbyssDetector _abyssDetector;

    private Mover _mover;
    private Jumper _jumper;
    private Flipper _flipper;
    private Health _health;
    private Wallet _wallet;
    private ComponentDetector<IDamageable> _damageableDetector;

    public event Action Died;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _groundDetector = GetComponent<GroundDetector>();
        _collectibleDetector = GetComponent<CollectibleDetector>();
        _abyssDetector = GetComponent<AbyssDetector>();
        
        _mover = new Mover(_rigidbody, _speed);
        _jumper = new Jumper(_rigidbody, _jumpForce);
        _flipper = new Flipper(transform);
        _health = new Health();
        _wallet = new Wallet();
        _damageableDetector = new ComponentDetector<IDamageable>(transform, _attackLayerMask, _attackRadius);

        _healthIndicator.Initialize(_health);
    }

    private void OnEnable()
    {
        _inputService.Jumped += OnInputJumped;
        _inputService.MovementStarted += OnInputMovementStarted;
        _inputService.MovementCanceled += OnInputMovementCanceled;
        _inputService.Attacked += OnInputAttacked;

        _health.Changed += OnHealthChanged;
        _health.BecameZero += OnHealthZero;

        _groundDetector.Detected += OnGroundDetected;
        _collectibleDetector.Detected += OnCollectibleDetected;
        _abyssDetector.Detected += OnAbyssDetected;

        _playerAnimationEvents.Attacked += OnPlayerAnimatorEventsAttacked;
        _damageableDetector.Detected += Attack;
    }

    private void OnDisable()
    {
        _inputService.Jumped -= OnInputJumped;
        _inputService.MovementStarted -= OnInputMovementStarted;
        _inputService.MovementCanceled -= OnInputMovementCanceled;
        _inputService.Attacked -= OnInputAttacked;

        _health.Changed -= OnHealthChanged;
        _health.BecameZero -= OnHealthZero;

        _groundDetector.Detected -= OnGroundDetected;
        _collectibleDetector.Detected -= OnCollectibleDetected;
        _abyssDetector.Detected -= OnAbyssDetected;

        _playerAnimationEvents.Attacked -= OnPlayerAnimatorEventsAttacked;
        _damageableDetector.Detected -= Attack;
    }

    private void FixedUpdate()
        => _mover.Move(_inputService.HorizontalAxesValue.x);

    public void Reset()
        => _health.Reset();

    public void Visit(Coin collectible)
        => _wallet.TakeCoins(collectible.Value);

    public void Visit(HealthPotion healthPotion)
        => _health.TakeHealth(healthPotion.HealthAmount);

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
        _playerAnimator.TakeDamage();
    }

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);

    private void OnHealthChanged()
        => _healthIndicator.Display();

    private void OnHealthZero()
        => Died?.Invoke();

    private void OnInputMovementStarted()
    {
        _flipper.Flip(_inputService.HorizontalAxesValue);
        _playerAnimator.StartMovement();
    }

    private void OnInputMovementCanceled()
        => _playerAnimator.StopMovement();

    private void OnInputAttacked()
        => _playerAnimator.Attack();

    private void OnInputJumped()
    {
        if (_jumper.CanJump)
        {
            _jumper.Jump();
            _playerAnimator.Jump();
        }
    }

    private void OnGroundDetected(bool isGrounded)
    {
        _jumper.OnGrounded(isGrounded);
        _playerAnimator.OnGrounded(isGrounded);
    }

    private void OnCollectibleDetected(ICollectible collectible)
        => collectible.Accept(this);

    private void OnAbyssDetected()
        => _health.Zeroize();

    private void OnPlayerAnimatorEventsAttacked()
        => _damageableDetector.Detect();
}