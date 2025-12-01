using System;
using UnityEngine;

public class Chaser
{
    private Rigidbody2D _rigidbody;
    private float _speed;
    private Mover _mover;
    private Flipper _flipper;

    private float _targetDetectionRange;

    public Chaser(Rigidbody2D rigidbody, float speed, float targetDetectionRange)
    {
        _rigidbody = rigidbody;
        _speed = speed;
        _targetDetectionRange = targetDetectionRange;

        _mover = new Mover(_rigidbody, _speed);

        bool isInverted = true;
        _flipper = new Flipper(_rigidbody.transform, isInverted);
    }

    public event Action TargetReached;

    public void Chase(Vector2 target)
    {
        Vector2 direction = (target - _rigidbody.position).normalized;
        float xDirection = Mathf.Sign(direction.x);

        _mover.Move(xDirection);
        _flipper.LookAt(target);

        if (IsTargetReached(target))
            TargetReached?.Invoke();
    }

    public bool IsTargetReached(Vector2 target)
        => Mathf.Abs(target.x - _rigidbody.position.x) <= _targetDetectionRange;
}