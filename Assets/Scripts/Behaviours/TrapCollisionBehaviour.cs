using UnityEngine;

public class BombCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Player Dead by Bomb");

        if (other.CompareTag("Player"))
        {
            PlayerStatus1 playerStatus = other.GetComponent<PlayerStatus1>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(1);
            }
        }
    }
}

public class HoleCollisionBehaviour : BaseCollisionBehaviour
{
    protected override void OnCollisionAction(GameObject self, Collider other)
    {
        Debug.Log("Player Dead by Hole");

        if (other.CompareTag("Player"))
        {
            PlayerStatus1 playerStatus = other.GetComponent<PlayerStatus1>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(1);
            }
        }
    }
}