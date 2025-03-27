using UnityEngine;

public abstract class BaseCollisionBehaviour : ICollisionBehaviour
{
    public void OnCollision(GameObject self, Collider other)
    {
        OnCollisionAction(self, other);
        
        UnityEngine.Object.Destroy(self);
    }

    protected abstract void OnCollisionAction(GameObject self, Collider other);
}