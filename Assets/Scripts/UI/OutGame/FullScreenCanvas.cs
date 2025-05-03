using UnityEngine;

public class FullScreenCanvas : DefaultCanvas
{
    public override DefaultCanvasType defaultCanvasType { get; protected set; } = DefaultCanvasType.FullScreen;
    protected override CanvasGroup canvasGroup { get; set; }
}