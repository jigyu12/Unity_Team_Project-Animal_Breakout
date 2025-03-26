using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadSegments;
    private List<ObjectPool<GameObject>> roadSegmentPools = new();

    private PlayerMove player;
    public RoadChunk currentRoadChunk;
    private List<RoadChunk> activeRoadChunks = new();

    private MapRotator mapRotator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();

        for (int i = 0; i < roadSegments.Count(); i++)
        {
            var index = i;
            roadSegmentPools.Add(ObjectPoolManager.Instance.CreateObjectPool(roadSegments[index], () => InstantiateRoadSegment(roadSegments[index]), OnGetRoadSegment, OnReleaseMapUnit));
        }

        mapRotator = GetComponent<MapRotator>();

        var roadChunck = new RoadChunk(this);
        roadChunck.CreateRoadChunk(new RoadChunkInformaion(Vector3.zero, false, true, 5));
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


    public RoadChunk CreateRoadVerticalChunk(Vector3 startPosition)
    {
        var roadChunck = new RoadChunk(this);
        roadChunck.CreateRoadChunk(new RoadChunkInformaion(startPosition, false, true, 8));
        activeRoadChunks.Add(roadChunck);
        return roadChunck;
    }

    public RoadChunk CreateRoadLeftChunk(Vector3 startPosition)
    {
        var roadChunck = new RoadChunk(this);
        roadChunck.CreateRoadChunk(new RoadChunkInformaion(startPosition, false, false));
        roadChunck.Rotate(-90f);
        activeRoadChunks.Add(roadChunck);
        return roadChunck;
    }

    public RoadChunk CreateRoadRightChunk(Vector3 startPosition)
    {
        var roadChunck = new RoadChunk(this);
        roadChunck.CreateRoadChunk(new RoadChunkInformaion(startPosition, false, false));
        roadChunck.Rotate(90f);
        activeRoadChunks.Add(roadChunck);
        return roadChunck;
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
}
