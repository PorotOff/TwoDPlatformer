using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float Speed;

    private void FixedUpdate()
        => Move();

    public abstract void Move();
}