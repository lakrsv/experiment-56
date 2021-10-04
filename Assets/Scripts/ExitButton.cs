using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private RectTransform _rect;
    private float _punchTime;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
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
