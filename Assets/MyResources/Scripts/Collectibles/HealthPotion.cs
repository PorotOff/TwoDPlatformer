using System;
using UnityEngine;

public class HealthPotion : MonoBehaviour, ICollectible
{
    [field: SerializeField] public int HealthAmount { get; private set; }

    public event Action<HealthPotion> Collected;

    public void Accept(ICollectibleVisitor visitor)
    {
        visitor.Visit(this);
        Collected?.Invoke(this);
    }
}