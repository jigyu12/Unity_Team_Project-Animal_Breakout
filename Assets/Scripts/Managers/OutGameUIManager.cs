using System;
using System.Collections;
using UnityEngine;

public class OutGameUIManager : MonoBehaviour
{
    public static Action<bool> onSwitchActiveLayoutGroupControllers;
    public static Action<bool> onSwitchActiveDefaultCanvases;
    public static Action<SwitchableCanvasType> onSwitchActiveSwitchableCanvas;
    public static Action<SwitchableCanvasType, bool, bool> onSwitchVisualizeSwitchableCanvas;

    private void Start()
    {
        StartCoroutine(DisableAfterFrameAllLayoutGroup(SwitchableCanvasType.Lobby));
    }
    
    public void EnableAllLayoutGroup(SwitchableCanvasType showCanvasType)
    {
        SwitchActiveDefaultCanvas(true);
        
        SwitchActiveLayoutGroupController(true);
        
        SwitchActiveSwitchableCanvas(showCanvasType);
    }
    
    public void DisableAllLayoutGroup(SwitchableCanvasType showCanvasType)
    {
        SwitchActiveDefaultCanvas(true);
        
        SwitchActiveLayoutGroupController(false);
        
        SwitchActiveSwitchableCanvas(showCanvasType);
    }
    
    public IEnumerator DisableAfterFrameAllLayoutGroup(SwitchableCanvasType showCanvasType)
    {
        SwitchActiveDefaultCanvas(true);
        
        SwitchVisualizeSwitchableCanvas(showCanvasType, false);
        
        yield return null;

        SwitchActiveLayoutGroupController(false);

        SwitchVisualizeSwitchableCanvas(showCanvasType, true);
        
        SwitchActiveSwitchableCanvas(showCanvasType);
    }

    private void SwitchVisualizeSwitchableCanvas(SwitchableCanvasType showCanvasType, bool isVisibleOtherCanvas, bool isVisibleShowCanvasType = true)
    {
        onSwitchVisualizeSwitchableCanvas?.Invoke(showCanvasType, isVisibleOtherCanvas, isVisibleShowCanvasType);
    }
    
    private void SwitchActiveSwitchableCanvas(SwitchableCanvasType type)
    {
        onSwitchActiveSwitchableCanvas?.Invoke(type);
    }
    
    private void SwitchActiveDefaultCanvas(bool isActive)
    {
        onSwitchActiveDefaultCanvases?.Invoke(isActive);
    }

    private void SwitchActiveLayoutGroupController(bool isActive)
    { 
        onSwitchActiveLayoutGroupControllers?.Invoke(isActive);
    }
}