using UnityEngine;
using VehicleBehaviour;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MainCharacterController : MonoBehaviour
{
    private Animator _animator;
    
    private Vector3 _forward, _right;
    private MousePositionInWorld _mousePosition;
    
    private CharacterController _characterController;
    private CameraController _cameraController;

    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float jumpHeight = 0.2f;

    [SerializeField] private float gravity = 2f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpSpeed = 0.5f;

    private float _directionY;

    private Renderer _renderer;

    private WheelVehicle vehicle;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraController = Camera.main.GetComponent<CameraController>();
        _cameraController.size = 7.5f;
        _mousePosition = GetComponent<MousePositionInWorld>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<Renderer>();
        
        _forward = Camera.main.transform.forward;
        _forward.y = 0;
        _forward = Vector3.Normalize(_forward);
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
    }

    public void getOutOfVehicle(Vector3 position, Quaternion rotation)
    {
        if (vehicle == null) return;

        vehicle = null;
        
        transform.position = position;
        transform.rotation = rotation;
        
        _cameraController.size = 7.5f;
        
        gameObject.SetActive(true);
        _characterController.enabled = true;
        _cameraController.target = transform;
    }


    public void getIntoVehicle(WheelVehicle wheelVehicle)
    {
        vehicle = wheelVehicle;
        
        _cameraController.size = 15;
        
        gameObject.SetActive(false);
        _characterController.enabled = false;
    }

    void Update()
    {
        if (vehicle != null) return;
        
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
        // Todo: Can most of this be replaced with a dot-product?
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal") * -1, 0, Input.GetAxis("Vertical"));
        
        Quaternion camRot = Quaternion.LookRotation(_forward);
        Vector3 lookDirection = camRot * ((_mousePosition.worldPosition - transform.position)).normalized;
        
        Quaternion lookRot = Quaternion.LookRotation(lookDirection);

        Vector3 rotatedInput = lookRot * inputDirection;

        _animator.SetFloat("velocityRight", rotatedInput.z, 0.1f, Time.deltaTime);
        _animator.SetFloat("velocityForward", rotatedInput.x, 0.1f, Time.deltaTime);
    }
}