using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _player;
    [SerializeField]
    private Transform _fakeCollider;
    [SerializeField]
    private Fire _fire;

    private Vector2 _startPosition;

    private bool _isActive = true;

    private void Awake()
    {
        _startPosition = _player.position;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        _fire.OverrideForce(150f);
        _player.transform.position = _startPosition;

        if (Mathf.Abs(_player.angularVelocity) > 700f)
        {
            _isActive = false;
            _fire.StopOverride();
            _fakeCollider.gameObject.SetActive(false);
        }
    }
}
