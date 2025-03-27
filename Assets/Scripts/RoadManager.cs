using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadSegmentPrefabs;
    private List<ObjectPool<GameObject>> roadSegmentPools = new();

    private PlayerMove player;
    [SerializeField]
    private MapObjectInformationManager mapObjectManager;

    public RoadChunk currentRoadChunk;

    public List<(int index, Action<Vector3>[,] actions)> mapObjects = new();
    private List<RoadChunk> activeRoadChunks = new();

    private MapRotator mapRotator;

    private void Awake()
    {
        enabled = false;
    }

    public void Initialize()
    {
        player = GameObject.FindGameObjectsWithTag("Player").First((gameObject) => gameObject.TryGetComponent<PlayerMove>(out PlayerMove move)).GetComponent<PlayerMove>();

        for (int i = 0; i < roadSegmentPrefabs.Count(); i++)
        {
            var index = i;
            roadSegmentPools.Add(ObjectPoolManager.Instance.CreateObjectPool(roadSegmentPrefabs[index], () => InstantiateRoadSegment(roadSegmentPrefabs[index]), OnGetRoadSegment, OnReleaseMapUnit));
        }

        mapRotator = GetComponent<MapRotator>();

        mapObjects.Add((0, mapObjectManager.GenerateMapObjectInformation(60, 3)));
        mapObjects.Add((60, mapObjectManager.GenerateMapObjectInformation(60, 3)));

        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(0, new RoadChunkInformaion(Vector3.zero, false, true, 5));
        CreateMapObjects(roadChunk);

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


    public RoadChunk CreateRoadVerticalChunk(int startIndex, Vector3 startPosition)
    {
        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(startIndex, new RoadChunkInformaion(startPosition, true, false, 8));
        activeRoadChunks.Add(roadChunk);

        CreateMapObjects(roadChunk);

        return roadChunk;
    }

    public RoadChunk CreateRoadLeftChunk(int startIndex, Vector3 startPosition)
    {
        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(startIndex, new RoadChunkInformaion(startPosition, false, false));
        roadChunk.Rotate(-90f);
        activeRoadChunks.Add(roadChunk);
        return roadChunk;
    }

    public RoadChunk CreateRoadRightChunk(int startIndex, Vector3 startPosition)
    {
        var roadChunk = new RoadChunk(this);
        roadChunk.CreateRoadChunk(startIndex, new RoadChunkInformaion(startPosition, false, false));
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
        if (roadChunk.EndIndex> mapObjects[objectIndex+1].index)
        {
            for (int i = start; i < mapObjects[objectIndex+1].index; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapObjects[objectIndex].actions[i - mapObjects[objectIndex].index, j]?.Invoke(roadChunk.GetRoadSegmentTilePosition(i, j));
                }
            }

            mapObjects.Add((mapObjects.Last().index + 60, mapObjectManager.GenerateMapObjectInformation(60, 3)));
            ProcessCreateMapObject(roadChunk, mapObjects[objectIndex+1].index, objectIndex+1);
            mapObjects.RemoveAt(0);
        }
        else
        {
            for (int i = start; i < roadChunk.EndIndex; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapObjects[objectIndex].actions[i - mapObjects[objectIndex].index, j]?.Invoke(roadChunk.GetRoadSegmentTilePosition(i, j));
                }
            }
        }
    }

}