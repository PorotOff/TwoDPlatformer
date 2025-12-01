public interface ICollectibleVisitor
{
    public void Visit(Coin coin);
    public void Visit(HealthPotion healthPotion);
}