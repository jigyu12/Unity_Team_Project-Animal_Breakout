using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BossProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    private float projectileLifeTimeDelay = 5f;
    private float projectileLifeTimer;
    
    private Transform releaseParent;
    private List<GameObject> pooledProjectileList;
    private ObjectPool<GameObject> projectilePool;
    
    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        
        projectileLifeTimer += Time.deltaTime;
        if (projectileLifeTimer >= projectileLifeTimeDelay)
        {
            Release();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.TryGetComponent(out PlayerStatus playerStatus);
            playerStatus.TakeDamage(1);

            Release();
        }
    }
    
    public void Initialize(Vector3 position, Vector3 direction, float speed, Transform releaseParent)
    {
        transform.localPosition = position;
        
        this.direction = direction;
        this.speed = speed;

        this.releaseParent = releaseParent;
        
        projectileLifeTimer = 0f;
    }

    public void SetPool(ObjectPool<GameObject> projectilePool, List<GameObject> pooledProjectileList)
    {
        this.pooledProjectileList = pooledProjectileList;
        this.projectilePool = projectilePool;
    }

    public void Release()
    {
        RemoveFromPooledList();
        ReleaseToPool();
    }
    
    public void ReleaseToPool()
    {
        transform.SetParent(releaseParent);
        projectilePool.Release(gameObject);
    }

    private void RemoveFromPooledList()
    {
        pooledProjectileList.Remove(gameObject);
    }
}