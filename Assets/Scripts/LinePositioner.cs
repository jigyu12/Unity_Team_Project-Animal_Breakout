using UnityEngine;

public class LinePositioner
{
    private readonly float unitWidth;
    private readonly int minX;
    private readonly int laneCount;

    public LinePositioner(int minX, int maxX, int laneCount)
    {
        this.minX = minX;
        this.laneCount = laneCount;
        unitWidth = Mathf.Abs(maxX - minX) / (float)laneCount;
    }

    public float GetXForIndex(int index)
    {
        index = Mathf.Clamp(index, 0, laneCount - 1);
        return minX + (index + 0.5f) * unitWidth;
    }

    public int GetIndexForX(float x)
    {
        int index = Mathf.FloorToInt((x - minX) / unitWidth);
        return Mathf.Clamp(index, 0, laneCount - 1);
    }
}
