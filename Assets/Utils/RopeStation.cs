using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RopeBuilder))]
public class RopeStation : Interactable
{
    private RopeBuilder _ropeBuilder;
    [HideInInspector] public bool holdsHose = false;

    [SerializeField] private Hose hose;

    private void Start()
    {
        _ropeBuilder = GetComponent<RopeBuilder>();
    }

    public override void Interact()
    {
        holdsHose = !holdsHose;

        if (holdsHose) _ropeBuilder.Enable();
        else _ropeBuilder.Disable();

        hose.enabled = holdsHose;
    }
}
