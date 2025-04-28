using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BossBehaviourController))]
public class BossProjectilePooler : MonoBehaviour
{
    protected GameManager_new gameManager;
    
    protected BossBehaviourController bossBehaviourController;
    
    [SerializeField] protected List<GameObject> bossProjectilePrefabs;
    
    protected readonly List<ObjectPool<GameObject>> bossProjectilePools = new();

    protected List<GameObject> pooledProjectiles = new();
    
    public int ProjectileCount => bossProjectilePools.Count;
    
    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);
        
        TryGetComponent(out bossBehaviourController);

        for (int i = 0; i < bossProjectilePrefabs.Count; ++i)
        {
            int index = i;
            
            ObjectPool<GameObject> bossProjectilePool = gameManager.ObjectPoolManager.CreateObjectPool(bossProjectilePrefabs[index],
                () => Instantiate(bossProjectilePrefabs[index]),
                obj => { obj.SetActive(true); },
                obj => { obj.SetActive(false); });
            
            bossProjectilePools.Add(bossProjectilePool);
        }
    }
    
    public BossProjectile GetBossProjectile(int index)
    {
        if (index < 0 || index >= bossProjectilePools.Count)
        {
            Debug.Assert(false, $"BossProjectilePooler: index {index} is out of range.");
            
            return null;
        }
        
        var bossProjectile = bossProjectilePools[index].Get();
        
        pooledProjectiles.Add(bossProjectile);
        
        bossProjectile.TryGetComponent(out BossProjectile bossProjectileComponent);
        bossProjectileComponent.SetPool(bossProjectilePools[index], pooledProjectiles);
        
        return bossProjectileComponent;
    }
    
    public void ClearPooledProjectiles()
    {
        foreach (var projectile in pooledProjectiles)
        {
            if (projectile != null)
            {
                projectile.TryGetComponent(out BossProjectile bossProjectile);
                
                bossProjectile.ReleaseToPool();
            }
        }
        
        pooledProjectiles.Clear();
    }
}