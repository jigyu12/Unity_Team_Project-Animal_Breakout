using UnityEngine;

public class BronzeCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
        
        self.SetActive(false);
        
        OnCoinAcquired?.Invoke(1);
    }
}

public class SilverCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
        
        self.SetActive(false);
        
        OnCoinAcquired?.Invoke(1); 
    }
}

public class GoldCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
        
        self.SetActive(false);
        
        OnCoinAcquired?.Invoke(1);
    }
}

public class PlatinumCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
        
        self.SetActive(false);
    }
}

public class DiamondCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }
        
        self.SetActive(false);
    }
}