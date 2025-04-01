using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MapObjectManager;

public struct RoadChunkInformaion
{
    public RoadChunkInformaion(Vector3 startPosition, int left = -1, int right = -1)
    {
        this.startPosition = startPosition;
        leftWayIndex = left;
        rightWayIndex = right;
    }

    public Vector3 startPosition;
    public int leftWayIndex;
    public int rightWayIndex;
}

public class RoadChunk : MonoBehaviour
{
    private Vector2Int chunkSize = new(3, 10);

    public Vector2Int ChunkSize => chunkSize;

    [ReadOnly]
    public List<RoadSegment[]> roadSegments = new();

    private RoadManager roadManager;
    private RoadSegment entrySegment;

    private int startIndex;
    private int endIndex;
    public int StartIndex => startIndex;
    public int EndIndex => endIndex;

    private List<RoadChunk> nextRoadChunks = new();

    public Action onCreateRoadChunk;
    public List<RoadChunk> NextRoadChunks
    {
        get => nextRoadChunks;
    }

    public void SetRoadManager(RoadManager roadManager)
    {
        this.roadManager = roadManager;
    }

    public void CreateRoadChunk(int startIndex, RoadChunkInformaion information)
    {
        this.startIndex = startIndex;

        var currSegment = roadManager.GetRoadSegment(WayType.Straight);
        entrySegment = currSegment;
        currSegment.transform.SetParent(transform);
        currSegment.transform.position = information.startPosition;
        currSegment.decoration?.UpdateDecoraionTiles(false, false, true);
        startIndex += currSegment.tileVerticalCount;
        roadSegments.Add(new RoadSegment[] { null, currSegment, null });

        for (int i = 1; i < ChunkSize.y; i++)
        {
            var nextSegment = roadManager.GetRoadSegment(WayType.Straight);
            nextSegment.transform.position = currSegment.NextPosition;
            nextSegment.transform.SetParent(transform);
            startIndex += nextSegment.tileVerticalCount;
            roadSegments.Add(new RoadSegment[] { null, nextSegment, null });

            if (i == information.leftWayIndex)
            {
                var leftSegment = roadManager.GetRoadSegment(WayType.Left);
                leftSegment.transform.position = nextSegment.NextLeftPosition;
                leftSegment.transform.SetParent(transform);
                roadSegments.Last()[0] = leftSegment;
            }
            else if (i == information.rightWayIndex)
            {
                var rightSegment = roadManager.GetRoadSegment(WayType.Right);
                rightSegment.transform.position = nextSegment.NextRightPosition;
                rightSegment.transform.SetParent(transform);
                roadSegments.Last()[2] = rightSegment;
            }

            nextSegment.decoration?.UpdateDecoraionTiles(information.leftWayIndex == i, information.rightWayIndex == i, true);
            currSegment = nextSegment;
        }
        endIndex = startIndex;

        entrySegment.SetEnterTriggerAction(OnEnterChunk);
        onCreateRoadChunk?.Invoke();
    }

    public void OnEnterChunk()
    {
        roadManager.ReleasePassedRoadChunks();
        roadManager.currentRoadChunk = this;
        var nextRoadChunk = roadManager.CreateNextRoadChunk(endIndex, roadSegments[ChunkSize.y - 1][1].NextPosition, out MapObjectsBlueprint mapObjectsBlueprint);
        nextRoadChunks.Add(nextRoadChunk);

        for (int i = 0; i < roadSegments.Count; i++)
        {
            if (roadSegments[i][0] != null)
            {
                nextRoadChunks.Add(roadManager.CreateRoadLeftChunk(endIndex, roadSegments[i][0].NextPosition, mapObjectsBlueprint));
            }

            if (roadSegments[i][2] != null)
            {
                nextRoadChunks.Add(roadManager.CreateRoadRightChunk(endIndex, roadSegments[i][2].NextPosition, mapObjectsBlueprint));
            }
        }
    }

    public Vector3 GetRoadSegmentTilePosition(int row, int col)
    {
        int remainRow = row;
        for (int i = 0; i < roadSegments.Count; i++)
        {
            if (remainRow < roadSegments[i][1].tileVerticalCount)
            {
                return roadSegments[i][1].GetTilePosition(remainRow, col);
            }
            remainRow -= roadSegments[i][1].tileVerticalCount;
        }
        return Vector3.zero;
    }

    public void ReleaseRoadSegments()
    {
        foreach (var segmentsList in roadSegments)
        {
            foreach (var segment in segmentsList)
            {
                if (segment != null)
                {
                    roadManager.ReleaseRoadSegment(segment);
                }
            }
        }
    }
}
