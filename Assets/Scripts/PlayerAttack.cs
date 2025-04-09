using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerAttackState currentAttackState = PlayerAttackState.Melee; // default
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAttackInput();
    }

    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // klik kiri
        {
            switch (currentAttackState)
            {
                case PlayerAttackState.Melee:
                    PerformMeleeAttack();
                    break;

                case PlayerAttackState.Ranged:
                    // PerformRangedAttack();
                    break;

                case PlayerAttackState.Magic:
                    // PerformMagicAttack();
                    break;
            }
        }
    }

    void PerformMeleeAttack()
    {
        Debug.Log("Melee Attack");
        if (animator != null)
        {
            animator.SetTrigger("MeleeAttack");
        }
    }
}
