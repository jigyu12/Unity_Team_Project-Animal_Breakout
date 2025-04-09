using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MapObjectManager : InGameManager
{
    //[SerializeField] private GameObject wallPrefab;
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
    private const int itemCount = 5;
    private const int itemGenerateTileCount = 3;

    private const int trapGenerateTileOffset = 3;

    private const float tileSize = 1f;
    private const float maxHillHeight = 1.5f;

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

    private Dictionary<int, MapObjectsBlueprint> generateMapObjectInformationDictionary = new();
    private Dictionary<int, List<RewardItemBlueprint>> generateRewardItemInformationDictionary = new();

    [SerializeField]
    private GameObject roadTransformRoot;

    public override void Initialize()
    {
        base.Initialize();

        InitializeMapObjectObjectPools();
    }

    private void Start()
    {
        GenerateMapObjectInformation(20, 3);
        GenerateRewardItemInformation();
    }

    private void InitializeMapObjectObjectPools()
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

        // wallPool = GameManager.ObjectPoolManager.CreateObjectPool(wallPrefab,
        //     () => Instantiate(wallPrefab, roadTransformRoot.transform),
        //     obj => { obj.SetActive(true); },
        //     obj => { obj.SetActive(false); });

        trapBombPool = GameManager.ObjectPoolManager.CreateObjectPool(trapBombPrefab,
            () => Instantiate(trapBombPrefab, roadTransformRoot.transform),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        trapHolePool = GameManager.ObjectPoolManager.CreateObjectPool(trapHolePrefab,
            () => Instantiate(trapHolePrefab, roadTransformRoot.transform),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        itemRewardCoinPool = GameManager.ObjectPoolManager.CreateObjectPool(itemRewardCoinPrefab,
            () => Instantiate(itemRewardCoinPrefab, roadTransformRoot.transform),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        itemHumanPool = GameManager.ObjectPoolManager.CreateObjectPool(itemHumanPrefab,
            () => Instantiate(itemHumanPrefab, roadTransformRoot.transform),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        itemPenaltyCoinPool = GameManager.ObjectPoolManager.CreateObjectPool(itemPenaltyCoinPrefab,
            () => Instantiate(itemPenaltyCoinPrefab, roadTransformRoot.transform),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
    }

    public struct MapObjectsBlueprint
    {
        public ObjectType[,] objectsTypes;
        public Func<Vector3, CollidableMapObject>[,] objectsConstructors;
    }

    private void GenerateMapObjectInformation(int rows, int cols)
    {
        foreach (var entry in DataTableManager.mapObjectsDataTable.GetTableEntries())
        {
            MapObjectsBlueprint mapObjectsBlueprint = new MapObjectsBlueprint();
            mapObjectsBlueprint.objectsTypes = new ObjectType[rows, cols];

            int prefabID = entry.Key;
            List<MapObjectData> dataList = entry.Value;


            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; j++)
                {
                    mapObjectsBlueprint.objectsTypes[i, j] = ObjectType.None;
                }
            }

            //mapObjectsBlueprint.wallIndex = rows - 1;
            mapObjectsBlueprint.objectsConstructors = new Func<Vector3, CollidableMapObject>[rows, cols];

            //SetCreateWallAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);
            for (int i = 0; i < dataList.Count; ++i)
            {
                var data = dataList[i];

                switch (data.Obj_Type)
                {
                    case MapObjectCSVType.Bomb:
                        {
                            SetCreateBombAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors,
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
    }

    public struct RewardItemBlueprint
    {
        public (int, int)[] startEndCoordinate; // Size : 2
        public int itemGroupCount; // Reward Item Count In Same Group

        public Func<Vector3, Vector3, int, CollidableMapObject[]> rewardItemConstructors;
    }

    private void GenerateRewardItemInformation()
    {
        foreach (var entry in DataTableManager.rewardItemsDataTable.GetTableEntries())
        {
            int prefabID = entry.Key;
            var dataList = entry.Value;

            for (int i = 0; i < dataList.Count; ++i)
            {
                RewardItemBlueprint rewardItemBlueprint = new RewardItemBlueprint();
                rewardItemBlueprint.startEndCoordinate = new (int, int)[2];

                var data = dataList[i];
                rewardItemBlueprint.startEndCoordinate[0] = (data.Start_Coor2, data.Start_Coor1);
                rewardItemBlueprint.startEndCoordinate[1] = (data.End_Coor2, data.End_Coor1);
                rewardItemBlueprint.itemGroupCount = data.Count;
                if (data.Pattern == RewardCoinPatternCSVType.Straight)
                {
                    rewardItemBlueprint.rewardItemConstructors = CreateRandomRewardCoin;
                }
                else if (data.Pattern == RewardCoinPatternCSVType.Hill)
                {
                    rewardItemBlueprint.rewardItemConstructors = CreateRandomRewardCoinWithHill;
                }

                if (i == 0)
                {
                    List<RewardItemBlueprint> rewardItemBlueprints = new();
                    rewardItemBlueprints.Add(rewardItemBlueprint);
                    generateRewardItemInformationDictionary.Add(prefabID, rewardItemBlueprints);
                }
                else
                {
                    generateRewardItemInformationDictionary[prefabID].Add(rewardItemBlueprint);
                }
            }
        }
    }

    public MapObjectsBlueprint GetMapObjectsBlueprint(int id)
    {
        if (generateMapObjectInformationDictionary.TryGetValue(id, out MapObjectsBlueprint blueprint))
        {
            return blueprint;
        }
        else
        {
            throw new KeyNotFoundException($" 맵 오브젝트 키 '{id}' 를 찾을 수 없습니다.");
        }
    }

    public List<RewardItemBlueprint> GetRewardItemBlueprint(int id)
    {
        if (generateRewardItemInformationDictionary.TryGetValue(id, out List<RewardItemBlueprint> blueprints))
        {
            return blueprints;
        }
        else
        {
            throw new KeyNotFoundException($" 리워드 아이템 '{id}' 를 찾을 수 없습니다.");
        }
    }

    // private void SetCreateWallAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    // {
    //     int rows = objectTypes.GetLength(0);
    //     int cols = objectTypes.GetLength(1);
    //
    //     int lastRowIndex = rows - 1;
    //     int middleColIndex = cols / 2;
    //
    //     for (int i = 0; i < cols; ++i)
    //     {
    //         objectTypes[lastRowIndex, i] = ObjectType.Wall;
    //     }
    //
    //     createMapObjectActionArray[lastRowIndex, middleColIndex] = CreateNormalWall;
    // }
    //
    // private void CreateNormalWall(Vector3 position)
    // {
    //     var wall = wallPool.Get();
    //     wall.transform.SetPositionAndRotation(position, Quaternion.identity);
    //     wall.TryGetComponent(out Wall wallComponent);
    //     wallComponent.Initialize(WallType.NormalWall);
    //     wallComponent.SetPool(wallPool);
    // }

    private void SetCreateBombAction(ObjectType[,] objectTypes, Func<Vector3, CollidableMapObject>[,] createMapObjectActionArray, int row, int col)
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

    private CollidableMapObject CreateBomb(Vector3 position)
    {
        var bomb = trapBombPool.Get();
        bomb.SetActive(true);
        bomb.transform.SetPositionAndRotation(position, Quaternion.identity);
        bomb.TryGetComponent(out Trap trapComponent);
        trapComponent.Initialize(TrapType.Bomb);
        trapComponent.SetPool(trapBombPool);
        return trapComponent;
    }

    private void SetCreateHoleAction(ObjectType[,] objectTypes, Func<Vector3, CollidableMapObject>[,] createMapObjectActionArray, int row, int col)
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

    private CollidableMapObject CreateHole(Vector3 position)
    {
        var hole = trapHolePool.Get();
        hole.SetActive(true);
        hole.transform.SetPositionAndRotation(position, Quaternion.identity);
        hole.TryGetComponent(out Trap trapComponent);
        trapComponent.Initialize(TrapType.Hole);
        trapComponent.SetPool(trapHolePool);
        return trapComponent;
    }

    private void SetCreateRandomRewardCoinAction(ObjectType[,] objectTypes,
        Action<Vector3>[,] createMapObjectActionArray)
    {
        // int rows = objectTypes.GetLength(0);
        // int cols = objectTypes.GetLength(1);
        //
        // for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; ++i)
        // {
        //     for (int j = 0; j < cols; ++j)
        //     {
        //         if (CanSpawnRewardCoin(objectTypes, i, j, out var isMiddleHoleExist))
        //         {
        //             if (isMiddleHoleExist)
        //             {
        //                 createMapObjectActionArray[i, j] = CreateRandomRewardCoinWithHill;
        //             }
        //             else
        //             {
        //                 createMapObjectActionArray[i, j] = CreateRandomRewardCoin;
        //             }
        //         }
        //     }
        // }
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

    private CollidableMapObject[] CreateRandomRewardCoin(Vector3 startPosition, Vector3 endPosition, int itemCount)
    {
        var array = new CollidableMapObject[itemCount];
        for (int i = 0; i < itemCount; ++i)
        {
            var rewardCoin = itemRewardCoinPool.Get();
            rewardCoin.SetActive(true);
            rewardCoin.transform.SetPositionAndRotation(Vector3.Lerp(startPosition, endPosition, (float)i / (itemCount - 1)), Quaternion.identity);
            rewardCoin.TryGetComponent(out ItemRewardCoin itemRewardCoinComponent);
            itemRewardCoinComponent.Initialize((RewardCoinItemType)Utils.GetEnumIndexByChance(rewardItemSpawnChances));
            itemRewardCoinComponent.SetPool(itemRewardCoinPool);
            array[i] = itemRewardCoinComponent;
        }
        return array;
    }

    private CollidableMapObject[] CreateRandomRewardCoinWithHill(Vector3 startPosition, Vector3 endPosition, int itemCount)
    {
        float maxHeight = maxHillHeight;
        int middleIndex = itemCount / 2;

        var array = new CollidableMapObject[itemCount];
        for (int i = 0; i < itemCount; ++i)
        {
            Vector3 spawnPosition = Vector3.Lerp(startPosition, endPosition, (float)i / (itemCount - 1));
            float distanceFromMiddle = Mathf.Abs(i - middleIndex);
            float heightOffset = maxHeight - (distanceFromMiddle / middleIndex * maxHeight);
            spawnPosition.y += heightOffset;

            var rewardCoin = itemRewardCoinPool.Get();
            rewardCoin.SetActive(true);
            rewardCoin.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            rewardCoin.TryGetComponent(out ItemRewardCoin itemRewardCoinComponent);
            itemRewardCoinComponent.Initialize((RewardCoinItemType)Utils.GetEnumIndexByChance(rewardItemSpawnChances));
            itemRewardCoinComponent.SetPool(itemRewardCoinPool);
            array[i] = itemRewardCoinComponent;
        }
        return array;
    }

    private void SetCreateRandomHumanAction(ObjectType[,] objectTypes, Func<Vector3, CollidableMapObject>[,] createMapObjectActionArray, int row, int col)
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

    private CollidableMapObject CreateRandomHuman(Vector3 position)
    {
        var human = itemHumanPool.Get();
        human.SetActive(true);
        human.transform.SetPositionAndRotation(position, Quaternion.identity);
        human.TryGetComponent(out ItemHuman itemHumanComponent);
        itemHumanComponent.Initialize((HumanItemType)Utils.GetEnumIndexByChance(humanSpawnChances));
        itemHumanComponent.SetPool(itemHumanPool);
        return itemHumanComponent;
    }

    private void SetCreateRandomPenaltyCoinAction(ObjectType[,] objectTypes, Func<Vector3, CollidableMapObject>[,] createMapObjectActionArray, int row, int col)
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

    private CollidableMapObject CreateRandomPenaltyCoin(Vector3 position)
    {
        var penaltyCoin = itemPenaltyCoinPool.Get();
        penaltyCoin.SetActive(true);
        penaltyCoin.transform.SetPositionAndRotation(position, Quaternion.identity);
        penaltyCoin.TryGetComponent(out ItemPenaltyCoin itemPenaltyCoinComponent);
        itemPenaltyCoinComponent.Initialize((PenaltyCoinItemType)Utils.GetEnumIndexByChance(penaltyCoinSpawnChances));
        itemPenaltyCoinComponent.SetPool(itemPenaltyCoinPool);
        return itemPenaltyCoinComponent;
    }
}