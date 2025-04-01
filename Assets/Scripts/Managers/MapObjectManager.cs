using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class MapObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject trapBombPrefab;
    [SerializeField] private GameObject trapHolePrefab;
    [SerializeField] private GameObject itemRewardCoinPrefab;
    [SerializeField] private GameObject itemHumanPrefab;
    [SerializeField] private GameObject itemPenaltyCoinPrefab;
    
    private ObjectPool<GameObject> wallPool;
    private ObjectPool<GameObject> trapBombPool;
    private ObjectPool<GameObject> trapHolePool;
    private ObjectPool<GameObject> itemRewardCoinPool;
    private ObjectPool<GameObject> itemHumanPool;
    private ObjectPool<GameObject> itemPenaltyCoinPool;

    private const int nonObjectTileCount = 6;
    private const int itemGroupCount = 5;
    private const int itemGenerateTileCount = 3;

    private const int trapGenerateTileOffset = 3;

    private const float tileSize = 1f;

    private const float spawnHoleChance = 0.3f; // 구덩이 스폰 확률

    private const float spawnRewardCoinChance = 0.3f; // 좋은 코인 스폰 확률
    private List<float> rewardItemSpawnChances = new();
    [SerializeField][ReadOnly] private float bronzeCoinSpawnChance = 0.5f;
    [SerializeField][ReadOnly] private float sliverCoinSpawnChance = 0.2f;
    [SerializeField][ReadOnly] private float goldCoinSpawnChance = 0.15f;
    [SerializeField][ReadOnly] private float platinumCoinSpawnChance = 0.1f;
    [SerializeField][ReadOnly] private float diamondCoinSpawnChance = 0.05f;

    private const float spawnHumanChance = 0.4f; // 인간 아이템 스폰 확률
    private List<float> humanSpawnChances = new();
    [SerializeField][ReadOnly] private float juniorResearcherSpawnChance = 0.6f;
    [SerializeField][ReadOnly] private float researcherSpawnChance = 0.3f;
    [SerializeField][ReadOnly] private float seniorResearcherSpawnChance = 0.1f;

    private const float spawnPenaltyCoinChance = 0.3f; // 안좋은 코인 스폰 확률
    private List<float> penaltyCoinSpawnChances = new();
    [SerializeField][ReadOnly] private float ghostCoinSpawnChance = 0.5f;
    [SerializeField][ReadOnly] private float poisonCoinSpawnChance = 0.2f;
    [SerializeField][ReadOnly] private float skullCoinSpawnChance = 0.15f;
    [SerializeField][ReadOnly] private float fireCoinSpawnChance = 0.1f;
    [SerializeField][ReadOnly] private float blackHoleCoinSpawnChance = 0.05f;

    private void Awake()
    {
        rewardItemSpawnChances.Add(bronzeCoinSpawnChance);
        rewardItemSpawnChances.Add(sliverCoinSpawnChance);
        rewardItemSpawnChances.Add(goldCoinSpawnChance);
        rewardItemSpawnChances.Add(platinumCoinSpawnChance);
        rewardItemSpawnChances.Add(diamondCoinSpawnChance);

        humanSpawnChances.Add(juniorResearcherSpawnChance);
        humanSpawnChances.Add(researcherSpawnChance);
        humanSpawnChances.Add(seniorResearcherSpawnChance);

        penaltyCoinSpawnChances.Add(ghostCoinSpawnChance);
        penaltyCoinSpawnChances.Add(poisonCoinSpawnChance);
        penaltyCoinSpawnChances.Add(skullCoinSpawnChance);
        penaltyCoinSpawnChances.Add(fireCoinSpawnChance);
        penaltyCoinSpawnChances.Add(blackHoleCoinSpawnChance);
        
        wallPool = ObjectPoolManager.Instance.CreateObjectPool(wallPrefab,
            () => Instantiate(wallPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        trapBombPool = ObjectPoolManager.Instance.CreateObjectPool(trapBombPrefab,
            () => Instantiate(trapBombPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
        
        trapHolePool = ObjectPoolManager.Instance.CreateObjectPool(trapHolePrefab,
            () => Instantiate(trapHolePrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        itemRewardCoinPool = ObjectPoolManager.Instance.CreateObjectPool(itemRewardCoinPrefab,
            () => Instantiate(itemRewardCoinPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        itemHumanPool = ObjectPoolManager.Instance.CreateObjectPool(itemHumanPrefab,
            () => Instantiate(itemHumanPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        itemPenaltyCoinPool = ObjectPoolManager.Instance.CreateObjectPool(itemPenaltyCoinPrefab,
            () => Instantiate(itemPenaltyCoinPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
    }

    private void Start()
    {
        foreach(var entry in DataTableManager.mapObjectsDataTable.GetTableEntries())
        {
            int prefabID = entry.Key;
            List<MapObjectsData> dataList = entry.Value;

            Debug.Log($"PrefabID: {prefabID}, Entries Count: {dataList.Count}");
        }

    }

    public struct MapObjectsBlueprint
    {
        public ObjectType[,] objectsTypes;
        public Action<Vector3>[,] objectsConstructors;

        public int wallIndex;
    }

    public Dictionary<int, MapObjectsBlueprint> GenerateMapObjectInformation(int rows, int cols)
    {
        Dictionary<int, MapObjectsBlueprint> generateMapObjectInformationDictionary = new();

        foreach (var entry in DataTableManager.mapObjectsDataTable.GetTableEntries())
        {
            MapObjectsBlueprint mapObjectsBlueprint = new MapObjectsBlueprint();
            mapObjectsBlueprint.objectsTypes = new ObjectType[rows, cols];
            
            int prefabID = entry.Key;
            List<MapObjectsData> dataList = entry.Value;

            
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; j++)
                {
                    mapObjectsBlueprint.objectsTypes[i, j] = ObjectType.None;
                }
            }
            
            mapObjectsBlueprint.wallIndex = rows - 1;
            
            mapObjectsBlueprint.objectsConstructors= new Action<Vector3>[rows, cols];
            
            SetCreateWallAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);
            for (int i = 0; i < dataList.Count; ++i)
            {
                var data = dataList[i];
                
                switch (data.Obj_Type)
                {
                    case MapObjectCSVType.Bomb:
                    {
                        SetCreateBombAction(mapObjectsBlueprint.objectsTypes,  mapObjectsBlueprint.objectsConstructors, 
                            data.Coor2, data.Coor1);
                    }
                        break;
                    case MapObjectCSVType.Hole:
                    {
                        SetCreateHoleAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors,
                        data.Coor2, data.Coor1);
                    }
                        break;
                    case MapObjectCSVType.Human:
                    {
                        SetCreateRandomHumanAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors,
                            data.Coor2, data.Coor1);
                    }
                        break;
                    case MapObjectCSVType.PenaltyCoin:
                    {
                        SetCreateRandomPenaltyCoinAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors,
                            data.Coor2, data.Coor1);

                    }
                        break;
                }
            }
            
            generateMapObjectInformationDictionary.Add(prefabID, mapObjectsBlueprint);
        }
        
        // SetCreateRandomRewardCoinAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);

        return generateMapObjectInformationDictionary;
    }

    private void SetCreateWallAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        int lastRowIndex = rows - 1;
        int middleColIndex = cols / 2;

        for (int i = 0; i < cols; ++i)
        {
            objectTypes[lastRowIndex, i] = ObjectType.Wall;
        }

        createMapObjectActionArray[lastRowIndex, middleColIndex] = CreateNormalWall;
    }

    private void CreateNormalWall(Vector3 position)
    {
        var wall = wallPool.Get();
        wall.transform.SetPositionAndRotation(position, Quaternion.identity);
        wall.TryGetComponent(out Wall wallComponent);
        wallComponent.Initialize(WallType.NormalWall);
        wallComponent.SetPool(wallPool);
    }

    private void SetCreateBombAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray, int row, int col)
    {
        // int rows = objectTypes.GetLength(0);
        // int cols = objectTypes.GetLength(1);
        //
        // for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; i += trapGenerateTileOffset)
        // {
        //     int randCol = Random.Range(0, cols);
        //
        //     if (objectTypes[i, randCol] != ObjectType.None)
        //         continue;
        //
        //     objectTypes[i, randCol] = ObjectType.TrapBomb;
        //     createMapObjectActionArray[i, randCol] = CreateBomb;
        // }

        if (objectTypes[row, col] != ObjectType.None)
        {
            Debug.Assert(false, $"Object {objectTypes[row, col].ToString()} is Already Exist in : [{row}, {col}]");
        
            return;
        }

        objectTypes[row, col] = ObjectType.TrapBomb;
        createMapObjectActionArray[row, col] = CreateBomb;
    }

    private void CreateBomb(Vector3 position)
    {
        var bomb = trapBombPool.Get();
        bomb.transform.SetPositionAndRotation(position, Quaternion.identity);
        bomb.TryGetComponent(out Trap trapComponent);
        trapComponent.Initialize(TrapType.Bomb);
        trapComponent.SetPool(trapBombPool);
    }

    private void SetCreateHoleAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray, int row, int col)
    {
        // int rows = objectTypes.GetLength(0);
        // int cols = objectTypes.GetLength(1);
        //
        // for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; i += trapGenerateTileOffset)
        // {
        //     if (!Utils.IsChanceHit(spawnHoleChance))
        //         continue;
        //
        //     List<int> colIndexes = new();
        //     for (int j = 0; j < cols; ++j)
        //     {
        //         if (objectTypes[i, j] != ObjectType.None)
        //             continue;
        //
        //         colIndexes.Add(j);
        //     }
        //
        //     if (colIndexes.Count == 0)
        //     {
        //         continue;
        //     }
        //
        //     int randCol = colIndexes[Random.Range(0, colIndexes.Count)];
        //     objectTypes[i, randCol] = ObjectType.TrapHole;
        //     createMapObjectActionArray[i, randCol] = CreateHole;
        // }
        
        if (objectTypes[row, col] != ObjectType.None)
        {
            Debug.Assert(false, $"Object {objectTypes[row, col].ToString()} is Already Exist in : [{row}, {col}]");
        
            return;
        }
        
        objectTypes[row, col] = ObjectType.TrapHole;
        createMapObjectActionArray[row, col] = CreateHole;
    }

    private void CreateHole(Vector3 position)
    {
        var hole = trapHolePool.Get();
        hole.transform.SetPositionAndRotation(position, Quaternion.identity);
        hole.TryGetComponent(out Trap trapComponent);
        trapComponent.Initialize(TrapType.Hole);
        trapComponent.SetPool(trapHolePool);
    }

    private void SetCreateRandomRewardCoinAction(ObjectType[,] objectTypes,
        Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                if (CanSpawnRewardCoin(objectTypes, i, j, out var isMiddleHoleExist))
                {
                    if (isMiddleHoleExist)
                    {
                        createMapObjectActionArray[i, j] = CreateRandomRewardCoinWithHill;
                    }
                    else
                    {
                        createMapObjectActionArray[i, j] = CreateRandomRewardCoin;
                    }
                }
            }
        }
    }

    private bool CanSpawnRewardCoin(ObjectType[,] objectTypes, int row, int col, out bool isMiddleHoleExist)
    {
        if (!Utils.IsChanceHit(spawnRewardCoinChance))
        {
            isMiddleHoleExist = false;

            return false;
        }

        bool canSpawn = true;

        int middleIndex = itemGenerateTileCount / 2;

        isMiddleHoleExist = false;

        for (int rowOffset = 0; rowOffset < itemGenerateTileCount; ++rowOffset)
        {
            var objectType = objectTypes[row + rowOffset, col];

            if (rowOffset == middleIndex)
            {
                if (objectType == ObjectType.TrapHole)
                {
                    isMiddleHoleExist = true;
                }
                else if (objectType == ObjectType.None)
                {
                    continue;
                }
                else
                {
                    canSpawn = false;

                    break;
                }
            }
            else
            {
                if (objectType != ObjectType.None)
                {
                    canSpawn = false;

                    break;
                }
            }
        }

        if (canSpawn)
        {
            for (int rowOffset = 0; rowOffset < itemGenerateTileCount; ++rowOffset)
            {
                if (isMiddleHoleExist && rowOffset == middleIndex)
                {
                    objectTypes[row + rowOffset, col] = ObjectType.ItemTrapMixed;
                }
                else
                {
                    objectTypes[row + rowOffset, col] = ObjectType.Item;
                }
            }
        }

        return canSpawn;
    }

    private void CreateRandomRewardCoin(Vector3 position)
    {
        Vector3 lastPosition = position + Vector3.forward * tileSize * (itemGenerateTileCount - 1);

        for (int i = 0; i < itemGroupCount; ++i)
        {
            var rewardCoin = itemRewardCoinPool.Get();
            rewardCoin.transform.SetPositionAndRotation(Vector3.Lerp(position, lastPosition, (float)i / (itemGroupCount - 1)), Quaternion.identity);
            rewardCoin.TryGetComponent(out ItemRewardCoin itemRewardCoinComponent);
            itemRewardCoinComponent.Initialize((RewardCoinItemType)Utils.GetEnumIndexByChance(rewardItemSpawnChances));
            itemRewardCoinComponent.SetPool(itemRewardCoinPool);
        }
    }

    private void CreateRandomRewardCoinWithHill(Vector3 position)
    {
        float maxHeight = 1.5f;
        int middleIndex = itemGroupCount / 2;

        Vector3 lastPosition = position + Vector3.forward * tileSize * (itemGenerateTileCount - 1);

        for (int i = 0; i < itemGroupCount; ++i)
        {
            Vector3 spawnPosition = Vector3.Lerp(position, lastPosition, (float)i / (itemGroupCount - 1));
            float distanceFromMiddle = Mathf.Abs(i - middleIndex);
            float heightOffset = maxHeight - (distanceFromMiddle / middleIndex * maxHeight);
            spawnPosition.y += heightOffset;
            
            var rewardCoin = itemRewardCoinPool.Get();
            rewardCoin.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            rewardCoin.TryGetComponent(out ItemRewardCoin itemRewardCoinComponent);
            itemRewardCoinComponent.Initialize((RewardCoinItemType)Utils.GetEnumIndexByChance(rewardItemSpawnChances));
            itemRewardCoinComponent.SetPool(itemRewardCoinPool);
        }
    }

    private void SetCreateRandomHumanAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray, int row, int col)
    {
        // int rows = objectTypes.GetLength(0);
        // int cols = objectTypes.GetLength(1);
        //
        // for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; ++i)
        // {
        //     if (!Utils.IsChanceHit(spawnHumanChance))
        //         continue;
        //
        //     List<int> colIndexes = new();
        //     for (int j = 0; j < cols; ++j)
        //     {
        //         if (objectTypes[i, j] != ObjectType.None)
        //             continue;
        //
        //         colIndexes.Add(j);
        //     }
        //
        //     if (colIndexes.Count == 0)
        //     {
        //         continue;
        //     }
        //
        //     int randCol = colIndexes[Random.Range(0, colIndexes.Count)];
        //     objectTypes[i, randCol] = ObjectType.Item;
        //     createMapObjectActionArray[i, randCol] = CreateRandomHuman;
        // }
        
        
        if (objectTypes[row, col] != ObjectType.None)
        {
            Debug.Assert(false, $"Object {objectTypes[row, col].ToString()} is Already Exist in : [{row}, {col}]");
        
            return;
        }
        
        objectTypes[row, col] = ObjectType.Item;
        createMapObjectActionArray[row, col] = CreateRandomHuman;
    }

    private void CreateRandomHuman(Vector3 position)
    {
        var human = itemHumanPool.Get();
        human.transform.SetPositionAndRotation(position, Quaternion.identity);
        human.TryGetComponent(out ItemHuman itemHumanComponent);
        itemHumanComponent.Initialize((HumanItemType)Utils.GetEnumIndexByChance(humanSpawnChances));
        itemHumanComponent.SetPool(itemHumanPool);
    }

    private void SetCreateRandomPenaltyCoinAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray, int row, int col)
    {
        // int rows = objectTypes.GetLength(0);
        // int cols = objectTypes.GetLength(1);
        //
        // for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; ++i)
        // {
        //     if (!Utils.IsChanceHit(spawnPenaltyCoinChance))
        //         continue;
        //
        //     List<int> colIndexes = new();
        //     for (int j = 0; j < cols; ++j)
        //     {
        //         if (objectTypes[i, j] != ObjectType.None)
        //             continue;
        //
        //         colIndexes.Add(j);
        //     }
        //
        //     if (colIndexes.Count == 0)
        //     {
        //         continue;
        //     }
        //
        //     int randCol = colIndexes[Random.Range(0, colIndexes.Count)];
        //     objectTypes[i, randCol] = ObjectType.Item;
        //     createMapObjectActionArray[i, randCol] = CreateRandomPenaltyCoin;
        // }
        
        if (objectTypes[row, col] != ObjectType.None)
        {
            Debug.Assert(false, $"Object {objectTypes[row, col].ToString()} is Already Exist in : [{row}, {col}]");
        
            return;
        }
        
        objectTypes[row, col] = ObjectType.Item;
        createMapObjectActionArray[row, col] = CreateRandomPenaltyCoin;
    }

    private void CreateRandomPenaltyCoin(Vector3 position)
    {
        var penaltyCoin = itemPenaltyCoinPool.Get();
        penaltyCoin.transform.SetPositionAndRotation(position, Quaternion.identity);
        penaltyCoin.TryGetComponent(out ItemPenaltyCoin itemPenaltyCoinComponent);
        itemPenaltyCoinComponent.Initialize((PenaltyCoinItemType)Utils.GetEnumIndexByChance(penaltyCoinSpawnChances));
        itemPenaltyCoinComponent.SetPool(itemPenaltyCoinPool);
    }
}