using UnityEngine;

public class JuniorResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
    }
}

public class ResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
    }
}

public class SeniorResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        OnScoreChanged?.Invoke(scoreToAdd);
    }
}