using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public Transform player;
    [SerializeField] private GameObject outlineTarget;
    [SerializeField] private string keyCode;
    [SerializeField] private float radius;

    [HideInInspector] public Outline _outline;
    
    public abstract void Interact();
    private void Awake()
    {
        _outline = outlineTarget.AddComponent<Outline>();

        _outline.enabled = false;
        _outline.OutlineMode = Outline.Mode.OutlineVisible;
        _outline.OutlineColor = Color.white;
        _outline.OutlineWidth = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, radius);
    }

    void Update()
    {
        float distance = (player.position - target.transform.position).magnitude;

        if (distance > radius)
        {
            _outline.enabled = false;
            return;
        }

        _outline.enabled = true;

        if (Input.GetButtonDown(keyCode))
        {
            Debug.Log("INTERACTED!!");
            Interact();
        }
    }
}
