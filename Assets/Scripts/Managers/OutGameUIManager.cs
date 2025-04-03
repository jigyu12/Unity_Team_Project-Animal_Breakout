using System.Collections.Generic;
using UnityEngine;

public class OutGameUIManager : MonoBehaviour
{
    [SerializeField] private List<DefaultCanvas> switchableCanvasList;
    
    [SerializeField] private ShopCanvas shopCanvas;
    [SerializeField] private LobbyCanvas lobbyCanvas;
    [SerializeField] private AnimalCanvas animalCanvas;
    
    [SerializeField] private MenuCanvas menuCanvas;

    private void OnEnable()
    {
        MenuPanel.OnBottomButtonClicked += MenuCanvasBottomButtonClickedHandler;
    }

    private void OnDisable()
    {
        MenuPanel.OnBottomButtonClicked -= MenuCanvasBottomButtonClickedHandler;
    }

    private void MenuCanvasBottomButtonClickedHandler(DefaultCanvasType type)
    {
        for(int i = 0; i < switchableCanvasList.Count; ++i)
        {
            switchableCanvasList[i].gameObject.SetActive(i == (int)type);
        }
    }
}