using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private string _isIdleParameter;
    [SerializeField] private string _isMoveParameter;
    [SerializeField] private string _jumpedParameter;
    [SerializeField] private Animator _animator;

    public void OnMovementStarted()
        => _animator.SetBool(_isMoveParameter, true);

    public void OnMovementCanceled()
        => _animator.SetBool(_isMoveParameter, false);

    public void OnJump()
        => _animator.SetTrigger(_jumpedParameter);

    public void OnGrounded(bool isGrounded)
    {
        if (isGrounded)
            _animator.SetBool(_isIdleParameter, true);
        else
            _animator.SetBool(_isIdleParameter, false);
    }
}