using UnityEngine;

public class JuniorResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
            SoundManager.Instance.PlaySfx(SfxClipId.RobotHit);
        }
    }
}

public class ResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }

    }
}

public class SeniorResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        //OnScoreChanged?.Invoke(scoreToAdd);

        var list = other.GetComponents<IItemTaker>();
        foreach (var taker in list)
        {
            taker.ApplyItem((int)scoreToAdd);
        }

    }
}