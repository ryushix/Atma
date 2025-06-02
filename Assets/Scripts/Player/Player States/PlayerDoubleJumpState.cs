using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    public PlayerDoubleJumpState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        // Debug.Log("Current State : DoubleJump");
        manager.playerRB.gravityScale = 3f;

        manager.animator.Play("PlayerJump_Stump");
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
