using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D enemyRB;
    public EnemyMovement enemyMovement;
    private EnemyBaseState currentState;

    public EnemyPatrolState patrolState;
    public EnemyChaseState chaseState;

    void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
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
