using UnityEngine;

public class CoinSpawnpoint : MonoBehaviour
{
    private Coin _coin = null;

    public bool IsFree => _coin == null;

    public void SetCollectible(Coin coin)
    {
        _coin = coin;
        _coin.Collected += MakeFree;
    }

    private void MakeFree(Coin coin)
    {
        _coin.Collected -= MakeFree;
        _coin = null;
    }
}