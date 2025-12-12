using UnityEngine;

public abstract class MinToMaxValueIndicator : MonoBehaviour
{
    protected float Min;
    protected float Max;

    public virtual void Initialize(float min, float max, float current)
    {
        Min = min;
        Max = max;

        Display(current);
    }

    public abstract void Display(float current);

    public abstract void SetActive(bool isActive);
}