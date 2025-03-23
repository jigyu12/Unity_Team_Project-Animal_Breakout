using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public MapWay way;
    
    public List<GameObject> objects;

    public float coolTime;

    private void OnEnable()
    {
        //InvokeRepeating("SpawnRandomIndex", 5f, coolTime);
        InvokeRepeating("SpawnRandomObject", 5f, coolTime);
    }

    public void SpawnRandomIndex()
    {
        int index = Random.Range(0, 2);
        SpawnEnemy(index);
    }

    public void SpawnEnemy(int index)
    {
        var enemy = Instantiate(enemyPrefab);
        var position = way.WayIndexToPosition(index);
        position.z = 100f;
        enemy.transform.position = position;
    }

    public void SpawnRandomObject()
    {
        var mapObject = objects[Random.Range(0, objects.Count)];
        var position = way.WayIndexToPosition(Random.Range(0, 2));
        position.z = 100f;
        Instantiate(mapObject, position, Quaternion.identity);
    }
}
