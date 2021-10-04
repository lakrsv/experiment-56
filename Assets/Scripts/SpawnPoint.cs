using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint ActiveSpawnPoint;

    [SerializeField]
    private ParticleSystem _particles;
    [SerializeField]
    private SpriteRenderer _spawnHamster;

    private bool _activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.ActiveSpawnPoint = this;
            if (!_activated)
            {
                _particles.Play();
                DOTween.Sequence()
                    .Append(
                    _spawnHamster.transform.DOMoveY(0.25f, 1.0f)
                    .SetRelative()
                    .SetEase(Ease.InOutSine))
                    .Append(_spawnHamster.transform.DOMoveY(-0.25f, 1.0f)
                    .SetRelative()
                    .SetEase(Ease.InOutSine))
                    .SetLoops(-1);
            }
        }
    }
}
