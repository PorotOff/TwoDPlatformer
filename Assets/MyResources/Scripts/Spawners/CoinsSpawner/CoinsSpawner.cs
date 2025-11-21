using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinsSpawner : MonoBehaviour
{
    [Header("Spawner settings")]
    [SerializeField] private List<CoinSpawnpoint> _spawnpoints;
    [SerializeField] private int _onLevelAmount;
    [Header("Pool settings")]
    [SerializeField] private Coin _prefab;
    [SerializeField] private Transform _instancesContainer;

    private IObjectPool<Coin> _pool;

    private void Awake()
        => _pool = new ObjectPool<Coin>(OnPoolCreate, OnPoolGet, OnPooRelease, OnPoolDestroy);

    private void Start()
        => Spawn(_onLevelAmount);

    private Coin OnPoolCreate()
        => Instantiate(_prefab, _instancesContainer);

    private void OnPoolGet(Coin coin)
        => coin.gameObject.SetActive(true);

    private void OnPooRelease(Coin coin)
        => coin.gameObject.SetActive(false);

    private void OnPoolDestroy(Coin coin)
        => Destroy(coin.gameObject);

    private void Spawn(int amount)
    {
        for (int i = 0; i < amount; i++)
            Spawn();
    }

    private void Spawn()
    {
        CoinSpawnpoint spawnpoint = null;

        do
        {
            int randomSpawnpointIndex = Random.Range(0, _spawnpoints.Count - 1);
            spawnpoint = _spawnpoints[randomSpawnpointIndex];
        }
        while (spawnpoint.IsFree == false);

        Coin coin = _pool.Get();

        coin.transform.position = spawnpoint.transform.position;
        spawnpoint.SetCollectible(coin);

        coin.Collected += OnCoinCollected;
    }

    private void OnCoinCollected(Coin coin)
    {
        coin.Collected -= OnCoinCollected;
        _pool.Release(coin);
        Spawn();
    }
}