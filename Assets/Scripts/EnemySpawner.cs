using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public MapWay way;

    public float coolTime;

    private void OnEnable()
    {
        InvokeRepeating("SpawnRandomIndex", 5f, coolTime);
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
}
