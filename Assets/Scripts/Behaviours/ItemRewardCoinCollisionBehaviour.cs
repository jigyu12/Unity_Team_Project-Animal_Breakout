using UnityEngine;

public class BronzeCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with BronzeCoin");
    }
}

public class SilverCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with SilverCoin");
    }
}

public class GoldCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with GoldCoin");
    }
}

public class PlatinumCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with PlatinumCoin");
    }
}

public class DiamondCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with DiamondCoin");
    }
}