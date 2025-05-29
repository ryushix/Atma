using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        // Debug.Log("Enemy State: Patrol");

        manager.animator.Play("EnemyMove");
        manager.enemyMovement.SetLowFriction();
    }

    public override void UpdateState()
    {
        if (manager.enemyMovement.IsPlayerInRange())
        {
            manager.SwitchState(manager.chaseState);
        }
    }

    public override void FixedUpdateState()
    {
        manager.enemyMovement.Patrol();
    }
}
