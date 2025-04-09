using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerAttackState currentAttackState = PlayerAttackState.Melee;

    void Update()
    {
        HandleAttackInput();
    }

    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (currentAttackState)
            {
                case PlayerAttackState.Melee:
                    MeleeAttack();
                    break;

                case PlayerAttackState.Ranged:
                    Debug.Log("Ranged Attack (belum diimplementasi)");
                    break;

                case PlayerAttackState.Magic:
                    Debug.Log("Magic Attack (belum diimplementasi)");
                    break;
            }
        }
    }

    void MeleeAttack()
    {
        Debug.Log("Melee Attack");
    }
}
