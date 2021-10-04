// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerInput.cs" author="Lars" company="None">
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

using Utilities.ObjectPool;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour
{
    private float _lastDashTime;

    private float _maxVelocity = 3f;

    private const float NoLightDieTimer = 1.5f;

    private float _acceleration = 0.05f;

    private bool _disableInput;

    private bool _inLight = true;

    private float _lightExitTime;

    private Vector2 _movement = new Vector2();

    private Rigidbody2D _rigidBody;

 

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

 

    private void Move()
    {
        var inputH = Input.GetAxisRaw("Horizontal");
        var inputV = Input.GetAxisRaw("Vertical");

        _movement.Set(inputH, inputV);
        _movement.Normalize();

        _rigidBody.AddForce(_movement * _acceleration * Time.fixedDeltaTime);
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, _maxVelocity);

    }

 

  
    // Use this for initialization
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
}