using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile3
{
    private Tile[] tiles = new Tile[3];
    private bool isWallOn;
}

public struct Tile
{
    private Vector2Int index;
    private bool isTrapOn;
}
