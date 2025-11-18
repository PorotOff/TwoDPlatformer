using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _targetReachRange = 0.1f;

    private Transform _currentWaypoint;
    private int _currentWaypointIndex = 0;

    private Flipper _flipper;

    private void Awake()
    {
        _flipper = new Flipper();
        _currentWaypoint = _waypoints[_currentWaypointIndex];
    }

    public override void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, Speed * Time.deltaTime);

        if (transform.position.IsEnoughClose(_currentWaypoint.position, _targetReachRange))
        {
            _flipper.Flip(transform, _currentWaypoint.position);
            SetNextWaypoint();
        }
    }

    public void SetNextWaypoint()
    {
        _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Count;
        _currentWaypoint = _waypoints[_currentWaypointIndex];
    }
}