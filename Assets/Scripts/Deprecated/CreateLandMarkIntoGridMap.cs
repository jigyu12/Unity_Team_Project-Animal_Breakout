using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateLandMarkIntoGridMap : MonoBehaviour
{
    public PlaneToGridMap GridMap;
    
    private const int NonTrapTileCount = 6;
    private const int NonSideStructureCount = 5;
    private const int ItemGroupCount = 5;
    private const int ItemGenerateTileCount = 3;
    private const int NonItemGenerateTileCount = 3;
    
    public Dictionary<int, CollidableMapObject> MapObjectDictionary = new();
    
    public WayType wayType;
    
    public GameObject sidePrefab;
    public GameObject wallPrefab;
    public GameObject trapPrefab;
    
    public GameObject humanPrefab;
    public GameObject rewardCoinPrefab;
    public GameObject penaltyCoinPrefab;
    
    private void Start()
    {
        wayType = (WayType)Random.Range(1, (int)WayType.Count);

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
        int rows = GridMap.MiddleTile.GetLength(0);
        int cols = GridMap.MiddleTile.GetLength(1);
        
        for (int i = 0 + NonTrapTileCount; i < GridMap.cols - NonTrapTileCount; i += 3)
        {
            int bombRow = Random.Range(0, rows);
            var bomb = Instantiate(trapPrefab, GridMap.MiddleTile[bombRow, i].transform.position + Vector3.up, Quaternion.identity);
            bomb.TryGetComponent(out Trap bombTrapComponent);
            bombTrapComponent.Init(TrapType.Bomb);

            var bombIndex = Utils.GetTileIndex(bombRow, i, cols);
            if(MapObjectDictionary.ContainsKey(bombIndex))
            {
                Debug.Assert(false, $"Object already exists in the map. Cant place bomb in [{bombRow} , {i}]");
            }
            else
            {
                if (!bomb.TryGetComponent(out Trap trapComponent))
                {
                    Debug.Assert(false, "Invalid Bomb GameObject");
                }
                else
                {
                    MapObjectDictionary.Add(bombIndex, trapComponent);
                }
            }

            float createHoleChance = Random.value;
            if (createHoleChance <= 0.3f)
            {
                int holeRow;
                while (true)
                {
                    holeRow = Random.Range(0, rows);

                    if (holeRow != bombRow)
                    {
                        break;
                    }
                }
                
                var hole = Instantiate(trapPrefab, GridMap.MiddleTile[holeRow, i].transform.position, Quaternion.identity);
                hole.TryGetComponent(out Trap holeTrapComponent);
                holeTrapComponent.Init(TrapType.Hole);
                
                var holeIndex = Utils.GetTileIndex(holeRow, i, cols);
                if(MapObjectDictionary.ContainsKey(holeIndex))
                {
                    Debug.Assert(false, $"Object already exists in the map. Cant place hole in [{holeRow} , {i}]");
                }
                else
                {
                    if (!hole.TryGetComponent(out Trap trapComponent))
                    {
                        Debug.Assert(false, "Invalid Hole GameObject");
                    }
                    else
                    {
                        MapObjectDictionary.Add(holeIndex, trapComponent);
                    }
                }
            }
        }
    }
    
    private void CreateRandomItem()
    {
        CreateRandomRewardCoin();
        CreateRandomHuman();
        CreatePenaltyCoin();
    }

    private void CreateRandomRewardCoin()
    {
        
    }
    
    private void CreateRandomHuman()
    {
        
    }
    
    private void CreatePenaltyCoin()
    {
        
    }
}