using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    //holds a reference to the active state in state machine
    //state machines can only be in one state a time
    PlayerBaseState currentState;
    public MoveState moveState = new MoveState();
    public AttackState attackState = new AttackState();
    public DodgeState dodgeState = new DodgeState();
    public GuardState guardState = new GuardState();

    public ThirdPersonActionsAsset playerControls;
    //public Animator animator;
    private Rigidbody rb;

    /*Notes:Awake is used for early initialization tasks that need to be performed before the game starts
    Use Awake for initializing variables or references that need to be set up before any other script is executed, 
    such as references to other objects or components that the script relies on

    Start is used for initialization tasks that require all objects to be initialized and ready
    Use Start for initialization tasks that require all objects in the scene to be initialized, 
    such as setting up gameplay-related variables or initializing the state of the object*/
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerControls = new ThirdPersonActionsAsset();
        //animator = this.GetComponent<Animator>();

        //starting state for the state machine
        currentState = moveState;

        //"this" is a reference to the context (this EXACT Monobehavior script)
        currentState.EnterState(this);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        //will call any logic in UpdateState from the current state every frame
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    /*void OnTriggerEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }*/
}
