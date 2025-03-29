using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private GameObject itemRewardCoin;
    [SerializeField] private GameObject itemHuman;
    [SerializeField] private GameObject itemPenaltyCoin;

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
    }

    public struct MapObjectsBlueprint
    {
        public ObjectType[,] objectsTypes;
        public Action<Vector3>[,] objectsConstructors;
    }

    public MapObjectsBlueprint GenerateMapObjectInformation(int rows, int cols)
    {
        MapObjectsBlueprint mapObjectsBlueprint = new MapObjectsBlueprint();
        mapObjectsBlueprint.objectsTypes = new ObjectType[rows, cols];

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; j++)
            {
                mapObjectsBlueprint.objectsTypes[i, j] = ObjectType.None;
            }
        }

        mapObjectsBlueprint.objectsConstructors= new Action<Vector3>[rows, cols];

        SetCreateWallAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);
        SetCreateBombAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);
        SetCreateHoleAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);
        SetCreateRandomRewardCoinAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);
        SetCreateRandomHumanAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);
        SetCreateRandomPenaltyCoinAction(mapObjectsBlueprint.objectsTypes, mapObjectsBlueprint.objectsConstructors);

        return mapObjectsBlueprint;
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

        createMapObjectActionArray[lastRowIndex, middleColIndex] = CreateWall;
    }

    private void CreateWall(Vector3 position)
    {
        Instantiate(wallPrefab, position, Quaternion.identity);
    }

    private void SetCreateBombAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; i += trapGenerateTileOffset)
        {
            int randCol = Random.Range(0, cols);

            if (objectTypes[i, randCol] != ObjectType.None)
                continue;

            objectTypes[i, randCol] = ObjectType.TrapBomb;
            createMapObjectActionArray[i, randCol] = CreateBomb;
        }
    }

    private void CreateBomb(Vector3 position)
    {
        var bomb = Instantiate(trapPrefab, position, Quaternion.identity);
        bomb.TryGetComponent(out Trap trapComponent);
        trapComponent.Init(TrapType.Bomb);
    }

    private void SetCreateHoleAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; i += trapGenerateTileOffset)
        {
            if (!Utils.IsChanceHit(spawnHoleChance))
                continue;

            List<int> colIndexes = new();
            for (int j = 0; j < cols; ++j)
            {
                if (objectTypes[i, j] != ObjectType.None)
                    continue;

                colIndexes.Add(j);
            }

            if (colIndexes.Count == 0)
            {
                continue;
            }

            int randCol = colIndexes[Random.Range(0, colIndexes.Count)];
            objectTypes[i, randCol] = ObjectType.TrapHole;
            createMapObjectActionArray[i, randCol] = CreateHole;
        }
    }

    private void CreateHole(Vector3 position)
    {
        var hole = Instantiate(trapPrefab, position, Quaternion.identity);
        hole.TryGetComponent(out Trap trapComponent);
        trapComponent.Init(TrapType.Hole);
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
            var rewardCoin = Instantiate(itemRewardCoin,
                Vector3.Lerp(position, lastPosition, (float)i / (itemGroupCount - 1)), Quaternion.identity);
            rewardCoin.TryGetComponent(out ItemRewardCoin itemRewardCoinComponent);
            itemRewardCoinComponent.Init((RewardCoinItemType)Utils.GetEnumIndexByChance(rewardItemSpawnChances));
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

            var rewardCoin = Instantiate(itemRewardCoin, spawnPosition, Quaternion.identity);
            rewardCoin.TryGetComponent(out ItemRewardCoin itemRewardCoinComponent);
            itemRewardCoinComponent.Init((RewardCoinItemType)Utils.GetEnumIndexByChance(rewardItemSpawnChances));
        }
    }

    private void SetCreateRandomHumanAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; ++i)
        {
            if (!Utils.IsChanceHit(spawnHumanChance))
                continue;

            List<int> colIndexes = new();
            for (int j = 0; j < cols; ++j)
            {
                if (objectTypes[i, j] != ObjectType.None)
                    continue;

                colIndexes.Add(j);
            }

            if (colIndexes.Count == 0)
            {
                continue;
            }

            int randCol = colIndexes[Random.Range(0, colIndexes.Count)];
            objectTypes[i, randCol] = ObjectType.Item;
            createMapObjectActionArray[i, randCol] = CreateRandomHuman;
        }
    }

    private void CreateRandomHuman(Vector3 position)
    {
        var human = Instantiate(itemHuman, position, Quaternion.identity);
        human.TryGetComponent(out ItemHuman itemHumanComponent);
        itemHumanComponent.Init((HumanItemType)Utils.GetEnumIndexByChance(humanSpawnChances));
    }

    private void SetCreateRandomPenaltyCoinAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        for (int i = 0 + nonObjectTileCount; i < rows - nonObjectTileCount; ++i)
        {
            if (!Utils.IsChanceHit(spawnPenaltyCoinChance))
                continue;

            List<int> colIndexes = new();
            for (int j = 0; j < cols; ++j)
            {
                if (objectTypes[i, j] != ObjectType.None)
                    continue;

                colIndexes.Add(j);
            }

            if (colIndexes.Count == 0)
            {
                continue;
            }

            int randCol = colIndexes[Random.Range(0, colIndexes.Count)];
            objectTypes[i, randCol] = ObjectType.Item;
            createMapObjectActionArray[i, randCol] = CreateRandomPenaltyCoin;
        }
    }

    private void CreateRandomPenaltyCoin(Vector3 position)
    {
        var penaltyCoin = Instantiate(itemPenaltyCoin, position, Quaternion.identity);
        penaltyCoin.TryGetComponent(out ItemPenaltyCoin itemPenaltyCoinComponent);
        itemPenaltyCoinComponent.Init((PenaltyCoinItemType)Utils.GetEnumIndexByChance(penaltyCoinSpawnChances));
    }
}