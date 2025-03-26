using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapUnit : MonoBehaviour
{
    public Mesh[] meshes;
    [SerializeField]
    private GameObject[] tileMeshFilters;

    [SerializeField]
    private Vector3 unitSize = new(1f, 0f, 1f);

    public WayType directionType;


    [ReadOnly]
    public TileMapSpawner mapSpawner;
    [SerializeField]
    private RoadEnterTrigger nextUnitSpawnTrigger;

    public Vector3 NextPosition
    {
        get => GetTilePosition(tileMeshFilters.Length / 3, 1);
    }
    public Vector3 NextLeftPosition
    {
        get => GetTilePosition(tileMeshFilters.Length / 3 - 2, 3);
    }
    public Vector3 NextRightPosition
    {
        get => GetTilePosition(tileMeshFilters.Length / 3 - 2, -1);
    }

    [ContextMenu("Update TilePosition")]
    public void UpdateTilePosition()
    {
        for (int i = 0; i < tileMeshFilters.Length; i++)
        {
            tileMeshFilters[i].transform.position = new Vector3(-(i % 3) * unitSize.x + unitSize.x, 0, (i / 3) * unitSize.z);
        }
    }

    [ContextMenu("Update TileMesh")]
    public void UpdateTileMesh()
    {
        for (int i = 0; i < tileMeshFilters.Length; i++)
        {
            tileMeshFilters[i].GetComponent<MeshFilter>().mesh = meshes[i % 2];
        }
    }


    public void Reset()
    {
        nextUnitSpawnTrigger.gameObject.SetActive(false);
    }

    public Vector3 GetTilePosition(int rowIndex, int colIndex)
    {
        Vector3 forward = Vector3.zero;
        forward.z = unitSize.z;
        Vector3 right = Vector3.zero;
        right.x = -unitSize.x;

        Vector3 position = tileMeshFilters[0].transform.position + (rowIndex * forward) + (colIndex * right);
        return position;
    }

    public void SetNextUnitSpawnTriggerOn()
    {
        nextUnitSpawnTrigger.gameObject.SetActive(true);
    }
}
