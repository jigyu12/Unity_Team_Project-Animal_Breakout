using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RotateButtonUI : UIElement
{
    [SerializeField] private Button rotateButton;
    private RotateButtonController controller;

    public void Initialize(RotateButtonController controller)
    {
        this.controller = controller;
        rotateButton.onClick.RemoveAllListeners();
        rotateButton.onClick.AddListener(() =>
        {
            controller.OnRotateButtonClicked();
            Hide();
        });
    }

    public void SetInteractable(bool value) => rotateButton.interactable = value;
    public void Show() => rotateButton.gameObject.SetActive(true);
    public void Hide() => rotateButton.gameObject.SetActive(false);
}
