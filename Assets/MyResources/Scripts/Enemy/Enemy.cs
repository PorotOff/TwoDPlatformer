using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageableDetector))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Movement settings")]
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _waypointReachRange = 0.1f;
    [SerializeField] private float _speed;
    [Header("Attack settings")]
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private float _targetPlayerDetectionRange = 10;
    [SerializeField] private int _damage;

    private Rigidbody2D _rigidbody;
    private DamageableDetector _attacker;

    private Flipper _flipper;
    private Mover _mover;
    private Patroller _patroller;
    private Stalker _stalker;
    private Health _health;

    private float _xMovementDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _attacker = GetComponent<DamageableDetector>();

        _flipper = new Flipper();
        _mover = new Mover(_rigidbody, _speed);
        _patroller = new Patroller(transform, _waypoints, _waypointReachRange);
        _stalker = new Stalker(transform, _targetPlayer, _targetPlayerDetectionRange);
        _health = new Health();

        _xMovementDirection = _patroller.GetDirectionToWaypoint();
    }

    private void OnEnable()
        => _attacker.Attacked += Attack;

    private void OnDisable()
        => _attacker.Attacked -= Attack;

    private void Start()
        => _flipper.Flip(transform, _patroller.CurrentWaypointPosition);

    private void FixedUpdate()
        => _mover.Move(_xMovementDirection);

    private void Update()
    {
        if (_stalker.IsTargetEnoughClose() == false)
        {
            if (_patroller.IsWaypointReached())
                OnWaypointReached();
        }
        else
        {
            OnPlayerDetected();
        }
    }

    private void OnWaypointReached()
    {
        _patroller.SetNextWaypoint();
        _flipper.Flip(transform, _patroller.CurrentWaypointPosition);

        _xMovementDirection = _patroller.GetDirectionToWaypoint();
    }

    private void OnPlayerDetected()
    {
        _xMovementDirection = _stalker.GetDirectionToTarget();
        _flipper.Flip(transform, _patroller.CurrentWaypointPosition);
    }

    public void TakeDamage(int damage)
        => _health.TakeDamage(damage);

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);
}