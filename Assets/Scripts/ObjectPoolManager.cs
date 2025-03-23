using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<GameObject, ObjectPool<GameObject>> pools = new();

    public ObjectPool<GameObject> CreateObjectPool(GameObject pooledObject, Func<GameObject> createFunc = null, Action<GameObject> actionOnGet = null)
    {
        ObjectPool<GameObject> pool = new
            (
                createFunc: createFunc ??= () => Instantiate(pooledObject), //null이면 기본
                actionOnGet: actionOnGet,
                //actionOnRelease: e =>,
                //actionOnDestroy: obj => obj.Dispose(),
                //collectionCheck: false,
                defaultCapacity: 100,
                maxSize: 100
            );

        pools.Add(pooledObject, pool);
        return pool;
    }

    public ObjectPool<GameObject> GetObjectPool(GameObject pooledObject)
    {
        if (pools.TryGetValue(pooledObject, out ObjectPool<GameObject> pool))
        {
            return pool;
        }
        else
        {
            throw new KeyNotFoundException($"No pool found for type {gameObject.name}.");
        }
    }
}
