using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ObjectPool;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField]
    private TrailRenderer _trail;
    [SerializeField]
    private Rigidbody2D _rigidbody;

    public bool IsEnabled => gameObject.activeInHierarchy;

    public void AddForce(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
        transform.rotation = Quaternion.LookRotation(force, Vector3.forward);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.forward);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _trail.enabled = true;
    }

    public void Deactivate()
    {
        _rigidbody.velocity = Vector2.zero;
        _trail.enabled = false;
        gameObject.SetActive(false);
    }
}
