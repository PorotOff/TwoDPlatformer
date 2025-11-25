using UnityEngine;

public class Wallet
{
    public int Coins { get; private set; }

    public Wallet()
        => Coins = 0;

    public void TakeCoins(int amount)
    {
        if (amount > 0)
            Coins += amount;

        Debug.Log($"Coins added. Now {Coins}");
    }

    public int GetCoins(int amount)
    {
        int coins = 0;

        if (amount > 0)
        {
            if (Coins - amount > 0)
            {
                coins = amount;
                Coins -= amount;
                Debug.Log($"Coins taken. Now {Coins}");
                return coins;
            }
            else
            {
                coins = Coins;
                Coins = 0;
                Debug.Log($"Coins taken. Now {Coins}");
                return coins;
            }
        }

        return coins;
    }
}