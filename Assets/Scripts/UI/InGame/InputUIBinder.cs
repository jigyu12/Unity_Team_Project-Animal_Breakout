using UnityEngine;
using UnityEngine.UI;

public class InputUIBinder : MonoBehaviour
{
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private RotateButtonController rotateButtonController;

    public void Bind(PlayerMove move)
    {
        leftButton.GetComponent<DirectionButton>().Initialize(move, leftButton, rotateButtonController, DirectionButton.Direction.Left);
        rightButton.GetComponent<DirectionButton>().Initialize(move, rightButton, rotateButtonController, DirectionButton.Direction.Right);
    }

    public void SetInteractable(bool interactable)
    {
        leftButton.interactable = interactable;
        rightButton.interactable = interactable;
    }
}
