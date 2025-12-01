using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnEnable()
        => _player.Died += OnPlayerDied;

    private void OnDisable()
        => _player.Died -= OnPlayerDied;

    private void OnPlayerDied()
    {
        _player.transform.position = transform.position;
        _player.Initialize();
    }
}