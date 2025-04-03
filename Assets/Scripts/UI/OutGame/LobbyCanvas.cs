using UnityEngine;

public class LobbyCanvas : DefaultCanvas
{
    public override DefaultCanvasType canvasType { get; protected set; } = DefaultCanvasType.Lobby;
    
    [SerializeField] private LobbyPanel lobbyPanel;
}