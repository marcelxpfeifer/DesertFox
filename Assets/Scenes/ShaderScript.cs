using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{
    private Renderer _renderer;
    public float x;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _renderer.material.SetFloat("MyVar", 100);
    }

    void Update()
    {
        x += Time.deltaTime;
        _renderer.material.SetFloat("MyVar", x);
    }
}
