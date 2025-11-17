using UnityEngine;

public static class Vector3Extentions
{
    public static float SqrDistance(this Vector3 current, Vector3 target)
        => (target - current).sqrMagnitude;

    public static bool IsEnoughClose(this Vector3 current, Vector3 target, float range)
        => SqrDistance(current, target) <= range;
}