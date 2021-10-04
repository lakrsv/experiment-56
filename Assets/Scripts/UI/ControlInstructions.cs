using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlInstructions : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Animator _wAnimator, _aAnimator, _sAnimator, _dAnimator, _stickAnimator;

    public bool PlayW, PlayA, PlayS, PlayD, PlayStick;

    // Start is called before the first frame update
    void Start()
    {
        _wAnimator.speed = PlayW ? 1 : 0;
        _aAnimator.speed = PlayA ? 1 : 0;
        _sAnimator.speed = PlayS ? 1 : 0;
        _dAnimator.speed = PlayD ? 1 : 0;
        _stickAnimator.speed = PlayStick ? 1 : 0;
    }
}
