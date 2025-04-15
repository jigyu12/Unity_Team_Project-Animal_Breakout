using JetBrains.Annotations;
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
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                player.SetCanTurn(true, gameObject, allowedDirection);
            }
            if (status != null && status.IsInvincible)
            {
                AutoTurn(player);
            }
        }
    }
    private void AutoTurn(PlayerMove player)
    {
        switch (allowedDirection)
        {
            case TurnDirection.Left:
                player.TryAutoRotateLeft();
                break;
            case TurnDirection.Right:
                player.TryAutoRotateRight();
                break;
            case TurnDirection.Both:
                if (UnityEngine.Random.value < 0.5f)
                    player.TryAutoRotateLeft();
                else
                    player.TryAutoRotateRight();
                break;
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

    private void UpdateLaneRangeRect()
    {

    }
}

