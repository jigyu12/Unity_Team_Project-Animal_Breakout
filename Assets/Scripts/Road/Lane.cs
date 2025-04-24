using UnityEngine;

public class Lane : MonoBehaviour
{
    public Vector3[] laneIndexPosition;

    public Vector3 LaneIndexToPosition(int index)
    {
        return laneIndexPosition[index];
    }
}