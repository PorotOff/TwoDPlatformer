using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class BarHealthIndicator : HealthIndicator
{
    protected Slider Slider;

    public override void Initialize(Health health)
    {
        base.Initialize(health);

        Slider = GetComponent<Slider>();

        Slider.minValue = 0f;
        Slider.maxValue = Health.Max;
        Slider.value = Health.Value;
    }

    public override void Display()
        => Slider.value = Health.Value;
}