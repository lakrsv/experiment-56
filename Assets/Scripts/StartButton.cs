using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField]
    private StartScreen _start;
    private RectTransform _rect;

    private PlayerControls _playerControls;

    private float _punchTime = 0f;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        if (_playerControls.Ground.Play.triggered)
        {
            OnPointerEnter(null);
            OnPointerClick(null);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _start.FadeOut();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Time.time - _punchTime > 0.5f)
        {
            _rect.DOPunchAnchorPos(Vector2.up * 25f, 0.25f);

        }
        _punchTime = Time.time;
    }
}
