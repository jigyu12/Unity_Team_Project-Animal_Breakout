using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class OutGameUIManager : MonoBehaviour, IManager
{
    private OutGameManager outGameManager;
    
    public static event Action<bool> onSwitchActiveLayoutGroupControllers;
    public static event Action<bool> onSwitchActiveAllDefaultCanvas;
    public static event Action<SwitchableCanvasType> onSwitchActiveSwitchableCanvas;
    public static event Action<SwitchableCanvasType, bool, bool> onSwitchVisualizeSwitchableCanvas;
    
    [SerializeField] private GameObject unlockAnimalPanelPrefab;
    private ObjectPool<GameObject> unlockAnimalPanelPool;
    public static event Action<GameObject> onAnimalUnlockPanelInstantiated;
    private readonly Dictionary<int, GameObject> animalUnlockPanelDictionary = new();
    
    [SerializeField] private GameObject lockAnimalPanelPrefab;
    private ObjectPool<GameObject> lockAnimalPanelPool;
    public static event Action<GameObject> onAnimalLockPanelInstantiated;
    private readonly Dictionary<int, GameObject> animalLockPanelDictionary = new();

    //대신 GameDataManager.Instance.AnimalUserDataList.AnimalUserDatas를 사용하세요
    //private readonly List<bool> animalIsUnlockInfoList = new();

    public SwitchableCanvasType CurrentSwitchableCanvasType { get; set; }
    public DefaultCanvasType CurrentDefaultCanvasTypeInSwitchableCanvasType { get; set; }

    [SerializeField] private GameObject alertPanelSpawnPanelRoot;
    [SerializeField] private GameObject alertPanelSpawnPanel;
    
    [SerializeField] private GameObject alertSingleButtonPanel;
    [SerializeField] private GameObject alertDoubleButtonPanel;
    private ObjectPool<GameObject> alertSingleButtonPanelPool;
    private ObjectPool<GameObject> alertDoubleButtonPanelPool;
    [SerializeField] private GameObject alertPanelReleaseParent;
    
    private readonly List<GameObject> alertSingleButtonPanelList = new();
    private readonly List<GameObject> alertDoubleButtonPanelList = new();
    
    public static event Action<DefaultCanvasType, bool, bool> onSwitchActiveDefaultCanvas;

    public static event Action onGachaScreenActive;
    
    [SerializeField] private GameObject gachaResultSlot;
    private ObjectPool<GameObject> gachaResultSlotPool;
    [SerializeField] private GameObject gachaResultSlotPanelReleaseParent;

    private void Start()
    {
        StartCoroutine(DisableAfterFrameAllLayoutGroup(SwitchableCanvasType.Lobby));

        CurrentSwitchableCanvasType = SwitchableCanvasType.Lobby;
        SetCurrentDefaultCanvasTypeInSwitchableCanvasType(CurrentSwitchableCanvasType);

        SwitchActiveDefaultCanvas(DefaultCanvasType.FullScreen, false);

        alertSingleButtonPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(alertSingleButtonPanel,
            () => Instantiate(alertSingleButtonPanel),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
        
        alertDoubleButtonPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(alertDoubleButtonPanel,
            () => Instantiate(alertDoubleButtonPanel),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
        
        gachaResultSlotPool = outGameManager.ObjectPoolManager.CreateObjectPool(gachaResultSlot,
            () => Instantiate(gachaResultSlot),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        GachaManager.onAnimalUnlocked += OnAnimalUnlockedHandler;
        
        // TempCode //
        
        unlockAnimalPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(unlockAnimalPanelPrefab,
            () => Instantiate(unlockAnimalPanelPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        lockAnimalPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(lockAnimalPanelPrefab,
            () => Instantiate(lockAnimalPanelPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        //var animalIdList = DataTableManager.animalDataTable.GetAnimalIDs();
        
        //for (int i = 0; i < animalIdList.Count; ++i)
        //{
        //    if (GameDataManager.Instance.startAnimalID == animalIdList[i])
        //    {
        //        animalIsUnlockInfoList.Add(true);
        //    }
        //    else
        //    {
        //        animalIsUnlockInfoList.Add(false);
        //    }
        //}
        
        foreach (var animalUserData in GameDataManager.Instance.AnimalUserDataList.AnimalUserDatas)
        {
            if (animalUserData.IsUnlock)
            {
                var unlockAnimalPanel = unlockAnimalPanelPool.Get();
                unlockAnimalPanel.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanel);
                animalUnlockPanel.SetAnimalUserData(animalUserData);
                onAnimalUnlockPanelInstantiated?.Invoke(unlockAnimalPanel);
                unlockAnimalPanel.transform.localScale = Vector3.one;
                animalUnlockPanelDictionary.Add(animalUnlockPanel.animalId, unlockAnimalPanel);
            }
            else
            {
                var lockAnimalPanel = lockAnimalPanelPool.Get();
                lockAnimalPanel.TryGetComponent(out LockedAnimalPanel animalLockPanel);
                animalLockPanel.SetAnimalUserData(animalUserData);
                onAnimalLockPanelInstantiated?.Invoke(lockAnimalPanel);
                lockAnimalPanel.transform.localScale = Vector3.one;
                animalLockPanelDictionary.Add(animalLockPanel.animalId, lockAnimalPanel);
            }
        }
        
        // TempCode //
    }
    
    private void OnDestroy()
    {
        GachaManager.onAnimalUnlocked -= OnAnimalUnlockedHandler;
    }
    
    public void Initialize()
    {
        
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

    public void SwitchVisualizeSwitchableCanvas(SwitchableCanvasType showCanvasType, bool isVisibleOtherCanvas, bool isVisibleShowCanvasType = true)
    {
        onSwitchVisualizeSwitchableCanvas?.Invoke(showCanvasType, isVisibleOtherCanvas, isVisibleShowCanvasType);
    }
    
    public void SwitchActiveSwitchableCanvas(SwitchableCanvasType type)
    {
        onSwitchActiveSwitchableCanvas?.Invoke(type);
    }
    
    private void SwitchActiveDefaultCanvas(bool isActive)
    {
        onSwitchActiveAllDefaultCanvas?.Invoke(isActive);
    }

    private void SwitchActiveLayoutGroupController(bool isActive)
    { 
        onSwitchActiveLayoutGroupControllers?.Invoke(isActive);
    }

    public void SetOutGameManager(OutGameManager outGameManager)
    {
        this.outGameManager = outGameManager;
    }

    public void ShowAlertSingleButtonPanel(AlertPanelInfoData alertPanelInfoData)
    {
        alertPanelSpawnPanelRoot.SetActive(true);

        var alertPanel = alertSingleButtonPanelPool.Get();
        alertPanel.transform.SetParent(alertPanelSpawnPanel.transform);
        alertPanel.transform.localPosition = Vector3.zero;
        
        alertPanel.TryGetComponent(out AlertPanel alertPanelComponent);
        alertPanelComponent.SetDescriptionTextAndButtonAction(alertPanelInfoData);
        
        alertSingleButtonPanelList.Add(alertPanel);
    }
    
    public void ShowAlertDoubleButtonPanel(AlertPanelInfoData alertPanelInfoData)
    {
        alertPanelSpawnPanelRoot.SetActive(true);
        
        var alertPanel = alertDoubleButtonPanelPool.Get();
        alertPanel.transform.SetParent(alertPanelSpawnPanel.transform);  
        alertPanel.transform.localPosition = Vector3.zero;
        
        alertPanel.TryGetComponent(out AlertPanel alertPanelComponent);
        alertPanelComponent.SetDescriptionTextAndButtonAction(alertPanelInfoData);
        
        alertDoubleButtonPanelList.Add(alertPanel);
    }

    public void HideAlertPanelSpawnPanelRoot()
    {
        for (int i = 0; i < alertSingleButtonPanelList.Count; ++i)
        {
            alertSingleButtonPanelList[i].transform.SetParent(alertPanelReleaseParent.transform);
            alertSingleButtonPanelPool.Release(alertSingleButtonPanelList[i]);
        }
        
        for (int i = 0; i < alertDoubleButtonPanelList.Count; ++i)
        {
            alertDoubleButtonPanelList[i].transform.SetParent(alertPanelReleaseParent.transform);
            alertDoubleButtonPanelPool.Release(alertDoubleButtonPanelList[i]);
        }
        
        alertSingleButtonPanelList.Clear();
        alertDoubleButtonPanelList.Clear();
        
        alertPanelSpawnPanelRoot.SetActive(false);
    }

    public void ShowFullScreenPanel(FullScreenType type)
    {
        SwitchActiveDefaultCanvas(DefaultCanvasType.FullScreen, true, true);

        switch (type)
        {
            case FullScreenType.GachaScreen:
                {
                    onGachaScreenActive?.Invoke();
                }
                break;
        }
    }

    public void HideFullScreenPanel()
    {
        SwitchActiveDefaultCanvas(DefaultCanvasType.FullScreen, false, true);
        SwitchActiveDefaultCanvas(DefaultCanvasType.Menu, true);
        SwitchActiveDefaultCanvas(CurrentDefaultCanvasTypeInSwitchableCanvasType, true);
    }

    public void SwitchActiveDefaultCanvas(DefaultCanvasType defaultCanvasType, bool isActive, bool inActiveOtherCanvas = false)
    {
        onSwitchActiveDefaultCanvas?.Invoke(defaultCanvasType, isActive, inActiveOtherCanvas);
    }

    public void SetCurrentDefaultCanvasTypeInSwitchableCanvasType(SwitchableCanvasType type)
    {
        switch (type)
        {
            case SwitchableCanvasType.Shop:
                {
                    outGameManager.OutGameUIManager.CurrentDefaultCanvasTypeInSwitchableCanvasType = DefaultCanvasType.Shop;
                }
                break;
            case SwitchableCanvasType.Lobby:
                {
                    outGameManager.OutGameUIManager.CurrentDefaultCanvasTypeInSwitchableCanvasType = DefaultCanvasType.Lobby;
                }
                break;
            case SwitchableCanvasType.Animal:
                {
                    outGameManager.OutGameUIManager.CurrentDefaultCanvasTypeInSwitchableCanvasType = DefaultCanvasType.Animal;
                }
                break;
            default:
                {
                    Debug.Assert(false,$"Invalid SwitchableCanvasType in {type}");
                }
                break;
        }
    }

    public GameObject GetGachaResultSlot()
    {
        var gachaResultSlot = gachaResultSlotPool.Get();
        
        return gachaResultSlot;
    }
    
    public void ReleaseGachaResultSlot(GameObject gachaResultSlot)
    {
        gachaResultSlot.transform.SetParent(gachaResultSlotPanelReleaseParent.transform);
        
        gachaResultSlotPool.Release(gachaResultSlot);
    }

    private void OnAnimalUnlockedHandler(int animalID)
    {
        if (!animalLockPanelDictionary.ContainsKey(animalID))
        {
            Debug.Assert(false, $"Invalid animalId in lockPanel {animalID}");
            
            return;
        }
        
        var lockAnimalPanel = animalLockPanelDictionary[animalID];
        animalLockPanelDictionary.Remove(animalID);
        lockAnimalPanelPool.Release(lockAnimalPanel);
            
        var animalUserData = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalID);
            
        var unlockAnimalPanel = unlockAnimalPanelPool.Get();
        unlockAnimalPanel.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanel);
        animalUnlockPanel.SetAnimalUserData(animalUserData);
        onAnimalUnlockPanelInstantiated?.Invoke(unlockAnimalPanel);
        unlockAnimalPanel.transform.localScale = Vector3.one;
        animalUnlockPanelDictionary.Add(animalUnlockPanel.animalId, unlockAnimalPanel);
    }
}