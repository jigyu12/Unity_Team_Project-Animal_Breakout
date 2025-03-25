using System.Collections.Generic;
using UnityEngine;

public class PlaneToGridMap : MonoBehaviour
{
    public GameObject planePrefab;

    public int rows;
    public int cols;

    public GameObject[,] LeftSideTile;
    public GameObject[,] RightSideTile;
    public GameObject[,] MiddleTile;
    
    public Dictionary<int, TrapType> Traps = new();

    private GameObject[,] tiles;
    private float tileSizeX, tileSizeZ;
    private Vector3 planeOrigin;
    
    public GameObject sidePrefab;

    void Awake()
    {
        CalculateTileSize();
        GenerateGrid();
    }

    void CalculateTileSize()
    {
        var planeRenderer = planePrefab.GetComponent<MeshRenderer>();

        tileSizeX = planeRenderer.bounds.size.x / rows;
        tileSizeZ = planeRenderer.bounds.size.z / cols;

        Vector3 planeCenter = planeRenderer.bounds.center;
        planeOrigin = new Vector3(
            planeCenter.x - planeRenderer.bounds.size.x / 2f,
            planeCenter.y,
            planeCenter.z - planeRenderer.bounds.size.z / 2f);
    }

    void GenerateGrid()
    {
        tiles = new GameObject[rows, cols];
    
        LeftSideTile = new GameObject[1, cols];
        RightSideTile = new GameObject[1, cols];
        MiddleTile = new GameObject[rows - 2, cols];

        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < cols; z++)
            {
                Vector3 position = GridToWorldPosition(x, z);

                GameObject tile = new GameObject($"Tile_{x}_{z}");
                tile.transform.position = position;
                tile.transform.parent = transform;

                var collider = tile.AddComponent<BoxCollider>();
                collider.size = new Vector3(tileSizeX, 0.01f, tileSizeZ);
                collider.center = Vector3.zero;

                tiles[x, z] = tile;

                if (x == 0)
                {
                    LeftSideTile[0, z] = tile;
                    //Instantiate(sidePrefab, tile.transform.position, Quaternion.identity);
                }
                else if (x == rows - 1)
                {
                    RightSideTile[0, z] = tile;
                    //Instantiate(sidePrefab, tile.transform.position, Quaternion.identity);
                }
                else
                {
                    MiddleTile[x - 1, z] = tile;
                }
            }
        }
    }

    public Vector3 GridToWorldPosition(int x, int z)
    {
        float worldX = planeOrigin.x + (x + 0.5f) * tileSizeX;
        float worldZ = planeOrigin.z + (z + 0.5f) * tileSizeZ;
        float worldY = planeOrigin.y;

        return new Vector3(worldX, worldY, worldZ);
    }

    public Vector2Int WorldPositionToGrid(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x - planeOrigin.x) / tileSizeX);
        int z = Mathf.FloorToInt((position.z - planeOrigin.z) / tileSizeZ);

        x = Mathf.Clamp(x, 0, rows - 1);
        z = Mathf.Clamp(z, 0, cols - 1);

        return new Vector2Int(x, z);
    }

    public GameObject GetTile(int row, int col)
    {
        if (row < 0 || row >= rows || col < 0 || col >= cols)
        {
            Debug.LogError($"Index out of bounds: ({row}, {col})");
            return null;
        }
        return tiles[row, col];
    }
}