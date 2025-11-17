public interface ICollectible
{
    public void Accept(ICollectibleVisitor visitor);
}