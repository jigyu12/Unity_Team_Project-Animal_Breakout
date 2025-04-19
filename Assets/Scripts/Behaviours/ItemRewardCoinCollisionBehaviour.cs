using UnityEngine;

public class BronzeCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
    }
}

public class SilverCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
    }
}

public class GoldCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
    }
}

public class PlatinumCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
    }
}

public class DiamondCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
    }
}