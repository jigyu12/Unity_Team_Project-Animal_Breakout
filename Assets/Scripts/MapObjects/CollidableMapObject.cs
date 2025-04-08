using UnityEngine;
using UnityEngine.Pool;

public abstract class CollidableMapObject : MonoBehaviour
{
    // Temporary Code //
    public GameObject player;
    public void Start()
    {
        player = GameObject.Find(Utils.PlayerRootName);
    }

    //private void Update()
    //{
    //    if (player != null && transform.position.z < player.transform.position.z - 2f)
    //    {
            
    //    }
    //}
    // Temporary Code //
    
    public abstract ObjectType ObjectType { get; protected set; }
    protected abstract BaseCollisionBehaviour CollisionBehaviour { get; set; }
    
    private ObjectPool<GameObject> pool;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utils.PlayerTag))
        {
            CollisionBehaviour.OnCollision(gameObject, other);
        }
    }

    public void SetPool(ObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void ReleasePool()
    {
        pool.Release(gameObject);
    }
}