using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private float recoveryTime = 0.5f;
    private float recoverTimer;
    private int damageAmount;
    private Vector2 _hitSource;
    public PlayerHitState(PlayerStateManager manager) : base(manager) { }

    public void SetupHit(int damage, Vector2 hitSource)
    {
        damageAmount = damage;
        _hitSource = hitSource;
    }
    public override void EnterState()
    {
        // Debug.Log("Current State: Hit");
        manager.playerHealth.TakeDamage(damageAmount, _hitSource);
        manager.playerRB.gravityScale = 3f;
        manager.animator.Play("PlayerHit");
        recoverTimer = recoveryTime;
    }

    public override void UpdateState()
    {
        recoverTimer -= Time.deltaTime;
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (recoverTimer <= 0f)
        {
            manager.playerMovement.EnableAbility();
            manager.playerMovement.EnableBasicMovements();
            // Decide next state based on movement
            if (manager.playerMovement.isFalling())
                manager.SwitchState(manager.fallState);

            if (manager.playerMovement.isGrounded())
            {
                if (Mathf.Abs(moveInput) > 0.1f)
                    manager.SwitchState(manager.moveState);
                else
                    manager.SwitchState(manager.idleState);
            }
        }
    }
}
