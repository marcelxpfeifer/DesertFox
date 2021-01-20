using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 4f;
    private Vector3 forward, right;
    private MousePositionInWorld _mousePosition;

    public CharacterController characterController;

    public float speed = 12f;
    public float gravity = 9.81f;

    public Vector3 velocity;
    
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        
        _mousePosition = gameObject.AddComponent<MousePositionInWorld>();
    }

    void Update()
    {
        NewMove();
    }

    void NewMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        Vector3 lookAtPosition = new Vector3(_mousePosition.worldPosition.x, transform.position.y, _mousePosition.worldPosition.z);
        transform.LookAt(lookAtPosition);
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 rightMovement = right * (movementSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        Vector3 upMovement = forward * (movementSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
        
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        var rightDirection = direction.normalized.x;
        
        transform.position += rightMovement;
        transform.position += upMovement;

        Vector3 lookAtPosition = new Vector3(_mousePosition.worldPosition.x, transform.position.y, _mousePosition.worldPosition.z);
        transform.LookAt(lookAtPosition);
    }
}