using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Hose : MonoBehaviour
{
    public bool enabled = false;
    public bool waterRunning = false;
    public float hoseLength = 0;

    [SerializeField] private RopeBuilder ropeBuilder;
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
        hoseLength = ropeBuilder.ropeLength;
        
        if (hose.activeSelf != enabled)
        {
            waterRunning = waterRunning && enabled;
            var waterEmission = water.emission;
            waterEmission.enabled = waterRunning;
            hose.SetActive(enabled);
        }

        if (!enabled) return;

        if (Input.GetButtonDown(waterTrigger))
        {
            waterRunning = !waterRunning;

            var waterEmission = water.emission;
            waterEmission.enabled = waterRunning;
            
            if (waterRunning) water.Play();
        }
    }
}
