using UnityEngine;

public class GhostCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with GhostCoin");
    }
}

public class PoisonCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with PoisonCoin");
    }
}

public class SkullCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with SkullCoin");
    }
}

public class FireCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with FireCoin");
    }
}

public class BlackHoleCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with BlackHoleCoin");
    }
}