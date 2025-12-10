using System.Collections;
using UnityEngine;

public class AnimatedBarMinToMaxValueIndicator : BarMinToMaxValueIndicator
{
    [SerializeField] private float _interpolationStep = 0.1f;

    private Coroutine _coroutine;

    public override void Display(float current)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(UpdateHealth());
    }

    private IEnumerator UpdateHealth()
    {
        while (Slider.value != Current)
        {
            Slider.value = Mathf.Lerp(Slider.value, Current, _interpolationStep);
            yield return null;
        }
    }
}