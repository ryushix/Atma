using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateManager manager;

    public PlayerBaseState(PlayerStateManager manager)
    {
        this.manager = manager;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void OnAnimationEnd() { }
}
