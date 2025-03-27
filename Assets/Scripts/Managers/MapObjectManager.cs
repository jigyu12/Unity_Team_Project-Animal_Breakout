using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapObjectInformationManager : MonoBehaviour
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

    private const float spawnHoleChance = 0.3f;

    private List<float> rewardItemSpawnChances = new();
    [SerializeField] [ReadOnly] private float bronzeCoinSpawnChance = 0.5f;
    [SerializeField] [ReadOnly] private float sliverCoinSpawnChance = 0.2f;
    [SerializeField] [ReadOnly] private float goldCoinSpawnChance = 0.15f;
    [SerializeField] [ReadOnly] private float platinumCoinSpawnChance = 0.1f;
    [SerializeField] [ReadOnly] private float diamondCoinSpawnChance = 0.05f;

    private const float spawnHumanChance = 0.4f;
    private List<float> humanSpawnChances = new();
    [SerializeField] [ReadOnly] private float juniorResearcherSpawnChance = 0.6f;
    [SerializeField] [ReadOnly] private float researcherSpawnChance = 0.3f;
    [SerializeField] [ReadOnly] private float seniorResearcherSpawnChance = 0.1f;
    
    private const float spawnPenaltyCoinChance = 0.3f;
    private List<float> penaltyCoinSpawnChances = new();
    [SerializeField] [ReadOnly] private float ghostCoinSpawnChance = 0.5f;
    [SerializeField] [ReadOnly] private float poisonCoinSpawnChance = 0.2f;
    [SerializeField] [ReadOnly] private float skullCoinSpawnChance = 0.15f;
    [SerializeField] [ReadOnly] private float fireCoinSpawnChance = 0.1f;
    [SerializeField] [ReadOnly] private float blackHoleCoinSpawnChance = 0.05f;

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

    public Action<Vector3>[,] GenerateMapObjectInformation(int rows, int cols)
    {
        ObjectType[,] objectTypes = new ObjectType[rows, cols];
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; j++)
            {
                objectTypes[i, j] = ObjectType.None;
            }
        }

        Action<Vector3>[,] createMapObjectActionArray = new Action<Vector3>[rows, cols];

        SetCreateWallAction(objectTypes, createMapObjectActionArray);
        SetCreateBombAction(objectTypes, createMapObjectActionArray);
        SetCreateHoleAction(objectTypes, createMapObjectActionArray);
        SetCreateRandomRewardCoinAction(objectTypes, createMapObjectActionArray);
        SetCreateRandomHumanAction(objectTypes, createMapObjectActionArray);
        SetCreateRandomPenaltyCoinAction(objectTypes, createMapObjectActionArray);

        return createMapObjectActionArray;
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
                if (CanSpawnRewardCoin(objectTypes, i, j))
                {
                    createMapObjectActionArray[i, j] = CreateRandomRewardCoin;
                }
            }
        }
    }

    private bool CanSpawnRewardCoin(ObjectType[,] objectTypes, int row, int col)
    {
        bool canSpawn = true;

        int middleIndex = itemGenerateTileCount / 2;

        bool isMiddleHoleExist = false;

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

        for (int rowOffset = 0; rowOffset < itemGenerateTileCount; ++rowOffset)
        {
            var objectType = objectTypes[row + rowOffset, col];

            if (isMiddleHoleExist && rowOffset == middleIndex)
            {
                objectType = ObjectType.ItemTrapMixed;
            }
            else
            {
                objectType = ObjectType.Item;
            }
        }

        return canSpawn;
    }

    private void CreateRandomRewardCoin(Vector3 position)
    {
        Vector3 lastPosition = position + Vector3.forward * tileSize * (itemGenerateTileCount - 1);

        for (int i = 0; i < itemGroupCount; ++i)
        {
            var rewardCoin = Instantiate(trapPrefab,
                Vector3.Lerp(position, lastPosition, (float)i / (itemGroupCount - 1)), Quaternion.identity);
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
        var paneltyCoin = Instantiate(itemPenaltyCoin, position, Quaternion.identity);
        paneltyCoin.TryGetComponent(out ItemPenaltyCoin itemPenaltyCoinComponent);
        itemPenaltyCoinComponent.Init((PenaltyCoinItemType)Utils.GetEnumIndexByChance(penaltyCoinSpawnChances));
    }
}