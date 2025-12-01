using UnityEngine;

public class Jumper
{
    private Rigidbody2D _rigidbody;
    private float _jumpForce;

    public bool CanJump { get; private set; }

    public Jumper(Rigidbody2D rigidbody, float jumpForce)
    {
        _rigidbody = rigidbody;
        _jumpForce = jumpForce;

        CanJump = false;
    }

    public void Jump()
        => _rigidbody.AddForce(_rigidbody.transform.up * _jumpForce, ForceMode2D.Impulse);

    public void OnGrounded(bool isGrounded)
        => CanJump = isGrounded;
}