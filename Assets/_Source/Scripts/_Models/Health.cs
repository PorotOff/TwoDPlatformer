using System;
using UnityEngine;

public class Health
{
    private int _value;

    public Health()
        => Reset();

    public event Action Changed;
    public event Action BecameZero;

    public int Max => 100;
    public int Value
    {
        get => _value;

        private set
        {
            _value = Mathf.Clamp(value, 0, Max);
            Changed?.Invoke();

            if (Value == 0)
                BecameZero?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
            Value -= damage;
    }

    public void Zeroize()
        => Value = 0;

    public void TakeHealth(int health)
    {
        if (health > 0)
            Value += health;
    }

    public void Reset()
        => Value = Max;
}