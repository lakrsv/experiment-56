using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPowerRing : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private SpriteMask _mask;
    [SerializeField]
    private GroundCheck _groundCheck;
    public float PowerLevel { get; private set; }

    private void Awake()
    {
        PowerLevel = 1.0f;
    }

    public void RemovePower(float powerToRemove)
    {
        PowerLevel -= powerToRemove;
        PowerLevel = Mathf.Clamp01(PowerLevel);
    }

    public void SetPower(float power)
    {
        PowerLevel = power;
        PowerLevel = Mathf.Clamp01(PowerLevel);
        _mask.alphaCutoff = 1 - PowerLevel;
    }

    private void LateUpdate()
    {
        if (_groundCheck != null && _groundCheck.IsGrounded)
        {
            PowerLevel += (Mathf.Abs(_rigidbody.angularVelocity) / 3000f) * Time.deltaTime;
            PowerLevel = Mathf.Clamp01(PowerLevel);
        }
        _mask.alphaCutoff = 1 - PowerLevel;
    }
}
