using UnityEngine;

public static class Utils
{
    public const string PlayerTag = "Player";
    
    public static void SetChildScaleFitToParent(GameObject child, GameObject parent)
    {
        child.transform.localScale = 
            new Vector3(1f / parent.transform.localScale.x, 1f / parent.transform.localScale.y, 1f / parent.transform.localScale.z);
    }

    public static int GetTileIndex(int x, int y, int rows)
    {
        return x + y * rows;
    }
}