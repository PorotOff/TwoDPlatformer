using System;

public class Health
{
    private int _maxHealth = 100;

    public int HealthValue { get; private set; }
    public bool IsZero => HealthValue <= 0;

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
            }
            else
            {
                HealthValue = 0;
                BecameZero?.Invoke();
            }
        }
    }

    public void Heal(int healthAmount)
    {
        if (healthAmount > 0)
        {
            if (HealthValue + healthAmount < _maxHealth)
                HealthValue += healthAmount;
            else
                HealthValue = _maxHealth;
        }
    }
}