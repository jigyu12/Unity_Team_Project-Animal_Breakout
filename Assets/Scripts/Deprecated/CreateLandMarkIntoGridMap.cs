using System.Collections.Generic;
using UnityEngine;

public class CreateLandMarkIntoGridMap : MonoBehaviour
{
    public PlaneToGridMap GridMap;
    
    private const int NonTrapTileCount = 6;
    private const int NonSideStructureCount = 5;
    
    public Dictionary<int, TrapType> Traps = new();
    
    public WayType wayType;
    
    public GameObject sidePrefab;
    public GameObject wallPrefab;
    
    private void Start()
    {
        wayType = (WayType)Random.Range(0, (int)WayType.Count);

        CreateLandMark();
    }

    private void CreateLandMark()
    {
        CreateLeftStructure();
        CreateRightStructure();
        CreateWall();
        CreateRandomTrap();
        CreateRandomItem();
    }
    
    private void CreateLeftStructure()
    {
        if (wayType != WayType.Left)
        {
            foreach (var tile in GridMap.LeftSideTile)
            {
                Instantiate(sidePrefab, tile.transform.position, Quaternion.identity);
            }
        }
        else
        {
            int rows = GridMap.LeftSideTile.GetLength(0);
            int cols = GridMap.LeftSideTile.GetLength(1);

            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < cols - NonSideStructureCount; ++col)
                {
                    var tile = GridMap.LeftSideTile[row, col];
                    Instantiate(sidePrefab, tile.transform.position, Quaternion.identity);
                }
            }
        }
    }
    
    private void CreateRightStructure()
    {
        if (wayType != WayType.Right)
        {
            foreach (var tile in GridMap.RightSideTile)
            {
                Instantiate(sidePrefab, tile.transform.position, Quaternion.identity);
            }
        }
        else
        {
            int rows = GridMap.RightSideTile.GetLength(0);
            int cols = GridMap.RightSideTile.GetLength(1);

            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < cols - NonSideStructureCount; ++col)
                {
                    var tile = GridMap.RightSideTile[row, col];
                    Instantiate(sidePrefab, tile.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void CreateWall()
    {
        if (wayType != WayType.Straight)
        {
            int rows = GridMap.MiddleTile.GetLength(0);
            int cols = GridMap.MiddleTile.GetLength(1);

            int middleRowIndex = rows / 2;
            int lastColIndex = cols - 1;
            
            GameObject targetTile = GridMap.MiddleTile[middleRowIndex, lastColIndex];

            Vector3 tilePos = targetTile.transform.position;

            float offsetZ = GridMap.planePrefab.GetComponent<MeshRenderer>().bounds.size.z / GridMap.cols * 0.5f;

            Vector3 wallPosition = tilePos + new Vector3(0f, 0f, offsetZ);
        
            Instantiate(wallPrefab, wallPosition, Quaternion.identity);
        }
    }
    
    private void CreateRandomTrap()
    {
        for (int i = 0 + NonTrapTileCount; i < GridMap.cols - NonTrapTileCount; i += 3)
        {
            
        }
    }
    
    private void CreateRandomItem()
    {
        
    }
}