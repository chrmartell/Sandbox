using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using DG.Tweening;

public class ThirdPersonController : MonoBehaviour
{
    //input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    [SerializeField] private Camera playerCamera;
    private Animator animator;

    [Header("Movement Variables")]
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [Header("Attack Variables")]
    public WeaponBehavior weaponBehavior;
    bool isAttacking = false;
    private float timeSinceAttack;
    public int currentAttack = 1;

    [Header("Dash Variables")]
    bool isDashing = false;
    public GameObject dashPoint; // Reference to the DashPoint GameObject
    public LayerMask obstacleLayer; // LayerMask to filter obstacles
    private Vector3 initialDashPointPosition; // Initial position of DashPoint when dash is initiated
    // Define the duration of the dash animation (in seconds)
    [SerializeField] private float dashDuration = 0.3f;  // Adjust this value to increase/decrease speed
    private bool cancelDash = false; // Flag to track if the dash should be cancelled

    [Header("Block Variables")]
    //bool isBlocking = false;
    public ShieldController shieldController;

    [Header("Shoot Variables")]
    public GameObject BulletPrefab;
    public Transform FirePosition;
    public GameObject MouseTarget;
    private InputAction shootAction;
    public float shootInterval = 0.1f; // Interval between shots when holding down shoot button
    private float lastShootTime; // To track time between shots
    bool isShooting = false;

    private void Awake() {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        animator = this.GetComponent<Animator>();
    }

    private void FixedUpdate() {
        timeSinceAttack += Time.fixedDeltaTime;

        if (!isAttacking)
        {
            if (!isDashing)
            {
                Move();
                // Check if shoot button is being held down for continuous shooting
                if (shootAction.triggered)
                {
                    if (Time.time - lastShootTime >= shootInterval)
                    {
                        Shoot();
                        lastShootTime = Time.time;
                    }
                }
            }
        }
        // Check if the player is currently dashing
        if (isDashing)
        {
            // Check for obstacles in the path of the dash
            RaycastHit hit;
            if (Physics.Raycast(transform.position, initialDashPointPosition - transform.position, out hit, Vector3.Distance(transform.position, initialDashPointPosition), obstacleLayer))
            {
                Debug.Log("Obstacle Detected: " + hit.collider.gameObject.name); // Log the name of the obstacle
                if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Ring"))
                {
                    // Obstacle detected, cancel dash
                    Debug.Log("Cannot Dash Through: " + hit.collider.tag);
                    cancelDash = true; // Set cancelDash flag to true
                }
            }
            else
            {
                Debug.Log("No Obstacle Detected");
            }
        }

        PlayerLookAt();
    }

    private void LateUpdate()
    {
        // Check if the dash should be cancelled
        if (cancelDash)
        {
            cancelDash = false; // Reset cancelDash flag
            isDashing = false; // Set dashing flag to false
            Debug.Log("cancelDash in LateUpdate");
            // Immediately stop the DoMove animation
            transform.DOKill();
            // Stop the dash animation
            //transform.DOComplete();
        }
    }
    /****************Movement - START****************/
    private void Move()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;
    
        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if(rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
    }

