using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyStateManager manager) : base(manager) { }

    public override void EnterState()
    {
        // Debug.Log("Enemy State: Patrol");

        manager.animator.Play("Patrolling");
        manager.enemyMovement.SetLowFriction();

        var health = manager.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.ShowHealthUI(false);
        }
    }

    public override void UpdateState()
    {
        manager.enemyMovement.Patrol();
        if (manager.enemyMovement.IsPlayerInRange())
        {
            manager.SwitchState(manager.chaseState);
        }
    }
}
