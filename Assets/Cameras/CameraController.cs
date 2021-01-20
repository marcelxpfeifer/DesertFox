using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float mouseWeight = 0.1f;
    [SerializeField] private Vector3 offset;
    private Vector3 _velocity = Vector3.zero;
    private MousePositionInWorld _mousePosition;
    
    void Start()
    {
        _mousePosition = gameObject.AddComponent<MousePositionInWorld>();
        transform.position += offset;
    }

    void LateUpdate()
    {
        var currentPos = transform.position;
        var targetPos = target.position + offset - (transform.position - _mousePosition.worldPosition) * mouseWeight;
        
        transform.position = Vector3.SmoothDamp(currentPos, targetPos, ref _velocity, smoothTime);
    }
}
