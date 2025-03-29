using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadSegment : MonoBehaviour
{
    public WayType directionType;

    [ReadOnly]
    public int tileVerticalCount;
    [SerializeField]
    private CheckerTile tiles;

    public RoadDecoration decoration;

    private void Awake()
    {
        tileVerticalCount = tiles.GetTileVerticalCount();
    }
    
    [SerializeField]
    private RoadEnterTrigger enterTrigger;

    public Vector3 NextPosition
    {
        get => GetTilePosition(tileVerticalCount, 1);
    }
    public Vector3 NextLeftPosition
    {
        get => GetTilePosition(tileVerticalCount - 2, 3);
    }
    public Vector3 NextRightPosition
    {
        get => GetTilePosition(tileVerticalCount - 2, -1);
    }


    public void Reset()
    {
        enterTrigger.gameObject.SetActive(false);
    }

    public Vector3 GetTilePosition(int rowIndex, int colIndex)
    {
        Vector3 forward = Vector3.zero;
        forward.z = tiles.TileSize.z;
        Vector3 right = Vector3.zero;
        right.x = -tiles.TileSize.x;

        Vector3 position = transform.position + (rowIndex * forward) + ((colIndex - 1) * right);
        return tiles.transform.rotation* (position- tiles.transform.position) + tiles.transform.position;
    }

    public void SetEnterTriggerAction(Action action)
    {
        enterTrigger.gameObject.SetActive(true);
        enterTrigger.Reset();
        enterTrigger.onRoadEnter += action;
    }
}
