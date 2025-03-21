using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour
{
    public GameObject straightPrefab;
    public GameObject leftTurnPrefab;
    public GameObject rightTurnPrefab;
    public int maxSegments = 5;

    private List<GameObject> activeSegments = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnNextSegment();
        }

    }

    public void SpawnNextSegment()
    {
        GameObject prefabToSpawn;

        float rand = Random.value;
        if (rand < 0.1f)
            prefabToSpawn = leftTurnPrefab;
        else if (rand < 0.2f)
            prefabToSpawn = rightTurnPrefab;
        else
            prefabToSpawn = straightPrefab;

        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (activeSegments.Count > 0)
        {
            MapSegment lastSegment = activeSegments[^1].GetComponent<MapSegment>();

            if (lastSegment.exitPoint == null)
            {
                Debug.LogError($"ExitPoint가 {lastSegment.name}에서 설정되지 않음! 인스펙터에서 연결하세요.");
                return;
            }

            spawnPosition = lastSegment.exitPoint.position;
            spawnPosition.y = -1f;

            spawnRotation = lastSegment.exitPoint.rotation;
        }
        else
        {
            spawnPosition = Vector3.zero;
            spawnPosition.y = -1f;
            spawnRotation = Quaternion.identity;
        }

        GameObject newSegment = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);
        activeSegments.Add(newSegment);

        MapSegment segmentComponent = newSegment.GetComponent<MapSegment>();
        if (segmentComponent != null)
        {
            segmentComponent.mapSpawn = this;
        }

        if (activeSegments.Count > maxSegments)
        {
            RemoveOldestSegment();
        }
    }

    public void RemoveOldestSegment()
    {
        if (activeSegments.Count > 0)
        {
            Destroy(activeSegments[0]);
            activeSegments.RemoveAt(0);
        }
    }

}
