using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _follow;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _sprite;



    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = _follow.transform.position;
        _animator.SetFloat("Speed", _follow.angularVelocity / (360f));

        var rotation = _follow.angularVelocity / 15f;
        rotation += Random.Range(-rotation, rotation);

        var flipX = _follow.angularVelocity > 0f;
        _sprite.flipX = flipX;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotation, Vector3.forward), 5f * Time.deltaTime);
    }
}
