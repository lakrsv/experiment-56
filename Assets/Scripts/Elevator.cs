using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;

    public float ElevationMax;

    private float _maxY;
    private float _minY;

    private float _playTime;
    private bool _shouldPlay = false;
    private bool _play = false;

    private void Awake()
    {
        _minY = _rigidbody.transform.position.y;
        _maxY = _rigidbody.transform.position.y + ElevationMax;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _shouldPlay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _shouldPlay = false;
        }
    }

    private void Update()
    {
        if (_shouldPlay != _play)
        {
            var timePassed = Time.time - _playTime;
            if (timePassed > 4f)
            {
                if (_shouldPlay)
                {
                    Play();
                } else
                {
                    Rewind();
                }
                _play = _shouldPlay;
                _playTime = Time.time;

            }
        }
    }

    private void Play()
    {
        _rigidbody.DOMoveY(_maxY, 2.0f)
             .SetDelay(0.5f)
             .SetEase(Ease.InOutSine)
             .SetUpdate(UpdateType.Fixed)
             .SetSpeedBased();
    }

    private void Rewind()
    {
        _rigidbody.DOMoveY(_minY, 2.0f)
            .SetDelay(0.5f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(UpdateType.Fixed)
            .SetSpeedBased();
    }
}
