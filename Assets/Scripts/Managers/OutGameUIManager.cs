using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class OutGameUIManager : MonoBehaviour, IManager
{
    private OutGameManager outGameManager;
    
    public static event Action<bool> onSwitchActiveLayoutGroupControllers;
    public static event Action<bool> onSwitchActiveDefaultCanvases;
    public static event Action<SwitchableCanvasType> onSwitchActiveSwitchableCanvas;
    public static event Action<SwitchableCanvasType, bool, bool> onSwitchVisualizeSwitchableCanvas;
    
    [SerializeField] private GameObject unlockAnimalPanelPrefab;
    private ObjectPool<GameObject> unlockAnimalPanelPool;
    public static event Action<GameObject> onAnimalUnlockPanelInstantiated;
    private readonly List<GameObject> animalUnlockPanelList = new();

    public SwitchableCanvasType CurrentSwitchableCanvasType { get; private set; }

    private void Start()
    {
        StartCoroutine(DisableAfterFrameAllLayoutGroup(SwitchableCanvasType.Lobby));

        CurrentSwitchableCanvasType = SwitchableCanvasType.Lobby;
        
        // TempCode //
        
        unlockAnimalPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(unlockAnimalPanelPrefab,
            () => Instantiate(unlockAnimalPanelPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        var animalIdList = DataTableManager.animalDataTable.GetAnimalIDs();
        for (int i = 0; i < animalIdList.Count; ++i)
        {
            var unlockAnimalPanel = unlockAnimalPanelPool.Get();
            unlockAnimalPanel.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanel);
            animalUnlockPanel.SetAnimalStatData(DataTableManager.animalDataTable.Get(animalIdList[i]));
            onAnimalUnlockPanelInstantiated?.Invoke(unlockAnimalPanel);
            unlockAnimalPanel.transform.localScale = Vector3.one;
            animalUnlockPanelList.Add(unlockAnimalPanel);
        }   
        
        // TempCode //
    }
    
    public void Initialize()
    {
        GameDataManager.Instance.Initialize();
    }

    public void Clear()
    {
        
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
    
    public void SwitchActiveSwitchableCanvas(SwitchableCanvasType type)
    {
        onSwitchActiveSwitchableCanvas?.Invoke(type);
        
        CurrentSwitchableCanvasType = type;
    }
    
    private void SwitchActiveDefaultCanvas(bool isActive)
    {
        onSwitchActiveDefaultCanvases?.Invoke(isActive);
    }

    private void SwitchActiveLayoutGroupController(bool isActive)
    { 
        onSwitchActiveLayoutGroupControllers?.Invoke(isActive);
    }

    public void SetOutGameManager(OutGameManager outGameManager)
    {
        this.outGameManager = outGameManager;
    }
}