using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AbilityView))]
public class VampirismAbility : MonoBehaviour
{
    [SerializeField, Min(0f)] private int _takingHealthAmount = 5;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private AbilityConfig _abilityConfig;

    private AbilityView _abilityView;

    private IHealable _healable;
    private ComponentDetector<IDamageable> _damageableDetector;
    private Coroutine _coroutine;
    private bool _isEnabled = true;

    public void Initialize(IHealable healable)
    {
        _healable = healable;

        _abilityView = GetComponent<AbilityView>();
        _abilityView.Initialize(_abilityConfig);

        float abilityRadius = _abilityConfig.Range / 2f;
        _damageableDetector = new ComponentDetector<IDamageable>(transform, _layerMask, abilityRadius);
    }

    public void ActivateAbility()
    {
        if (_isEnabled)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Work());
        }
    }

    public void DeactivateAbility()
    {
        if (_coroutine != null)
                StopCoroutine(_coroutine);

        _isEnabled = true;
        _abilityView.DeactivateArea();
        _abilityView.Initialize(_abilityConfig.DurationSeconds, 0f);
    }

    private IEnumerator Work()
    {
        _isEnabled = false;
        float _spentWorkTimeSeconds = 0f;
        float _spentDetectionTimeSeconds = 0f;

        Detect();
        _abilityView.Initialize(_abilityConfig.DurationSeconds, _spentWorkTimeSeconds);
        _abilityView.ActivateArea();

        while (_spentWorkTimeSeconds < _abilityConfig.DurationSeconds)
        {
            _spentWorkTimeSeconds += Time.deltaTime;
            _spentDetectionTimeSeconds += Time.deltaTime;

            if (_spentDetectionTimeSeconds >= 1f)
            {
                Detect();
                _spentDetectionTimeSeconds = 0f;
            }

            _abilityView.Work(_spentWorkTimeSeconds);

            yield return null;
        }

        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        float _cooldownRemainingSeconds = _abilityConfig.CooldownSeconds;

        _abilityView.Initialize(_abilityConfig.CooldownSeconds, _cooldownRemainingSeconds);
        _abilityView.DeactivateArea();

        while (_cooldownRemainingSeconds > 0f)
        {
            _cooldownRemainingSeconds -= Time.deltaTime;
            _abilityView.Work(_cooldownRemainingSeconds);
            yield return null;
        }

        _isEnabled = true;
    }

    private void Detect()
    {
        if (_damageableDetector.TryDetectClosest(out IDamageable damageable))
        {
            damageable.TakeDamage(_takingHealthAmount);
            _healable.TakeHealth(_takingHealthAmount);
        }
    }
}