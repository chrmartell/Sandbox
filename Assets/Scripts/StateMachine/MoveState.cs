using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player){
        Debug.Log("Enter the MoveState");

        //Register event listener for Move action
        player.playerControls.Player.Move.performed += OnMovePerformed;

        //player.animator.SetBool("isMoving", true);
    }

    public override void UpdateState(PlayerStateManager player){
        //Debug.Log("Hello from UpdateState in MoveState");

        //player.SwitchState(player.AttackState);
    }

    public override void ExitState(PlayerStateManager player){
        Debug.Log("Exit the MoveState");

        // Unregister event listener for Move action
        player.playerControls.Player.Move.performed -= OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Handle Move input if needed
    }


    /*public override void OnTriggerEnter(PlayerStateManager player, Collision collision){
        //
    }*/
}
