using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerWallSlideState : PlayerBaseState
{
    public PlayerWallSlideState(PlayerStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        Debug.Log("Current State: Wall Slide");
        manager.playerRB.gravityScale = 3f;
        manager.playerMovement.isWallSliding = true;
        manager.animator.Play("PlayerFall");

    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        int wallDir = manager.playerMovement.GetWallDirection();

        manager.playerMovement.WallSlide();

        if (manager.playerMovement.isGrounded())
        {
            manager.SwitchState(Mathf.Abs(moveInput) > 0.1f ? manager.moveState : manager.idleState);
            return;
        }


        if (Input.GetKeyDown(KeyCode.Space) && manager.playerMovement.IsTouchingWall())
        {
            if (moveInput == wallDir && manager.playerMovement.canWallClimb)
            {
                manager.SwitchState(manager.wallClimbState);
            }
            else if (moveInput == -wallDir && manager.playerMovement.canWallJump)
            {
                manager.SwitchState(manager.jumpState);
                return;
            }
            else if (manager.playerMovement.canWallClimb)
            {
                manager.SwitchState(manager.wallClimbState);
            }
        }

        if (!manager.playerMovement.IsTouchingWall())
        {
            manager.SwitchState(manager.fallState);
            return;
        }
    }
}
