public class VampirismAbility : Abillity
{
    private IDamageable _damageable;
    private IHealable _healable;
    private int _takingHealthAmount;

    public VampirismAbility(IDamageable damageable, IHealable healable, int takingHealthAmount)
    {
        _damageable = damageable;
        _healable = healable;
        _takingHealthAmount = takingHealthAmount;
    }

    public override void Work()
    {
        _damageable.TakeDamage(_takingHealthAmount);
        _healable.TakeHealth(_takingHealthAmount);
    }
}