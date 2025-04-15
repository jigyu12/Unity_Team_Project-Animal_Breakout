using System;
using UnityEngine;
using UnityEngine.Pool;

public class TempBossProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private ObjectPool<GameObject> projectilePool;

    private float projectileLifeTimeDelay = 5f;
    private float projectileLifeTimer;

    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        
        projectileLifeTimer += Time.deltaTime;
        if (projectileLifeTimer >= projectileLifeTimeDelay)
        {
            projectilePool.Release(gameObject);
        }
    }
    
    public void Initialize(Vector3 position, Vector3 direction, float speed, ObjectPool<GameObject> projectilePool)
    {
        transform.localPosition = position;
        
        this.direction = direction;
        this.speed = speed;
        this.projectilePool = projectilePool;

        projectileLifeTimer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.TryGetComponent(out PlayerStatus playerStatus);
            playerStatus.TakeDamage(1);
            projectilePool.Release(gameObject);
        }
    }
}