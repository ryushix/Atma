using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float lastInputTime;
    private bool attackPressed;
    private bool attackBuffered;

    public PlayerAttackState(PlayerStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        manager.playerRB.gravityScale = 10f;
        lastInputTime = Time.time;
        attackPressed = false;
        attackBuffered = false;
        manager.playerMovement.StopMovement();
        manager.playerMovement.DisableBasicMovements();
        manager.playerMovement.DisableAbility();
        // manager.playerMovement.SetHighFriction();


        manager.playerAttack.OnComboStepChanged += OnComboStepChanged;

        manager.playerAttack.StartCombo();
    }

    public override void UpdateState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Simpan input ke buffer
        if (Input.GetKeyDown(KeyCode.Return))
        {
            attackBuffered = true;
            lastInputTime = Time.time;
        }

        // Hanya lanjut combo saat timing animasi tepat
        if (attackBuffered && manager.playerAttack.CanContinueCombo())
        {
            attackBuffered = false;
            manager.playerAttack.ContinueCombo();
        }

        // Keluar state kalau animasi benar-benar selesai
        if (manager.playerAttack.IsComboOver())
        {
            manager.playerAttack.OnComboStepChanged -= OnComboStepChanged;
            
            manager.playerMovement.EnableBasicMovements();
            manager.playerMovement.EnableAbility();
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

    private void OnComboStepChanged(int comboStep)
    {
        if (comboStep == 2 || comboStep == 3)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveInput) > 0.1f)
            {
                manager.playerMovement.FlipCharacter(moveInput);
            }
        }
    }
}
