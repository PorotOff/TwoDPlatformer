using UnityEngine;

public class AbilityView : MonoBehaviour
{
    [SerializeField] private MinToMaxValueIndicator _actionIndicator;
    
    private GameObject _area;

    public void Initialize(AbilityConfig abilityConfig)
    {
        Initialize(abilityConfig.DurationSeconds, 0f);

        _area = Instantiate(abilityConfig.Area, transform);

        _area.transform.localScale = new Vector3(abilityConfig.Range, abilityConfig.Range, abilityConfig.Range);
        DeactivateArea();
    }

    public void Initialize(float maxSeconds, float currentSeconds)
        => _actionIndicator.Initialize(0f, maxSeconds, currentSeconds);

    public void Work(float currentSeconds)
        => _actionIndicator.Display(currentSeconds);

    public void ActivateArea()
        => _area.SetActive(true);

    public void DeactivateArea()
        => _area.SetActive(false);
}