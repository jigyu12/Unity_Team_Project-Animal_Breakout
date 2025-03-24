using UnityEngine;

public class CreateLandMarkIntoGridMap : MonoBehaviour
{
    public PlaneToGridMap GridMap;
    
    private const int NonTrapTileCount = 6;
    
    private void Start()
    {
        CreateLeftStructure();
        CreateRightStructure();
        
        CreateRandomItemAndTrap();
    }
    
    private void CreateLeftStructure()
    {
        foreach (var tile in GridMap.LeftSideTile)
        {
            Instantiate(GridMap.sidePrefab, tile.transform.position, Quaternion.identity);
        }
    }
    
    private void CreateRightStructure()
    {
        foreach (var tile in GridMap.RightSideTile)
        {
            Instantiate(GridMap.sidePrefab, tile.transform.position, Quaternion.identity);
        }
    }

    private void CreateRandomItemAndTrap()
    {
        CreateRandomTrap();
        CreateRandomItem();
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