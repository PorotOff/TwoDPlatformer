using UnityEngine;

public class Stalker
{
    private const float LeftDirection = -1;
    private const float RighDirection = 1;

    private Transform _transform;
    private Transform _target;
    private float _targetDetectionRange;

    public Stalker(Transform transform, Transform target, float targetDetectionRange)
    {
        _transform = transform;
        _target = target;
        _targetDetectionRange = targetDetectionRange;
    }

    public float GetDirectionToTarget()
    {
        if (_target.position.x < _transform.position.x)
            return LeftDirection;
        else
            return RighDirection;
    }

    public bool IsTargetEnoughClose()
        => Mathf.Abs(_target.position.x - _transform.position.x) <= _targetDetectionRange;
}