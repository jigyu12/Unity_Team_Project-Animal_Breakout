using UnityEngine;

public class MenuCanvas : DefaultCanvas
{
    public override DefaultCanvasType defaultCanvasType { get; protected set; } = DefaultCanvasType.Menu;
    protected override CanvasGroup canvasGroup { get; set; }
}