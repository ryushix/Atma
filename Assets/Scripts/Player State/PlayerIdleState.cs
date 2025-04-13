using System;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        manager.playerMovement.isDashing = false;
        manager.playerMovement.dashedAfterJump = false;
        manager.playerMovement.hasDoubleJumped = false;
        
        manager.animator.Play("PlayerIdle");
        manager.playerMovement.StopMovement();
        Debug.Log("Current State : Idle");
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Math.Abs(moveInput) > 0.1)
            manager.SwitchState(manager.moveState);

        if (Input.GetKeyDown(KeyCode.Space))
            manager.SwitchState(manager.jumpState);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            manager.SwitchState(manager.dashState);
    }
}
