public abstract class EnemyBaseState
{
    protected EnemyStateManager manager;

    public EnemyBaseState(EnemyStateManager manager)
    {
        this.manager = manager;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
}
