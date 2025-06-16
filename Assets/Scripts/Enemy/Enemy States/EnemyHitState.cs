using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    private float recoveryTime = 0.1f;
    private float recoverTimer;
    private int damageAmount;
    private Vector2 _hitSource;
    public EnemyHitState(EnemyStateManager manager) : base(manager) { }

    public void SetupHit(int damage, Vector2 hitSource)
    {
        damageAmount = damage;
        _hitSource = hitSource;
    }
    public override void EnterState()
    {
        Debug.Log("Current State: Hit");
        manager.enemyMovement.StopMovement();
        manager.enemyHealth.TakeDamage(damageAmount, _hitSource);
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
