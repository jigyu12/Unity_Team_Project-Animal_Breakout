using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwipeTurnTrigger : MonoBehaviour
{
    public TurnDirection allowedDirection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.SetCanTurn(true, gameObject, allowedDirection);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.SetCanTurn(false, gameObject, allowedDirection);
            }
        }
    }
}

