using System;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        Debug.Log("Current State : Idle");
        
        manager.animator.Play("PlayerIdle");
        manager.playerMovement.StopMovement();
        manager.playerMovement.SetHighFriction();

        manager.playerMovement.isDashing = false;
        manager.playerMovement.dashedAfterJump = false;
        manager.playerMovement.hasDoubleJumped = false;
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (manager.playerMovement.isFalling())
        {
            manager.SwitchState(manager.fallState);
        }
        if (Math.Abs(moveInput) > 0.1)
            manager.SwitchState(manager.moveState);

        if (Input.GetKeyDown(KeyCode.Space))
            manager.SwitchState(manager.jumpState);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            manager.SwitchState(manager.dashState);
    }
}
