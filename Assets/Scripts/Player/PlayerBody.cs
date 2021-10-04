using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _thisRigidbody;
    [SerializeField]
    private Rigidbody2D _playerRigidbody;

    // Update is called once per frame
    private void LateUpdate()
    {
        _thisRigidbody.angularVelocity = _playerRigidbody.angularVelocity;
        _thisRigidbody.transform.position = _playerRigidbody.transform.position;
    }
}
