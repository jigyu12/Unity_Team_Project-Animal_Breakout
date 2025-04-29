using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    #endregion

    public int restartChanceCount = 1;
    // private int restartCount = 0;

    private void Awake()
    {
#if UNITY_EDITOR
        //프레임제한 풀기
        Application.targetFrameRate = -1;
#elif UNITY_ANDROID || UNITY_IOS
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
            SetTimeScale(0);
        });
        AddGameStateExitAction(GameState.GameStop, () =>
        {
            SetTimeScale(1);
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


        //findManagers.Find((manager) => manager.TryGetComponent<GameUIManager>(out gameUIManager));
        //gameUIManager.SetGameManager(this);
        //managers.Add(gameUIManager);
        gameUIManager = AddManagerToManagers<GameUIManager>(findManagers);


        //findManagers.Find((manager) => manager.TryGetComponent<MapObjectManager>(out mapObjectManager));
        //mapObjectManager.SetGameManager(this);
        //managers.Add(mapObjectManager);
        mapObjectManager = AddManagerToManagers<MapObjectManager>(findManagers);

        //findManagers.Find((manager) => manager.TryGetComponent<TempleRunStyleRoadMaker>(out roadMaker));
        //roadMaker.SetGameManager(this);
        //managers.Add(roadMaker);
        roadMaker = AddManagerToManagers<TempleRunStyleRoadMaker>(findManagers);

        //findManagers.Find((manager) => manager.TryGetComponent<PlayerManager>(out playerManager));
        //playerManager.SetGameManager(this);
        //managers.Add(playerManager);
        playerManager = AddManagerToManagers<PlayerManager>(findManagers);

        //findManagers.Find((manager) => manager.TryGetComponent<CameraManager>(out cameraManager));
        //cameraManager.SetGameManager(this);
        //managers.Add(cameraManager);
        cameraManager = AddManagerToManagers<CameraManager>(findManagers);

        //findManagers.Find((manager) => manager.TryGetComponent<SkillManager>(out skillManager));
        //skillManager.SetGameManager(this);
        //managers.Add(skillManager);
        skillManager = AddManagerToManagers<SkillManager>(findManagers);

        //findManagers.Find((manager) => manager.TryGetComponent<StageManager>(out stageManager));
        //stageManager.SetGameManager(this);
        //managers.Add(stageManager);
        stageManager = AddManagerToManagers<StageManager>(findManagers);

        //findManagers.Find((manager) => manager.TryGetComponent<BossManager>(out bossManager));
        //bossManager.SetGameManager(this);
        //managers.Add(bossManager);
        bossManager = AddManagerToManagers<BossManager>(findManagers);

        inGameCountManager = AddManagerToManagers<InGameCountManager>(findManagers);

        damageTextManager = AddManagerToManagers<DamageTextManager>(findManagers);

        foreach (var manager in managers)
        {
            manager.Initialize();
        }

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
        Time.timeScale = scale;
    }
    public void OnGameOver()
    {
        Debug.Log("Game Over!");
        UIManager.ShowGameOverPanel();

        SetTimeScale(0);
    }
}
