using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "CONFIGURATIONS/Ability", order = 0)]
public class AbilityConfig : ScriptableObject
{
    [field: Header("View settings")]
    [field: SerializeField] public GameObject Area { get; private set; }
    [field: SerializeField, Min(0f)] public float Range { get; private set; } = 6f;
    [field: Header("Work settings")]
    [field: SerializeField, Min(0f)] public float DurationSeconds { get; private set; } = 6f;
    [field: SerializeField, Min(0f)] public float CooldownSeconds { get; private set; } = 4f;
    [field: SerializeField, Min(0f)] public float ResponseIntervalSeconds { get; private set; } = 1f;
}