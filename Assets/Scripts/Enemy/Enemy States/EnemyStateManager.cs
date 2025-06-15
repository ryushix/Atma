using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public Animator animator;
    public EnemyMovement enemyMovement;
    private EnemyBaseState currentState;

    public EnemyPatrolState patrolState;
    public EnemyChaseState chaseState;
    public EnemyHitState hitState;

    void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();

        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
        hitState = new EnemyHitState(this);
    }

    void Start()
    {
        currentState = patrolState;
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void SwitchState(EnemyBaseState newState)
    {
        currentState = newState;
        newState.EnterState();
    }
}
