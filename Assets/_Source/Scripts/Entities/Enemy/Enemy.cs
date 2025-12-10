using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(AbyssDetector))]
[RequireComponent(typeof(CertainFrequencyPlayerDetector))]
[RequireComponent(typeof(CertainFrequencyAttacker))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Movement settings")]
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _waypointReachRange = 0.1f;
    [SerializeField] private float _speed;
    [Header("Health settings")]
    [SerializeField] private AnimatedBarMinToMaxValueIndicator _healthIndicator;
    [Header("Attack settings")]
    [SerializeField] private LayerMask _attackLayerMask;
    [SerializeField] private float _attackRadius = 1;
    [SerializeField] private int _damage;
    [Header("Animations settings")]
    [SerializeField] private EnemyAnimationEvents _enemyAnimationEvents;
    
    private Rigidbody2D _rigidbody;
    private EnemyAnimator _enemyAnimator;
    private AbyssDetector _abyssDetector;
    private CertainFrequencyPlayerDetector _playerDetector;
    private CertainFrequencyAttacker _certainFrequencyAttacker;
    
    private Patroller _patroller;
    private Chaser _chaser;
    private Health _health;
    private ComponentDetector<IDamageable> _damageableDetector;
    
    private Player _player;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<EnemyAnimator>();
        _abyssDetector = GetComponent<AbyssDetector>();
        _playerDetector = GetComponent<CertainFrequencyPlayerDetector>();
        _certainFrequencyAttacker = GetComponent<CertainFrequencyAttacker>();
        
        _patroller = new Patroller(_rigidbody, _speed, _waypoints, _waypointReachRange);
        _chaser = new Chaser(_rigidbody, _speed, _attackRadius);
        _health = new Health();
        _damageableDetector = new ComponentDetector<IDamageable>(transform, _attackLayerMask, _attackRadius);

        _healthIndicator.Initialize(0, _health.Max, _health.Value);
    }

    private void OnEnable()
    {
        _certainFrequencyAttacker.Attacked += OnAttackerAttacked;
        _enemyAnimationEvents.Attacked += OnAnimatorEventsAttacked;
        _damageableDetector.Detected += Attack;

        _health.Changed += OnHealthChanged;
        _health.BecameZero += Die;
        _abyssDetector.Detected += Die;

        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerNotDetected += OnPlayerNotDetected;
    }

    private void OnDisable()
    {
        _certainFrequencyAttacker.Attacked -= OnAttackerAttacked;
        _enemyAnimationEvents.Attacked -= OnAnimatorEventsAttacked;
        _damageableDetector.Detected -= Attack;

        _health.Changed -= OnHealthChanged;
        _health.BecameZero -= Die;
        _abyssDetector.Detected -= Die;

        _playerDetector.PlayerDetected -= OnPlayerDetected;
        _playerDetector.PlayerNotDetected -= OnPlayerNotDetected;

        _patroller.Dispose();
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            if (_chaser.IsTargetReached(_player.transform.position))
            {
                _enemyAnimator.StopMovement();
                _certainFrequencyAttacker.StartAttack();
            }
            else
            {
                _enemyAnimator.StartMovement();
                _certainFrequencyAttacker.StopAttack();
                _chaser.Chase(_player.transform.position);
            }
        }
        else
        {
            _patroller.Patrol();
        }
    }

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
        _enemyAnimator.TakeDamage();
    }

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);

    private void OnHealthChanged()
        => _healthIndicator.Display(_health.Value);

    private void Die()
    {
        gameObject.SetActive(false);
        _healthIndicator.SetActive(false);
    }

    private void OnAttackerAttacked()
        => _enemyAnimator.Attack();

    private void OnAnimatorEventsAttacked()
        => _damageableDetector.Detect();

    private void OnPlayerDetected(Player player)
        => _player = player;

    private void OnPlayerNotDetected()
        => _player = null;
}