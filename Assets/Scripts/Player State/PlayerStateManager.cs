using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;

    void Start()
    {
        currentState = new PlayerIdleState(this);
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
