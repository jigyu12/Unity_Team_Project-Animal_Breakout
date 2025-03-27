using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapObjectInformationManager : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject trapPrefab;

    private const int NonTrapTileCount = 6;

    private const float spawnHoleChance = 0.3f;

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

<<<<<<< Updated upstream
        CreateBombs(objectTypes, createMapObjectActionArray);
        CreateHoles(objectTypes, createMapObjectActionArray);
        
=======
        SetCreateWallAction(objectTypes, createMapObjectActionArray);
        SetCreateBombAction(objectTypes, createMapObjectActionArray);
        SetCreateHoleAction(objectTypes, createMapObjectActionArray);

>>>>>>> Stashed changes
        // Todo..

        return createMapObjectActionArray;
    }

<<<<<<< Updated upstream
    private void CreateBombs(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
=======
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
>>>>>>> Stashed changes
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        for (int i = 0 + NonTrapTileCount; i < rows - NonTrapTileCount; i += 3)
        {
            int randCol = Random.Range(0, cols);

            if (objectTypes[i, randCol] != ObjectType.None)
                continue;

            objectTypes[i, randCol] = ObjectType.Trap;
            createMapObjectActionArray[i, randCol] = CreateBomb;
        }
    }

    private void CreateBomb(Vector3 position)
    {
        var bomb = Instantiate(trapPrefab, position, Quaternion.identity);
        bomb.TryGetComponent(out Trap bombTrapComponent);
        bombTrapComponent.Init(TrapType.Bomb);
    }

    private void CreateHoles(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);

        for (int i = 0 + NonTrapTileCount; i < rows - NonTrapTileCount; i += 3)
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
            objectTypes[i, randCol] = ObjectType.Trap;
            createMapObjectActionArray[i, randCol] = CreateHole;
        }
    }

    private void CreateHole(Vector3 position)
    {
        var hole = Instantiate(trapPrefab, position, Quaternion.identity);
        hole.TryGetComponent(out Trap holeTrapComponent);
        holeTrapComponent.Init(TrapType.Hole);
    }
}