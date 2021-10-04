using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundCheck))]
public class Grapple : MonoBehaviour
{
    [SerializeField]
    private PhysicsMaterial2D _playerNormal;
    [SerializeField]
    private PhysicsMaterial2D _slippery;

    [SerializeField]
    private Rigidbody2D _world;

    [SerializeField]
    private DistanceJoint2D _distanceJoint;
    [SerializeField]
    private Crosshair _crosshair;
    private Rigidbody2D _rigidbody;
    private GroundCheck _groundCheck;

    private PlayerControls _playerControls;

    private Rope _currentRope;
    private bool _isGrappling = false;
    private bool _attached = false;

    private Vector2 _currentPoint;
    private Vector2 _targetAttachPoint;

    [SerializeField]
    private LineRenderer _lineRenderer;

    public bool IsGrappling => _isGrappling;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _distanceJoint = GetComponent<DistanceJoint2D>();
        _groundCheck = GetComponent<GroundCheck>();
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.Respawning)
        {
            return;
        }
        var grappling = _playerControls.Ground.Grapple.ReadValue<float>() > 0;
        if (grappling && !_isGrappling)
        {
            Debug.Log("Grapple time!");
            _isGrappling = StartGrapple();
        }
        else if (!grappling && _isGrappling)
        {
            _isGrappling = !StopGrapple();
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.Respawning)
        {
            if (_currentRope != null || _isGrappling || _attached)
            {
                _isGrappling = false;
                _attached = false;
                if (_currentRope != null)
                {
                    _currentRope.Detach();
                }
                _distanceJoint.enabled = false;

            }
            return;
        }
        if (_isGrappling && !_attached)
        {
            var nextPoint = Vector2.MoveTowards(_currentPoint, _targetAttachPoint, 25f * Time.deltaTime);
            _currentPoint = nextPoint;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _currentPoint);

            if (_currentPoint == _targetAttachPoint)
            {
                _attached = true;
                _currentRope = ObjectPools.Instance.GetPooledObject<Rope>();
                _distanceJoint.enabled = true;
                _currentRope.Attach(_distanceJoint, _targetAttachPoint);
                _currentRope.Activate();

                _lineRenderer.positionCount = 0;
            }
        }
    }


    public bool StartGrapple()
    {
        var startPosition = _rigidbody.position;

        var aimDirection = _crosshair.GetAimDirection(transform.position);
        int currentAngle = aimDirection.x > 0 ? -45 : 45;
        int angleDecrement = (int)Mathf.Sign(currentAngle) * 5;

        RaycastHit2D raycast;
        do
        {
            var rayDirection = Quaternion.AngleAxis(currentAngle, Vector3.forward) * Vector2.up;
            raycast = Physics2D.Raycast(startPosition, rayDirection, 10f, LayerMask.GetMask(Layers.GRAPPLE, Layers.FLOOR));
            currentAngle -= angleDecrement;
        } while ((!raycast || raycast.transform.gameObject.layer != LayerMask.NameToLayer(Layers.GRAPPLE)) && currentAngle - angleDecrement != angleDecrement);

        if (!raycast || raycast.transform.gameObject.layer != LayerMask.NameToLayer(Layers.GRAPPLE))
        {
            return false;
        }

        _currentPoint = transform.position;
        _targetAttachPoint = raycast.point;

        return true;
    }

    public bool StopGrapple()
    {
        if (!_attached)
        {
            return false;
        }
        _attached = false;
        _currentRope.Detach();
        _currentRope = null;
        _distanceJoint.enabled = false;

        return true;
    }
}
