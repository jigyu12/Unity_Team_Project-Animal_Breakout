using UnityEngine;

public abstract class CollidableMapObject : MonoBehaviour
{
    public abstract ObjectType ObjectType { get; protected set; }
    protected abstract ICollisionBehaviour CollisionBehaviour { get; set; }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utils.PlayerTag))
        {
            CollisionBehaviour.OnCollision(gameObject, other);
        }
    }
}