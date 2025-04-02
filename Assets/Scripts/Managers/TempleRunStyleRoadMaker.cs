using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;


//템플런식 길 생성
public class TempleRunStyleRoadMaker : RoadManager
{
    public GameObject[] roadWayPrefabs;
    private List<ObjectPool<GameObject>> roadWayPools = new();

    private PlayerMove player;

    private List<RoadWay> activeRoadWays = new();

    private RoadWay currentRoad;

    public Action<RoadWay> onCurrentLinkChanged;

    public RoadSegment initialRoadSegment;
    public int roadChunkSize = 10;


    private void Start()
    {
        enabled = false;
        var relayRunManager = GameObject.FindObjectOfType<RelayRunManager>();

        relayRunManager.onLoadPlayer += (playerStatus) => GetPlayer(playerStatus.gameObject.GetComponent<PlayerMove>());
        relayRunManager.onDiePlayer += (playerStatus) => enabled = false;

        for (int i = 0; i < roadWayPrefabs.Count(); i++)
        {
            var prefabIndex = i;
            roadWayPools.Add(
                ObjectPoolManager.Instance.CreateObjectPool(roadWayPrefabs[prefabIndex],
               ()=>Instantiate(roadWayPrefabs[prefabIndex], transform),
                OnGetRoadWay,
                OnRelease));
        }

        int randomIndex = UnityEngine.Random.Range(0, roadWayPrefabs.Count());
        var roadWay = CreateRoadWay(0, randomIndex);

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
        roadWay.release = ()=>roadWayPools[randomIndex].Release(roadWay.gameObject);

        roadWay.SetEntryTriggerAction(() => SetCurrentRoadWay(roadWay));
        roadWay.index = index;
        activeRoadWays.Add(roadWay);
        return roadWay;
    }


    public void SetCurrentRoadWay(RoadWay roadWay)
    {
        currentRoad = roadWay;
        CreateNextRoadWay(currentRoad);
        onCurrentLinkChanged?.Invoke(currentRoad);

        ReleasePassedRoadWay();
    }

    public void CreateNextRoadWay(RoadWay previousRoadWay)
    {
        var startPoints = previousRoadWay.GetNextRoadWayPoints();
        //previousRoadWay.
        int randomIndex = UnityEngine.Random.Range(0, roadWayPrefabs.Count());
        var roadWays = new RoadWay[startPoints.Count];
        foreach (var trs in startPoints)
        {
            var roadWay = CreateRoadWay(previousRoadWay.index + 1, randomIndex);

            roadWay.transform.position = trs.position;
            roadWay.transform.rotation = Quaternion.Euler(0, trs.angle, 0);
        }
    }

    public void ReleasePassedRoadWay()
    {
        while (activeRoadWays.Count() != 0)
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

    private int[] GetSeperatedRandomNumber(int start, int end, int count)
    {
        var array = new int[count];
        array[0] = UnityEngine.Random.Range(start, end);
        for (int curr = 1; curr < count; curr++)
        {
            while (true)
            {
                int temp = UnityEngine.Random.Range(start, end);
                bool isUnique = true;
                for (int prev = 0; prev < curr; prev++)
                {
                    if (Mathf.Abs(array[prev] - temp) <= 1)
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    array[curr] = temp;
                    break;
                }
            }
        }
        Array.Sort(array);
        return array;
    }
}

