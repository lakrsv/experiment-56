//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(GroundCheck))]
//public class PlayerMovement : MonoBehaviour
//{
//    [SerializeField]
//    private float _maxVelocity = 4f;
//    [SerializeField]
//    private float _acceleration = 5000f;

//    private GroundCheck _groundCheck;
//    private Rigidbody2D _rigidbody;
//    private PlayerControls _playerControls;


//    // Start is called before the first frame update
//    private void Awake()
//    {
//        _rigidbody = GetComponent<Rigidbody2D>();
//        _groundCheck = GetComponent<GroundCheck>();
//        _playerControls = new PlayerControls();
//    }

//    private void OnEnable()
//    {
//        _playerControls.Enable();
//    }

//    private void OnDisable()
//    {
//        _playerControls.Disable();
//    }

//    // Update is called once per frame
//    private void FixedUpdate()
//    {
//        if (_groundCheck.IsGrounded)
//        {
//            Move();
//        }
//    }

//    private void Move()
//    {
//        var movement = _playerControls.Ground.Move.ReadValue<float>();
//        _rigidbody.AddForce(Vector2.right * movement * _acceleration * Time.fixedDeltaTime);
//        //_rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxVelocity);
//    }
//}
