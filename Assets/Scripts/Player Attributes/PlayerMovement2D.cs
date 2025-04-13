using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private Collider2D playerCollider;

    public Transform groundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;

    public LayerMask groundLayer;
    public LayerMask platformLayer;

    private PlayerMovementState currentState;

    [SerializeField]private bool isGrounded;

    [Header("Movement Settings")]
    private float moveInput;
    [SerializeField] private bool facingRight = true;
    public float moveSpeed = 4f;

    [Header("Jump Settings")]
    [SerializeField] private bool hasDoubleJumped;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private int wallDirection;
    [SerializeField] private float jumpForce = 10f;

    [Header("Wall Jump Settings")]
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private float wallJumpDistance = 5f;
    [SerializeField] private bool isWallSliding;

    [Header("Wall Slide Settings")]
    [SerializeField] private float wallSlideSpeed = 2f;

    [Header("Dash Settings")]
    public bool isDashing;
    private float dashTimeLeft;
    [SerializeField]private float dashSpeed = 10f;
    [SerializeField]private float dashDuration = 0.5f;
    [SerializeField]private bool jumpedAfterDash;
    [SerializeField]private float dashDirection;

    [Header("Physics Material Settings")]
    public PhysicsMaterial2D highFrictionMaterial;
    public PhysicsMaterial2D lowFrictionMaterial;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerAnim = GetComponent<Animator>();
    }
    
    void Move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        playerRb.linearVelocity = new Vector2(moveInput * moveSpeed, playerRb.linearVelocity.y);
        FlipCharacter();
    }

    void MoveInAir()
    {
        playerRb.linearVelocity = new Vector2(moveInput * moveSpeed, playerRb.linearVelocity.y);
        FlipCharacter();
    }
    
    void StartDash()
    {
        if (!isDashing)
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            dashDirection = facingRight ? 1 : -1;
            if (!isGrounded) jumpedAfterDash = true;
            // ChangeState(PlayerMovementState.Dash);
        }
    }

    void ContinueDash()
    {
        if (dashTimeLeft > 0)
        {
            playerRb.linearVelocity = new Vector2(dashDirection * dashSpeed, playerRb.linearVelocity.y);
            dashTimeLeft -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && !jumpedAfterDash)
            {
                Jump();
            }
        }
        else
        {
            isDashing = false;
            // ChangeState(moveInput != 0 ? PlayerMovementState.Walk : PlayerMovementState.Idle);
        }
    }

    void Jump()
    {
        if (isGrounded || currentState == PlayerMovementState.Dash)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
            // ChangeState(PlayerMovementState.Jump);
        }
        else if (!hasDoubleJumped && !jumpedAfterDash)
        {
            DoubleJump();
        }
    }

    void DoubleJump()
    {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);

        // if (!isDashing)ChangeState(PlayerMovementState.DoubleJump);

        hasDoubleJumped = true;
    }



    void WallSlide()
    {
        isWallSliding = true;
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, -wallSlideSpeed);
    }

    void WallJump()
    {
        float jumpDir = -wallDirection;
        playerRb.linearVelocity = new Vector2(jumpDir * wallJumpDistance, wallJumpForce);
        hasDoubleJumped = false;
        jumpedAfterDash = false;
        // ChangeState(PlayerMovementState.Jump);
    }

    void FlipCharacter()
    {
        if ((moveInput < 0 && facingRight) || (moveInput > 0 && !facingRight))
        {
            facingRight = !facingRight;
            Vector2 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
        }
    }
}