using System;
using UnityEngine;

public class Health
{
    private int _maxHealth = 100;

    public int HealthValue { get; private set; }

    public event Action BecameZero;

    public Health()
        => HealthValue = _maxHealth;

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            if (HealthValue - damage > 0)
            {
                HealthValue -= damage;
                Debug.Log($"Damage taken. Now health: {HealthValue}");
            }
            else
            {
                Die();
            }
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
        {
            if (HealthValue + healthAmount < _maxHealth)
            {
                HealthValue += healthAmount;
                Debug.Log($"Health added. Now health: {HealthValue}");
            }
            else
            {
                HealthValue = _maxHealth;
                Debug.Log($"Now health is max: {HealthValue}");
            }
        }
    }
}