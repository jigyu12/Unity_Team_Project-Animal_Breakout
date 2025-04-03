using UnityEngine;

public class MenuCanvas : DefaultCanvas
{
    public override DefaultCanvasType canvasType { get; protected set; } = DefaultCanvasType.Menu;
    
    [SerializeField] private MenuPanel menuPanel;
}