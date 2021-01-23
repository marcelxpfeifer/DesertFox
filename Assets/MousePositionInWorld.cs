using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionInWorld : MonoBehaviour
{
    private Plane _plane;

    [SerializeField] private Transform offset;
    
    public Vector3 worldPosition;

    private void Awake()
    {
        _plane = new Plane(Vector3.up, offset.position.y);
    }

    void Update()
    {
        _plane.distance = offset.position.y;

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
    }
}
