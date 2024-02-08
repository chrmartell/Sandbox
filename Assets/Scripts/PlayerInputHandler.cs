using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string aim = "Aim";
    [SerializeField] private string dash = "Dash";
    [SerializeField] private string attack = "Attack";
    [SerializeField] private string shoot = "Shoot";
    [SerializeField] private string block = "Block";
    [SerializeField] private string pause = "Pause";

    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction dashAction;
    private InputAction attackAction;
    private InputAction shootAction;
    private InputAction blockAction;
    private InputAction pauseAction;

    public Vector2 MoveInput {get; private set;}
    public Vector2 AimInput {get; private set;}
    public bool DashTriggered {get; private set;}
    public bool AttackTriggered {get; private set;}
    public bool ShootTriggered {get; private set;}
    public bool BlockTriggered {get; private set;}
    public bool PauseTriggered {get; private set;}

    public static PlayerInputHandler Instance {get; private set;}

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        aimAction = playerControls.FindActionMap(actionMapName).FindAction(aim);
        dashAction = playerControls.FindActionMap(actionMapName).FindAction(dash);
        attackAction = playerControls.FindActionMap(actionMapName).FindAction(attack);
        shootAction = playerControls.FindActionMap(actionMapName).FindAction(shoot);
        blockAction = playerControls.FindActionMap(actionMapName).FindAction(block);
        pauseAction = playerControls.FindActionMap(actionMapName).FindAction(pause);

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        aimAction.performed += context => AimInput = context.ReadValue<Vector2>();
        aimAction.canceled += context => AimInput = Vector2.zero;

        dashAction.performed += context => DashTriggered = true;
        dashAction.canceled += context => DashTriggered = false;

        attackAction.performed += context => AttackTriggered = true;
        attackAction.canceled += context => AttackTriggered = false;

        shootAction.performed += context => ShootTriggered = true;
        shootAction.canceled += context => ShootTriggered = false;

        blockAction.performed += context => BlockTriggered = true;
        blockAction.canceled += context => BlockTriggered = false;

        pauseAction.performed += context => PauseTriggered = true;
        pauseAction.canceled += context => PauseTriggered = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        aimAction.Enable();
        dashAction.Enable();
        attackAction.Enable();
        shootAction.Enable();
        blockAction.Enable();
        pauseAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        aimAction.Disable();
        dashAction.Disable();
        attackAction.Disable();
        shootAction.Disable();
        blockAction.Disable();
        pauseAction.Disable();
    }
}
