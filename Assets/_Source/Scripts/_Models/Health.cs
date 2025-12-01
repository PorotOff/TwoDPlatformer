using System;
using UnityEngine;

public class Health
{
    private int _maxHealth = 100;

    public event Action BecameZero;

    public int HealthValue { get; private set; }

    public Health()
        => HealthValue = _maxHealth;

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            HealthValue = Mathf.Max(0, HealthValue - damage);

            if (HealthValue == 0)
                BecameZero?.Invoke();
        }
    }

    public void Die()
    {
        HealthValue = 0;
        BecameZero?.Invoke();
    }

    public void Heal(int healthAmount)
    {
        if (healthAmount > 0)
            HealthValue = Mathf.Min(_maxHealth, HealthValue + healthAmount);
    }
}