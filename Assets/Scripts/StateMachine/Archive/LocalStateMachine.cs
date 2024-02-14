using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalStateMachine : MonoBehaviour
{
    private enum PlayerState
    {
        Moving,
        Attacking
    }

    private PlayerState currentState = PlayerState.Moving;
    [SerializeField]
    private Camera playerCamera;
    private Animator animator;
    public WeaponBehavior weaponBehavior;

    // Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    // Movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    //Booleans
    //bool isMoving = true;
    bool isAttacking = false;
    private bool isAttackAnimationPlaying = false;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        animator = this.GetComponent<Animator>();
    }

    /*****FixedUpdate is every physics calc, Update is every frame*****/
    private void FixedUpdate()
    {
        HandleState();

        // Other common actions
        LookAt();
    }

    /*****State Switch Case*****/
    private void HandleState()
    {
        switch (currentState)
        {
            case PlayerState.Moving:
                HandleMovement();
                break;

            case PlayerState.Attacking:
                HandleAttack();
                break;
        }
    }

    private void HandleMovement()
    {
        if (!isAttackAnimationPlaying)
        {
            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

            // Only move if the attack animation is not playing
            rb.AddForce(forceDirection, ForceMode.Impulse);
            forceDirection = Vector3.zero;

            // Other movement-related actions
            if (rb.velocity.y < 0f)
                rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

            Vector3 horizontalVelocity = rb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
                rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void OnEnable()
    {
        playerActionsAsset.Player.Attack.started += StartAttack;
        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.Attack.started -= StartAttack;
        playerActionsAsset.Player.Disable();
    }
    /*****Attack Section - START*****/
    private void HandleAttack()
    {
        // Handle attack state
        animator.SetTrigger("attack");
        rb.velocity = Vector3.zero;

        // Set the boolean variable to true when the attack animation starts
        isAttackAnimationPlaying = true;
    }

    private void StartAttack(InputAction.CallbackContext obj)
    {
        if (!isAttacking)
        {
            Debug.Log("Attack button pressed. Starting attack.");

            // Transition to the Attacking state when attack is started
            currentState = PlayerState.Attacking;

            // Set the boolean variable to true when the attack animation starts
            isAttackAnimationPlaying = true;

            // Set isAttacking to true to prevent multiple attack triggers
            isAttacking = true;
        }
    }

    // Unity Animator event function (called at the end of the attack animation)
    public void OnAttackAnimationEnd()
    {
        Debug.Log("Attack animation ended. Transitioning back to Moving state.");

        // Set the boolean variable to false when the attack animation ends
        isAttackAnimationPlaying = false;

        // Transition back to moving state
        currentState = PlayerState.Moving;

        // Reset isAttacking to allow for the next attack trigger
        isAttacking = false;
    }

    // Example function triggering EnableSwordHitbox from an external event
    public void TriggerEnableSwordHitbox()
    {
        weaponBehavior.EnableSwordHitbox();
    }

    // Example function triggering DisableSwordHitbox from an external event
    public void TriggerDisableSwordHitbox()
    {
        weaponBehavior.DisableSwordHitbox();
    }
    /*****Attack Section - END*****/
}
