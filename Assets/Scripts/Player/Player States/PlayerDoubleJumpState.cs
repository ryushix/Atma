using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    public PlayerDoubleJumpState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        Debug.Log("Current State : DoubleJump");
        manager.playerRB.gravityScale = 3f;

        manager.animator.Play("PlayerJump");
        manager.playerMovement.DoubleJump();
        manager.playerMovement.SetLowFriction();
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        manager.playerMovement.VariableJump();

        if (!manager.playerMovement.isDashing)
        {
            manager.playerMovement.MoveInAir(moveInput);
        }
        if (!manager.playerMovement.isJumping)
        {
            if (manager.playerMovement.isGrounded())
            {
                if (Mathf.Abs(moveInput) > 0.1f)
                    manager.SwitchState(manager.moveState);
                else
                    manager.SwitchState(manager.idleState);
            }
            if (manager.playerMovement.IsTouchingWall())
            {
                manager.SwitchState(manager.wallSlideState);
            }
            if (manager.playerMovement.isFalling())
            {
                manager.SwitchState(manager.fallState);
            }
        }
        if (manager.playerMovement.isFalling())
        {
            manager.SwitchState(manager.fallState);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !manager.playerMovement.dashedAfterJump)
        {
            manager.SwitchState(manager.dashState);
        }
    }

}
