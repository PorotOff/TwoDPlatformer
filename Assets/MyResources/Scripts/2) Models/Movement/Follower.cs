using UnityEngine;

public class Follower
{
    private Transform _transform;
    private float _speed;

    public Follower(Transform transform, float speed)
    {
        _transform = transform;
        _speed = speed;
    }

    public void Follow(Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x, _transform.position.y, _transform.position.z);
        _transform.position = Vector3.MoveTowards(_transform.position, targetPosition, _speed * Time.deltaTime);
    }
}