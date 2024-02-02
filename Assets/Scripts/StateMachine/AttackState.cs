using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player){
        Debug.Log("Hello from the AttackState");
    }

    public override void UpdateState(PlayerStateManager player){
        //
    }

    public override void ExitState(PlayerStateManager player){
        //
    }

    /*public override void OnTriggerEnter(PlayerStateManager player, Collision collision){
        //
    }*/
}
