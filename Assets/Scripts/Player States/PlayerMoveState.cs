using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        Debug.Log("Current State : Move");
        manager.playerRB.gravityScale = 10f;

        manager.playerMovement.isDashing = false;
        manager.playerMovement.dashedAfterJump = false;
        manager.playerMovement.hasDoubleJumped = false;
        manager.playerMovement.SetLowFriction();
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        manager.playerMovement.Move(moveInput);
        manager.playerMovement.TryDropPlatform();

        if(manager.playerMovement.isFalling())
        {
            manager.SwitchState(manager.fallState);
        }
        if (manager.playerMovement.IsTouchingWall() && !manager.playerMovement.isGrounded())
        {
            manager.SwitchState(manager.wallSlideState);
        }
        if (Mathf.Abs(moveInput) < 0.1f)
        {
            manager.SwitchState(manager.idleState);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            manager.SwitchState(manager.jumpState);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            manager.SwitchState(manager.dashState);
        }
    }

}
