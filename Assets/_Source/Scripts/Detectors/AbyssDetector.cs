using System;
using UnityEngine;

public class AbyssDetector : MonoBehaviour
{
    [SerializeField] private float _bottomYBorder = -10f;

    public event Action Detected;

    private void Update()
    {
        if (transform.position.y < _bottomYBorder)
            Detected?.Invoke();
    }
}