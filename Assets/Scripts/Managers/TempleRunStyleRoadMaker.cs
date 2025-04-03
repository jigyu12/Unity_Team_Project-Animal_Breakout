using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Progress;


//템플런식 길 생성
public class TempleRunStyleRoadMaker : RoadManager
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
    public int precreateRoadWayCount=3;

    private Dictionary<int, MapObjectManager.MapObjectsBlueprint> item;
    private Dictionary<int, List<MapObjectManager.RewardItemBlueprint>> reward;

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
                ObjectPoolManager.Instance.CreateObjectPool(roadWayPrefabs[prefabIndex],
               () => Instantiate(roadWayPrefabs[prefabIndex], transform),
                OnGetRoadWay,
            OnRelease));
        }

        item= mapObjectManager.GenerateMapObjectInformation(10 * 20, 3);
        reward = mapObjectManager.GenerateRewardItemInformation();


        int randomIndex = UnityEngine.Random.Range(0, roadWayPrefabs.Count());
        var roadWay = CreateRoadWay(0, randomIndex);
        CreateNextRoadWay(roadWay);
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

    public void CreateNextRoadWay(RoadWay previousRoadWay)
    {
        var startPoints = previousRoadWay.GetNextRoadWayPoints();   
        int randomIndex = UnityEngine.Random.Range(0, roadWayPrefabs.Count());
 
        foreach (var trs in startPoints)
        { 
            var roadWay = CreateRoadWay(previousRoadWay.index + 1, randomIndex);
            roadWay.transform.position = trs.position;
            roadWay.transform.rotation = Quaternion.Euler(0, trs.angle, 0);
            //roadWay.entrySegment

            for (int i = 0; i < item[1].objectsConstructors.GetLength(0); i++)
            {
                for (int j = 0; j < item[1].objectsConstructors.GetLength(1); j++)
                {
                    item[1].objectsConstructors[i, j]?.Invoke(roadWay.entrySegment.GetTilePosition(i, j));
                }
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
            if (nowRoadWay.index+1 < currentRoad.index)
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

    //private IEnumerator CreateNextRoadWay()
    //{
    //    if(currentMode== RoadMakeMode.RandomWay)
    //    {


    //        yield return;
    //    }
    //    else
    //    {

    //    }
    //}

    private void CreateNextRoadWay(int next, RoadWay targetRoadWay)
    {
        if(targetRoadWay.NextRoadWays.Count==0)
        {
            CreateNextRoadWay(targetRoadWay);
        }

        CreateNextRoadWay(next - 1, targetRoadWay);
    }
}

