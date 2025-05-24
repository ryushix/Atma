using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private Collider2D playerCollider;

    public Transform groundCheck;
    public Transform leftWallCheck;
    public Transform rightWallCheck;

    public LayerMask groundLayer;
    public LayerMask platformLayer;

    [Header("Movement Settings")]
    [SerializeField] private bool facingRight = true;
    public float moveSpeed = 7f;

    [Header("Jump Settings")]
    public bool hasDoubleJumped;
    public bool isTouchingWall;
    [SerializeField] private int wallDirection;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpPower = 8f;
    public float jumpDuration = 0.15f;
    private float jumpTimer;
    private bool isJumping;

    [Header("Wall Slide Settings")]
    public bool isWallSliding;
    [SerializeField] private float wallSlideSpeed = 2f;

    [Header("Dash Settings")]
    public bool isDashing = false;
    public bool dashedAfterJump;
    private float dashDirection;
    private Coroutine dashCoroutine;
    [SerializeField]private float dashDuration = 0.5f;
    [SerializeField]private float dashForce = 15f;

    [Header("Physics Material Settings")]
    public PhysicsMaterial2D highFrictionMaterial;
    public PhysicsMaterial2D lowFrictionMaterial;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerAnim = GetComponent<Animator>();

        groundCheck = transform.Find("GroundCheck");
        leftWallCheck = transform.Find("LeftWallCheck");
        rightWallCheck = transform.Find("RightWallCheck");

        groundLayer = LayerMask.GetMask("Ground");
        platformLayer = LayerMask.GetMask("Platform");
    }
    void Start()
    {
        
    }

    void Update()
    {
        // bool touchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, 0.1f, groundLayer);
        // bool touchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, 0.1f, groundLayer);

        // isTouchingWall = touchingLeftWall || touchingRightWall;
        // wallDirection = touchingLeftWall ? -1 : touchingRightWall ? 1 : 0;
    }

    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer | platformLayer);
    }

    public bool isFalling()
    {
        return playerRb.linearVelocity.y < -1f && !isGrounded();

    }
    public void Move(float moveInput)
    {
        playerRb.linearVelocity = new Vector2(moveInput * moveSpeed, playerRb.linearVelocity.y);
        playerAnim.Play("PlayerRun_Stump");
        FlipCharacter(moveInput);
    }

    public void StopMovement()
    {
        playerRb.linearVelocity = Vector2.zero;
    }

    public void MoveInAir(float moveInput)
    {
        playerRb.linearVelocity = new Vector2(moveInput * moveSpeed, playerRb.linearVelocity.y);
        FlipCharacter(moveInput);
    }

    public void Jump()
    {
        jumpTimer = jumpDuration;
        isJumping = true;

        if (isDashing)
        {
            float dashMove = (Mathf.Abs(dashDirection) > 0) ? dashDirection : (facingRight ? 1 : -1);
            playerRb.linearVelocity = new Vector2(dashMove * dashForce, jumpForce);
        }
        else
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
        }
    }

    public void DoubleJump()
    {
        jumpTimer = jumpDuration;
        isJumping = true;
        hasDoubleJumped = true;

        if (isDashing)
        {
            float dashMove = (Mathf.Abs(dashDirection) > 0) ? dashDirection : (facingRight ? 1 : -1);
            playerRb.linearVelocity = new Vector2(dashMove * dashForce, jumpForce);
        }
        else
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
        }
    }

    public void VariableJump()
    {
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimer > 0)
            {
                float moveInput = Input.GetAxisRaw("Horizontal");
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    public void Dash()
    {
        if (!isGrounded() && dashedAfterJump)
        {
            return;
        }

        isDashing = true;
        dashDirection = facingRight ? 1 : -1;

        if (!isGrounded())
        {
            dashedAfterJump = true;
        }
        
        if (dashCoroutine != null)
            StopCoroutine(dashCoroutine);

        dashCoroutine = StartCoroutine(PerformDash(isGrounded()));
    }

    private IEnumerator PerformDash(bool grounded)
    {
        isDashing = true;

        float originalGravity = playerRb.gravityScale;

        if (!grounded)
        {
            playerRb.gravityScale = originalGravity * 0.5f;
        }

        playerRb.linearVelocity = new Vector2(dashDirection * dashForce, playerRb.linearVelocity.y);

        yield return new WaitForSeconds(dashDuration);

        playerRb.gravityScale = originalGravity;
        isDashing = false;
    }


    public void MoveDuringDash(float moveInput)
    {
        if (!isDashing) return;

        float moveDirection = moveInput;

        if (Mathf.Abs(moveInput) < 0.1f)
        {
            moveDirection = facingRight ? 1 : -1;
        }

        playerRb.linearVelocity = new Vector2(moveDirection * dashForce, playerRb.linearVelocity.y);
        FlipCharacter(moveDirection);
    }

    public bool IsTouchingWall()
    {
        bool touchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, 0.1f, groundLayer);
        bool touchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, 0.1f, groundLayer);

        isTouchingWall = touchingLeftWall || touchingRightWall;
        wallDirection = touchingLeftWall ? -1 : touchingRightWall ? 1 : 0;

        return isTouchingWall;
    }

    public void WallSlide()
    {
        isWallSliding = true;
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, -wallSlideSpeed);
    }

    public void WallJump()
    {
        float jumpDir = -wallDirection;
        playerRb.linearVelocity = new Vector2(jumpDir * jumpPower, jumpForce);
        hasDoubleJumped = false;
        // dashedAfterJump = false;
        // ChangeState(PlayerMovementState.Jump);
    }

    public void TryDropPlatform()
    {
        if (Input.GetKeyDown(KeyCode.S) && IsOnPlatform())
        {
            StartCoroutine(DisablePlatformCollision());
        }
    }


    private bool IsOnPlatform()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, platformLayer);
    }

    private IEnumerator DisablePlatformCollision()
    {
        Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, 0.1f, platformLayer);
        if (platform != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, true);
            yield return new WaitForSeconds(0.3f);
            Physics2D.IgnoreCollision(playerCollider, platform, false);
        }
    }

    void FlipCharacter(float moveInput)
    {
        if ((moveInput < 0 && facingRight) || (moveInput > 0 && !facingRight))
        {
            facingRight = !facingRight;
            Vector2 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
        }
    }

    public void SetHighFriction()
    {
        if (playerCollider != null)
        {
            playerCollider.sharedMaterial = highFrictionMaterial;
        }
    }

    public void SetLowFriction()
    {
        if (playerCollider != null)
        {
            playerCollider.sharedMaterial = lowFrictionMaterial;
        }
    }
}