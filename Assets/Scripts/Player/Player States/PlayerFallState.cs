using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        // Debug.Log("Current State : Fall");
        manager.playerRB.gravityScale = 3f;
        
        manager.animator.Play("PlayerFall_Handed");
        manager.playerMovement.SetLowFriction();
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        manager.playerMovement.MoveInAir(moveInput);
        // Debug.Log(manager.playerMovement.isGrounded());
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
