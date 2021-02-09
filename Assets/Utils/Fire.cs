using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private ParticleSystem fire;
    [SerializeField] public float health;
    [SerializeField] public int maxHealth = 100;
    private SphereCollider _sphereCollider;
    [SerializeField] private float regenerationRate = 0.5f;
    private Vector3 initialFireScale;
    public int hits = 0;
    
    void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        var fireEmission = fire.emission;
        fireEmission.enabled = health > 0;
        health = maxHealth;
        initialFireScale = fire.transform.localScale;
    }

    private void Update()
    {
        var fireEmission = fire.emission;
        fireEmission.enabled = health > 0;

        if (health < maxHealth && health != 0)
        {
            health += Time.deltaTime * regenerationRate;
        }

        fire.transform.localScale = initialFireScale * (health / maxHealth);

        _sphereCollider.enabled = health != 0;
    }

    private void OnParticleCollision(GameObject other)
    {
        hits++;
        if (health > 0)
        {
            if (health - 1 < 0)
            {
                health = 0;
            }
            else
            {
                health--;
            }
        }
    }
}