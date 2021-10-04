using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.ObjectPool;

public class Rope : MonoBehaviour, IPoolable
{
    private const float EPSILON = 0.01f;

    [SerializeField]
    private Rigidbody2D _start;
    [SerializeField]
    private Rigidbody2D _end;
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private DistanceJoint2D _endDistanceJoint;

    private LinkedList<CollisionPoint> _linePoints = new LinkedList<CollisionPoint>();

    private Vector2 _previousEndPosition = Vector3.zero;
    private Vector2 _previousStartPosition = Vector3.zero;

    private int _ropeEndIgnoreMask;
    private int _ropeStartIgnoreMask;

    private float _totalRopeLength;

    private Rigidbody2D _originalEnd;
    private DistanceJoint2D _originalDistanceJoint;

    public bool IsEnabled => gameObject.activeInHierarchy;

    private bool _attached = false;

    public void Attach(DistanceJoint2D joint, Vector2 attachPoint)
    {
        _start.transform.position = attachPoint;
        _start.position = attachPoint;

        _start.gameObject.SetActive(true);


        _end = joint.attachedRigidbody;
        _linePoints.Clear();
        _lineRenderer.positionCount = 0;

        _previousEndPosition = _end.position;
        _previousStartPosition = _start.position;

        _endDistanceJoint = joint;
        _endDistanceJoint.connectedBody = _start;
        _endDistanceJoint.anchor = Vector2.zero;
        _endDistanceJoint.connectedAnchor = Vector2.zero;

        _endDistanceJoint.maxDistanceOnly = true;
        _endDistanceJoint.autoConfigureDistance = false;
        _endDistanceJoint.distance = Vector2.Distance(_end.position, _start.position);
        _totalRopeLength = _endDistanceJoint.distance;
    }

    public void Detach()
    {
        _originalEnd.gameObject.transform.position = _end.transform.position;
        var velocity = _end.velocity;

        _originalDistanceJoint.distance = _endDistanceJoint.distance;
        _originalDistanceJoint.connectedAnchor = _endDistanceJoint.connectedAnchor;
        _originalDistanceJoint.connectedBody = _endDistanceJoint.connectedBody;
        _originalDistanceJoint.anchor = _endDistanceJoint.anchor;
        _originalDistanceJoint.maxDistanceOnly = true;

        _end = _originalEnd;
        _endDistanceJoint = _originalDistanceJoint;

        _end.gameObject.SetActive(true);

        Debug.Log(velocity);
        _end.AddForce(velocity, ForceMode2D.Impulse);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _originalEnd = _end;
        _originalDistanceJoint = _endDistanceJoint;
        _originalEnd.gameObject.SetActive(false);

        _ropeEndIgnoreMask = ~LayerMask.GetMask(Layers.ROPE_END, Layers.PLAYER, Layers.ELEVATOR);
        _ropeStartIgnoreMask = ~LayerMask.GetMask(Layers.ROPE_START, Layers.PLAYER, Layers.ELEVATOR);
    }

