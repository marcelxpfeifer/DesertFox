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

    private void Awake()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.positionCount = segmentCount + 1;
        _lineRenderer.startColor = Color.black;
        _lineRenderer.endColor = Color.black;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        
        _positions.Add(start.position);
    }

    void Start()
    {
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

    void Update()
    {
        RaycastHit lastPosHitInfo;
        Vector3 previousToLastPos = Vector3.down;

        bool lPosObstructed = Physics.SphereCast(end.position, 0.1f, (_positions.Last() - end.position).normalized, out lastPosHitInfo, (_positions.Last() - end.position).magnitude - 0.01f);
        
        bool previousToLastPosObstructed = true;

        Vector3 lPosHitPos = lastPosHitInfo.point + 0.1f * lastPosHitInfo.normal;

        if (lPosObstructed) _positions.Add(lastPosHitInfo.point + 0.1f * lastPosHitInfo.normal);
        
        if (_positions.Count >= 2)
        {
            previousToLastPos = _positions[_positions.Count - 2];
            previousToLastPosObstructed = Physics.Linecast(end.position, previousToLastPos);
        }
        
        if (previousToLastPos != Vector3.down && !previousToLastPosObstructed && !lPosObstructed)
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
