using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        Debug.Log("Current State : Fall");
        manager.animator.Play("PlayerFall");
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        manager.playerMovement.MoveInAir(moveInput);
        if (manager.playerMovement.isGrounded())
        {            
            if (Mathf.Abs(moveInput) > 0.1f)
                manager.SwitchState(manager.moveState);
            else
                manager.SwitchState(manager.idleState);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !manager.playerMovement.hasDoubleJumped)
        {
            manager.SwitchState(manager.doubleJumpState);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !manager.playerMovement.dashedAfterJump)
        {
            manager.SwitchState(manager.dashState);
        }
    }
}
