using UnityEngine;

public class GhostCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
        
        self.SetActive(false);
    }
}

public class PoisonCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
        
        self.SetActive(false);
    }
}

public class SkullCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
        
        self.SetActive(false);
    }
}

public class FireCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
        
        self.SetActive(false);
    }
}

public class BlackHoleCoinCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
        
        self.SetActive(false);
    }
}