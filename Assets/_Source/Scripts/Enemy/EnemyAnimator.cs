using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private readonly int IsMove = Animator.StringToHash(nameof(IsMove));
    private readonly int Attacked = Animator.StringToHash(nameof(Attacked));
    private readonly int Damaged = Animator.StringToHash(nameof(Damaged));
    private readonly int Died = Animator.StringToHash(nameof(Died));

    public void StartMovement()
    {
        if (_animator.GetBool(IsMove) == false)
            _animator.SetBool(IsMove, true);
    }

    public void StopMovement()
    {
        if (_animator.GetBool(IsMove))
            _animator.SetBool(IsMove, false);
    }

    public void Attack()
        => _animator.SetTrigger(Attacked);

    public void TakeDamage()
        => _animator.SetTrigger(Damaged);

    public void Die()
        => _animator.SetTrigger(Died);
}