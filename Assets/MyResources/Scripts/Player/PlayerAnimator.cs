using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int _isIdle = Animator.StringToHash(nameof(_isIdle));
    private readonly int _isMove = Animator.StringToHash(nameof(_isMove));
    private readonly int _jumped = Animator.StringToHash(nameof(_jumped));

    public void OnMovementStarted()
        => _animator.SetBool(_isIdle, true);

    public void OnMovementCanceled()
        => _animator.SetBool(_isMove, false);

    public void OnJump()
        => _animator.SetTrigger(_jumped);

    public void OnGrounded(bool isGrounded)
    {
        if (isGrounded)
            _animator.SetBool(_isIdle, true);
        else
            _animator.SetBool(_isIdle, false);
    }
}