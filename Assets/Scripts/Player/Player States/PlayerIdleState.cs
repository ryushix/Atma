using System;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager manager) : base(manager) { }
    
    public override void EnterState()
    {
        // Debug.Log("Current State : Idle");
        manager.animator.Play("PlayerIdle");
        manager.playerMovement.SetHighFriction();
        manager.playerRB.gravityScale = 10f;

        manager.playerMovement.isDashing = false;
        manager.playerMovement.dashedAfterJump = false;
        manager.playerMovement.hasDoubleJumped = false;
    }

    public override void UpdateState()
    {
        // Debug.Log(manager.playerMovement.isGrounded());
        float moveInput = Input.GetAxisRaw("Horizontal");
        manager.playerMovement.TryDropPlatform();
        int wallDir = manager.playerMovement.GetWallDirection();

        if (Input.GetKeyDown(KeyCode.J))
        {
            manager.playerAttack.attackPushForce = 75f;
            manager.SwitchState(manager.attackState);
        }

        if (manager.playerMovement.isFalling())
        {
            manager.SwitchState(manager.fallState);
        }
        if (Math.Abs(moveInput) > 0.1)
            manager.SwitchState(manager.moveState);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (manager.playerMovement.IsTouchingWall())
            {
                manager.SwitchState(manager.jumpState);
            }
            else
            {
                manager.SwitchState(manager.jumpState);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && manager.playerMovement.canDash)
            manager.SwitchState(manager.dashState);
    }
}
