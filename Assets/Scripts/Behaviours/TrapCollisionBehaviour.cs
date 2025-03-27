using UnityEngine;

public class BombCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Player Dead by Bomb");
    }
}

public class HoleCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Player Dead by Hole");
    }
}