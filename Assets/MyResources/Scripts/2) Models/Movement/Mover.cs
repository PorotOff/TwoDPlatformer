using UnityEngine;

public class Mover
{
    private Rigidbody2D _rigidbody;

    private float _speed;
    private float _jumpForce;

    public bool CanJump { get; private set; }

    public Mover(Rigidbody2D rigidbody, float speed)
    {
        _rigidbody = rigidbody;
        _speed = speed;        
        CanJump = false;
    }

    public Mover(Rigidbody2D rigidbody, float speed, float jumpForce) : this (rigidbody, speed)
        => _jumpForce = jumpForce;

    public void Move(float xDirection)
    {
        Vector2 velocity = new Vector2(xDirection * _speed, _rigidbody.velocity.y);
        _rigidbody.velocity = velocity;
    }

    public void Jump()
        => _rigidbody.AddForce(_rigidbody.transform.up * _jumpForce, ForceMode2D.Impulse);

    public void OnGrounded(bool isGrounded)
        => CanJump = isGrounded;
}