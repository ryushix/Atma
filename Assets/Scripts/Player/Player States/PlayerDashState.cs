using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        Debug.Log("Current State : Dash");
        manager.playerRB.gravityScale = 3f;

        manager.playerMovement.Dash();
        manager.playerMovement.SetLowFriction();
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // manager.playerMovement.MoveDuringDash(moveInput);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (manager.playerMovement.isGrounded())
            {
                manager.SwitchState(manager.jumpState);
            }
            else if (!manager.playerMovement.hasDoubleJumped)
            {
                manager.SwitchState(manager.doubleJumpState);
            }
        }

        if (!manager.playerMovement.isDashing)
        {
            if (manager.playerMovement.isGrounded())
            {
                manager.SwitchState(Mathf.Abs(moveInput) > 0.1f ? manager.moveState : manager.idleState);
            }
            else
            {
                manager.SwitchState(manager.fallState);
            }
        }
    }
}
