using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        // Debug.Log("Current State : Jump");
        manager.playerRB.gravityScale = 3f;
        
        manager.playerMovement.Jump();
        manager.animator.Play("PlayerJump_Stump");
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

        if (manager.playerMovement.isFalling())
        {
            manager.SwitchState(manager.fallState);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !manager.playerMovement.hasDoubleJumped && manager.playerMovement.canDoubleJump)
        {
            manager.SwitchState(manager.doubleJumpState);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !manager.playerMovement.dashedAfterJump && manager.playerMovement.canDash)
        {
            manager.SwitchState(manager.dashState);
        }
    }

}
