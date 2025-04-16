using System;
using UnityEngine;

public class BossManager : InGameManager
{
    [SerializeField] private GameObject bossPrefab;
    private BossStatus bossStatus;
    //private ObjectPool<GameObject> bossPool;
    
    [SerializeField] private GameObject parentGameObjectToSpawnBoss;
    public static readonly Vector3 spawnLocalPosition = new Vector3(0f, 1f, 10f);
    
    private GameManager_new gameManager;
    
    public static event Action<BossStatus> onSpawnBoss;
    
    private void Start()
    {
        // bossPool = GameManager.ObjectPoolManager.CreateObjectPool(bossPrefab,
        //     () => Instantiate(bossPrefab),
        //     obj => { obj.SetActive(true); },
        //     obj => { obj.SetActive(false); });
        
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);
        GameManager.StageManager.onBossStageEnter += SpawnBoss;
    }

    private void OnDestroy()
    {
        gameManager.StageManager.onBossStageEnter -= SpawnBoss;
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
        //var boss = bossPool.Get();
        var boss = Instantiate(bossPrefab, parentGameObjectToSpawnBoss.transform);
        boss.transform.localPosition = spawnLocalPosition;
        boss.TryGetComponent(out bossStatus);
        bossStatus.InitializeStatus(100f);

        //bossStatus.SetPool(bossPool);
        onSpawnBoss?.Invoke(bossStatus);
    }
}