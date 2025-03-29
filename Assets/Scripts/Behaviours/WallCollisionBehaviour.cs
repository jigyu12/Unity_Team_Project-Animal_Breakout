using UnityEngine;

public class NormalWallCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with Normal Wall");
    }
}

public class ReinforcedWallCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with Reinforced Wall");
    }
}