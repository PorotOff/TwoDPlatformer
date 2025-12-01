using UnityEngine;

public class HealthPotion : Collectible
{
    [field: SerializeField] public int HealthAmount { get; private set; }

    public override void Accept(ICollectibleVisitor visitor)
    {
        visitor.Visit(this);
        base.Accept(visitor);
    }
}