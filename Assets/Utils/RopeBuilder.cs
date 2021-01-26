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

    void Start()
    {
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
            Gizmos.color = Color.HSVToRGB((i / _positions.Count) * 360, 1, 1);
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

    void FixedUpdate()
    {
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
            Vector3 secondObstructionCheckpoint = _positions.Last() + lastToEndDirection * 0.1f;

            Vector3 checkPointToPrevToLast = previousToLastPos.Value - secondObstructionCheckpoint;

            debugPosition = secondObstructionCheckpoint;

            previousToLastPosObstructed = Physics.SphereCast(secondObstructionCheckpoint, 0.09f, checkPointToPrevToLast.normalized, out _, 0.2f);
        }
        
        if (previousToLastPos != null && !previousToLastPosObstructed && !lPosObstructed)
        {
            _positions.RemoveAt(_positions.Count - 1);
        }
        
        Debug.Log(_positions.Count);

        ConnectPositionsDebug();

        List<Vector3> linePos = new List<Vector3>(_positions);
        
        linePos.Add(end.position);
        
        _lineRenderer.positionCount = linePos.Count;
        _lineRenderer.SetPositions(linePos.ToArray());
        
        // _distance = (end.position - start.position).magnitude;
        //
        // var points = new Vector3[segmentCount + 1];
        //
        // for (int i = 0; i <= segmentCount; i++)
        // {
        //     float t = i / (float) segmentCount;
        //     
        //     Vector3 segmentOffset = new Vector3(0, ropeProfile.Evaluate(t) * curveMultiplier, 0);
        //     Vector3 position = Vector3.Lerp(start.position, end.position, t);
        //
        //     points[i] = position + segmentOffset;
        // }
        //
        // _lineRenderer.SetPositions(points);
    }
}
