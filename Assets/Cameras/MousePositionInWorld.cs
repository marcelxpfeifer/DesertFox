using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionInWorld : MonoBehaviour
{
    TerrainCollider terrainCollider;
    public Vector3 worldPosition;
    Ray ray;

    void Start()
    {
        terrainCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        if(terrainCollider.Raycast(ray, out hitData, 1000))
        {
            worldPosition = hitData.point;
        }
    }
}
