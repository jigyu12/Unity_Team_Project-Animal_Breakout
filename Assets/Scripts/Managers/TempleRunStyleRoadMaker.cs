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

    public enum ItemSetMode
    {
        None,
        TrapAndReward,
    }

    private RoadMakeMode currentRoadMakeMode;
    private ItemSetMode currentMapObjectsMode;

    public GameObject[] roadWayPrefabs;
    private List<ObjectPool<GameObject>> roadWayPools = new();

    private List<RoadWay> activeRoadWays = new();
    private RoadWay currentRoad;

    public Action<RoadWay, RoadWay> onCurrentWayChanged; //previous, current ��

    [SerializeField]
    private RoadWay initialRoadWay;

    //[SerializeField]
    //private RoadWayRotator roadWayRotator;

    public int roadChunkSize = 10;
    public int precreateRoadWayCount;

    private Queue<int> nextRoadWayType = new();


    private void Awake()
    {
        SetRoadMakeMode(RoadMakeMode.RandomWay);
    }

    public override void Initialize()
    {
        base.Initialize();

        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReady, InitialRoadSetting);

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

    private void InitialRoadSetting()
    {
        currentRoad = initialRoadWay;
        onCurrentWayChanged += RemoveInitialRoadWay;

        SetMapObjectMakeMode(ItemSetMode.TrapAndReward);

        PushNextRoadType();
        CreateNNextRoadWay(precreateRoadWayCount, initialRoadWay, currentMapObjectsMode != ItemSetMode.None);
    }

    private void RemoveInitialRoadWay(RoadWay previous, RoadWay current)
    {
        if (previous.Equals(initialRoadWay))
        {
            Destroy(previous.gameObject);
        }
        onCurrentWayChanged -= RemoveInitialRoadWay;
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
        var previousRoad = currentRoad;
        currentRoad = roadWay;
        onCurrentWayChanged?.Invoke(previousRoad, currentRoad);

        CreateNNextRoadWay(precreateRoadWayCount, currentRoad, currentMapObjectsMode != ItemSetMode.None);
        ReleasePassedRoadWay();
    }

    public void CreateNextRoadWay(RoadWay previousRoadWay, bool createMapObject = true)
    {
        if (previousRoadWay.NextRoadWays.Count != 0)
            return;

        var startPoints = previousRoadWay.GetNextRoadWayPoints();
        int wayTypeIndex = GetNextRoadWayType();

        foreach (var trs in startPoints)
        {
            var roadWay = CreateRoadWay(previousRoadWay.index + 1, wayTypeIndex);
            roadWay.transform.SetPositionAndRotation(trs.position, trs.rotation);

            //�ʿ�����Ʈ ����
            if (createMapObject)
            {
                int randomIndex1 = UnityEngine.Random.Range(1, 4);
                int randomIndex2 = UnityEngine.Random.Range(1, 4);
                roadWay.SetMapObjects(RoadWay.RoadSegmentType.Entry, GameManager.MapObjectManager.GetMapObjectsBlueprint(randomIndex1));
                roadWay.SetRewardItemObjects(RoadWay.RoadSegmentType.Entry, GameManager.MapObjectManager.GetRewardItemBlueprint(randomIndex1));

                roadWay.SetMapObjects(RoadWay.RoadSegmentType.None, GameManager.MapObjectManager.GetMapObjectsBlueprint(randomIndex2));
                roadWay.SetRewardItemObjects(RoadWay.RoadSegmentType.None, GameManager.MapObjectManager.GetRewardItemBlueprint(randomIndex2));
            }

            previousRoadWay.AddNextRoadWay(roadWay);
        }
    }

    //public void CreateLastRoadWay()
    //{
    //    if (activeRoadWays.Count == 0)
    //    {
    //        return;
    //    }

    //    int lastIndex = activeRoadWays.Last().index;
    //    int count = activeRoadWays.Count;
    //    for (int i = count - 1; i >= 0; i--)
    //    {
    //        if (lastIndex == activeRoadWays[i].index)
    //        {
    //            CreateNextRoadWay(activeRoadWays[i]);
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //}

    private Queue<RoadWay> releaseQueue = new();

    public void ReleasePassedRoadWay()
    {
        foreach (var nowRoadWay in activeRoadWays)
        {
            if (nowRoadWay.index < currentRoad.index)
            {
                //������ �� ����
                releaseQueue.Enqueue(nowRoadWay);
            }
            else if (nowRoadWay.index == currentRoad.index && nowRoadWay != currentRoad)
            {
                //���� Way�� ������ ��� ������� ����ȱ� ����
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

    //n�� ���� �ʿ��� ����
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

    #region mapObjectsMode

    private void SetMapObjectMakeMode(ItemSetMode mode)
    {
        if (currentMapObjectsMode == mode)
        {
            return;
        }
        currentMapObjectsMode = mode;
    }

    #endregion

    #region roadMakeMode
    public void SetRoadMakeMode(RoadMakeMode mode)
    {
        if (currentRoadMakeMode == mode)
        {
            return;
        }
        currentRoadMakeMode = mode;
        PushNextRoadType();
    }

    public void SetRoadMakeModeImmediate(RoadMakeMode mode)
    {
        if (currentRoadMakeMode == mode)
        {
            return;
        }
        currentRoadMakeMode = mode;
        nextRoadWayType.Clear();
        PushNextRoadType();
    }

    private int GetNextRoadWayType()
    {
        if (nextRoadWayType.Count == 0)
        {
            if (currentRoadMakeMode == RoadMakeMode.RandomWay)
            {
                SetRoadMakeMode(RoadMakeMode.InfinityVertical);
            }
            else if (currentRoadMakeMode == RoadMakeMode.InfinityVertical)
            {
                SetRoadMakeMode(RoadMakeMode.RandomWay);
            }
        }

        return nextRoadWayType.Dequeue();
    }

    private void PushNextRoadType()
    {
        if (currentRoadMakeMode == RoadMakeMode.RandomWay)
        {
            for (int i = 0; i < roadChunkSize; i++)
            {
                nextRoadWayType.Enqueue(UnityEngine.Random.Range(1, roadWayPrefabs.Count()));
            }
        }
        else if (currentRoadMakeMode == RoadMakeMode.InfinityVertical)
        {
            for (int i = 0; i < roadChunkSize; i++)
            {
                nextRoadWayType.Enqueue(0);
            }
        }
    }

    #endregion
}
