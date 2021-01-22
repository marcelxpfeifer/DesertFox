using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MainCharacterController : MonoBehaviour
{
    private TextMesh _debugText;
    
    private Animator _animator;
    
    [SerializeField] private float movementSpeed = 4f;
    private Vector3 _forward, _right;
    private MousePositionInWorld _mousePosition;
    
    private CharacterController _controller;

    [SerializeField] private float speed = 4f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private float playerSpeed = 4f;
    [SerializeField] private float jumpHeight = 0.2f;
    
    [SerializeField] private Vector3 velocity;

    private float _directionY;
    
    void Start()
    {
        _forward = Camera.main.transform.forward;
        _forward.y = 0;
        _forward = Vector3.Normalize(_forward);
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
        
        _mousePosition = gameObject.AddComponent<MousePositionInWorld>();

        _controller = gameObject.GetComponent<CharacterController>();
        _animator = gameObject.GetComponent<Animator>();
        _debugText = gameObject.AddComponent<TextMesh>();
    }

    void Update()
    {
        Move();
            
        _debugText.text = "isGrounded " + _controller.isGrounded;

        WatchVelocity();
    }

    void Move()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 direction = _forward * vInput + _right * hInput;

        if (_controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                _directionY = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        }

        _directionY += gravity * Time.deltaTime;

        direction.y = _directionY;

        _controller.Move(direction * (movementSpeed * Time.deltaTime));
        
        if (direction != Vector3.zero)
        {
            Vector3 mPos = _mousePosition.worldPosition;
            gameObject.transform.LookAt(new Vector3(mPos.x, transform.position.y, mPos.z));
        }
    }

    void WatchVelocity()
    {
        _animator.SetFloat("velocity", new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).sqrMagnitude);
    }
}