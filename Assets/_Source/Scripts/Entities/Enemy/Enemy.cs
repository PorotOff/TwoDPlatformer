using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(CertainFrequencyPlayerDetector))]
[RequireComponent(typeof(CertainFrequencyAttacker))]
public class Enemy : Entity
{
    [Header("Movement settings")]
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _waypointReachRange = 0.1f;
    [Header("Animations settings")]
    [SerializeField] private EnemyAnimationEvents _enemyAnimationEvents;

    private EnemyAnimator _enemyAnimator;
    private CertainFrequencyPlayerDetector _playerDetector;
    private CertainFrequencyAttacker _certainFrequencyAttacker;
    
    private Patroller _patroller;
    private Chaser _chaser;
    
    private Player _player;

    protected override void Awake()
    {
        base.Awake();
        
        _enemyAnimator = GetComponent<EnemyAnimator>();
        _playerDetector = GetComponent<CertainFrequencyPlayerDetector>();
        _certainFrequencyAttacker = GetComponent<CertainFrequencyAttacker>();
        
        _patroller = new Patroller(Rigidbody, Speed, _waypoints, _waypointReachRange);
        _chaser = new Chaser(Rigidbody, Speed, AttackRadius);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _certainFrequencyAttacker.Attacked += OnAttackerAttacked;
        _enemyAnimationEvents.Attacked += DetectDamageable;

        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerNotDetected += OnPlayerNotDetected;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _certainFrequencyAttacker.Attacked -= OnAttackerAttacked;
        _enemyAnimationEvents.Attacked -= DetectDamageable;

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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _enemyAnimator.TakeDamage();
    }

    private void OnAttackerAttacked()
        => _enemyAnimator.Attack();

    private void OnPlayerDetected(Player player)
        => _player = player;

    private void OnPlayerNotDetected()
        => _player = null;
}