using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public static Action<DefaultCanvasType> OnBottomButtonClicked;
    
    [SerializeField] private Button shopBottomButton;
    [SerializeField] private Button lobbyBottomButton;
    [SerializeField] private Button animalBottomButton;

    private void Start()
    {
        shopBottomButton.onClick.RemoveAllListeners();
        shopBottomButton.onClick.AddListener(() => OnBottomButtonClicked?.Invoke(DefaultCanvasType.Shop));
        
        lobbyBottomButton.onClick.RemoveAllListeners();
        lobbyBottomButton.onClick.AddListener(() => OnBottomButtonClicked?.Invoke(DefaultCanvasType.Lobby));
        
        animalBottomButton.onClick.RemoveAllListeners();
        animalBottomButton.onClick.AddListener(() => OnBottomButtonClicked?.Invoke(DefaultCanvasType.Animal));
    }
}