using System.Collections.Generic;
using UnityEngine;

public class OutGameManager : MonoBehaviour
{
    #region manager
    
    private List<IManager> managers = new();
    
    private ObjectPoolManager objectPoolManager;
    public ObjectPoolManager ObjectPoolManager => objectPoolManager;
    
    private OutGameUIManager outGameUIManager;
    public OutGameUIManager OutGameUIManager => outGameUIManager;
    
    private GachaManager gachaManager;
    public GachaManager GachaManager => gachaManager;

    private EnforceAnimalManager enforceAnimalManager;
    public EnforceAnimalManager EnforceAnimalManager => enforceAnimalManager;
    #endregion

    [HideInInspector] public bool isGameQuitPanelShow;

    private void Awake()
    {
        isGameQuitPanelShow = false;
    }
    
    private void Start()
    {
        InitializeManagers();
    }
    
    private void OnDestroy()
    {
        foreach (var manager in managers)
        {
            manager.Clear();
        }
    }
    
#if UNITY_ANDROID
    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (outGameUIManager.isFullScreenActive)
            {
                return;
            }
            
            outGameUIManager.HideLastAlertPanel();
            outGameUIManager.HideAlertPanelSpawnPanelRoot();
            
            if (outGameUIManager.CurrentSwitchableCanvasType == SwitchableCanvasType.Lobby)
            {
                if (!isGameQuitPanelShow)
                {
                    outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.QuitGame));
                    
                    isGameQuitPanelShow = true;
                }
            }
            else
            {
                outGameUIManager.SwitchVisualizeSwitchableCanvas(SwitchableCanvasType.Lobby, true);
                outGameUIManager.SwitchActiveSwitchableCanvas(SwitchableCanvasType.Lobby);
                
                MenuPanel.onMenuBottomButtonClicked?.Invoke(SwitchableCanvasType.Lobby);
            }
        }
    }
#endif
    
    private void InitializeManagers()
    {
        GameDataManager.Instance.Initialize();
        
        objectPoolManager = new ObjectPoolManager();
        managers.Add(ObjectPoolManager);

        GameObject.FindGameObjectWithTag("OutGameUIManager").TryGetComponent(out outGameUIManager);
        outGameUIManager.SetOutGameManager(this);
        managers.Add(outGameUIManager);
        
        GameObject.FindGameObjectWithTag("GachaManager").TryGetComponent(out gachaManager);
        gachaManager.SetOutGameManager(this);
        managers.Add(gachaManager);

        GameObject.FindGameObjectWithTag("EnforceAnimalManager").TryGetComponent(out enforceAnimalManager);
        enforceAnimalManager.SetOutGameManager(this);
        managers.Add(enforceAnimalManager);

        foreach (var manager in managers)
        {
            manager.Initialize();
        }
    }
    
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}