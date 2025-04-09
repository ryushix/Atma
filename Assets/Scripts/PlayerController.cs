using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool facingRight = true;
    private Collider2D playerCollider;
    private float moveInput;
    private bool isDashing;
    private float dashTimeLeft;
    private bool isGrounded;
    private bool hasDoubleJumped;
    private bool isTouchingWall;
    private int wallDirection;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public LayerMask groundLayer;
    public LayerMask platformLayer;

    [Header("Wall Jump Settings")]
    public float wallJumpForce = 8f;
    public float wallJumpPush = 5f;
    public float wallSlideSpeed = 2f;
    private bool isWallSliding;

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    private bool jumpedAfterDash;
    private float dashDirection;

    public PhysicsMaterial2D highFrictionMaterial;
    public PhysicsMaterial2D lowFrictionMaterial;


    private PlayerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        ChangeState(PlayerState.Idle);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer | platformLayer);

        bool touchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, 0.1f, groundLayer);
        bool touchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, 0.1f, groundLayer);

        isTouchingWall = touchingLeftWall || touchingRightWall;
        wallDirection = touchingLeftWall ? -1 : touchingRightWall ? 1 : 0;

        if (isGrounded)
        {
            hasDoubleJumped = false;
            isWallSliding = false;
            jumpedAfterDash = false;
            ChangeState(moveInput != 0 ? PlayerState.Walk : PlayerState.Idle);
        }

        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            ChangeState(PlayerState.WallSlide);
        }

        if (isDashing)
        {
            ContinueDash();
        }

        switch (currentState)
        {
            case PlayerState.Idle:
                if (moveInput != 0) ChangeState(PlayerState.Walk);
                if (Input.GetKeyDown(KeyCode.LeftShift)) StartDash();
                if (Input.GetKeyDown(KeyCode.Space)) Jump();
                break;

            case PlayerState.Walk:
                Move();
                if (moveInput == 0) ChangeState(PlayerState.Idle);
                if (Input.GetKeyDown(KeyCode.LeftShift)) StartDash();
                if (Input.GetKeyDown(KeyCode.Space)) Jump();
                break;

            case PlayerState.Jump:
                if (Input.GetKeyDown(KeyCode.Space) && !hasDoubleJumped) DoubleJump();
                MoveInAir();
                if (Input.GetKeyDown(KeyCode.LeftShift)) StartDash();
                break;

            case PlayerState.DoubleJump:
                MoveInAir();
                if (Input.GetKeyDown(KeyCode.LeftShift)) StartDash();
                break;

            case PlayerState.WallSlide:
                WallSlide();
                if (Input.GetKeyDown(KeyCode.Space)) WallJump();
                break;
        }

        if (Input.GetKeyDown(KeyCode.S) && IsOnPlatform()) 
        {
            StartCoroutine(DisablePlatformCollision());
        }
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            if (Mathf.Abs(moveInput) < 0.01f && !isDashing)
            {
                playerCollider.sharedMaterial = highFrictionMaterial;
            }
            else
            {
                playerCollider.sharedMaterial = lowFrictionMaterial;
            }
        }
        else
        {
            playerCollider.sharedMaterial = lowFrictionMaterial;
        }
    }


    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        FlipCharacter();
    }

    void MoveInAir()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
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
            ChangeState(PlayerState.Dash);
        }
    }

    void ContinueDash()
    {
        if (dashTimeLeft > 0)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
            dashTimeLeft -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && !jumpedAfterDash)
            {
                Jump();
            }
        }
        else
        {
            isDashing = false;
            ChangeState(moveInput != 0 ? PlayerState.Walk : PlayerState.Idle);
        }
    }




    void Jump()
    {
        if (isGrounded || currentState == PlayerState.Dash)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            ChangeState(PlayerState.Jump);
        }
        else if (!hasDoubleJumped && !jumpedAfterDash)
        {
            DoubleJump();
        }
    }

    void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // Jangan ganti state ke DoubleJump jika masih dash
        if (!isDashing)
            ChangeState(PlayerState.DoubleJump);

        hasDoubleJumped = true;
    }



    void WallSlide()
    {
        isWallSliding = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
    }

    void WallJump()
    {
        float jumpDir = -wallDirection;
        rb.linearVelocity = new Vector2(jumpDir * wallJumpPush, wallJumpForce);
        hasDoubleJumped = false;
        jumpedAfterDash = false;
        ChangeState(PlayerState.Jump);
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

    void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }

    bool IsOnPlatform()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, platformLayer);
    }

    System.Collections.IEnumerator DisablePlatformCollision()
    {
        Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, 0.1f, platformLayer);
        if (platform != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, true);
            yield return new WaitForSeconds(0.3f);
            Physics2D.IgnoreCollision(playerCollider, platform, false);
        }
    }
}
