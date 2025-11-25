using UnityEngine;

public class Mover
{
    private Rigidbody2D _rigidbody;
    private float _speed;

    public Mover(Rigidbody2D rigidbody, float speed)
    {
        _rigidbody = rigidbody;
        _speed = speed;
    }

    public void Move(float xDirection)
    {
        Vector2 velocity = new Vector2(xDirection * _speed, _rigidbody.velocity.y);
        _rigidbody.velocity = velocity;
    }
}