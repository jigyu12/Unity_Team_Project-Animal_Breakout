using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutGameUIManager : MonoBehaviour
{
    public static Action<bool> onSwitchActiveLayoutGroupControllers;
    public static Action<bool> onSwitchActiveDefaultCanvases;
    public static Action<SwitchableCanvasType> onSwitchActiveSwitchableCanvas;
    public static Action<SwitchableCanvasType, bool, bool> onSwitchVisualizeSwitchableCanvas;
    
    public static Action<LevelInfoData> onLevelExpInitialized;
    public static Action<int> onExpChanged;
    
    // Level TempCode //
#if UNITY_EDITOR
        
    public static readonly Dictionary<int, int> expToLevelUpDictionary = new();    
    public readonly int maxLevel = 5;
    private static bool isAddToDict;
        
#endif
    // Level TempCode //
    
    private void Start()
    {
        StartCoroutine(DisableAfterFrameAllLayoutGroup(SwitchableCanvasType.Lobby));
        
        // Level TempCode //
#if UNITY_EDITOR
        
        if (!isAddToDict)
        {
            isAddToDict = true;
            
            expToLevelUpDictionary.Add(1, 110);
            expToLevelUpDictionary.Add(2, 120);
            expToLevelUpDictionary.Add(3, 130);
            expToLevelUpDictionary.Add(4, 140);
            expToLevelUpDictionary.Add(5, 150);
        }

        LevelInfoData initialData = new(maxLevel);
        int level = 1;
        initialData.SaveLevelInfoData(level, expToLevelUpDictionary[level], 0);
        
        onLevelExpInitialized?.Invoke(initialData);

#endif
        // Level TempCode //
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