using UnityEngine;

public class PlayerAnimator : EntityAnimator
{
    private readonly int IsMove = Animator.StringToHash(nameof(IsMove));
    private readonly int IsIdle = Animator.StringToHash(nameof(IsIdle));
    private readonly int Idled = Animator.StringToHash(nameof(Idled));
    private readonly int Jumped = Animator.StringToHash(nameof(Jumped));
    private readonly int Attacked = Animator.StringToHash(nameof(Attacked));
    private readonly int Damaged = Animator.StringToHash(nameof(Damaged));

    public void StartMovement()
        => Animator.SetBool(IsMove, true);

    public void StopMovement()
        => Animator.SetBool(IsMove, false);

    public void Jump()
        => Animator.SetTrigger(Jumped);

    public void Idle(bool isGrounded)
    {
        if (isGrounded)
        {
            Animator.SetTrigger(Idled);
            Animator.SetBool(IsIdle, true);
        }
        else
        {
            Animator.SetBool(IsIdle, false);
        }
    }

    public void Attack()
        => Animator.SetTrigger(Attacked);

    public void TakeDamage()
        => Animator.SetTrigger(Damaged);
}