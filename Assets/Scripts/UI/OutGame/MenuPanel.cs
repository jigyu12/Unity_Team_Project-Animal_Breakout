using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public static event Action<SwitchableCanvasType> onMenuBottomButtonClicked;
    
    [SerializeField] private Button shopBottomButton;
    [SerializeField] private Button lobbyBottomButton;
    [SerializeField] private Button animalBottomButton;

    private void Start()
    {
        shopBottomButton.onClick.RemoveAllListeners();
        shopBottomButton.onClick.AddListener(() => onMenuBottomButtonClicked?.Invoke(SwitchableCanvasType.Shop));
        
        lobbyBottomButton.onClick.RemoveAllListeners();
        lobbyBottomButton.onClick.AddListener(() => onMenuBottomButtonClicked?.Invoke(SwitchableCanvasType.Lobby));
        
        animalBottomButton.onClick.RemoveAllListeners();
        animalBottomButton.onClick.AddListener(() => onMenuBottomButtonClicked?.Invoke(SwitchableCanvasType.Animal));
    }
}