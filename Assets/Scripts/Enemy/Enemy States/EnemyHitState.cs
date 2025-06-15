using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    private float recoveryTime = 0.5f;
    private float recoverTimer;
    private int damageAmount;
    private Vector2 _hitSource;
    public EnemyHitState(EnemyStateManager manager) : base(manager) { }

    public void SetupHit(int damage, Vector2 hitSource)
    {
        _hitSource = hitSource;
        damageAmount = damage;
    }
    public override void EnterState()
    {
        manager.animator.Play("EnemyMove");
        manager.enemyMovement.SetLowFriction();
        recoverTimer = recoveryTime;

        var health = manager.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.ShowHealthUI(true);
        }
    }

    public override void UpdateState()
    {
        recoverTimer -= Time.deltaTime;
        if (recoverTimer <= 0)
        {
            if (!manager.enemyMovement.IsPlayerInRange())
            {
                manager.SwitchState(manager.patrolState);
            }
            else
            {
                manager.SwitchState(manager.chaseState);
            }
        }
    }

    public override void FixedUpdateState()
    {
        manager.enemyMovement.Chase();
    }
}
