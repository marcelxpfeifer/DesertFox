using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private PlayerController playerController;

    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    public float jumpHeight = 3f;

    private Animator animator;
    private Vector3 lastPosition;

    private bool didPositionChange;
    private bool isGrounded;
    private bool isJumping;
    private float gravity;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        animator = gameObject.GetComponent<Animator>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        watchGravityVelocity();
        watchPositionDiff();
        watchGroundedState();
        watchJumpingState();
    }

    void watchGravityVelocity()
    {
        velocity = playerController.velocity;
        gravity = playerController.gravity;
    }

    void watchGroundedState()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void watchJumpingState()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void watchPositionDiff() 
    {
        if(lastPosition != transform.position) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }

        lastPosition = transform.position;
    }
}
