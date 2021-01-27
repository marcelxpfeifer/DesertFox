using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RopeBuilder : MonoBehaviour
{
    [SerializeField] private AnimationCurve ropeProfile = new AnimationCurve(
        new Keyframe(0 , 0),
        new Keyframe(0.5f, -1),
        new Keyframe(1, 0)
    );
    [SerializeField] private float curveMultiplier = 1;
    
    public int segmentCount = 10;
    public Transform start;
    public Transform end;

    private float _distance;
    private LineRenderer _lineRenderer;
    
    private List<Vector3> _positions = new List<Vector3>();

    private Vector3 debugPosition = Vector3.down;
    
    public bool enabled = false;

    private void Awake()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.positionCount = 1;
        _lineRenderer.startColor = Color.black;
        _lineRenderer.endColor = Color.black;
        _lineRenderer.numCornerVertices = 5;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        
        _positions.Add(start.position);
    }

    private void OnDrawGizmos()
    {
        if (debugPosition != Vector3.down)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(debugPosition, 0.05f);
        }
        
        
        for (int i = 0; i < _positions.Count; i++)
        {
            if (i == 0) continue;
            Gizmos.color = Color.HSVToRGB(i / (float)_positions.Count, 1, 1);
            Gizmos.DrawSphere(_positions[i], 0.005f);
            Gizmos.DrawWireSphere(_positions[i], 0.1f);
        }
        
    }

    void ConnectPositionsDebug()
    {
        for (int i = 0; i < _positions.Count; i++)
        {
            if (i + 1 < _positions.Count)
            {
                Debug.DrawLine(_positions[i], _positions[i + 1]);
            }
            else
            {
                Debug.DrawLine(_positions[i], end.position);
            }
        }
    }

    public void Disable()
    {
        enabled = false;
        _lineRenderer.enabled = false;
        _positions.Clear();
    }

    public void Enable()
    {
        enabled = true;
        _lineRenderer.enabled = true;
        _positions.Clear();
        _positions.Add(start.position);
    }

    void FixedUpdate()
    {
        _lineRenderer.enabled = enabled;
        
        if (!enabled) return;
        
        float sphereCastRadius = 0.1f;
        RaycastHit lastPosHitInfo;
        Vector3? previousToLastPos = null;

        Vector3 endToLastDirection = (_positions.Last() - end.position).normalized;
        float endToLastDistance = (_positions.Last() - end.position).magnitude;

        bool lPosObstructed = Physics.SphereCast(end.position, sphereCastRadius, endToLastDirection, out lastPosHitInfo, endToLastDistance - 0.05f);
        
        bool previousToLastPosObstructed = true;

        Vector3 lastPosHitPos = lastPosHitInfo.point + sphereCastRadius * lastPosHitInfo.normal;

        if (lPosObstructed && (lastPosHitPos - _positions.Last()).magnitude > 0.01) _positions.Add(lastPosHitPos);
        
        if (_positions.Count >= 2)
        {
            previousToLastPos = _positions[_positions.Count - 2];

            Vector3 lastToEndDirection = (end.position - _positions.Last()).normalized;
            Vector3 secondObstructionCheckpoint = _positions.Last() + lastToEndDirection * 0.5f;

            Vector3 checkPointToPrevToLast = previousToLastPos.Value - secondObstructionCheckpoint;

            debugPosition = secondObstructionCheckpoint;

            previousToLastPosObstructed = Physics.SphereCast(secondObstructionCheckpoint, sphereCastRadius, checkPointToPrevToLast.normalized, out _, 0.5f);
        }

        if (previousToLastPos != null && !previousToLastPosObstructed && !lPosObstructed)
        {
            _positions.RemoveAt(_positions.Count - 1);
        }

        ConnectPositionsDebug();

        List<Vector3> linePos = new List<Vector3>(_positions);
        
        linePos.Add(end.position);
        
        _lineRenderer.positionCount = linePos.Count;
        _lineRenderer.SetPositions(linePos.ToArray());
    }
}
