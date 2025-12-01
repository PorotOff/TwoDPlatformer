using System;
using UnityEngine;

public abstract class Collectible : MonoBehaviour, ICollectible
{
    public event Action<Collectible> Collected;

    public virtual void Accept(ICollectibleVisitor visitor)
        => Collected?.Invoke(this);
}