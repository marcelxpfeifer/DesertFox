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
        watchPositionDiff();
        watchGroundedState();
        watchJumpingState();
    }

    void watchGroundedState()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void watchJumpingState()
    {
        Debug.Log("Watchung Jump State");
        Debug.Log(isGrounded);
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jumping");
            playerController.velocity.y = Mathf.Sqrt(jumpHeight * -2f * playerController.gravity);
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
