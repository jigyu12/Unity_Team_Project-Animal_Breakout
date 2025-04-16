using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TempBossProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private ObjectPool<GameObject> projectilePool;

    private float projectileLifeTimeDelay = 5f;
    private float projectileLifeTimer;

    private List<GameObject> tempBossProjectileList;

    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        
        projectileLifeTimer += Time.deltaTime;
        if (projectileLifeTimer >= projectileLifeTimeDelay)
        {
            tempBossProjectileList.Remove(gameObject);
            projectilePool.Release(gameObject);
        }
    }
    
    public void Initialize(Vector3 position, Vector3 direction, float speed, ObjectPool<GameObject> projectilePool, List<GameObject> tempBossProjectileList)
    {
        transform.localPosition = position;
        
        this.direction = direction;
        this.speed = speed;
        this.projectilePool = projectilePool;

        projectileLifeTimer = 0f;

        this.tempBossProjectileList = tempBossProjectileList;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.TryGetComponent(out PlayerStatus playerStatus);
            playerStatus.TakeDamage(1);

            tempBossProjectileList.Remove(gameObject);
            projectilePool.Release(gameObject);
        }
    }
}