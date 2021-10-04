using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _startText;
    [SerializeField]
    private TextMeshProUGUI _exitText;
    [SerializeField]
    private CanvasGroup _overlay;

    private bool _fading = false;

    private void Awake()
    {
        _overlay.alpha = 1f;
        _startText.alpha = 0f;
        _exitText.alpha = 0f;
        var fadeOutOverlay = _overlay.DOFade(0f, 1.0f)
            .SetDelay(0.5f);

        var fadeInStart = _startText.DOFade(1f, 0.5f)
            .SetEase(Ease.InSine);
        var fadeInExit = _exitText.DOFade(1f, 0.5f)
            .SetEase(Ease.InSine);

        DOTween.Sequence()
            .Append(fadeOutOverlay)
            .Append(fadeInStart)
            .Append(fadeInExit);
    }

    public void FadeOut()
    {
        if (_fading)
        {
            return;
        }
        _fading = true;
        _overlay.DOFade(1f, 1.0f).OnComplete(() => SceneManager.LoadScene(1));
    }
}
