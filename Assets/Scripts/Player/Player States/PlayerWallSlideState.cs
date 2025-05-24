using UnityEngine;

public class PlayerWallSlideState : PlayerBaseState
{
    public PlayerWallSlideState(PlayerStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        Debug.Log("Current State: Wall Slide");
        manager.playerMovement.isWallSliding = true;
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        manager.playerMovement.WallSlide();

        if (manager.playerMovement.isGrounded())
        {
            manager.SwitchState(Mathf.Abs(moveInput) > 0.1f ? manager.moveState : manager.idleState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            manager.playerMovement.WallJump();
            manager.SwitchState(manager.jumpState);
            return;
        }

        if (!manager.playerMovement.IsTouchingWall())
        {
            manager.SwitchState(manager.fallState);
            return;
        }
    }
}
