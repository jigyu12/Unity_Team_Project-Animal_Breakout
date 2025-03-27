using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecorationTile : MonoBehaviour
{
    public Mesh[] meshes;
    [SerializeField]
    private MeshFilter[] tileMeshFilters;

    public Vector3 TileSize
    {
        get => tileSize;
    }
    private Vector3 tileSize = new(1f, 0f, 1f);

    [ContextMenu("Update TilePosition")]
    public void UpdateTilePosition()
    {
        for (int i = 0; i < tileMeshFilters.Length; i++)
        {
            tileMeshFilters[i].transform.position = new Vector3(0, 0, (i) * tileSize.z);
        }
    }

    [ContextMenu("Update TileMesh")]
    public void UpdateTileMesh()
    {
        for (int i = 0; i < tileMeshFilters.Length; i++)
        {
            tileMeshFilters[i].mesh = meshes[i%meshes.Count()];
        }
    }


    [ContextMenu("Update TileMesh Random")]
    public void UpdateTileMeshRandom()
    {
        for (int i = 0; i < tileMeshFilters.Length; i++)
        {
            int index = Random.Range(0, meshes.Count());
            tileMeshFilters[i].mesh = meshes[index];
        }
    }

    public Vector3 GetTileSize()
    {
        return tileSize;
    }

}
