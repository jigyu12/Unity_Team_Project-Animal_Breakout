using UnityEngine;

public class JuniorResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with Junior Researcher");
    }
}

public class ResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with Researcher");
    }
}

public class SeniorResearcherCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Collision with Senior Researcher");
    }
}