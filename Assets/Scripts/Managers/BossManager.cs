using System;
using UnityEngine;
using UnityEngine.Pool;

public class BossManager : InGameManager
{
    [SerializeField] private GameObject bossPrefab;
    private BossStatus bossStatus;
    private ObjectPool<GameObject> bossPool;
    
    [SerializeField] private GameObject parentGameObjectToSpawnBoss;
    private readonly Vector3 spawnLocalPosition = new Vector3(0f, 1f, 15f);
    
    public static event Action<GameObject> onSpawnBoss;
    
    private void Start()
    {
        bossPool = GameManager.ObjectPoolManager.CreateObjectPool(bossPrefab,
            () => Instantiate(bossPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
        
        SpawnBoss();
    }

    public override void Initialize()
    {
        base.Initialize();
        
        
    }

    public override void Clear()
    {
        base.Clear();
        
        
    }

    private void SpawnBoss()
    {
        var boss = bossPool.Get();
        boss.transform.SetParent(parentGameObjectToSpawnBoss.transform);
        boss.transform.localPosition = spawnLocalPosition;
        boss.TryGetComponent(out bossStatus);
        bossStatus.InitializeStatus(100f);
        bossStatus.SetPool(bossPool);
        onSpawnBoss?.Invoke(boss);
    }
}