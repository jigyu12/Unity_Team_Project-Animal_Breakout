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
                objectTypes[rows, cols] = ObjectType.None;
            }
        }
        
        Action<Vector3>[,] createMapObjectActionArray = new Action<Vector3>[rows, cols];

        SetCreateWallAction(objectTypes, createMapObjectActionArray);
        SetCreateBombAction(objectTypes, createMapObjectActionArray);
        SetCreateHoleAction(objectTypes, createMapObjectActionArray);
        
        // Todo..
        
        return createMapObjectActionArray;
    }

    private void SetCreateWallAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);
        
        int lastRowIndex = rows - 1;
        int middleColIndex = cols / 2;

        for(int i = 0; i < cols; ++i)
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
        
        for (int i = 0 + NonTrapTileCount; i < rows - NonTrapTileCount; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                int randCol = Random.Range(0, cols);
                
                if(objectTypes[i, randCol] != ObjectType.None)
                    continue;
                
                objectTypes[i, randCol] = ObjectType.Trap;
                createMapObjectActionArray[i, randCol] = CreateBomb;
            }
        }
    }
    
    private void CreateBomb(Vector3 position)
    {
        var bomb = Instantiate(trapPrefab, position, Quaternion.identity);
        bomb.TryGetComponent(out Trap bombTrapComponent);
        bombTrapComponent.Init(TrapType.Bomb);
    }

    private void SetCreateHoleAction(ObjectType[,] objectTypes, Action<Vector3>[,] createMapObjectActionArray)
    {
        int rows = objectTypes.GetLength(0);
        int cols = objectTypes.GetLength(1);
        
        for (int i = 0 + NonTrapTileCount; i < rows - NonTrapTileCount; ++i)
        {
            if(!Utils.IsChanceHit(spawnHoleChance))
                continue;
            
            List<int> colIndexes = new();
            for (int j = 0; j < cols; ++j)
            {
                if(objectTypes[i, j] != ObjectType.None)
                    continue;
                
                colIndexes.Add(j);
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