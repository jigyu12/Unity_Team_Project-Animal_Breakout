using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BossBehaviourController : MonoBehaviour
{
    private GameManager_new gameManager;
    
    private GameObject playerRoot;
    private Lane lane;
    
    private Vector3 attackSpawnLocalPosition;
    private Vector3 localDirectionToPlayer;
    
    private bool isAttacked;
    
    private WaitForSeconds attackWaitTime = new(3f);
    
    [SerializeField] private GameObject tempBossProjectilePrefab;
    private ObjectPool<GameObject> tempBossProjectilePool;
    
    private void Start()
    {
        playerRoot = GameObject.FindGameObjectWithTag("PlayerParent");
        playerRoot.TryGetComponent(out lane);
        
        localDirectionToPlayer = (playerRoot.transform.position - transform.position).normalized;
        
        transform.localPosition = BossManager.spawnLocalPosition;
        
        attackSpawnLocalPosition = new Vector3(
            BossManager.spawnLocalPosition.x, 
            BossManager.spawnLocalPosition.y,
            BossManager.spawnLocalPosition.z - 1f);

        isAttacked = false;
        
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);
        tempBossProjectilePool = gameManager.ObjectPoolManager.CreateObjectPool(tempBossProjectilePrefab,
            () => Instantiate(tempBossProjectilePrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
    }
    
    private void Update()
    {
        if (!isAttacked)
        {
            StartCoroutine(TestMove());
        }
    }

    private IEnumerator TestMove()
    {
        isAttacked = true;

        Vector3 attackPosition = lane.LaneIndexToPosition(Random.Range(0, 3));
        var tempBossProjectile = tempBossProjectilePool.Get();
        tempBossProjectile.TryGetComponent(out TempBossProjectile tempBossProjectileComponent);
        tempBossProjectile.transform.SetParent(transform);
        tempBossProjectileComponent.Initialize(attackPosition, localDirectionToPlayer, 5f, tempBossProjectilePool);
        
        yield return attackWaitTime;

        isAttacked = false;
    }
}