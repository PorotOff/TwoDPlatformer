using System;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public int Value => 1;

    public event Action<Coin> Collected;

    public void Accept(ICollectibleVisitor visitor)
    {
        visitor.Visit(this);
        Collected?.Invoke(this);
    }
}