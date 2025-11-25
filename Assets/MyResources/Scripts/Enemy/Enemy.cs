using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamageableDetector))]
[RequireComponent(typeof(AbyssDetector))]
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
    
    private DamageableDetector _damageableDetector;
    private AbyssDetector _abyssDetector;

    private Follower _follower;
    private Patroller _patroller;
    private Stalker _stalker;
    private Health _health;
    private Flipper _flipper;

    private void Awake()
    {
        _damageableDetector = GetComponent<DamageableDetector>();
        _abyssDetector = GetComponent<AbyssDetector>();

        _follower = new Follower(transform, _speed);
        _patroller = new Patroller(transform, _waypoints, _waypointReachRange);
        _stalker = new Stalker(transform, _targetPlayer, _targetPlayerDetectionRange);
        _health = new Health();
        _flipper = new Flipper();
    }

    private void OnEnable()
    {
        _health.BecameZero += OnDie;
        _damageableDetector.Detected += Attack;
        _abyssDetector.Detected += OnDie;
    }

    private void OnDisable()
    {
        _health.BecameZero -= OnDie;
        _damageableDetector.Detected -= Attack;
        _abyssDetector.Detected -= OnDie;
    }

    private void Start()
        => _flipper.Flip(transform, _patroller.CurrentWaypointPosition);

    private void Update()
    {
        if (_stalker.IsTargetEnoughClose())
        {
            OnPlayerDetected();               
        }
        else
        {
            OnPlayerNotDetected();

            if (_patroller.IsWaypointReached())
                OnWaypointReached();
        }
    }

    public void TakeDamage(int damage)
        => _health.TakeDamage(damage);

    public void Attack(IDamageable damageable)
        => damageable.TakeDamage(_damage);

    private void OnWaypointReached()
        => _patroller.SetNextWaypoint();

    private void OnPlayerNotDetected()
    {
        _follower.Follow(_patroller.CurrentWaypointPosition);
        _flipper.Flip(transform, _patroller.CurrentWaypointPosition);
    }

    private void OnPlayerDetected()
    {
        _follower.Follow(_targetPlayer.position);
        _flipper.Flip(transform, _targetPlayer.position);
    }

    private void OnDie()
        => gameObject.SetActive(false);
}