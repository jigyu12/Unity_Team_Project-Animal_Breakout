using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static MapObjectManager;

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

    public RoadSegment entrySegment;

    public int index;

    private List<Transform> nextPoints = new();
    public List<Transform> NextPoints => nextPoints;

    private List<RoadWay> nextRoadways = new();
    public List<RoadWay> NextRoadWays => nextRoadways;

    private List<CollidableMapObject> mapObjects = new();
    
    private readonly Vector3 yOffsetItem = new Vector3(0, 0.5f, 0); 
    private readonly Vector3 yOffsetBomb = new Vector3(0, 1f, 0); 

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

    public void SetMapObjects(RoadSegmentType type, MapObjectManager.MapObjectsBlueprint blueprint)
    {
        foreach (var roadSegmentWithType in roadSegments)
        {
            if (roadSegmentWithType.roadSegmentType == type)
            {
                for (int i = 0; i < blueprint.objectsConstructors.GetLength(0); i++)
                {
                    for (int j = 0; j < blueprint.objectsConstructors.GetLength(1); j++)
                    {
                        if (blueprint.objectsConstructors[i, j] != null)
                        {
                            var mapObject = blueprint.objectsConstructors[i, j].Invoke(roadSegmentWithType.roadSegment.GetTilePosition(i, j));
                            if (mapObject.TryGetComponent(out Trap trap) && trap.TrapType == TrapType.Bomb)
                            {
                                mapObject.transform.position += yOffsetBomb;
                            }
                            else if (mapObject.TryGetComponent(out ItemPenaltyCoin penaltyCoin))
                            {
                                mapObject.transform.position += yOffsetItem;
                            }
                            mapObjects.Add(mapObject);
                            mapObject.transform.SetParent(roadSegmentWithType.roadSegment.transform);
                            mapObject.transform.rotation = Quaternion.LookRotation(-roadSegmentWithType.roadSegment.transform.forward);
                        }
                    }
                }
            }
        }
    }

    public void SetRewardItemObjects(RoadSegmentType type, List<MapObjectManager.RewardItemBlueprint> blueprints)
    {
        foreach (var roadSegmentWithType in roadSegments)
        {
            if (roadSegmentWithType.roadSegmentType == type)
            {
                foreach (var unit in blueprints)
                {
                    var startPosition = roadSegmentWithType.roadSegment.GetTilePosition(unit.startEndCoordinate[0].Item1, unit.startEndCoordinate[0].Item2) + yOffsetItem;
                    var endPosition = roadSegmentWithType.roadSegment.GetTilePosition(unit.startEndCoordinate[1].Item1, unit.startEndCoordinate[1].Item2) + yOffsetItem;
                    if (unit.rewardItemConstructors != null)
                    {
                        var array = unit.rewardItemConstructors.Invoke(startPosition, endPosition, unit.itemGroupCount);
                        array.ToList().ForEach((item) => mapObjects.Add(item));
                        array.ToList().ForEach((item) =>
                        {
                            item.transform.SetParent(roadSegmentWithType.roadSegment.transform);
                            item.transform.rotation = Quaternion.LookRotation(-roadSegmentWithType.roadSegment.transform.forward);
                        }
                            );
                    }
                }
            }
        }
    }

    public void SetEntryTriggerAction(Action action)
    {
        entrySegment.SetEnterTriggerAction(action);
    }

    public List<Transform> GetNextRoadWayPoints()
    {
        nextPoints.Clear();
        for (int i = 0; i < roadSegments.Count(); i++)
        {
            if (roadSegments[i].roadSegmentType == RoadSegmentType.Exit)
            {
                nextPoints.Add(roadSegments[i].roadSegment.NextTransform);
            }
        }
        return nextPoints;
    }

    public void AddNextRoadWay(RoadWay next)
    {
        nextRoadways.Add(next);
    }

    private void OnDrawGizmos()
    {
        if (nextPoints.Count != 0)
        {
            Gizmos.color = Color.blue;
            foreach (var point in nextPoints)
            {
                var temp = point.position;
                temp.y = 5f;
                Gizmos.DrawLine(point.position, temp);
            }
        }
    }

    public Action release;

    public void OnGet()
    {
        nextRoadways.Clear();
        transform.rotation = Quaternion.identity;
        mapObjects.Clear();
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
        foreach (var item in mapObjects)
        {
            item.ReleasePool();
        }
        release.Invoke();
    }
}
