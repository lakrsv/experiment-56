using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Utilities;

public class GameManager : MonoSingleton<GameManager>
{
    public bool Respawning { get; private set; }
    [HideInInspector]
    public SpawnPoint ActiveSpawnPoint;

    [SerializeField]
    private CanvasGroup _overlay;
    [SerializeField]
    private Rigidbody2D _player;
    [SerializeField]
    private BallPowerRing _power;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private TilemapRenderer _world;
    [SerializeField]
    private TextMeshProUGUI _winText;

    private void Awake()
    {
        _overlay.alpha = 1.0f;
        _overlay.DOFade(0.0f, 2.0f)
            .SetEase(Ease.InSine);
    }

    public void WinGame()
    {
        Respawning = true;
        var overlayFadeIn = _overlay.DOFade(1.0f, 3.0f)
                .SetEase(Ease.OutSine);
        _winText.text = "Thank you for playing my Ludum Dare entry.\n\n- Lars";
        _winText.alpha = 0f;
        var winTextFadeIn = _winText.DOFade(1f, 4.0f)
            .SetDelay(0.5f);
        var winTextFadeOut = _winText.DOFade(0f, 2.0f);
        DOTween.Sequence()
            .Append(overlayFadeIn)
            .Append(winTextFadeIn)
            .AppendInterval(3.0f)
            .Append(winTextFadeOut)
            .AppendInterval(0.5f)
            .OnComplete(() => SceneManager.LoadScene(0));
    }

    public void Respawn(bool instant = false)
    {
        if (instant)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        Respawning = true;
        if (ActiveSpawnPoint == null)
        {
            var overlayFade = _overlay.DOFade(1.0f, 1.0f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
        else
        {
            var overlayFadeIn = _overlay.DOFade(1.0f, 1.0f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
            {
                _player.constraints = RigidbodyConstraints2D.FreezeAll;
                _player.velocity = Vector2.zero;
                _player.transform.position = ActiveSpawnPoint.transform.position;
                _camera.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 1f, _camera.transform.position.z);
                _power.SetPower(1.0f);
                _world.sortingOrder = 0;
            });
            var overlayFadeOut = _overlay.DOFade(0.0f, 1.0f)
                .SetDelay(0.5f)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
            {
                _player.constraints = RigidbodyConstraints2D.None;
                Respawning = false;
            });
            DOTween.Sequence()
                .Append(overlayFadeIn)
                .Append(overlayFadeOut);
        }
    }
}
