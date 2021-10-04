using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GroundCheck : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        IsGrounded = DoGroundCheck();
        Debug.Log("Grounded: " + IsGrounded);
    }

    private bool DoGroundCheck()
    {
        var layer = LayerMask.GetMask(Layers.FLOOR, Layers.GRAPPLE, Layers.ELEVATOR);
        var raycast = Physics2D.Raycast(_rigidbody.position, Vector2.down, 0.75f, layer);
        return raycast;
    }
}
