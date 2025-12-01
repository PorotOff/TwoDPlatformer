using System;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public event Action Attacked;
    public event Action Damaged;
    public event Action Died;

    public void Attack()
        => Attacked?.Invoke();

    public void TakeDamage()
        => Damaged?.Invoke();
        
    public void Die()
        => Died?.Invoke();
}