    private void PlayerLookAt()
    {
        if(isShooting)
        {
            // Face the MouseTarget when shooting
            Vector3 targetDirection = MouseTarget.transform.position - transform.position;
            targetDirection.y = 0f; // Keep the player's rotation flat
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }
        else{
            Vector3 direction = rb.velocity;
            direction.y = 0f;

            if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
                this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
            else
                rb.angularVelocity = Vector3.zero;
        }
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
    private void OnEnable() {
        //Subscribe to events
        playerActionsAsset.Player.Attack.performed += DoAttack;
        playerActionsAsset.Player.Dash.performed += DoDash;
        playerActionsAsset.Player.Block.performed += DoBlock;

        shootAction = playerActionsAsset.Player.Shoot;
        shootAction.started += _ => StartShooting();
        shootAction.canceled += _ => StopShooting();

        move = playerActionsAsset.Player.Move;
        
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable() {
        //Unsubscribe from events
        playerActionsAsset.Player.Attack.performed -= DoAttack;
        playerActionsAsset.Player.Dash.performed -= DoDash;
        playerActionsAsset.Player.Block.performed -= DoBlock;

        shootAction.started -= _ => StartShooting();
        shootAction.canceled -= _ => StopShooting();
        
        playerActionsAsset.Player.Disable();
    }
    /****************Attack - START****************/
    private void DoAttack(InputAction.CallbackContext obj)
    {
        isAttacking = true;
        rb.velocity = Vector3.zero;
        Debug.Log("DoAttack_isAttacking " + isAttacking);

        currentAttack++;

        if(currentAttack > 3) 
            currentAttack = 1;

        //Reset
        if(timeSinceAttack > 1.2f)
            currentAttack = 1;

        //Call Attack Triggers
        animator.SetTrigger("attack" + currentAttack);
                
        //Reset Timer
        timeSinceAttack = 0;
        /*if (!isAttacking)
        {
            isAttacking = true;
            rb.velocity = Vector3.zero;
            Debug.Log("DoAttack_isAttacking " + isAttacking);

            currentAttack++;

            if (currentAttack > 3)
                currentAttack = 1;

            // Call Attack Triggers
            animator.SetTrigger("attack" + currentAttack);

            // Reset Timer
            timeSinceAttack = 0;
        }
        else if (currentAttack < 3 && timeSinceAttack > 0.5f) // Assuming each attack animation takes at least 0.5 seconds
        {
            currentAttack++;
            animator.SetTrigger("attack" + currentAttack);
            timeSinceAttack = 0;
        }
        else if (currentAttack == 3 && timeSinceAttack > 0.5f)
        {
            // Reset the combo if the player hasn't attacked in 0.5 seconds after the third attack
            currentAttack = 1;
            animator.SetTrigger("attack" + currentAttack);
            timeSinceAttack = 0;
        }
        else if (timeSinceAttack > 1.2f) // Reset the combo if no attack is performed for 1.2 seconds
        {
            currentAttack = 1;
            animator.SetTrigger("attack" + currentAttack);
            timeSinceAttack = 0;
        }*/
    }

    // Called using animation event
    public void TriggerEnableSwordHitbox()
    {
        weaponBehavior.EnableSwordHitbox();
    }

    // Called using animation event
    public void TriggerDisableSwordHitbox()
    {
        weaponBehavior.DisableSwordHitbox();
    }
    //This will be used at animation event
    public void ResetAttack()
    {
        isAttacking = false;
        Debug.Log("ResetAttack_isAttacking " + isAttacking);
    } 
    /****************Attack - END****************/
    /****************Dash - START****************/
    private void DoDash(InputAction.CallbackContext obj)
    {
        // Capture the current X and Z position of the DashPoint
        initialDashPointPosition = new Vector3(dashPoint.transform.position.x, 0f, dashPoint.transform.position.z);

        // Make the player face the direction of initialDashPointPosition
        transform.LookAt(initialDashPointPosition);

        // Check if the dash should be cancelled before starting
        if (cancelDash)
        {
            cancelDash = false; // Reset cancelDash flag
            isDashing = false; // Set dashing flag to false
            // Immediately stop the DoMove animation
            transform.DOKill();
            return; // Exit the function without starting the dash animation
        }

        // Tween the player to the X and Z position of the DashPoint with the defined duration
        transform.DOMove(initialDashPointPosition, dashDuration).SetEase(Ease.OutQuad).OnStart(() =>
        {
            isDashing = true; // Set dashing flag to true when dash starts
        }).OnComplete(() =>
        {
            isDashing = false; // Set dashing flag to false when dash completes
        });
    }
    /****************Dash - END****************/
    /****************Block - START****************/
    private void DoBlock(InputAction.CallbackContext obj)
    {
        rb.velocity = Vector3.zero;
        Debug.Log("DoBlock");
        animator.SetTrigger("block");
    }
    // Called using animation event
    public void DoEnableShield()
    {
        shieldController.EnableShield();
    }

    // Called using animation event
    public void DoDisableShield()
    {
        shieldController.DisableShield();
    }
    /****************Block - END****************/
    /****************Shoot - START****************/
    private void StartShooting()
    {
        isShooting = true;
        InvokeRepeating(nameof(Shoot), 0f, shootInterval);
        animator.SetBool("shoot", true);
    }

    private void StopShooting()
    {
        isShooting = false;
        CancelInvoke(nameof(Shoot));
        animator.SetBool("shoot", false);
    }

    private void Shoot()
    {
        // Calculate direction towards MouseTarget
        Vector3 direction = (MouseTarget.transform.position - FirePosition.position).normalized;
        direction.y = 0; // Set Y component to 0 to maintain current Y position

        // Instantiate bullet
        GameObject bullet = Instantiate(BulletPrefab, FirePosition.position, Quaternion.identity);

        // Apply force towards MouseTarget (with Y component maintained)
        float bulletSpeed = 10f; // Adjust the speed as needed
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
    }
    /****************Shoot - END****************/
}
