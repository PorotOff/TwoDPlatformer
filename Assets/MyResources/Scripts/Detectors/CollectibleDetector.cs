using System;
using UnityEngine;

public class CollectibleDetector : MonoBehaviour
{
    public event Action<ICollectible> Detected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
            Detected?.Invoke(collectible);
    }
}