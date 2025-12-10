using System;
using System.Collections;
using UnityEngine;

public class CertainFrequencyAttacker : MonoBehaviour
{
    [SerializeField] private float _attackCooldownSeconds;

    private Coroutine _coroutine;
    private bool _isAttacking;

    public event Action Attacked;

    public void StartAttack()
    {
        if (_isAttacking == false)
        {
            _coroutine = StartCoroutine(Attack());
            _isAttacking = true;
        }
    }

    public void StopAttack()
    {
        if (_isAttacking && _coroutine != null)
        {
            StopCoroutine(_coroutine);
            _isAttacking = false;
        }
    }

    private IEnumerator Attack()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(_attackCooldownSeconds);

        while (enabled)
        {
            Attacked?.Invoke();
            yield return wait;
        }
    }
}