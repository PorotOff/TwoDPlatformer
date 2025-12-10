using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(AbyssDetector))]
public abstract class Entity : MonoBehaviour, IDamageable, IHealable, IAttacker
{
    [Header("Movement settings")]
    [SerializeField] private float _speed;
    [Header("Health settings")]
    [SerializeField] private AnimatedBarMinToMaxValueIndicator _healthIndicator;
    [Header("Attack settings")]
    [SerializeField] private LayerMask _attackLayerMask;
    [SerializeField] private float _attackRadius;
    [SerializeField] private int _damage;

    private Rigidbody2D _rigidbody;
    private GroundDetector _groundDetector;
    private AbyssDetector _abyssDetector;

    private Mover _mover;
    private Flipper _flipper;
    private Health _health;
    private ComponentDetector<IDamageable> _damageableDetector;

    public event Action Died;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();
        _abyssDetector = GetComponent<AbyssDetector>();
        
        _mover = new Mover(_rigidbody, _speed);
        _flipper = new Flipper(transform);
        _health = new Health();
        _damageableDetector = new ComponentDetector<IDamageable>(transform, _attackLayerMask, _attackRadius);

        _healthIndicator.Initialize(0, _health.Max, _health.Value);
    }

    private void OnEnable()
    {
        _health.Changed += OnHealthChanged;
        _health.BecameZero += OnHealthZero;

        _groundDetector.Detected += OnGroundDetected;
        _abyssDetector.Detected += OnAbyssDetected;        
        _damageableDetector.Detected += Attack;
    }

    private void OnDisable()
    {
        _health.Changed -= OnHealthChanged;
        _health.BecameZero -= OnHealthZero;

        _groundDetector.Detected -= OnGroundDetected;
        _abyssDetector.Detected -= OnAbyssDetected;
        _damageableDetector.Detected -= Attack;
    }

    public virtual void TakeDamage(int damage)
        => _health.TakeDamage(damage);

    public void TakeHealth(int health)
        => _health.TakeHealth(health);

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);

    protected abstract void OnGroundDetected(bool isGrounded);

    private void OnHealthChanged()
        => _healthIndicator.Display(_health.Value);

    private void OnHealthZero()
        => Died?.Invoke();

    private void OnAbyssDetected()
        => _health.Zeroize();
}