using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerFallState fallState = new PlayerFallState();


    void Start()
    {
        currentState = idleState;
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
    }

    void FxedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState = newState;
        newState.EnterState();
    }
}
