using Excellcube.EasyTutorial;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

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
    private readonly List<GameObject> animalUnlockPanelList = new();

    [SerializeField] private GameObject lockAnimalPanelPrefab;
    private ObjectPool<GameObject> lockAnimalPanelPool;
    public static event Action<GameObject> onAnimalLockPanelInstantiated;
    private readonly Dictionary<int, GameObject> animalLockPanelDictionary = new();

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
    public static event Action<AnimalUserData> onEnforceSuccessScreenActive;

    [SerializeField] private GameObject gachaResultSlot;
    private ObjectPool<GameObject> gachaResultSlotPool;
    [SerializeField] private GameObject gachaResultSlotPanelReleaseParent;

    [SerializeField] private TMP_Dropdown animalStatDropDown;
    [SerializeField] private Toggle animalListSortToggle;
    [SerializeField] private Canvas animalCanvas;
    private CanvasGroup animalCanvasGroup;
    private readonly WaitForEndOfFrame waitEndOfFrame = new();

    [SerializeField] private GameObject settingPanel;

    [SerializeField] private GameObject enforceAnimalPanel;
    private ObjectPool<GameObject> enforceAnimalPanelPool;
    [SerializeField] private GameObject enforceAnimalPanelReleaseParent;
    private readonly List<GameObject> enforceAnimalPanelList = new();

    public GameObject lastAlertPanel { get; private set; }
    public GameObject lastEnforceAnimalPanel { get; private set; }

    public bool isFullScreenActive { get; private set; }

    public static event Action<FullScreenType> onSpecificFullScreenActive;
    
    [SerializeField] private ECEasyTutorial tutorial;
    
    [SerializeField] private GameObject actionLogImage;

    [SerializeField] private GameObject touchBlockPanel;

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

        enforceAnimalPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(enforceAnimalPanel,
            () => Instantiate(enforceAnimalPanel),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        GachaManager.onAnimalUnlocked += OnAnimalUnlockedHandler;
        GachaManager.onAnimalUnlockedFinished += StartSortUnlockAnimalPanelCoroutine;
        animalStatDropDown.onValueChanged.AddListener(SortUnlockAnimalPanel);
        animalListSortToggle.onValueChanged.AddListener(SortUnlockAnimalPanel);

        unlockAnimalPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(unlockAnimalPanelPrefab,
            () => Instantiate(unlockAnimalPanelPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });

        lockAnimalPanelPool = outGameManager.ObjectPoolManager.CreateObjectPool(lockAnimalPanelPrefab,
            () => Instantiate(lockAnimalPanelPrefab),
            obj => { obj.SetActive(true); },
            obj => { obj.SetActive(false); });
        
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
                animalUnlockPanelList.Add(unlockAnimalPanel);
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

        animalCanvas.TryGetComponent(out animalCanvasGroup);
        SortUnlockAnimalPanel();

        isFullScreenActive = false;
        
        StartCoroutine(DisableTouchBlockPanelCoroutine());
    }

    private void OnDestroy()
    {
        GachaManager.onAnimalUnlocked -= OnAnimalUnlockedHandler;
        GachaManager.onAnimalUnlockedFinished -= StartSortUnlockAnimalPanelCoroutine;
        animalStatDropDown.onValueChanged.RemoveAllListeners();
        animalListSortToggle.onValueChanged.RemoveAllListeners();
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

        alertPanel.transform.GetChild(0).TryGetComponent(out AlertPanel alertPanelComponent);
        alertPanelComponent.SetDescriptionTextAndButtonAction(alertPanelInfoData);
        alertPanelComponent.SetReleaseBySelf(alertSingleButtonPanelPool, alertSingleButtonPanelList, alertPanelReleaseParent);

        alertSingleButtonPanelList.Add(alertPanel);

        lastAlertPanel = alertPanel;
    }

    public void ShowAlertDoubleButtonPanel(AlertPanelInfoData alertPanelInfoData)
    {
        alertPanelSpawnPanelRoot.SetActive(true);

        var alertPanel = alertDoubleButtonPanelPool.Get();
        alertPanel.transform.SetParent(alertPanelSpawnPanel.transform);
        alertPanel.transform.localPosition = Vector3.zero;

        alertPanel.transform.GetChild(0).TryGetComponent(out AlertPanel alertPanelComponent);
        alertPanelComponent.SetDescriptionTextAndButtonAction(alertPanelInfoData);
        alertPanelComponent.SetReleaseBySelf(alertDoubleButtonPanelPool, alertDoubleButtonPanelList, alertPanelReleaseParent);

        alertDoubleButtonPanelList.Add(alertPanel);

        lastAlertPanel = alertPanel;
    }

    public void ShowEnforceAnimalPanel(AnimalUserData animalUserData)
    {
        alertPanelSpawnPanelRoot.SetActive(true);

        var enforcePanel = enforceAnimalPanelPool.Get();
        enforcePanel.transform.SetParent(alertPanelSpawnPanel.transform);
        enforcePanel.transform.localPosition = Vector3.zero;

        enforcePanel.transform.GetChild(0).TryGetComponent(out EnforceAnimalPanel enforceAnimalPanelComponent);
        enforceAnimalPanelComponent.SetTargetAnimalUserData(animalUserData);

        enforceAnimalPanelList.Add(enforcePanel);

        lastEnforceAnimalPanel = enforcePanel;
    }

    public void ShowSettingPanel()
    {
        alertPanelSpawnPanelRoot.SetActive(true);

        settingPanel.SetActive(true);
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

        for (int i = 0; i < enforceAnimalPanelList.Count; ++i)
        {
            enforceAnimalPanelList[i].transform.SetParent(enforceAnimalPanelReleaseParent.transform);
            enforceAnimalPanelPool.Release(enforceAnimalPanelList[i]);
        }

        alertSingleButtonPanelList.Clear();
        alertDoubleButtonPanelList.Clear();
        enforceAnimalPanelList.Clear();

        settingPanel.SetActive(false);

        alertPanelSpawnPanelRoot.SetActive(false);
        
        outGameManager.isGameQuitPanelShow = false;
    }

    public void HideLastAlertPanel()
    {
        if (lastAlertPanel != null)
        {
            lastAlertPanel.transform.GetChild(0).TryGetComponent(out AlertPanel alertPanelComponent);
            alertPanelComponent.Release();
            
            outGameManager.isGameQuitPanelShow = false;
        }
    }

    public void ShowFullScreenPanel(FullScreenType type)
    {
        isFullScreenActive = true;
        
        SwitchActiveDefaultCanvas(DefaultCanvasType.FullScreen, true, true);

        onSpecificFullScreenActive?.Invoke(type);

        switch (type)
        {
            case FullScreenType.GachaScreen:
                {
                    onGachaScreenActive?.Invoke();
                }
                break;
            case FullScreenType.EnforceSuccessScreen:
                {
                    onEnforceSuccessScreenActive?.Invoke(GameDataManager.Instance.targetEnforceAnimalPanel.animalUserData);
                }
                break;
        }
    }

    public Action onHideFullScreenPanel;

    public void HideFullScreenPanel()
    {
        SwitchActiveDefaultCanvas(DefaultCanvasType.FullScreen, false, true);
        SwitchActiveDefaultCanvas(DefaultCanvasType.Menu, true);
        SwitchActiveDefaultCanvas(CurrentDefaultCanvasTypeInSwitchableCanvasType, true);

        isFullScreenActive = false;
        //튜토리얼을 위해 임시로 추가
        onHideFullScreenPanel?.Invoke();
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
                    Debug.Assert(false, $"Invalid SwitchableCanvasType in {type}");
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
        animalUnlockPanelList.Add(unlockAnimalPanel);
    }

    private void SortUnlockAnimalPanel(int dropDownValue)
    {
        SortUnlockAnimalPanel();
    }

    public void SortUnlockAnimalPanel(bool toggleValue)
    {
        SortUnlockAnimalPanel();
    }

    public void SortUnlockAnimalPanel()
    {
        var dropDownValue = animalStatDropDown.value;
        var toggleValueIsOn = animalListSortToggle.isOn;

        switch (dropDownValue)
        {
            case (int)AnimalStatDropDownSortType.Level:
                {
                    if (toggleValueIsOn)
                    {
                        animalUnlockPanelList.Sort((animalUnlockPanel1, animalUnlockPanel2) =>
                        {
                            animalUnlockPanel1.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent1);
                            var animalId1 = animalUnlockPanelComponent1.animalId;

                            animalUnlockPanel2.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent2);
                            var animalId2 = animalUnlockPanelComponent2.animalId;

                            var animalUserData1 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId1);
                            var animalUserData2 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId2);

                            var compare1 = animalUserData1.Level.CompareTo(animalUserData2.Level);
                            if (compare1 != 0)
                            {
                                return compare1;
                            }

                            return animalUserData1.AnimalStatData.AnimalID.CompareTo(animalUserData2.AnimalStatData.AnimalID);
                        });
                    }
                    else
                    {
                        animalUnlockPanelList.Sort((animalUnlockPanel1, animalUnlockPanel2) =>
                        {
                            animalUnlockPanel1.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent1);
                            var animalId1 = animalUnlockPanelComponent1.animalId;

                            animalUnlockPanel2.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent2);
                            var animalId2 = animalUnlockPanelComponent2.animalId;

                            var animalUserData1 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId2);
                            var animalUserData2 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId1);

                            var compare = animalUserData1.Level.CompareTo(animalUserData2.Level);
                            if (compare != 0)
                            {
                                return compare;
                            }

                            return animalUserData1.AnimalStatData.AnimalID.CompareTo(animalUserData2.AnimalStatData.AnimalID);
                        });
                    }
                }
                break;
            case (int)AnimalStatDropDownSortType.Grade:
                {
                    if (toggleValueIsOn)
                    {
                        animalUnlockPanelList.Sort((animalUnlockPanel1, animalUnlockPanel2) =>
                        {
                            animalUnlockPanel1.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent1);
                            var animalId1 = animalUnlockPanelComponent1.animalId;

                            animalUnlockPanel2.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent2);
                            var animalId2 = animalUnlockPanelComponent2.animalId;

                            var animalUserData1 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId1);
                            var animalUserData2 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId2);

                            var compare = animalUserData1.AnimalStatData.Grade.CompareTo(animalUserData2.AnimalStatData.Grade);
                            if (compare != 0)
                            {
                                return compare;
                            }

                            return animalUserData1.AnimalStatData.AnimalID.CompareTo(animalUserData2.AnimalStatData.AnimalID);
                        });
                    }
                    else
                    {
                        animalUnlockPanelList.Sort((animalUnlockPanel1, animalUnlockPanel2) =>
                        {
                            animalUnlockPanel1.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent1);
                            var animalId1 = animalUnlockPanelComponent1.animalId;

                            animalUnlockPanel2.TryGetComponent(out UnlockedAnimalPanel animalUnlockPanelComponent2);
                            var animalId2 = animalUnlockPanelComponent2.animalId;

                            var animalUserData1 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId2);
                            var animalUserData2 = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(animalId1);

                            var compare = animalUserData1.AnimalStatData.Grade.CompareTo(animalUserData2.AnimalStatData.Grade);
                            if (compare != 0)
                            {
                                return compare;
                            }

                            return animalUserData1.AnimalStatData.AnimalID.CompareTo(animalUserData2.AnimalStatData.AnimalID);
                        });
                    }
                }
                break;
        }

        for (int i = 0; i < animalUnlockPanelList.Count; ++i)
        {
            animalUnlockPanelList[i].transform.SetSiblingIndex(i);
        }
    }

    private void StartSortUnlockAnimalPanelCoroutine()
    {
        StartCoroutine(SortUnlockAnimalPanelCoroutine());
    }

    private IEnumerator SortUnlockAnimalPanelCoroutine()
    {
        SortUnlockAnimalPanel();

        animalCanvas.gameObject.SetActive(true);
        animalCanvasGroup.alpha = 0;
        animalCanvasGroup.interactable = false;
        animalCanvasGroup.blocksRaycasts = false;

        yield return waitEndOfFrame;

        animalCanvas.gameObject.SetActive(false);
        animalCanvasGroup.alpha = 1;
        animalCanvasGroup.interactable = true;
        animalCanvasGroup.blocksRaycasts = true;
    }
    
    public void InActiveLastAnimalEnforcePanelDetectTouch()
    {
        lastEnforceAnimalPanel.TryGetComponent(out DetectTouchInOtherUIScreenDoHideAllAlertPanel detectTouchInOtherUIScreenDoHideAllAlertPanel);
        detectTouchInOtherUIScreenDoHideAllAlertPanel.enabled = false;
    }
    
    public void ActiveLastAnimalEnforcePanelDetectTouch()
    {
        lastEnforceAnimalPanel.TryGetComponent(out DetectTouchInOtherUIScreenDoHideAllAlertPanel detectTouchInOtherUIScreenDoHideAllAlertPanel);
        detectTouchInOtherUIScreenDoHideAllAlertPanel.enabled = true;
    }
    
    public void InActiveActionLogImage()
    {
        StartCoroutine(InActiveActionLogImageCoroutine());
    }

    private IEnumerator InActiveActionLogImageCoroutine()
    {
        yield return null;
        actionLogImage.SetActive(false);
    }

    public void ActiveActionLogImage()
    {
        actionLogImage.SetActive(true);
    }

    private IEnumerator DisableTouchBlockPanelCoroutine()
    {
        yield return null;

        if (touchBlockPanel == null)
        {
            yield break;
        }
        
        touchBlockPanel.SetActive(false);
    }
}