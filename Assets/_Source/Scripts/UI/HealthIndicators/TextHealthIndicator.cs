using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextHealthIndicator : HealthIndicator
{
    private TextMeshProUGUI _text;

    public override void Initialize(Health health)
    {
        base.Initialize(health);
        _text = GetComponent<TextMeshProUGUI>();
    }

    public override void Display()
        => _text.text = $"{Health.Value}/{Health.Max}";
}