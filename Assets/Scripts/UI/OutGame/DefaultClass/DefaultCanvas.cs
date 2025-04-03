using UnityEngine;

public abstract class DefaultCanvas : MonoBehaviour
{
    public abstract DefaultCanvasType canvasType { get; protected set; }
    
    [SerializeField] private Canvas staticCanvas;
    [SerializeField] private Canvas dynamicCanvas;
}