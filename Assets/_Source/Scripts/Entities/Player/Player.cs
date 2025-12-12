using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(CollectibleDetector))]
[RequireComponent(typeof(VampirismAbility))]
public class Player : Entity, ICollectibleVisitor
{
    [Header("Movement settings")]
    [SerializeField] private InputService _inputService;
    [SerializeField] private float _jumpForce = 3f;
    [Header("Animations settings")]
    [SerializeField] private PlayerAnimationEvents _playerAnimationEvents;

    private PlayerAnimator _playerAnimator;
    private GroundDetector _groundDetector;
    private CollectibleDetector _collectibleDetector;
    private VampirismAbility _vampirismAbility;

    private Mover _mover;
    private Jumper _jumper;
    private Flipper _flipper;
    private Wallet _wallet;

    protected override void Awake()
    {
        base.Awake();

        _playerAnimator = GetComponent<PlayerAnimator>();
        _groundDetector = GetComponent<GroundDetector>();
        _collectibleDetector = GetComponent<CollectibleDetector>();
        _vampirismAbility = GetComponent<VampirismAbility>();
        
        _mover = new Mover(Rigidbody, Speed);
        _jumper = new Jumper(Rigidbody, _jumpForce);
        _flipper = new Flipper(transform);
        _wallet = new Wallet();

        _vampirismAbility.Initialize(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _inputService.Jumped += OnInputJumped;
        _inputService.MovementStarted += OnInputMovementStarted;
        _inputService.MovementCanceled += OnInputMovementCanceled;

        _inputService.Attacked += OnInputAttacked;
        _inputService.UsedAbility += OnInputUsedAbility;

        _groundDetector.Detected += OnGroundDetected;
        _collectibleDetector.Detected += OnCollectibleDetected;

        _playerAnimationEvents.Attacked += DetectDamageable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _inputService.Jumped -= OnInputJumped;
        _inputService.MovementStarted -= OnInputMovementStarted;
        _inputService.MovementCanceled -= OnInputMovementCanceled;

        _inputService.Attacked -= OnInputAttacked;
        _inputService.UsedAbility -= OnInputUsedAbility;

        _groundDetector.Detected -= OnGroundDetected;
        _collectibleDetector.Detected -= OnCollectibleDetected;

        _playerAnimationEvents.Attacked -= DetectDamageable;
    }

    private void FixedUpdate()
        => _mover.Move(_inputService.HorizontalAxesValue.x);

    public override void Reset()
    {
        base.Reset();
        _vampirismAbility.DeactivateAbility();
    }

    public void Visit(Coin collectible)
        => _wallet.TakeCoins(collectible.Value);

    public void Visit(HealthPotion healthPotion)
        => TakeHealth(healthPotion.HealthAmount);

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _playerAnimator.TakeDamage();
    }

    protected override void OnHealthZero()
    {
        _playerAnimator.Idle(true);
        base.OnHealthZero();
    }

    private void OnInputMovementStarted()
    {
        _flipper.Flip(_inputService.HorizontalAxesValue);
        _playerAnimator.StartMovement();
    }

    private void OnInputMovementCanceled()
        => _playerAnimator.StopMovement();

    private void OnInputAttacked()
        => _playerAnimator.Attack();

    private void OnInputUsedAbility()
        => _vampirismAbility.ActivateAbility();

    private void OnInputJumped()
    {
        if (_jumper.CanJump)
        {
            _jumper.Jump();
            _playerAnimator.Jump();
        }
    }

    private void OnGroundDetected(bool isGrounded)
    {
        _jumper.OnGrounded(isGrounded);
        _playerAnimator.Idle(isGrounded);
    }

    private void OnCollectibleDetected(ICollectible collectible)
        => collectible.Accept(this);

    // TO-DO: Исправить баг с аниматором при респауне игрока.
    // TO-DO: Исправить баг с индикатором и полем способности, чтобы после смерти они нормально восстанавливались.
}