using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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

    private GameObject player;

    private List<RoadWay> activeRoadWays = new();
    private RoadWay currentRoad;

    public Action<RoadWay> onCurrentLinkChanged;

    public RoadSegment initialRoadSegment;

    //[SerializeField]
    //private RoadWayRotator roadWayRotator;

    public int roadChunkSize = 10;
    public int precreateRoadWayCount;

    private Queue<int> nextRoadWayType = new();

    public void SetRoadMakeMode(RoadMakeMode mode)
    {
        if (currentMode == mode)
        {
            return;
        }

        currentMode = mode;
    }

    private void Awake()
    {
        SetRoadMakeMode(RoadMakeMode.InfinityVertical);
    }

    public override void Initialize()
    {
        base.Initialize();
        //GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReady, () => roadWayRotator.SetPlayerMove(GameManager.PlayerManager));
        //GameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, () => SetRoadMakeMode(RoadMakeMode.RandomWay));
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, () => SetRoadMakeMode(RoadMakeMode.RandomWay));

        for (int i = 0; i < roadWayPrefabs.Count(); i++)
        {
            var prefabIndex = i;
            roadWayPools.Add(
                GameManager.ObjectPoolManager.CreateObjectPool(roadWayPrefabs[prefabIndex],
               () => Instantiate(roadWayPrefabs[prefabIndex], transform),
                OnGetRoadWay,
            OnRelease));
        }

        var roadWay = CreateRoadWay(0, 0);
        CreateNNextRoadWay(precreateRoadWayCount, roadWay, false);
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
        var previous = currentRoad;
        currentRoad = roadWay;
        onCurrentLinkChanged?.Invoke(currentRoad);

        CreateNNextRoadWay(precreateRoadWayCount, currentRoad);
        ReleasePassedRoadWay();
    }

    public void CreateNextRoadWay(RoadWay previousRoadWay, bool createMapObject = true)
    {
        if (previousRoadWay.NextRoadWays.Count != 0)
            return;

        var startPoints = previousRoadWay.GetNextRoadWayPoints();
        int randomIndex;
        if (currentMode == RoadMakeMode.InfinityVertical)
        {
            randomIndex = 0;    //0번이 직선 길임
        }
        else
        {
            randomIndex = UnityEngine.Random.Range(1, roadWayPrefabs.Count());
        }


        foreach (var trs in startPoints)
        {
            var roadWay = CreateRoadWay(previousRoadWay.index + 1, randomIndex);
            roadWay.transform.rotation = trs.rotation;
            roadWay.transform.position = trs.position;

            if (createMapObject)
            {
                roadWay.SetMapObjects(RoadWay.RoadSegmentType.Entry, GameManager.MapObjectManager.GetMapObjectsBlueprint(1));
                roadWay.SetRewardItemObjects(RoadWay.RoadSegmentType.Entry, GameManager.MapObjectManager.GetRewardItemBlueprint(1));

                roadWay.SetMapObjects(RoadWay.RoadSegmentType.None, GameManager.MapObjectManager.GetMapObjectsBlueprint(2));
                roadWay.SetRewardItemObjects(RoadWay.RoadSegmentType.None, GameManager.MapObjectManager.GetRewardItemBlueprint(2));
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

    private Queue<RoadWay> releaseQueue = new();

    public void ReleasePassedRoadWay()
    {
        foreach (var nowRoadWay in activeRoadWays)
        {
            if (nowRoadWay.index < currentRoad.index)
            {
                releaseQueue.Enqueue(nowRoadWay);
            }
            else if (nowRoadWay.index == currentRoad.index && nowRoadWay != currentRoad)
            {
                AddToReleaseQueueLinkedRoadWay(nowRoadWay);
            }
        }

        while (releaseQueue.Count > 0)
        {
            var target = releaseQueue.Dequeue();
            activeRoadWays.Remove(target);
            target.Release();
        }
    }

    private void AddToReleaseQueueLinkedRoadWay(RoadWay root)
    {
        releaseQueue.Enqueue(root);
        foreach (var next in root.NextRoadWays)
        {
            AddToReleaseQueueLinkedRoadWay(next);
        }
    }

    private void CreateNNextRoadWay(int next, RoadWay previous, bool createMapObject = true)
    {
        if (next <= 0)
            return;

        if (previous.NextRoadWays.Count == 0)
        {
            CreateNextRoadWay(previous, createMapObject);
        }

        foreach (var n in previous.NextRoadWays)
        {
            CreateNNextRoadWay(next - 1, n, createMapObject);
        }
    }

}
