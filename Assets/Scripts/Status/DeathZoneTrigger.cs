// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DeathZoneTrigger : MonoBehaviour
// {
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             PlayerStatus player = other.GetComponent<PlayerStatus>();
//             if (player != null)
//             {
//                 Debug.Log("DeathZone");
//                 player.TakeDamage(1);
//             }
//         }
//     }

// }
using UnityEngine;
public class DeathZoneTrigger : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Start()
    {
        // 인게임 매니저 구조 기반으로 GameManager에서 직접 접근
        playerManager = GameObject.FindWithTag("GameManager")
            .GetComponent<GameManager_new>().PlayerManager;

        if (playerManager == null)
            Debug.LogError("PlayerManager를 찾을 수 없습니다!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                Debug.Log("DeathZone Triggered");

                if (transform.childCount > 0)
                {
                    var respawnPoint = transform.GetChild(0);
                    playerManager.SetPendingRespawnInfo(respawnPoint.position, respawnPoint.rotation, respawnPoint.forward);
                }

                player.TakeDamage(1);
            }
        }
    }
}

