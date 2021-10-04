using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class DistanceSortOrder : MonoBehaviour
{
    private SortingGroup _sortingGroup;

    private void Awake()
    {
        _sortingGroup = GetComponent<SortingGroup>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        _sortingGroup.sortingOrder = 1000 - Mathf.FloorToInt(transform.position.y * 5);
    }
}
