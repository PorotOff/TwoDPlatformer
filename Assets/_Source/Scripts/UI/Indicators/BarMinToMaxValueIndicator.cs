using UnityEngine;
using UnityEngine.UI;

public class BarMinToMaxValueIndicator : MinToMaxValueIndicator
{
    [SerializeField] protected Slider Slider;

    public override void Initialize(float min, float max, float current)
    {
        Slider.minValue = min;
        Slider.maxValue = max;

        base.Initialize(min, max, current);
    }

    public override void Display(float current)
        => Slider.value = current;

    public override void SetActive(bool isActive)
        => Slider.gameObject.SetActive(isActive);
}