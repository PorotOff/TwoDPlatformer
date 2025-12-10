using TMPro;
using UnityEngine;

public class TextMinToMaxValueIndicator : MinToMaxValueIndicator
{
    [SerializeField] private TextMeshProUGUI _text;      

    public override void Display(float current)
        => _text.text = $"{current}/{Max}";

    public override void SetActive(bool isActive)
        => _text.gameObject.SetActive(isActive);
}