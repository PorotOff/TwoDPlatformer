using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AbyssDetector))]
[RequireComponent(typeof(CertainFrequencyPlayerDetector))]
[RequireComponent(typeof(CertainFrequencyAttacker))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(EnemyAnimationEvents))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Movement settings")]
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _waypointReachRange = 0.1f;
    [SerializeField] private float _speed;
    [Header("Attack settings")]
    [SerializeField] private float _attackRadius = 1;
    [SerializeField] private int _damage;
    
    private Rigidbody2D _rigidbody;
    private AbyssDetector _abyssDetector;
    private CertainFrequencyPlayerDetector _playerDetector;
    private CertainFrequencyAttacker _certainFrequencyAttacker;
    private EnemyAnimator _enemyAnimator;
    private EnemyAnimationEvents _enemyAnimationEvents;
    
    private Patroller _patroller;
    private Chaser _chaser;
    private Health _health;
    private ComponentDetector<IDamageable> _damageableDetector;
    
    private Player _player;

    private void Awake()
    {
        _abyssDetector = GetComponent<AbyssDetector>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerDetector = GetComponent<CertainFrequencyPlayerDetector>();
        _certainFrequencyAttacker = GetComponent<CertainFrequencyAttacker>();
        _enemyAnimator = GetComponent<EnemyAnimator>();
        _enemyAnimationEvents = GetComponent<EnemyAnimationEvents>();
        
        _patroller = new Patroller(_rigidbody, _speed, _waypoints, _waypointReachRange);
        _chaser = new Chaser(_rigidbody, _speed, _attackRadius);
        _health = new Health();
        _damageableDetector = new ComponentDetector<IDamageable>(transform, _attackRadius);
    }

    private void OnEnable()
    {
        _certainFrequencyAttacker.Attacked += OnAttackerAttacked;
        _enemyAnimationEvents.Attacked += OnAnimatorEventsAttacked;
        _damageableDetector.Detected += Attack;

        _health.BecameZero += OnHealthZero;
        _enemyAnimationEvents.Died += OnAnimatorEventsDied;
        _abyssDetector.Detected += Die;

        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerNotDetected += OnPlayerNotDetected;
    }

    private void OnDisable()
    {
        _certainFrequencyAttacker.Attacked -= OnAttackerAttacked;
        _enemyAnimationEvents.Attacked -= OnAnimatorEventsAttacked;
        _damageableDetector.Detected -= Attack;

        _health.BecameZero -= OnHealthZero;
        _enemyAnimationEvents.Died -= OnAnimatorEventsDied;
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
                _certainFrequencyAttacker.StartAttack();
            }
            else
            {
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

    private void Die()
        => gameObject.SetActive(false);

    private void OnAttackerAttacked()
        => _enemyAnimator.Attack();

    private void OnAnimatorEventsAttacked()
        => _damageableDetector.Detect();

    private void OnAnimatorEventsDied()
        => Die();

    private void OnHealthZero()
    {
        _enemyAnimator.Die();
        _certainFrequencyAttacker.StopAttack();
    }

    private void OnPlayerDetected(Player player)
        => _player = player;

    private void OnPlayerNotDetected()
        => _player = null;


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (transform.right * (_attackRadius / 2)), _attackRadius);
    }
    
    // TO-DO: Реализовать смерть по событию в анимации для врага и игрока (Нужно сделать так, чтобы враг и игрок после старта сметри не реагировали друг на друга).
}