using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class RoadWay : MonoBehaviour, IObjectPoolable
{
    public enum RoadSegmentType
    {
        Entry,
        None,
        Exit,
    }

    [Serializable]
    class EntryExitType
    {
        public RoadSegmentType roadSegmentType;
        public RoadSegment roadSegment;
    }

    [SerializeField]
    private EntryExitType[] roadSegments;

    private RoadSegment entrySegment;

    public int index;


    public struct StartPoint
    {
        public StartPoint(Vector3 position, float angle)
        {
            this.position = position;
            this.angle = angle;
        }

        public Vector3 position;
        public float angle;
    }

    private List<StartPoint> nextPoints = new();
    public List<StartPoint> NextPoints => nextPoints;

    private List<RoadWay> nextRoadways = new();
    public List<RoadWay> NextRoadWays => nextRoadways;



    public void Awake()
    {
        for (int i = 0; i < roadSegments.Count(); i++)
        {
            if (roadSegments[i].roadSegmentType == RoadSegmentType.Entry)
            {
                entrySegment = roadSegments[i].roadSegment;
                break;
            }
        }
    }

    public void SetEntryTriggerAction(Action action)
    {
        entrySegment.SetEnterTriggerAction(action);
    }

    private void SetNextRoadWayPoints()
    {
        nextPoints.Clear();
        for (int i = 0; i < roadSegments.Count(); i++)
        {
            if (roadSegments[i].roadSegmentType == RoadSegmentType.Exit)
            {
                nextPoints.Add(new StartPoint(roadSegments[i].roadSegment.NextPosition, roadSegments[i].roadSegment.GetTileRotation()));
            }
        }
    }

    public void AddNextRoadWay(RoadWay next)
    {
        nextRoadways.Add(next);
    }

    public Action release;

    public void OnGet()
    {
        SetNextRoadWayPoints();
        nextRoadways.Clear();

        transform.rotation = Quaternion.identity;
        foreach (var roadSegment in roadSegments)
        {
            roadSegment.roadSegment.Reset();
        }
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }

    public void Release()
    {
        release.Invoke();
    }
}
