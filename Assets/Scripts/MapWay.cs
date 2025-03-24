using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWay : MonoBehaviour
{
    public int minX;
    public int maxX;
    private int unit;

    private void Awake()
    {
        unit = Mathf.Abs((maxX - minX) / 3);
    }

    public int PositionToWayIndex(Vector3 position)
    {
        int index = Mathf.FloorToInt((position.x - minX) / unit);

        index = Mathf.Clamp(index, 0, 2);
        return index;
    }

    public Vector3 WayIndexToPosition(int index)
    {
        var position = Vector3.zero;
        position.x = index * unit + 0.5f * unit + minX;
        return position;
    }
    public float WayIndexToX(int index)
    {
        return index * unit + 0.5f * unit + minX;
    }
}
