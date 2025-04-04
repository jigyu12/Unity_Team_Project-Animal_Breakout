using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;


public class TempleRunStyleRoadMaker : InGameManager
{
   
    public enum RoadMakeMode
    {
        RandomWay,
        InfinityVertical,
    }

    public RoadMakeMode currentMode;

    public GameObject[] roadWayPrefabs;
    private List<ObjectPool<GameObject>> roadWayPools = new();

    private PlayerMove player;

    private List<RoadWay> activeRoadWays = new();

    private RoadWay currentRoad;

    public Action<RoadWay> onCurrentLinkChanged;

    public RoadSegment initialRoadSegment;
    private MapObjectManager mapObjectManager;

    public int roadChunkSize = 10;
    public int precreateRoadWayCount = 3;


    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < roadWayPrefabs.Count(); i++)
        {
            var prefabIndex = i;
            roadWayPools.Add(
                GameManager.ObjectPoolManager.CreateObjectPool(roadWayPrefabs[prefabIndex],
               () => Instantiate(roadWayPrefabs[prefabIndex], transform),
                OnGetRoadWay,
            OnRelease));
        }
    }

    private void Start()
    {
        enabled = false;
        var gameManager = GameObject.FindObjectOfType<GameManager>();
        mapObjectManager = GameObject.FindObjectOfType<MapObjectManager>();

        gameManager.onPlayerSpawned += (playerStatus) => GetPlayer(playerStatus.gameObject.GetComponent<PlayerMove>());
        gameManager.onPlayerDied += (playerStatus) => enabled = false;

        for (int i = 0; i < roadWayPrefabs.Count(); i++)
        {
            var prefabIndex = i;
            roadWayPools.Add(
                GameManager.ObjectPoolManager.CreateObjectPool(roadWayPrefabs[prefabIndex],
               () => Instantiate(roadWayPrefabs[prefabIndex], transform),
                OnGetRoadWay,
            OnRelease));
        }

        int randomIndex = UnityEngine.Random.Range(0, roadWayPrefabs.Count());
        var roadWay = CreateRoadWay(0, randomIndex);
        CreateNextRoadWay(roadWay, false);
    }

    public void GetPlayer(PlayerMove player)
    {
        this.player = player;
        enabled = true;
    }

    private void OnGetRoadWay(GameObject roadWay)
    {
        roadWay.GetComponent<RoadWay>().OnGet();
    }

    private void OnRelease(GameObject roadWay)
    {
        roadWay.GetComponent<RoadWay>().OnRelease();
    }

    private RoadWay CreateRoadWay(int index, int randomIndex)
    {
        //var roadWay = Instantiate(roadWayPrefab, gameObject.transform).GetComponent<RoadWay>();
        var roadWay = roadWayPools[randomIndex].Get().GetComponent<RoadWay>();
        roadWay.release = () => roadWayPools[randomIndex].Release(roadWay.gameObject);

        roadWay.SetEntryTriggerAction(() => SetCurrentRoadWay(roadWay));
        roadWay.index = index;
        activeRoadWays.Add(roadWay);

        return roadWay;
    }

    public void SetCurrentRoadWay(RoadWay roadWay)
    {
        currentRoad = roadWay;
        onCurrentLinkChanged?.Invoke(currentRoad);

        //CreateLastRoadWay();
        CreateNextRoadWay(currentRoad);
        ReleasePassedRoadWay();
    }

    public void CreateNextRoadWay(RoadWay previousRoadWay, bool createMapObject = true)
    {
        var startPoints = previousRoadWay.GetNextRoadWayPoints();
        int randomIndex = UnityEngine.Random.Range(0, roadWayPrefabs.Count());

        foreach (var trs in startPoints)
        {
            var roadWay = CreateRoadWay(previousRoadWay.index + 1, randomIndex);
            roadWay.transform.position = trs.position;
            roadWay.transform.rotation = Quaternion.Euler(0, trs.angle, 0);

            if (createMapObject)
            {
                roadWay.SetMapObjects(RoadWay.RoadSegmentType.Entry, mapObjectManager.GetMapObjectsBlueprint(1));
                roadWay.SetRewardItemObjects(RoadWay.RoadSegmentType.Entry, mapObjectManager.GetRewardItemBlueprint(1));

                roadWay.SetMapObjects(RoadWay.RoadSegmentType.None, mapObjectManager.GetMapObjectsBlueprint(2));
                roadWay.SetRewardItemObjects(RoadWay.RoadSegmentType.None, mapObjectManager.GetRewardItemBlueprint(2));
            }

            previousRoadWay.AddNextRoadWay(roadWay);
        }
    }

    public void CreateLastRoadWay()
    {
        if (activeRoadWays.Count == 0)
        {
            return;
        }

        int lastIndex = activeRoadWays.Last().index;
        int count = activeRoadWays.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            if (lastIndex == activeRoadWays[i].index)
            {
                CreateNextRoadWay(activeRoadWays[i]);
            }
            else
            {
                break;
            }
        }
    }

    public void ReleasePassedRoadWay()
    {
        while (activeRoadWays.Count != 0)
        {
            var nowRoadWay = activeRoadWays[0];
            if (nowRoadWay.index + 1 < currentRoad.index)
            {
                activeRoadWays.RemoveAt(0);
                nowRoadWay.Release();
            }
            else
            {
                break;
            }
        }
    }

    private void NEst(int next, RoadWay targetRoadWay)
    {
        if (targetRoadWay.NextRoadWays.Count == 0)
        {
            CreateNextRoadWay(targetRoadWay);
        }

        NEst(next - 1, targetRoadWay);
    }

}

