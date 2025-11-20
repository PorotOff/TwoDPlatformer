using System;
using UnityEngine;

public class Mover
{
    private float _waypointReachRange;

    public Mover() { }

    public Mover(float targetReachRange)
        => _waypointReachRange = targetReachRange;

    public event Action WaypointReached;

    public void Move(Rigidbody2D rigidbody, Vector2 velocity)
        => rigidbody.velocity = velocity;

    public void Follow(Transform currentTransform, Vector3 targetPosition, float speed)
    {
        currentTransform.position = Vector3.MoveTowards(currentTransform.position, targetPosition, speed * Time.deltaTime);

        if (currentTransform.position.IsEnoughClose(targetPosition, _waypointReachRange))
            WaypointReached?.Invoke();
    }
}