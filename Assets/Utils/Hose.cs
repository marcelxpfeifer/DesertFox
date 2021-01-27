using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Hose : MonoBehaviour
{
    public bool enabled = false;
    public bool waterRunning = false;

    [SerializeField] private string waterTrigger = "Fire1";
    [SerializeField] private GameObject hose;
    [SerializeField] private ParticleSystem water;
    
    private CharacterController _characterController;
    
    private void Awake()
    {
        hose.SetActive(enabled);
        var waterEmission = water.emission;
        waterEmission.enabled = waterRunning;

        _characterController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        if (hose.activeSelf != enabled)
        {
            waterRunning = waterRunning && enabled;
            var waterEmission = water.emission;
            waterEmission.enabled = waterRunning;
            hose.SetActive(enabled);
        }

        if (!enabled) return;
        
        Debug.DrawLine(Vector3.zero, _characterController.transform.forward * 10);

        if (Input.GetButtonDown(waterTrigger))
        {
            waterRunning = !waterRunning;
            
            Debug.Log(waterRunning);

            var waterEmission = water.emission;
            waterEmission.enabled = waterRunning;
            
            if (waterRunning) water.Play();
        }
    }
}
