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
    [SerializeField] private PlayerAnimationEvents _playerAnimationEvents;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackRadius;

    private Rigidbody2D _rigidbody;
    private PlayerAnimator _playerAnimator;
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
        _wallet = new Wallet();
        _damageableDetector = new ComponentDetector<IDamageable>(transform, _attackRadius);

        Initialize();
    }

    private void OnEnable()
    {
        _inputService.Jumped += OnJump;
        _inputService.MovementStarted += OnMovementStarted;
        _inputService.MovementCanceled += _playerAnimator.StopMovement;
        _inputService.Attacked += _playerAnimator.Attack;

        _health.BecameZero += OnHealthZero;

        _groundDetector.Detected += OnGroundDetected;
        _collectibleDetector.Detected += OnCollectibleDetected;
        _abyssDetector.Detected += OnAbyssDetected;

        _playerAnimationEvents.Attacked += _damageableDetector.Detect;
        _damageableDetector.Detected += Attack;
    }

    private void OnDisable()
    {
        _inputService.Jumped -= OnJump;
        _inputService.MovementStarted -= OnMovementStarted;
        _inputService.MovementCanceled -= _playerAnimator.StopMovement;
        _inputService.Attacked -= _playerAnimator.Attack;

        _health.BecameZero -= OnHealthZero;

        _groundDetector.Detected -= OnGroundDetected;
        _collectibleDetector.Detected -= OnCollectibleDetected;
        _abyssDetector.Detected -= OnAbyssDetected;

        _playerAnimationEvents.Attacked -= _damageableDetector.Detect;
        _damageableDetector.Detected -= Attack;
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
    {
        _health.TakeDamage(damage);
        _playerAnimator.TakeDamage();
    }

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);

    private void OnHealthZero()
        => Died?.Invoke();

    private void OnMovementStarted()
    {
        _flipper.Flip(_inputService.HorizontalAxesValue);
        _playerAnimator.StartMovement();
    }

    private void OnJump()
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
        => _health.Die();

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (transform.right * (_attackRadius / 2)), _attackRadius);
    }
}