using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    //movement fields
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    private ThirdPersonActionsAsset playerActions;
    private InputAction move;

    [SerializeField] private Camera playerCamera;

    void Awake()
    {
        Debug.Log("Awake in PlayerMovementController");
        rb = this.GetComponent<Rigidbody>();
        playerActions = new ThirdPersonActionsAsset();
        move = playerActions.Player.Move;

        //move.performed += OnMovePerformed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        LookAt();
    }

    private void OnEnable() {
        move = playerActions.Player.Move;
    }

    /****************Movement - START****************/
    private void Move()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        if(forceDirection != Vector3.zero)Debug.Log("Reading inputs in PlayerMovementController Move");
        else Debug.Log("Not reading inputs in PlayerMovementController Move");

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if(rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
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
    /****************Movement - END****************/
}
