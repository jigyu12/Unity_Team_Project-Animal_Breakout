using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public enum RoadMakeMode
{
    RandomWay,
    Vertical,
}

public enum ItemSetMode
{
    None,
    TrapAndReward,
}

public class TempleRunStyleRoadMaker : InGameManager
{
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

    public int currentRoadWayCount = 10;
    public Action onCurrentRoadWayEmpty;

    public int precreateRoadWayCount;

    private Queue<int> nextRoadWayTypeQueue = new();


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

        GameManager.StageManager.SetInitialRoadMode();
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
                int randomIndex1 = GameDataManager.Instance.GetNextRandomMapObjectsPrefabId();
                int randomIndex2 = GameDataManager.Instance.GetNextRandomMapObjectsPrefabId();
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
    public void SetMapObjectMakeMode(ItemSetMode mode)
    {
        Debug.Log("Road MapObject Mode : " + mode.ToString());
        //if (currentMapObjectsMode == mode)
        //{
        //    return;
        //}
        currentMapObjectsMode = mode;
    }

    #endregion

    #region roadMakeMode
    public void SetRoadMakeMode(RoadMakeMode mode, int roadWayCount)
    {
        Debug.Log("Road Make Mode : "+ mode.ToString());
        currentRoadMakeMode = mode;
        currentRoadWayCount = roadWayCount;
        nextRoadWayTypeQueue.Clear();
        PushNextRoadTypes(currentRoadMakeMode);
    }

    //public void SetRoadMakeModeImmediate(RoadMakeMode mode)
    //{
    //    if (currentRoadMakeMode == mode)
    //    {
    //        return;
    //    }
    //    currentRoadMakeMode = mode;
    //    nextRoadWayType.Clear();
    //    PushNextRoadTypes();
    //}

    private int GetNextRoadWayType()
    {
        if (nextRoadWayTypeQueue.Count == 0)
        {
            onCurrentRoadWayEmpty?.Invoke();
        }

        if (currentRoadWayCount == -1)
        {
            return nextRoadWayTypeQueue.Peek();
        }
        else
        {
            return nextRoadWayTypeQueue.Dequeue();
        }
    }

    private void PushNextRoadTypes(RoadMakeMode roadMakeMode)
    {
        if (roadMakeMode == RoadMakeMode.RandomWay)
        {
            for (int i = 0; i < Mathf.Abs(currentRoadWayCount); i++)
            {
                nextRoadWayTypeQueue.Enqueue(UnityEngine.Random.Range(1, roadWayPrefabs.Count()));
            }
        }
        else if (roadMakeMode == RoadMakeMode.Vertical)
        {
            for (int i = 0; i < Mathf.Abs(currentRoadWayCount); i++)
            {
                nextRoadWayTypeQueue.Enqueue(0);
            }
        }
    }

    #endregion
}
