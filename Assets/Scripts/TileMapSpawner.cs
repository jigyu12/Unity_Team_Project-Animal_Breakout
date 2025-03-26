using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class TileMapSpawner : MonoBehaviour
{
    public GameObject verticalUnit;
    public GameObject leftTurnUnit;
    public GameObject rightTurnUnit;

    private ObjectPool<GameObject> verticalUnitPool;
    private ObjectPool<GameObject> leftTurnUnitPool;
    private ObjectPool<GameObject> rightTurnUnitPool;

    [SerializeField]
    private PlayerMove player;

    private MapRotator mapRotator;

    public int maxAvoidChance;
    private int avoidCount;

    private List<MapUnit> activeMapUnits = new();

    private void Start()
    {
        verticalUnitPool = ObjectPoolManager.Instance.CreateObjectPool(verticalUnit, () => CreateMapUnit(verticalUnit), OnGetMapUnit, OnReleaseMapUnit);
        leftTurnUnitPool = ObjectPoolManager.Instance.CreateObjectPool(leftTurnUnit, () => CreateMapUnit(leftTurnUnit), OnGetMapUnit, OnReleaseMapUnit);
        rightTurnUnitPool = ObjectPoolManager.Instance.CreateObjectPool(rightTurnUnit, () => CreateMapUnit(rightTurnUnit), OnGetMapUnit, OnReleaseMapUnit);

        var information = new TileChuckInformation(null, 10, true, false, 3);

        mapRotator=GetComponent<MapRotator>();
        CreateTileChuck(information);
    }

    private void Update()
    {
        Queue<MapUnit> releaseQueue = new();

        foreach (var unit in activeMapUnits)
        {
            if (unit.transform.position.z + 60f < player.transform.position.z)
            {
                releaseQueue.Enqueue(unit);
            }
        }

        while (releaseQueue.Count > 0)
        {
            var unit = releaseQueue.Dequeue();
            activeMapUnits.Remove(unit);
            switch (unit.directionType)
            {
                case WayType.Straight:
                    verticalUnitPool.Release(unit.gameObject);
                    break;
                case WayType.Left:
                    leftTurnUnitPool.Release(unit.gameObject);
                    break;
                case WayType.Right:
                    rightTurnUnitPool.Release(unit.gameObject);
                    break;
            }
        }
    }

    private GameObject CreateMapUnit(GameObject mapUnit)
    {
        var unit = Instantiate(mapUnit, gameObject.transform);
        unit.GetComponent<MapUnit>().mapSpawner = this;
        return unit;
    }


    private void OnGetMapUnit(GameObject mapUnit)
    {
        mapUnit.GetComponent<MapUnit>().Reset();
        mapUnit.transform.rotation = Quaternion.identity;
        mapUnit.gameObject.SetActive(true);
    }
    private void OnReleaseMapUnit(GameObject mapUnit)
    {
        mapUnit.gameObject.SetActive(false);
    }

    public void SpawnNextTileChuck(MapUnit lastUnit)
    {
        var information = new TileChuckInformation(lastUnit, 10, true, false, 8);
        CreateTileChuck(information);
    }

    public struct TileChuckInformation
    {
        public TileChuckInformation(MapUnit prev, int unitLength, bool left, bool right, int turnIndex = -1)
        {
            previousTileUnit = prev;
            this.unitLength = unitLength;
            this.isLeftWayExist = left;
            this.isRightWayExist = right;
            this.turnIndex = (left || right ? (turnIndex == -1 ? turnIndex = unitLength - 1 : turnIndex) : -1);
        }

        public MapUnit previousTileUnit;

        public int unitLength;
        public int turnIndex;
        public bool isLeftWayExist;
        public bool isRightWayExist;
    }

    public void CreateTileChuck(TileChuckInformation information)
    {
        var currUnit = verticalUnitPool.Get().GetComponent<MapUnit>();
        currUnit.transform.position = information.previousTileUnit?.NextPosition ?? Vector3.zero;
        currUnit.transform.localRotation= Quaternion.Euler(0, -mapRotator.currentYRotation, 0);
        activeMapUnits.Add(currUnit);
        for (int i = 1; i < information.unitLength; i++)
        {
            var nextUnit = verticalUnitPool.Get().GetComponent<MapUnit>();
            nextUnit.transform.position = currUnit.NextPosition;
            nextUnit.transform.transform.localRotation = Quaternion.Euler(0, -mapRotator.currentYRotation, 0);
            activeMapUnits.Add(nextUnit);
            if (i == information.turnIndex)
            {
                if (information.isLeftWayExist)
                {
                    var leftUnit = leftTurnUnitPool.Get().GetComponent<MapUnit>();
                    activeMapUnits.Add(leftUnit);
                    leftUnit.transform.position = currUnit.NextLeftPosition;
                    leftUnit.transform.transform.localRotation = Quaternion.Euler(0, -mapRotator.currentYRotation, 0);
                    leftUnit.SetNextUnitSpawnTriggerOn();
                }

                if (information.isRightWayExist)
                {
                    var rightUnit = rightTurnUnitPool.Get().GetComponent<MapUnit>();
                    activeMapUnits.Add(rightUnit);
                    rightUnit.transform.position = currUnit.NextRightPosition;
                    rightUnit.transform.transform.localRotation = Quaternion.Euler(0, -mapRotator.currentYRotation, 0);
                    rightUnit.SetNextUnitSpawnTriggerOn();
                }
            }
            currUnit = nextUnit;
        }
        currUnit.SetNextUnitSpawnTriggerOn();
    }
}
