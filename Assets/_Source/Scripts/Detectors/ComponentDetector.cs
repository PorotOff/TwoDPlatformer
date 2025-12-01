using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ComponentDetector<T>
{
    private Transform _transform;
    private float _detectionRadius;
    private float _halfDetectionRadius;

    public ComponentDetector(Transform transform, float detectionRadius)
    {
        _transform = transform;  
        _detectionRadius = detectionRadius;
        _halfDetectionRadius = _detectionRadius / 2;
    }

    public event Action<T> Detected;
    public event Action NotDetected;

    public void Detect()
    {
        List<Collider2D> hits = Physics2D.OverlapCircleAll(_transform.position + (_transform.right * _halfDetectionRadius), _detectionRadius).ToList();

        foreach (var hit in hits)
        {
            if (hit.transform != _transform && hit.TryGetComponent(out T damageable))
            {
                Detected?.Invoke(damageable);
                return;
            }
        }

        NotDetected?.Invoke();
    }
}