using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinsSpawner : MonoBehaviour
{
    [Header("Spawner settings")]
    [SerializeField] private List<CoinSpawnpoint> _spawnpoints;
    [SerializeField] private int _coinsOnLevelAmount;
    [Header("Pool settings")]
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private Transform _instancesContainer;

    private IObjectPool<Coin> _coinsPool;

    private void Awake()
        => _coinsPool = new ObjectPool<Coin>(OnPoolCreate, OnPoolGet, OnPooRelease, OnPoolDestroy);

    private void Start()
        => Spawn(_coinsOnLevelAmount);

    private Coin OnPoolCreate()
        => Instantiate(_coinPrefab, _instancesContainer);

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

        Coin coin = _coinsPool.Get();

        coin.transform.position = spawnpoint.transform.position;
        spawnpoint.SetCoin(coin);

        coin.Collected += OnCoinCollected;
    }

    private void OnCoinCollected(Coin coin)
    {
        coin.Collected -= OnCoinCollected;
        _coinsPool.Release(coin);
        Spawn();
    }

    // TO-DO: Реализовать сбор монеток.
    // TO-DO: Реализовать передвижение врагов, пока что от точки до точки (по заданию).
}