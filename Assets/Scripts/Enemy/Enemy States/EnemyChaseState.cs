using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        Debug.Log("Enemy State: Chase");

        manager.animator.Play("EnemyMove");
        manager.enemyMovement.SetLowFriction();
    }

    public override void UpdateState()
    {
        if (!manager.enemyMovement.IsPlayerInRange())
        {
            manager.SwitchState(manager.patrolState);
        }
    }

    public override void FixedUpdateState()
    {
        manager.enemyMovement.Chase();
    }
}
