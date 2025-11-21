using UnityEngine;
using System.Collections.Generic;

public class Patroller
{
    private const float LeftDirection = -1;
    private const float RighDirection = 1;

    private Transform _transform;
    private List<Transform> _waypoints;
    private Transform _currentWaypoint;
    private int _currentWaypointIndex;
    private float _waypointReachRange;

    public Patroller(Transform transform, List<Transform> waypoints, float targetReachRange)
    {
        _transform = transform;
        _waypoints = waypoints;
        _waypointReachRange = targetReachRange;

        _currentWaypointIndex = 0;
        _currentWaypoint = _waypoints[_currentWaypointIndex];
    }

    public Vector3 CurrentWaypointPosition => _currentWaypoint.position;

    public float GetDirectionToWaypoint()
    {
        if (_currentWaypoint.position.x < _transform.position.x)
            return LeftDirection;
        else
            return RighDirection;
    }
    
    public bool IsWaypointReached()
        => Mathf.Abs(_currentWaypoint.position.x - _transform.position.x) <= _waypointReachRange;

    public void SetNextWaypoint()
    {
        _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Count;
        _currentWaypoint = _waypoints[_currentWaypointIndex];
    }
}