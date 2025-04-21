using UnityEngine;
using UnityEngine.UI;
public class RotateButtonController : MonoBehaviour
{
    private GameManager_new gameManager;
    private PlayerMove playerMove;
    private SwipeTurnTrigger currentTurnTrigger;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);
        playerMove = gameManager.PlayerManager.playerMove;

        // RotateButton.onClick.RemoveAllListeners();
        // RotateButton.onClick.AddListener(OnRotateButtonClicked);
    }
    public void OnRotateButtonClicked()
    {
        if (playerMove == null || currentTurnTrigger == null) return;

        switch (currentTurnTrigger.allowedDirection)
        {
            case TurnDirection.Left:
                playerMove.TryAutoRotateLeft();
                break;
            case TurnDirection.Right:
                playerMove.TryAutoRotateRight();
                break;
            case TurnDirection.Both:
                // 유저가 선택하거나 기본 왼쪽
                playerMove.TryAutoRotateLeft();
                break;
        }
    }
    
    public void SetCurrentTurnTrigger(SwipeTurnTrigger trigger)
    {
        currentTurnTrigger = trigger;
    }
}
