using UnityEngine;

public class AnimalCanvas : SwitchableCanvas
{
    public override DefaultCanvasType defaultCanvasType { get; protected set; } = DefaultCanvasType.Animal;
    protected override CanvasGroup canvasGroup { get; set; }
    
    public override SwitchableCanvasType switchableCanvasType { get; protected set; } = SwitchableCanvasType.Animal;
}