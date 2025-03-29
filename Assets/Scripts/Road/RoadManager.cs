using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static MapObjectManager;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadSegmentPrefabs;
    private List<ObjectPool<GameObject>> roadSegmentPools = new();

    private PlayerMove player;
    [SerializeField]
    private MapObjectManager mapObjectManager;

    public RoadChunk currentRoadChunk;

    public List<(int index, MapObjectsBlueprint objects)> mapObjectsBlueprints = new();
    private List<RoadChunk> activeRoadChunks = new();


    private void Start()
    {
        enabled = false;
        var relayRunManager = GameObject.FindObjectOfType<RelayRunManager>();

        relayRunManager.onLoadPlayer += (playerStatus) => GetPlayer(playerStatus.gameObject.GetComponent<PlayerMove>());
        relayRunManager.onDiePlayer += (playerStatus) => enabled = false;

        for (int i = 0; i < roadSegmentPrefabs.Count(); i++)
        {
            var index = i;
            roadSegmentPools.Add(ObjectPoolManager.Instance.CreateObjectPool(roadSegmentPrefabs[index], () => InstantiateRoadSegment(roadSegmentPrefabs[index]), OnGetRoadSegment, OnReleaseMapUnit));
        }

        mapObjectsBlueprints.Add((0, mapObjectManager.GenerateMapObjectInformation(60, 3)));
        mapObjectsBlueprints.Add((60, mapObjectManager.GenerateMapObjectInformation(60, 3)));

        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(0, new RoadChunkInformaion(Vector3.zero, left:5));
        CreateMapObjects(roadChunk);
    }

    public void GetPlayer(PlayerMove player)
    {
        this.player = player;
        enabled = true;
    }

    public RoadSegment GetRoadSegment(WayType type)
    {
        return roadSegmentPools[(int)type].Get().GetComponent<RoadSegment>();
    }
    public void ReleaseRoadSegment(RoadSegment roadSegment)
    {
        roadSegmentPools[(int)roadSegment.directionType].Release(roadSegment.gameObject);
    }

    private GameObject InstantiateRoadSegment(GameObject roadSegment)
    {
        var unit = Instantiate(roadSegment, gameObject.transform);
        return unit;
    }
    private void OnGetRoadSegment(GameObject roadSegment)
    {
        roadSegment.GetComponent<RoadSegment>().Reset();
        roadSegment.transform.rotation = Quaternion.identity;
        roadSegment.gameObject.SetActive(true);
    }
    private void OnReleaseMapUnit(GameObject roadSegment)
    {
        roadSegment.gameObject.SetActive(false);
    }

    private ObjectType GetMapObjectsType(int row, int col)
    {
        for (int i = 0; i < mapObjectsBlueprints.Count; i++)
        {
            if (mapObjectsBlueprints[i].index > row)
            {
                return mapObjectsBlueprints[i - 1].objects.objectsTypes[row - mapObjectsBlueprints[i].index, col];
            }
        }
        return ObjectType.None;
    }

    private Action<Vector3> GetMapObjectsConstructor(int row, int col)
    {
        for (int i = 0; i < mapObjectsBlueprints.Count; i++)
        {
            if (mapObjectsBlueprints[i].index > row)
            {
                return mapObjectsBlueprints[i - 1].objects.objectsConstructors[row - mapObjectsBlueprints[i].index, col];
            }
        }
        return null;
    }

    //public RoadChunk CreateRoadChunk(int startIndex, Vector3 startPosition)
    //{
    //    mapObjectsBlueprints

    //    var roadChunk = new RoadChunk(this);
    //    roadChunk.CreateRoadChunk(startIndex, new RoadChunkInformaion(startPosition));
    //    activeRoadChunks.Add(roadChunk);

    //    return roadChunk;
    //}

    public RoadChunk CreateRoadVerticalChunk(int startIndex, Vector3 startPosition)
    {
        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(startIndex, new RoadChunkInformaion(startPosition, left: 5));
        activeRoadChunks.Add(roadChunk);

        CreateMapObjects(roadChunk);

        return roadChunk;
    }

    public RoadChunk CreateRoadLeftChunk(int startIndex, Vector3 startPosition)
    {
        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(startIndex, new RoadChunkInformaion(startPosition, left: 5));
        roadChunk.Rotate(-90f);
        activeRoadChunks.Add(roadChunk);
        return roadChunk;
    }

    public RoadChunk CreateRoadRightChunk(int startIndex, Vector3 startPosition)
    {
        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(startIndex, new RoadChunkInformaion(startPosition, right:5));
        roadChunk.Rotate(90f);
        activeRoadChunks.Add(roadChunk);
        return roadChunk;
    }

    public void ReleasePassedRoadChunks()
    {
        var releaseList = new List<RoadChunk>();
        foreach (var roadChunk in activeRoadChunks)
        {
            if (roadChunk.roadSegments[9][1].transform.position.z + 50f < player.transform.position.z)
            {
                releaseList.Add(roadChunk);
            }
        }

        foreach (var roadChunk in releaseList)
        {
            activeRoadChunks.Remove(roadChunk);
            roadChunk.ReleaseRoadSegments();
        }
    }

    public void CreateMapObjects(RoadChunk roadChunk)
    {
        ProcessCreateMapObject(roadChunk, roadChunk.StartIndex, 0);
    }

    private void ProcessCreateMapObject(RoadChunk roadChunk, int start, int objectIndex)
    {
        if (roadChunk.EndIndex > mapObjectsBlueprints[objectIndex + 1].index)
        {
            for (int i = start; i < mapObjectsBlueprints[objectIndex + 1].index; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapObjectsBlueprints[objectIndex].objects.objectsConstructors[i - mapObjectsBlueprints[objectIndex].index, j]?.Invoke(roadChunk.GetRoadSegmentTilePosition(i, j));
                }
            }

            mapObjectsBlueprints.Add((mapObjectsBlueprints.Last().index + 60, mapObjectManager.GenerateMapObjectInformation(60, 3)));
            ProcessCreateMapObject(roadChunk, mapObjectsBlueprints[objectIndex + 1].index, objectIndex + 1);
            mapObjectsBlueprints.RemoveAt(0);
        }
        else
        {
            for (int i = start; i < roadChunk.EndIndex; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapObjectsBlueprints[objectIndex].objects.objectsConstructors[i - mapObjectsBlueprints[objectIndex].index, j]?.Invoke(roadChunk.GetRoadSegmentTilePosition(i, j));
                }
            }
        }
    }

}