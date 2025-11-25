using UnityEngine;

public class Stalker
{
    private Transform _transform;
    private Transform _target;
    private float _targetDetectionRange;

    public Stalker(Transform transform, Transform target, float targetDetectionRange)
    {
        _transform = transform;
        _target = target;
        _targetDetectionRange = targetDetectionRange;
    }

    public bool IsTargetEnoughClose()
        => Mathf.Abs(_target.position.x - _transform.position.x) <= _targetDetectionRange;
}