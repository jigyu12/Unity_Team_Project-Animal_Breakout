using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirectionButton : MonoBehaviour, IPointerDownHandler
{
    public enum Direction { Left, Right, Rotate }
    public Direction direction;

    private PlayerMove playerMove;
    private RotateButtonController rotateButtonController;

    private Button button;

    public void Initialize(PlayerMove move, Button button, RotateButtonController rotateController, Direction dir)
    {
        playerMove = move;
        this.button = button;
        rotateButtonController = rotateController;
        direction = dir;
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerMove == null) return;
        if (!button.interactable) return;

        switch (direction)
        {
            case Direction.Left:
                playerMove.MoveLeft();
                break;
            case Direction.Right:
                playerMove.MoveRight();
                break;
            case Direction.Rotate:
                rotateButtonController?.OnRotateButtonClicked();
                break;
        }
    }
}
