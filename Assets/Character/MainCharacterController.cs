using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    private Animator _animator;
    
    [SerializeField] private float movementSpeed = 4f;
    private Vector3 forward, right;
    private MousePositionInWorld _mousePosition;
    
    private CharacterController controller;

    [SerializeField] private float speed = 4f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private float playerSpeed = 4f;
    [SerializeField] private float jumpHeight = 1f;
    
    private Vector3 lastPosition;
    [SerializeField] private Vector3 velocity;
    private bool playerGrounded;
    
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        
        _mousePosition = gameObject.AddComponent<MousePositionInWorld>();

        controller = gameObject.GetComponent<CharacterController>();
        _animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log(controller.isGrounded);
        
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * (Time.deltaTime * playerSpeed));

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // NewMove();
        WatchVelocity();
    }

    void NewMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        Vector3 lookAtPosition = new Vector3(_mousePosition.worldPosition.x, transform.position.y, _mousePosition.worldPosition.z);
        transform.LookAt(lookAtPosition);
    }

    void Move()
    {
        Vector3 rightMovement = right * (movementSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        Vector3 upMovement = forward * (movementSpeed * Time.deltaTime * Input.GetAxis("Vertical"));

        transform.position += rightMovement;
        transform.position += upMovement;

        Vector3 lookAtPosition = new Vector3(_mousePosition.worldPosition.x, transform.position.y, _mousePosition.worldPosition.z);
        transform.LookAt(lookAtPosition);
    }

    void WatchVelocity()
    {
        _animator.SetFloat("velocity", new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).sqrMagnitude);
    }
}