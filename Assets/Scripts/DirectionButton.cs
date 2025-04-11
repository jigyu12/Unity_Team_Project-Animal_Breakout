using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirectionButton : MonoBehaviour, IPointerDownHandler
{
    public enum Direction { Left, Right }
    public Direction direction;

    private PlayerMove playerMove;
    private Button button;

    public void Initialize(PlayerMove move, Button button)
    {
        playerMove = move;
        this.button = button;
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
        }
    }
}
