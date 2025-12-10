using System;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public event Action Attacked;

    public void Attack()
        => Attacked?.Invoke();
}