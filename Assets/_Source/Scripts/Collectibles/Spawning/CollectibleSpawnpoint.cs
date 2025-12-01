using UnityEngine;

public class CollectibleSpawnpoint : MonoBehaviour
{
    private Collectible _collectible = null;

    public bool IsFree => _collectible == null;

    public void SetCollectible(Collectible collectible)
    {
        if (IsFree)
        {
            _collectible = collectible;
            _collectible.Collected += MakeFree;
        }
    }

    private void MakeFree(Collectible collectible)
    {
        if (IsFree == false)
        {
            _collectible.Collected -= MakeFree;
            _collectible = null;
        }
    }
}