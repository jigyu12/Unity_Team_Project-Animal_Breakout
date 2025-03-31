using UnityEngine;

public class BombCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
               // playerStatus.TakeDamage(1);
            }
        }
    }
}

public class HoleCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                //playerStatus.TakeDamage(1);
            }
        }
    }
}