using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Spawner settings")]
    [SerializeField] private List<CollectibleSpawnpoint> _spawnpoints;
    [SerializeField] private int _onLevelAmount;
    [Header("Pool settings")]
    [SerializeField] private Collectible _prefab;
    [SerializeField] private Transform _instancesContainer;

    private IObjectPool<Collectible> _pool;

    private void Awake()
        => _pool = new ObjectPool<Collectible>(OnPoolCreate, OnPoolGet, OnPooRelease, OnPoolDestroy);

    private void Start()
        => Spawn(_onLevelAmount);

    private Collectible OnPoolCreate()
        => Instantiate(_prefab, _instancesContainer);

    private void OnPoolGet(Collectible collectible)
        => collectible.gameObject.SetActive(true);

    private void OnPooRelease(Collectible collectible)
        => collectible.gameObject.SetActive(false);

    private void OnPoolDestroy(Collectible collectible)
        => Destroy(collectible.gameObject);

    private void Spawn(int amount)
    {
        for (int i = 0; i < amount; i++)
            Spawn();
    }

    private void Spawn()
    {
        List<CollectibleSpawnpoint> freeSpawnpoints = GetFreeSpawnpoints();

        if (freeSpawnpoints.Count > 0)
        {
            CollectibleSpawnpoint spawnpoint = freeSpawnpoints[Random.Range(0, freeSpawnpoints.Count - 1)];
            Collectible collectible = _pool.Get();

            collectible.transform.position = spawnpoint.transform.position;
            spawnpoint.SetCollectible(collectible);

            collectible.Collected += OnCollected;
        }
    }

    private List<CollectibleSpawnpoint> GetFreeSpawnpoints()
        => _spawnpoints.Where(spawnpoint => spawnpoint.IsFree).ToList();

    private void OnCollected(Collectible collectible)
    {
        collectible.Collected -= OnCollected;
        _pool.Release(collectible);
        Spawn();
    }
}