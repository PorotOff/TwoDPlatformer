public class Wallet
{
    public int Coins { get; private set; }

    public Wallet()
        => Coins = 0;

    public void TakeCoins(int amount)
    {
        if (amount > 0)
            Coins += amount;
    }
}