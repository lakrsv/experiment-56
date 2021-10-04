using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;

[RequireComponent(typeof(Rigidbody2D))]
public class Fire : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private Crosshair _crosshair;
    [SerializeField]
    private Grapple _grapple;
    [SerializeField]
    private BallPowerRing _power;

    private PlayerControls _playerControls;

    private float _walkFireForce = 250f;
    private float _defaultFireForce = 800f;
    private float _fireForce = 800f;

    private bool _override = false;

    [SerializeField]
    private ParticleSystem _particles;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public void OverrideForce(float force)
    {
        _override = true;
        _fireForce = force;
    }

    public void StopOverride()
    {
        _override = false;
        _fireForce = _defaultFireForce;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(GameManager.Instance == null)
        {
            var aimDirection = -Vector2.right;
            _rigidbody.AddForce(aimDirection * Time.fixedDeltaTime * 100f);
            _rigidbody.AddTorque(-10f * Time.deltaTime);
            return;
        }
        if (GameManager.Instance.Respawning)
        {
            return;
        }

        if (_power.PowerLevel > 0f && _playerControls.Ground.Aim.ReadValue<Vector2>().magnitude > 0.5f)
        {
            Debug.Log("Fire!");
            var force = _override ? _fireForce : _grapple.IsGrappling ? _fireForce : _walkFireForce;
            Debug.Log(force);
            var aimDirection = _crosshair.GetAimDirection(transform.position);
            _rigidbody.AddForce(aimDirection * Time.fixedDeltaTime * force);

            _power.RemovePower(0.2f * Time.deltaTime);

            //var projectile = ObjectPools.Instance.GetPooledObject<Projectile>();
            //projectile.transform.position = _projectileSpawn.position;
            //projectile.AddForce(aimDirection * 50f);

            _particles.Play();
        }
        else
        {
            _particles.Stop();
        }
    }
}
