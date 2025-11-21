using System;
using UnityEngine;

public class DamageableDetector : MonoBehaviour
{
    public event Action<IDamageable> Attacked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            Attacked?.Invoke(damageable);
    }
}