using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Transform _pivot;
    private float _aimSpeed = 5.0f;
    private float _radius = 1.5f;

    private Vector2 _currentAim = Vector2.right;

    private PlayerControls _playerControls;

    public Vector2 GetAimDirection(Vector3 origin)
    {
        return (transform.position - origin).normalized;
    }

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }
    // Start is called before the first frame update
    private void Start()
    {
        _pivot = transform.parent;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.Respawning)
        {
            return;
        }
        //if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        //{
        //    var currentMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //    currentMousePosition.z = 0;
        //    transform.position = currentMousePosition;
        //}
        //else
        //{
            GamepadAim();
        //}
    }

    private void GamepadAim()
    {
        var aim = _playerControls.Ground.Aim.ReadValue<Vector2>();
        if (aim == Vector2.zero)
        {
            return;
        }

        _currentAim += aim * _aimSpeed * Time.fixedDeltaTime;
        _currentAim = Vector2.ClampMagnitude(_currentAim, 1.0f);

        transform.localPosition = new Vector3(_currentAim.x * _radius, _currentAim.y * _radius, 0f);
    }
}
