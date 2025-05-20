using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class SwipeTurnTrigger : MonoBehaviour
{
    public TurnDirection allowedDirection;

    private GameUIManager gameUIManager;

    private void Start()
    {
        var GameManager = GameObject.FindGameObjectWithTag(Utils.GameManagerTag);
        var GameManager_new = GameManager.GetComponent<GameManager_new>();
        gameUIManager = GameManager_new.UIManager;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameUIManager.ShowRotateButton();
            PlayerMove player = other.GetComponent<PlayerMove>();
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            RotateButtonController rotateButtonController = gameUIManager.rotateButtonController;
            rotateButtonController.SetCurrentTurnTrigger(this);
            if (player != null)
            {
                player.SetCanTurn(true, gameObject, allowedDirection);

            }
            // if (status != null && status.IsInvincible)
            // {
            //     gameUIManager.UnShowRotateButton();
            //     AutoTurn(player);
            // }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            RotateButtonController rotateButtonController = gameUIManager.rotateButtonController;
            if (status != null && status.IsInvincible)
            {
                gameUIManager.UnShowRotateButton();
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
    public void ForceAutoTurnIfInside(GameObject playerObj)
    {
        if (playerObj.CompareTag("Player"))
        {
            var player = playerObj.GetComponent<PlayerMove>();
            var status = playerObj.GetComponent<PlayerStatus>();

            if (player != null && status != null && status.IsInvincible)
            {
                AutoTurn(player);
                gameUIManager.UnShowRotateButton();
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
                gameUIManager.UnShowRotateButton();
            }
        }
    }

    private void UpdateLaneRangeRect()
    {

    }
}

