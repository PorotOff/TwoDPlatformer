using UnityEngine;

[RequireComponent(typeof(Player))]
public class CollectibleTriggerCollisionHandler : MonoBehaviour
{
    private Player _player;

    private void Awake()
        => _player = GetComponent<Player>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
            collectible.Accept(_player);
    }
}