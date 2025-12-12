using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AbyssDetector))]
public abstract class Entity : MonoBehaviour, IDamageable, IHealable, IAttacker
{
    [Header("Movement settings")]
    [SerializeField] protected float Speed;
    [Header("Health settings")]
    [SerializeField] protected AnimatedBarMinToMaxValueIndicator HealthIndicator;
    [Header("Attack settings")]
    [SerializeField] protected LayerMask AttackLayerMask;
    [SerializeField] protected float AttackRadius;
    [SerializeField] protected int Damage;

    protected Rigidbody2D Rigidbody;
    protected AbyssDetector AbyssDetector;

    protected Health Health;
    protected ComponentDetector<IDamageable> DamageableDetector;

    public event Action Died;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        AbyssDetector = GetComponent<AbyssDetector>();

        Health = new Health();

        float offset = AttackRadius / 2f;
        Vector2 offsetDirection = transform.forward;
        DamageableDetector = new ComponentDetector<IDamageable>(transform, AttackLayerMask, AttackRadius, offset, offsetDirection);

        HealthIndicator.Initialize(0, Health.Max, Health.Value);
    }

    protected virtual void OnEnable()
    {
        Health.Changed += OnHealthChanged;
        Health.BecameZero += OnHealthZero;

        AbyssDetector.Detected += OnAbyssDetected;
        DamageableDetector.Detected += Attack;
    }

    protected virtual void OnDisable()
    {
        Health.Changed -= OnHealthChanged;
        Health.BecameZero -= OnHealthZero;

        AbyssDetector.Detected -= OnAbyssDetected;
        DamageableDetector.Detected -= Attack;
    }

    public virtual void TakeDamage(int damage)
        => Health.TakeDamage(damage);

    public void TakeHealth(int health)
        => Health.TakeHealth(health);

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(Damage);

    public virtual void Reset()
    {
        HealthIndicator.SetActive(true);
        Health.Reset();
    }

    protected void DetectDamageable()
        => DamageableDetector.Detect();

    protected virtual void OnHealthZero()
    {
        gameObject.SetActive(false);
        HealthIndicator.SetActive(false);

        Died?.Invoke();
    }

    private void OnHealthChanged()
        => HealthIndicator.Initialize(0, Health.Max, Health.Value);

    private void OnAbyssDetected()
        => Health.Zeroize();
}