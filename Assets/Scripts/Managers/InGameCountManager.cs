using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCountManager : InGameManager
{
    private PlayerMove playerMove;

    private void UpdateJumpCoount()
    {
        GameManager.PlayerManager.playerMove.isJumping = true;
    }
}
