using System;
using UnityEngine;

public class DamageableDetector : MonoBehaviour
{
    public event Action<IDamageable> Detected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            Detected?.Invoke(damageable);
    }
}