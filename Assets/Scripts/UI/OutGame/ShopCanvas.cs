using UnityEngine;

public class ShopCanvas : SwitchableCanvas
{
    public override DefaultCanvasType defaultCanvasType { get; protected set; } = DefaultCanvasType.Shop;
    protected override CanvasGroup canvasGroup { get; set; }

    public override SwitchableCanvasType switchableCanvasType { get; protected set; } = SwitchableCanvasType.Shop;
}