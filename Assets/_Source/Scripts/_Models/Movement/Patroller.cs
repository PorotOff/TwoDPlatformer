using UnityEngine;
using System.Collections.Generic;
using System;

public class Patroller : IDisposable
{
    private Rigidbody2D _rigidbody;
    private Chaser _chaser;
    private float _speed;

    private List<Transform> _waypoints;
    private Transform _currentWaypoint;
    private int _currentWaypointIndex;

    public Patroller(Rigidbody2D rigidbody, float speed, List<Transform> waypoints, float targetReachRange)
    {
        _rigidbody = rigidbody;
        _speed = speed;
        _waypoints = waypoints;

        _currentWaypointIndex = 0;
        _currentWaypoint = _waypoints[_currentWaypointIndex];
        _chaser = new Chaser(_rigidbody, _speed, targetReachRange);

        _chaser.TargetReached += SetNextWaypoint;
    }

    public Vector3 CurrentWaypointPosition => _currentWaypoint.position;

    public void Dispose()
        => _chaser.TargetReached -= SetNextWaypoint;

    public void Patrol()
        => _chaser.Chase((Vector2)_currentWaypoint.position);

    private void SetNextWaypoint()
    {
        _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Count;
        _currentWaypoint = _waypoints[_currentWaypointIndex];
    }
}