    // TODO - See push example scene.. bug if start is visible from end?
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_attached)
        {
            return;
        }

        UpdateStart();
        UpdateEnd();

        var usedRope = 0f;
        if (_linePoints.Count > 0)
        {
            var lastPos = _linePoints.Last.Value.WorldPoint;
            usedRope += Vector2.Distance(lastPos, _start.position);
            for (int i = 1; i < _linePoints.Count; ++i)
            {
                var pos1 = _linePoints.ElementAt(i - 1).WorldPoint;
                var pos2 = _linePoints.ElementAt(i).WorldPoint;
                usedRope += Vector2.Distance(pos1, pos2);
            }
            var collision = _linePoints.First.Value;
            _endDistanceJoint.connectedBody = collision.Body;
            _endDistanceJoint.connectedAnchor = collision.LocalPoint;
            _endDistanceJoint.distance = _totalRopeLength - usedRope;

            if (EPSILON > _endDistanceJoint.distance)
            {
                _linePoints.RemoveFirst();
            }

        }
        else
        {
            _totalRopeLength = Mathf.Min(_totalRopeLength, Vector2.Distance(_end.position, _start.position));
            _endDistanceJoint.connectedBody = _start;
            _endDistanceJoint.distance = _totalRopeLength;
            _endDistanceJoint.connectedAnchor = Vector2.zero;
        }
    }

    private void LateUpdate()
    {
        RenderRope();
    }

    private void UpdateStart()
    {
        if (_previousStartPosition != _start.position)
        {
            UpdateRope(_end, _start, _ropeStartIgnoreMask, () => _linePoints.Last.Value, () =>
            {
                var last = _linePoints.Last;
                _linePoints.RemoveLast();
                return last.Value;
            }, (value) => _linePoints.AddLast(value));
            _previousStartPosition = _start.position;
        }
    }
    private void UpdateEnd()
    {
        if (_previousEndPosition != _end.position)
        {
            UpdateRope(_start, _end, _ropeEndIgnoreMask, () => _linePoints.First.Value, () =>
            {
                var first = _linePoints.First;
                _linePoints.RemoveFirst();
                return first.Value;
            }, (value) => _linePoints.AddFirst(value));
            _previousEndPosition = _end.position;
        }
    }

    private void UpdateRope(Rigidbody2D fromBody, Rigidbody2D toBody, int mask, Func<CollisionPoint> peek, Func<CollisionPoint> pop, Action<CollisionPoint> push)
    {
        TryUnravelRope(fromBody, toBody, pop, push, mask);
        TryAddRopeHingePoint(fromBody, toBody, peek, push, mask);
    }

    private void RenderRope()
    {
        var points = new Vector3[_linePoints.Count + 2];
        points[0] = _end.transform.position;

        _linePoints.Select(x => x.WorldPoint).ToArray().CopyTo(points, 1);

        points[points.Length - 1] = _start.position;

        _lineRenderer.positionCount = 0;
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }

    private void TryAddRopeHingePoint(Rigidbody2D fromBody, Rigidbody2D toBody, Func<CollisionPoint> peek, Action<CollisionPoint> push, int mask)
    {
        var start = _linePoints.Count == 0 ? new CollisionPoint(Vector2.zero, fromBody) : peek();

        var startPos = start.WorldPoint;
        var endVisible = Physics2D.Linecast(startPos, toBody.position).rigidbody == toBody;
        if (!endVisible)
        {
            var hit = Physics2D.Linecast(toBody.position, startPos, mask);

            if (hit.collider != null && hit.rigidbody != fromBody)
            {
                float distance = float.PositiveInfinity;
                if (_linePoints.Count > 0)
                {
                    var peekPoint = peek();
                    var peekPos = peekPoint.WorldPoint;
                    distance = Vector2.Distance(peekPos, hit.point);
                }
                if (distance > EPSILON)
                {
                    push(new CollisionPoint(hit.transform.InverseTransformPoint(hit.point), hit.rigidbody));
                }
            }
        }
    }

    private void TryUnravelRope(Rigidbody2D fromBody, Rigidbody2D toBody, Func<CollisionPoint> pop, Action<CollisionPoint> push, int mask)
    {
        if (_linePoints.Count == 0)
        {
            return;
        }

        var currentPoint = pop();
        var previousPoint = _linePoints.Count > 0 ? pop() : new CollisionPoint(Vector2.zero, fromBody);

        var previousPos = previousPoint.WorldPoint;
        var direction = ((Vector2)previousPos - toBody.position).normalized;
        var hit = Physics2D.Raycast(toBody.position, direction, float.PositiveInfinity, mask);
        var previousPointHitDistance = Vector2.Distance(hit.point, previousPos);

        var previousPointIsVisible = previousPointHitDistance < EPSILON || hit.rigidbody == fromBody;

        if (previousPointIsVisible)
        {
            if ((Vector2)previousPos != fromBody.position)
            {
                push(previousPoint);
            }
        }
        else
        {
            if ((Vector2)previousPos != fromBody.position)
            {
                push(previousPoint);
            }
            push(currentPoint);
        }
    }

    public void Activate()
    {
        _attached = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _attached = false;
        gameObject.SetActive(false);
    }

    private struct CollisionPoint
    {
        public Vector3 LocalPoint { get; private set; }
        public Rigidbody2D Body { get; private set; }

        public Vector3 WorldPoint
        {
            get
            {
                return Body.transform.TransformPoint(LocalPoint);
            }
        }

        public CollisionPoint(Vector3 point, Rigidbody2D body)
        {
            LocalPoint = point;
            Body = body;
        }
    }
}

