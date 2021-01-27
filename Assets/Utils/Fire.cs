using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private ParticleSystem fire;
    [SerializeField] public float health;
    void Awake()
    {
        var fireEmission = fire.emission;
        fireEmission.enabled = health > 0;
    }

    private void Update()
    {
        var fireEmission = fire.emission;
        fireEmission.enabled = health > 0;
    }
}
