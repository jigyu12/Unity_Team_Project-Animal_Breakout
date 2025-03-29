using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

public class RoadChunk
{
    public Vector2Int chunkSize = new(3, 60);

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

    public RoadChunk(RoadManager roadManager)
    {
        this.roadManager = roadManager;
    }

    public void CreateRoadChunk(int startIndex, RoadChunkInformaion information)
    {
        this.startIndex = startIndex;

        var currSegment = roadManager.GetRoadSegment(WayType.Straight);
        entrySegment = currSegment;
        currSegment.transform.position = information.startPosition;
        currSegment.decoration?.UpdateDecoraionTiles(false, false, true);
        startIndex += currSegment.tileVerticalCount;
        roadSegments.Add(new RoadSegment[] { null, currSegment, null });
        
        for (int i = 1; i < chunkSize.y; i++)
        {
            var nextSegment = roadManager.GetRoadSegment(WayType.Straight);
            nextSegment.transform.position = currSegment.NextPosition;
            startIndex += nextSegment.tileVerticalCount;
            roadSegments.Add(new RoadSegment[] { null, nextSegment, null });

            if (i == information.leftWayIndex)
            {
                var leftSegment = roadManager.GetRoadSegment(WayType.Left);
                leftSegment.transform.position = nextSegment.NextLeftPosition;
                roadSegments.Last()[0] = leftSegment;
            }
            else if (i == information.rightWayIndex)
            {
                var rightSegment = roadManager.GetRoadSegment(WayType.Right);
                rightSegment.transform.position = nextSegment.NextRightPosition;
                roadSegments.Last()[2] = rightSegment;
            }

            nextSegment.decoration?.UpdateDecoraionTiles(information.leftWayIndex ==i, information.rightWayIndex ==i, true);
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
        nextRoadChunks.Add(roadManager.CreateRoadVerticalChunk(endIndex, roadSegments[9][1].NextPosition));

        for (int i = 0; i < roadSegments.Count; i++)
        {
            if (roadSegments[i][0] != null)
            {
                nextRoadChunks.Add(roadManager.CreateRoadLeftChunk(endIndex, roadSegments[i][0].NextPosition));
            }

            if (roadSegments[i][2] != null)
            {
                nextRoadChunks.Add(roadManager.CreateRoadRightChunk(endIndex, roadSegments[i][2].NextPosition));
            }
        }
    }

    public Vector3 GetRoadSegmentTilePosition(int row, int col)
    {
        int remainRow = row - startIndex;
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

    public void Rotate(float angle)
    {
        RotateAround(entrySegment.transform.position, angle);
    }

    public void RotateAround(Vector3 pivot, float angle)
    {
        foreach (var segmentsList in roadSegments)
        {
            foreach (var segment in segmentsList)
            {
                if (segment != null)
                {
                    segment.transform.RotateAround(pivot, Vector3.up, angle);
                }
            }
        }
    }
}
