using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public Rigidbody2D playerRB;
    PlayerBaseState currentState;

    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerJumpState jumpState;
    public PlayerDoubleJumpState doubleJumpState;
    public PlayerFallState fallState;
    public PlayerDashState dashState;
    public PlayerWallSlideState wallSlideState;
    public PlayerWallClimbState wallClimbState;
    public PlayerAttackState attackState;


    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();

        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        jumpState = new PlayerJumpState(this);
        fallState = new PlayerFallState(this);
        dashState = new PlayerDashState(this);
        doubleJumpState = new PlayerDoubleJumpState(this);
        wallSlideState = new PlayerWallSlideState(this);
        wallClimbState = new PlayerWallClimbState(this);
        attackState = new PlayerAttackState(this);
    }
    void Start()
    {
        currentState = idleState;
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState = newState;
        newState.EnterState();
    }

    public void OnAttackAnimationEnd()
    {
        currentState.OnAnimationEnd();
    }

    public void DisableControls()
    {
        currentState = idleState;
        currentState.EnterState();
        this.enabled = false;
    }

    public void EnableControls()
    {
        this.enabled = true;
    }
}
