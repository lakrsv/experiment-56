using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lava : MonoBehaviour
{
    [SerializeField]
    private TilemapRenderer _world;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _world.sortingOrder = 10;
            GameManager.Instance.Respawn();
        }
    }
}
