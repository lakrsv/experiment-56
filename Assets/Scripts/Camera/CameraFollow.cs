using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float _yOffset = 1f;
    private float _smoothTime = 0.3f;


    private Vector3 _velocity;
    private Transform _player;

    private void LateUpdate()
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        var targetPosition = new Vector3(_player.position.x, _player.position.y + _yOffset, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    }
}
