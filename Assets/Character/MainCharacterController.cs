using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MainCharacterController : MonoBehaviour
{
    private Animator _animator;
    
    private Vector3 _forward, _right;
    private MousePositionInWorld _mousePosition;
    
    private CharacterController _characterController;

    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float jumpHeight = 0.2f;

    [SerializeField] private float gravity = 2f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpSpeed = 0.5f;

    private float _directionY;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mousePosition = gameObject.GetComponent<MousePositionInWorld>();
        _animator = gameObject.GetComponent<Animator>();
        
        _forward = Camera.main.transform.forward;
        _forward.y = 0;
        _forward = Vector3.Normalize(_forward);
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
    }

    void Update()
    {
        Move();
        WatchVelocity();
    }

    void Move()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
    
        Vector3 direction = _forward * vInput + _right * hInput;
    
        if (_characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                _directionY = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        }
    
        _directionY += gravity * Time.deltaTime;
    
        direction.y = _directionY;
    
        var result = _characterController.Move(direction * (movementSpeed * Time.deltaTime));

        if (direction != Vector3.zero)
        {
            Vector3 mPos = _mousePosition.worldPosition;
            gameObject.transform.LookAt(new Vector3(mPos.x, transform.position.y, mPos.z));
        }
    }

    void WatchVelocity()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal") * -1, 0, Input.GetAxis("Vertical"));
        
        Quaternion camRot = Quaternion.LookRotation(_forward);
        Vector3 lookDirection = camRot * ((_mousePosition.worldPosition - transform.position)).normalized;
        
        Quaternion lookRot = Quaternion.LookRotation(lookDirection);

        Vector3 rotatedInput = lookRot * inputDirection;
        
        Debug.DrawLine(transform.position + Vector3.up, lookDirection);
        Debug.DrawLine(Vector3.up, rotatedInput);
        
        _animator.SetFloat("velocityRight", rotatedInput.z, 0.1f, Time.deltaTime);
        _animator.SetFloat("velocityForward", rotatedInput.x, 0.1f, Time.deltaTime);
    }
}