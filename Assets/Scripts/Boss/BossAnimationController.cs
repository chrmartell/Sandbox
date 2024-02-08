using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPosition;
    private float bossSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        // Initialize previousPosition with current position
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate boss speed based on change in position
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        bossSpeed = distanceMoved / Time.deltaTime;

        // Update previousPosition for the next frame
        previousPosition = transform.position;

        // Update animator with boss speed
        animator.SetFloat("bossSpeed", bossSpeed);
    }
    /*private Rigidbody rb;
    private float maxSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {
        float speed = rb.velocity.magnitude / maxSpeed;
        Debug.Log("Speed: " + speed);
        animator.SetFloat("bossSpeed", speed);
    }*/
}
