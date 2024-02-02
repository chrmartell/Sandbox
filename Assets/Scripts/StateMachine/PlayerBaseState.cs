using UnityEngine;

public abstract class PlayerBaseState
{
    //abstract on each method means we need to define their functionality in each class that derive from PlayerBaseState
    public abstract void EnterState(PlayerStateManager player);

    public abstract void UpdateState(PlayerStateManager player);

    public abstract void ExitState(PlayerStateManager player);

    //public abstract void OnTriggerEnter(PlayerStateManager player, Collision collision);
}