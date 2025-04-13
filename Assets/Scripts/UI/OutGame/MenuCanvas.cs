using UnityEngine;

public class MenuCanvas : DefaultCanvas
{
    public override DefaultCanvasType defaultCanvasType { get; protected set; } = DefaultCanvasType.Menu;
    protected override CanvasGroup canvasGroup { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        
        LobbyPanel.onGameStartButtonClicked += GameStartButtonClickedHandler;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        LobbyPanel.onGameStartButtonClicked -= GameStartButtonClickedHandler;
    }

    private void GameStartButtonClickedHandler()
    {
        gameObject.SetActive(false);
    }
}