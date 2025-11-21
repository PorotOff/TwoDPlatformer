using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HealthPotionSpawner : MonoBehaviour
{
    [Header("Spawner settings")]
    [SerializeField] private List<HealthPotionSpawnpoint> _spawnpoints;
    [SerializeField] private int _onLevelAmount;
    [Header("Pool settings")]
    [SerializeField] private HealthPotion _prefab;
    [SerializeField] private Transform _instancesContainer;

    private IObjectPool<HealthPotion> _pool;

    private void Awake()
        => _pool = new ObjectPool<HealthPotion>(OnPoolCreate, OnPoolGet, OnPooRelease, OnPoolDestroy);

    private void Start()
        => Spawn(_onLevelAmount);

    private HealthPotion OnPoolCreate()
        => Instantiate(_prefab, _instancesContainer);

    private void OnPoolGet(HealthPotion healthPotion)
        => healthPotion.gameObject.SetActive(true);

    private void OnPooRelease(HealthPotion healthPotion)
        => healthPotion.gameObject.SetActive(false);

    private void OnPoolDestroy(HealthPotion healthPotion)
        => Destroy(healthPotion.gameObject);

    private void Spawn(int amount)
    {
        for (int i = 0; i < amount; i++)
            Spawn();
    }

    private void Spawn()
    {
        HealthPotionSpawnpoint spawnpoint = null;

        do
        {
            int randomSpawnpointIndex = Random.Range(0, _spawnpoints.Count - 1);
            spawnpoint = _spawnpoints[randomSpawnpointIndex];
        }
        while (spawnpoint.IsFree == false);

        HealthPotion healthPotion = _pool.Get();

        healthPotion.transform.position = spawnpoint.transform.position;
        spawnpoint.SetCollectible(healthPotion);

        healthPotion.Collected += OnCoinCollected;
    }

    private void OnCoinCollected(HealthPotion healthPotion)
    {
        healthPotion.Collected -= OnCoinCollected;
        _pool.Release(healthPotion);
        Spawn();
    }
}