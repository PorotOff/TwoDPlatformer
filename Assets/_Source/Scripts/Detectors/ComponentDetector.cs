using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class ComponentDetector<T> where T : class
{
    private Transform _transform;
    private LayerMask _layerMask;
    private float _detectionRadius;
    private float _offset;
    private Vector3 _offsetDirection;

    public ComponentDetector(Transform transform, LayerMask layerMask, float detectionRadius)
    {
        _transform = transform;
        _layerMask = layerMask;
        _detectionRadius = detectionRadius;
        
        _offset = 0f;
        _offsetDirection = Vector3.zero;
    }

    public ComponentDetector(Transform transform, LayerMask layerMask, float detectionRadius, float offset, Vector3 offsetDirection) : this(transform, layerMask, detectionRadius)
    {
        _offset = offset;
        _offsetDirection = offsetDirection;
    }

    public event Action<T> Detected;
    public event Action NotDetected;

    public void Detect()
    {
        List<Collider2D> hits = GetHits();

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out T component))
            {
                Detected?.Invoke(component);
                return;
            }
        }

        NotDetected?.Invoke();
    }

    public bool TryDetectClosest(out T component)
    {
        List<Collider2D> hits = GetHits();

        if (hits.Count == 0)
        {
            component = null;
            return false;
        }

        Transform closestHit = hits[0].transform;

        for (int i = 1; i < hits.Count; i++)
        {
            Transform hit = hits[i].transform;

            if (hit.TryGetComponent<T>(out _))
            {
                float closestDistance = (_transform.position - closestHit.position).sqrMagnitude;
                float nextDistance = (_transform.position - hit.position).sqrMagnitude;

                if (nextDistance < closestDistance)
                    closestHit = hit;
            }
        }

        if (closestHit.TryGetComponent(out component))
        {
            Detected?.Invoke(component);
            return true;
        }
        else
        {
            NotDetected?.Invoke();
            return false;
        }
    }

    private List<Collider2D> GetHits()
    {
        List<Collider2D> hits = Physics2D.OverlapCircleAll(_transform.position + (_offsetDirection * _offset), _detectionRadius, _layerMask).ToList();
        return hits.Where(hit => hit.transform != _transform).ToList();
    }
}