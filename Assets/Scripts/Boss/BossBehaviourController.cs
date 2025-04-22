using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BossStatus))]
public class BossBehaviourController : MonoBehaviour
{
    private GameManager_new gameManager;
    
    private GameObject playerRoot;
    
    [SerializeField] private GameObject tempBossProjectilePrefab;
    
    private Lane lane;
    public Lane Lane { get; private set; }
    
    private Vector3 localDirectionToPlayer;
    public Vector3 LocalDirectionToPlayer { get; private set; }
    
    private ObjectPool<GameObject> tempBossProjectilePool;
    public ObjectPool<GameObject> TempBossProjectilePool { get; private set; }

    private GameObject projectileReleaseParent;
    public GameObject ProjectileReleaseParent { get; private set; }
    
    private readonly List<GameObject> tempBossProjectileList = new();
    public List<GameObject> TempBossProjectileList { get; private set; }
    
    private BossStatus bossStatus;
    public BossStatus BossStatus { get; private set; }
    
    public int PatternUseCount { get; private set; }
    
    public float BossPatternSelectRandomValue { get; private set; }
    
    private BehaviorTree<BossBehaviourController> behaviorTree;
    
    //private Vector3 attackSpawnLocalPosition;
    
    //private bool isAttacked;
    
    //private WaitForSeconds attackWaitTime = new(3f);
    
    private void Start()
    {
        playerRoot = GameObject.FindGameObjectWithTag("PlayerParent");
        playerRoot.TryGetComponent(out lane);
        Lane = lane;
        
        localDirectionToPlayer = (playerRoot.transform.position - transform.position).normalized;
        LocalDirectionToPlayer = localDirectionToPlayer;
        
        transform.localPosition = BossManager.spawnLocalPosition;
        
        // attackSpawnLocalPosition = new Vector3(
        //     BossManager.spawnLocalPosition.x, 
        //     BossManager.spawnLocalPosition.y,
        //     BossManager.spawnLocalPosition.z - 1f);

        //isAttacked = false;
        
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);
        tempBossProjectilePool = gameManager.ObjectPoolManager.CreateObjectPool(tempBossProjectilePrefab,
            () => Instantiate(tempBossProjectilePrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
        TempBossProjectilePool = tempBossProjectilePool;

        projectileReleaseParent = GameObject.FindGameObjectWithTag("ProjectileParent");
        ProjectileReleaseParent = projectileReleaseParent;
        
        TryGetComponent(out bossStatus);
        BossStatus = bossStatus;

        PatternUseCount = 0;
        SetBossPatternSelectRandomValue();

        TempBossProjectileList = tempBossProjectileList;
    }

    private void OnDestroy()
    {
        foreach (var tempBossProjectile in tempBossProjectileList)
        {
            if (tempBossProjectile != null)
            {
                continue;
            }
            
            tempBossProjectile.transform.SetParent(projectileReleaseParent.transform);
            tempBossProjectilePool.Release(tempBossProjectile);
        }
    }
    
    private void Update()
    {
        if (behaviorTree is not null)
        {
            behaviorTree.Update();
        }

        // if (!isAttacked)
        // {
        //     StartCoroutine(TestAttack());
        // }
    }

    public void InitBehaviorTree(BossBehaviourTreeType bossBehaviourTreeType)
    {
        behaviorTree = BossBehaviourTreeFactory.GetBossBehaviorTree(this, bossBehaviourTreeType);
    }

    public void AddPatternUseCount()
    {
        ++PatternUseCount;
    }

    public void ClearPatternUseCount()
    {
        PatternUseCount = 0;
    }

    public void SetBossPatternSelectRandomValue()
    {
        BossPatternSelectRandomValue = Random.value;
    }

    // private IEnumerator TestAttack()
    // {
    //     isAttacked = true;
    //
    //     // temp code //
    //     
    //     //TryGetComponent(out BossStatus bossStatus); 
    //     //bossStatus.OnDamage(20f);
    //     
    //     // temp code //
    //     
    //     Vector3 attackPosition = lane.LaneIndexToPosition(Random.Range(0, 3));
    //     var tempBossProjectile = tempBossProjectilePool.Get();
    //     tempBossProjectile.TryGetComponent(out TempBossProjectile tempBossProjectileComponent);
    //     tempBossProjectile.transform.SetParent(transform);
    //     tempBossProjectileComponent.Initialize(attackPosition, localDirectionToPlayer, 5f, tempBossProjectilePool, tempBossProjectileList, projectileReleaseParent.transform);
    //     tempBossProjectileList.Add(tempBossProjectile);
    //     
    //     yield return attackWaitTime;
    //
    //     isAttacked = false;
    // }
}