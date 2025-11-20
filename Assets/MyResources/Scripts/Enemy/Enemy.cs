using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _targetReachRange = 0.1f;
    [SerializeField] private float _speed;

    private Flipper _flipper;
    private Mover _mover;
    private Patroller _patroller;

    private void Awake()
    {
        _flipper = new Flipper();
        _mover = new Mover(_targetReachRange);
        _patroller = new Patroller(_waypoints);
    }

    private void Start()
        => _flipper.Flip(transform, _patroller.CurrentWaypointPosition);

    private void Update()
        => _mover.Follow(transform, _patroller.CurrentWaypointPosition, _speed);
}