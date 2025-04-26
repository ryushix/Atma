using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateManager manager) : base(manager) { }
    public override void EnterState()
    {
        Debug.Log("Current State : Dash");

        manager.playerMovement.Dash();
        manager.playerMovement.SetLowFriction();
    }

    public override void UpdateState()
    {
        if(!manager.playerMovement.isDashing)
        {
            if (manager.playerMovement.isGrounded())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    manager.SwitchState(manager.jumpState);
                }
                
                float moveInput = Input.GetAxisRaw("Horizontal");
                if (Mathf.Abs(moveInput) > 0.1f)
                    manager.SwitchState(manager.moveState);
                else
                    manager.SwitchState(manager.idleState);
            }
            else if (!manager.playerMovement.isGrounded() && Input.GetKeyDown(KeyCode.Space) && !manager.playerMovement.hasDoubleJumped)
            {
                manager.SwitchState(manager.doubleJumpState);
            }
            
        }
    }
}
