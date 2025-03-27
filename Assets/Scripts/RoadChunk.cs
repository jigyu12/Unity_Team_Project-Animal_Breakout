using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoadChunkInformaion
{
    public RoadChunkInformaion(Vector3 startPosition, bool left, bool right, int turnIndex = -1)
    {
        this.startPosition = startPosition;
        this.isLeftWayExist = left;
        this.isRightWayExist = right;
        this.turnIndex = (left || right ? (turnIndex == -1 ? turnIndex = 10 - 1 : turnIndex) : -1);
    }

    public Vector3 startPosition;
    public int turnIndex;
    public bool isLeftWayExist;
    public bool isRightWayExist;
}

public class RoadChunk
{
    public Vector2Int chunkSize = new(3, 10);
    public List<RoadSegment[]> roadSegments = new List<RoadSegment[]>();

    private RoadManager roadManager;
    private RoadSegment entrySegment;
    private List<RoadChunk> nextRoadChunks = new();
    public List<RoadChunk> NextRoadChunks
    {
        get => nextRoadChunks;
    }

    public RoadChunk(RoadManager roadManager)
    {
        this.roadManager = roadManager;
    }

    public void CreateRoadChunk(RoadChunkInformaion information)
    {
        var currSegment = roadManager.GetRoadSegment(WayType.Straight);
        entrySegment = currSegment;
        currSegment.transform.position = information.startPosition;
        currSegment.decoration?.UpdateDecoraionTiles(false, false, true);
        roadSegments.Add(new RoadSegment[] { null, currSegment, null });
        for (int i = 1; i < chunkSize.y; i++)
        {
            var nextSegment = roadManager.GetRoadSegment(WayType.Straight);
            nextSegment.transform.position = currSegment.NextPosition;
            if (i == information.turnIndex)
            {
                if (information.isLeftWayExist)
                {
                    var leftSegment = roadManager.GetRoadSegment(WayType.Left);
                    leftSegment.transform.position = nextSegment.NextLeftPosition;
                    roadSegments.Add(new RoadSegment[] { leftSegment, nextSegment, null });
                }
                else if (information.isRightWayExist)
                {
                    var rightSegment = roadManager.GetRoadSegment(WayType.Right);
                    rightSegment.transform.position = nextSegment.NextRightPosition;
                    roadSegments.Add(new RoadSegment[] { null, nextSegment, rightSegment });
                }
                nextSegment.decoration?.UpdateDecoraionTiles(information.isLeftWayExist, information.isRightWayExist, true);
            }
            else
            {
                roadSegments.Add(new RoadSegment[] { null, nextSegment, null });
                nextSegment.decoration?.UpdateDecoraionTiles(false, false, true);
            }

            currSegment = nextSegment;
        }

        entrySegment.SetEnterTriggerAction(OnEnterChunk);
    }

    public void OnEnterChunk()
    {
        roadManager.ReleasePassedRoadChunks();
        roadManager.currentRoadChunk = this;
        nextRoadChunks.Add(roadManager.CreateRoadVerticalChunk(roadSegments[9][1].NextPosition));

        for (int i = 0; i < roadSegments.Count; i++)
        {
            if (roadSegments[i][0] != null)
            {
                nextRoadChunks.Add(roadManager.CreateRoadLeftChunk(roadSegments[i][0].NextPosition));
            }

            if (roadSegments[i][2] != null)
            {
                nextRoadChunks.Add(roadManager.CreateRoadRightChunk(roadSegments[i][2].NextPosition));
            }
        }
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
