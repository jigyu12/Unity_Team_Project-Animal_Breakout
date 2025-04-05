using UnityEngine;

public class LobbyCanvas : SwitchableCanvas
{
    public override DefaultCanvasType defaultCanvasType { get; protected set; } = DefaultCanvasType.Lobby;
    protected override CanvasGroup canvasGroup { get; set; }

    public override SwitchableCanvasType switchableCanvasType { get; protected set; } = SwitchableCanvasType.Lobby;
}