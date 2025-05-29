using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        // Debug.Log("Current State : Attack");
        manager.playerRB.gravityScale = 10f;
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (manager.playerAttack.comboFinished)
        {
            manager.playerAttack.comboFinished = false; // Reset flag

            if (Mathf.Abs(moveInput) > 0.1f)
            {
                manager.SwitchState(manager.moveState);
            }
            else
            {
                manager.SwitchState(manager.idleState);
            }

            return;
        }

        if (manager.playerMovement.isGrounded())
        {
            if (manager.playerAttack.canAttack)
            {
                manager.playerAttack.TryAttack();
            }

            if (Mathf.Abs(moveInput) > 0.1f)
            {
                manager.SwitchState(manager.moveState);
            }
            else
            {
                manager.SwitchState(manager.idleState);
            }
        }
    }

}
