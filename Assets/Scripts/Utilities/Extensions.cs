using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector2 GetRandomPointInCircleAround(this Vector3 position, float radius)
    {
        float angle = Random.Range(0f, 1f) * (Mathf.PI * 2);
        float rad = Random.Range(0, 1f) * radius;

        var xPos = position.x + rad * Mathf.Cos(angle);
        var yPos = position.y + rad * Mathf.Sin(angle);

        return new Vector2(xPos, yPos);
    }

    public static T GetRandomFromArray<T>(this T[] array)
    {
        var index = Random.Range(0, array.Length);
        return array[index];
    }
}
