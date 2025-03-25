using UnityEngine;

public interface ICollisionBehaviour
{
    void OnCollision(GameObject self, Collider other);
}

public abstract class BaseCollisionBehaviour : ICollisionBehaviour
{
    public void OnCollision(GameObject self, Collider other)
    {
        OnCollisionAction(self, other);
        
        UnityEngine.Object.Destroy(self);
    }

    protected abstract void OnCollisionAction(GameObject self, Collider other);
}

public class BombCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        
    }
}

public class HoleCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        
    }
}