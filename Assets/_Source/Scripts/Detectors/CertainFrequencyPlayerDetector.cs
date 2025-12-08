using System;
using System.Collections;
using UnityEngine;

public class CertainFrequencyPlayerDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _detectingLayerMask;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private float _detectionFrequencySeconds;

    private ComponentDetector<Player> _playerDetector;

    private Coroutine _coroutine;

    public event Action<Player> PlayerDetected;
    public event Action PlayerNotDetected;

    private void Awake()
        => _playerDetector = new ComponentDetector<Player>(transform, _detectingLayerMask, _detectionRadius);

    private void OnEnable()
    {
        _playerDetector.Detected += OnComponentDetected;
        _playerDetector.NotDetected += OnComponentNotDetected;

        _coroutine = StartCoroutine(DetectWithFrequency());
    }

    private void OnDisable()
    {
        _playerDetector.Detected -= OnComponentDetected;   
        _playerDetector.NotDetected -= OnComponentNotDetected;
        
        StopCoroutine(_coroutine);
    }

    public IEnumerator DetectWithFrequency()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(_detectionFrequencySeconds);

        while (enabled)
        {
            _playerDetector.Detect();
            yield return wait;
        }
    }

    private void OnComponentDetected(Player player)
        => PlayerDetected?.Invoke(player);

    private void OnComponentNotDetected()
        => PlayerNotDetected?.Invoke();
}