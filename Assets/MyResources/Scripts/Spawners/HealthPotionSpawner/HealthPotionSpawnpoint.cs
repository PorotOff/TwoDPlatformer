using UnityEngine;

public class HealthPotionSpawnpoint : MonoBehaviour
{
    private HealthPotion _healthPotion = null;

    public bool IsFree => _healthPotion == null;

    public void SetCollectible(HealthPotion healthPotion)
    {
        _healthPotion = healthPotion;
        _healthPotion.Collected += MakeFree;
    }

    private void MakeFree(HealthPotion healthPotion)
    {
        _healthPotion.Collected -= MakeFree;
        _healthPotion = null;
    }
}