public class Coin : Collectible
{
    public int Value => 1;

    public override void Accept(ICollectibleVisitor visitor)
    {
        visitor.Visit(this);
        base.Accept(visitor);
    }
}