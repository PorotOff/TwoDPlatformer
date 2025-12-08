using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedBarHealthIndicator : BarHealthIndicator
{
    [SerializeField] private float _interpolationStep = 0.1f;

    private Coroutine _coroutine;

    public override void Display()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(UpdateHealth());
    }

    private IEnumerator UpdateHealth()
    {
        while (Slider.value != Health.Value)
        {
            Slider.value = Mathf.Lerp(Slider.value, Health.Value, _interpolationStep);
            yield return null;
        }
    }
}