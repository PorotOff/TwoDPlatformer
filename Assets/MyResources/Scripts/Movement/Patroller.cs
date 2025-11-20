using UnityEngine;
using System.Collections.Generic;

public class Patroller
{
    private List<Transform> _waypoints;
    private Transform _currentWaypoint;
    private int _currentWaypointIndex = 0;

    public Patroller(List<Transform> waypoints)
    {
        _waypoints = waypoints;
        _currentWaypoint = _waypoints[_currentWaypointIndex];
    }

    public Vector3 CurrentWaypointPosition => _currentWaypoint.position;

    public void SetNextWaypoint()
    {
        _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Count;
        _currentWaypoint = _waypoints[_currentWaypointIndex];
    }
}