using System;
using System.Collections.Generic;
using System.Linq;
using UnityCommunity.UnitySingleton;
using UnityEngine;

public class GameManager_new : MonoBehaviour
{
    public enum GameState
    {
        WaitLoading,
        GameReady,
        GamePlay,
        GameStop,
        //GameReStart,
        GameOver,
        GameClear,
        Max,
    }

    //enter->start->exit순으로 돈다
    private Action[] gameStateEnterAction;
    private Action[] gameStateStartAction;
    private Action[] gameStateExitAction;


    private GameState previousState;
    private GameState currentState;

    private float previousTimeScale;
    private float previousStopTimeScale;

    #region manager
    private List<IManager> managers = new();

    private ObjectPoolManager objectPoolManager;
    public ObjectPoolManager ObjectPoolManager => objectPoolManager;

    private GameUIManager gameUIManager;
    public GameUIManager UIManager => gameUIManager;

    private TempleRunStyleRoadMaker roadMaker;
    public TempleRunStyleRoadMaker RoadMaker => roadMaker;

    private MapObjectManager mapObjectManager;
    public MapObjectManager MapObjectManager => mapObjectManager;

    private PlayerManager playerManager;
    public PlayerManager PlayerManager => playerManager;

    private CameraManager cameraManager;
    public CameraManager CameraManager => cameraManager;

    private SkillManager skillManager;
    public SkillManager SkillManager => skillManager;

    private StageManager stageManager;
    public StageManager StageManager => stageManager;

    private BossManager bossManager;
    public BossManager BossManager => bossManager;

    private InGameCountManager inGameCountManager;
    public InGameCountManager InGameCountManager => inGameCountManager;
    private DamageTextManager damageTextManager;

    public DamageTextManager DamageTextManager => damageTextManager;

    private PassiveEffectManager passiveEffectManager;
    public PassiveEffectManager PassiveEffectManager => passiveEffectManager;

    #endregion

    public int restartChanceCount = 1;
    // private int restartCount = 0;

    private void Awake()
    {
        NativeServiceManager.Instance.InitializeSingleton();


#if UNITY_EDITOR
        //프레임제한 풀기
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
#elif UNITY_ANDROID || UNITY_IOS
        QualitySettings.vSyncCount = 0;
Application.targetFrameRate = 120;
#endif

        // SceneManager.sceneLoaded += SceneManagerEx.Instance.OnSceneLoaded;
        SceneManagerEx.Instance.onLoadComplete += OnPlayerReady;
        InitializeStateEnterExitActions();
        SetGameState(GameState.WaitLoading);
    }

    private void InitializeStateEnterExitActions()
    {
        gameStateEnterAction = new Action[(int)GameState.Max];
        gameStateExitAction = new Action[(int)GameState.Max];
        gameStateStartAction = new Action[(int)GameState.Max];


        AddGameStateStartAction(GameState.GameOver, OnGameOver);
        AddGameStateStartAction(GameState.GameStop, () =>
        {
            previousStopTimeScale = Time.timeScale;
            SetTimeScale(0);
        });
        AddGameStateExitAction(GameState.GameStop, () =>
        {
            SetTimeScale(previousStopTimeScale);
        });
    }

    public void AddGameStateEnterAction(GameState state, Action action)
    {
        gameStateEnterAction[(int)state] += action;
    }
    public void AddGameStateStartAction(GameState state, Action action)
    {
        gameStateStartAction[(int)state] += action;
    }

    public void AddGameStateExitAction(GameState state, Action action)
    {
        gameStateExitAction[(int)state] += action;
    }

    public void RemoveGameStateEnterAction(GameState state, Action action)
    {
        gameStateEnterAction[(int)state] -= action;
    }
    public void RemoveGameStateStartAction(GameState state, Action action)
    {
        gameStateStartAction[(int)state] -= action;
    }

    public void RemoveGameStateExitAction(GameState state, Action action)
    {
        gameStateExitAction[(int)state] -= action;
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

    private void InitializeManagers()
    {

        objectPoolManager = new ObjectPoolManager();
        managers.Add(ObjectPoolManager);

        var findManagers = GameObject.FindGameObjectsWithTag("Manager").ToList();

        gameUIManager = AddManagerToManagers<GameUIManager>(findManagers);

        mapObjectManager = AddManagerToManagers<MapObjectManager>(findManagers);

        roadMaker = AddManagerToManagers<TempleRunStyleRoadMaker>(findManagers);

        playerManager = AddManagerToManagers<PlayerManager>(findManagers);

        cameraManager = AddManagerToManagers<CameraManager>(findManagers);

        skillManager = AddManagerToManagers<SkillManager>(findManagers);

        stageManager = AddManagerToManagers<StageManager>(findManagers);

        bossManager = AddManagerToManagers<BossManager>(findManagers);

        inGameCountManager = AddManagerToManagers<InGameCountManager>(findManagers);

        damageTextManager = AddManagerToManagers<DamageTextManager>(findManagers);

        passiveEffectManager = AddManagerToManagers<PassiveEffectManager>(findManagers);

        foreach (var manager in managers)
        {
            manager.Initialize();
        }

        gameUIManager.InitializedUIElements();

        GameDataManager.Instance.Initialize();
    }

    private T AddManagerToManagers<T>(List<GameObject> list) where T : InGameManager
    {
        T managerT = null;
        list.Find((manager) =>
        {
            if (manager.TryGetComponent<T>(out T tempManager))
            {
                managerT = tempManager;
                return true;
            }
            else
            {
                return false;
            }
        });

        if (managerT != null)
        {
            managerT.SetGameManager(this);
            managers.Add(managerT);
        }
        else
        {
            Debug.Log(typeof(T) + " is not Ready");
        }

        return managerT;
    }

    public void SetGameState(GameState gameState)
    {
        previousState = currentState;
        currentState = gameState;

        gameStateExitAction[(int)previousState]?.Invoke();
        gameStateEnterAction[(int)currentState]?.Invoke();
        gameStateStartAction[(int)currentState]?.Invoke();
    }

    public void RestartGameState()
    {
        gameStateExitAction[(int)currentState]?.Invoke();
        currentState = previousState;
        gameStateStartAction[(int)currentState]?.Invoke();
    }

    public GameState GetCurrentGameState()
    {
        return currentState;
    }

    private void OnPlayerReady()
    {
        playerManager.SetPlayer();
        SetGameState(GameState.GameReady);

        SceneManagerEx.Instance.onLoadComplete -= OnPlayerReady;
    }

    public void SetTimeScale(float scale)
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = scale;
    }
    public void OnGameOver()
    {
        Debug.Log("Game Over!");
        UIManager.ShowGameOverPanel();

        //게임 결과 적용
        var result = gameUIManager.uiElements[(int)UIElementEnums.GameResultPanel] as ResultPanelUI;
        GameDataManager.Instance.ApplyRunResult(inGameCountManager.ScoreSystem.GetFinalScore(), result.TrackingTime);

        SetTimeScale(0);
    }
}
