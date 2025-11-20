using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int IsIdle = Animator.StringToHash(nameof(IsIdle));
    private readonly int IsMove = Animator.StringToHash(nameof(IsMove));
    private readonly int Jumped = Animator.StringToHash(nameof(Jumped));

    public void OnMovementStarted()
        => _animator.SetBool(IsMove, true);

    public void OnMovementCanceled()
        => _animator.SetBool(IsMove, false);

    public void OnJump()
        => _animator.SetTrigger(Jumped);

    public void OnGrounded(bool isGrounded)
    {
        if (isGrounded)
            _animator.SetBool(IsIdle, true);
        else
            _animator.SetBool(IsIdle, false);
    }
}