using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int IsIdle = Animator.StringToHash(nameof(IsIdle));
    private readonly int IsMove = Animator.StringToHash(nameof(IsMove));
    private readonly int Jumped = Animator.StringToHash(nameof(Jumped));
    private readonly int Attacked = Animator.StringToHash(nameof(Attacked));
    private readonly int Damaged = Animator.StringToHash(nameof(Damaged));
    private readonly int Died = Animator.StringToHash(nameof(Died));

    public void StartMovement()
        => _animator.SetBool(IsMove, true);

    public void StopMovement()
        => _animator.SetBool(IsMove, false);

    public void Jump()
        => _animator.SetTrigger(Jumped);

    public void OnGrounded(bool isGrounded)
    {
        if (isGrounded)
            _animator.SetBool(IsIdle, true);
        else
            _animator.SetBool(IsIdle, false);
    }

    public void Attack()
        => _animator.SetTrigger(Attacked);

    public void TakeDamage()
        => _animator.SetTrigger(Damaged);

    public void Die()
        => _animator.SetTrigger(Died);
}