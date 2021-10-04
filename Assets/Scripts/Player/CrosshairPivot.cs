using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairPivot : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = _player.position;
    }
}
