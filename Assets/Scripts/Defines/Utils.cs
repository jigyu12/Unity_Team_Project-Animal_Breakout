using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public const string PlayerTag = "Player";
    
    public static void SetChildScaleFitToParent(GameObject child, GameObject parent)
    {
        child.transform.localScale = 
            new Vector3(1f / parent.transform.localScale.x, 1f / parent.transform.localScale.y, 1f / parent.transform.localScale.z);
    }

    public static bool IsChanceHit(float chance)
    {
        return Random.value <= chance;
    }

    /// <summary>
    /// The order of probabilities must match the order of their corresponding enum values.
    /// </summary>
    public static int GetEnumIndexByChance(List<float> chances)
    {
        if (chances == null || chances.Count == 0)
        {
            throw new System.ArgumentException("The chances list is null or empty.", nameof(chances));
        }
        
        float randValue = Random.value;
        
        float sum = 0f;
        for (int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];

            if (randValue <= sum)
            {
                return i;
            }
        }
        
        throw new System.ArgumentException("The sum of input chances must be equal to 1f.", nameof(chances));
    }
    
    public static int GetTileIndex(int row, int col, int cols)
    {
        return col + row * cols;
    }
}