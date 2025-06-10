using UnityEngine;

public class PlayerWallClimbState : PlayerBaseState
{
    private float climbDuration = 0.3f; // Durasi climb sebelum kembali ke slide
    private float climbTimer;
    
    public PlayerWallClimbState(PlayerStateManager manager) : base(manager) { }
    
    public override void EnterState()
    {
        Debug.Log("Current State : WallClimb");
        manager.playerRB.gravityScale = 1f; // Gravity lebih kecil saat climb
        manager.playerMovement.WallClimb();
        manager.animator.Play("PlayerJump");
        climbTimer = climbDuration;
        
    }

    public override void UpdateState()
    {
        climbTimer -= Time.deltaTime;

        float moveInput = Input.GetAxisRaw("Horizontal");
        manager.playerMovement.MoveInAir(moveInput);

        // Jika waktu climb habis atau tidak menyentuh dinding lagi, kembali ke slide atau fall
        if (climbTimer <= 0 || !manager.playerMovement.IsTouchingWall())
        {
            if (manager.playerMovement.IsTouchingWall())
            {
                manager.SwitchState(manager.wallSlideState);
            }
            else
            {
                manager.SwitchState(manager.fallState);
            }
        }
    }
}