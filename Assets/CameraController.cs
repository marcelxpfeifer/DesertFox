using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] public Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float mouseWeight = 0.1f;
    [SerializeField] private Vector3 offset;
    private Vector3 _velocity = Vector3.zero;
    private MousePositionInWorld _mousePosition;

    public float size;

    void Awake()
    {
        _mousePosition = gameObject.AddComponent<MousePositionInWorld>();
        transform.position += offset;
        transform.eulerAngles = new Vector3(45, 45, 0);
        size = Camera.main.orthographicSize;
    }

    void LateUpdate()
    {
        var currentPos = transform.position;
        var targetPos = target.position + offset - (transform.position - _mousePosition.worldPosition) * mouseWeight;
        
        transform.position = Vector3.SmoothDamp(currentPos, targetPos, ref _velocity, smoothTime);

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, size, 0.02f);
    }
}
