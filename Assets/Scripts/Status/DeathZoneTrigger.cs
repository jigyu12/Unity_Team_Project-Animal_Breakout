using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneTrigger : MonoBehaviour
{
    private PlayerManager playerManager;
    public GameObject respawnPrefab;

    public void SetPlayerManager(PlayerManager manager)
    {
        playerManager = manager;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                Debug.Log("DeathZone");
                player.TakeDamage(1);
            }
        }
    }

}
