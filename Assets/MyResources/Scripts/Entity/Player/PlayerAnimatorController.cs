using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(GroundChecker))]
public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private string _isIdleParameter;
    [SerializeField] private string _isMoveParameter;
    [SerializeField] private string _jumpedParameter;
    [SerializeField] private Animator _animator;

    private Player _player;
    private GroundChecker _groundChecker;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void OnEnable()
    {
        _player.MovementStarted += OnMovementStarted;
        _player.MovementCanceled += OnMovementCanceled;
        _player.Jumped += OnJumped;
        _groundChecker.Grounded += OnGrounded;
    }

    private void OnDisable()
    {
        _player.MovementStarted -= OnMovementStarted;
        _player.MovementCanceled -= OnMovementCanceled;
        _player.Jumped -= OnJumped;
        _groundChecker.Grounded -= OnGrounded;
    }

    private void OnMovementStarted()
        => _animator.SetBool(_isMoveParameter, true);

    private void OnMovementCanceled()
        => _animator.SetBool(_isMoveParameter, false);

    private void OnJumped()
        => _animator.SetTrigger(_jumpedParameter);

    private void OnGrounded(bool isGrounded)
    {
        if (isGrounded)
            _animator.SetBool(_isIdleParameter, true);
        else
            _animator.SetBool(_isIdleParameter, false);
    }
}