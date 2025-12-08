using UnityEngine;

public abstract class HealthIndicator : MonoBehaviour
{
    protected Health Health;

    public virtual void Initialize(Health health)
        => Health = health;

    public abstract void Display();

    public void Hide(bool isHide = true)
        => gameObject.SetActive(!isHide);
}