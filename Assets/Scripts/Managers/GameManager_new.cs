using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_new : MonoBehaviour
{
    public enum GameState
    {
        WaitLoading,
        GameReady,
        GameStart,
        GamePlay,
        GameStop,
        GameOver,
        GameClear,
        Max,
    }

    private List<Action> gameStateEnterAction;
    private List<Action> gameStateExitAction;

    private GameState currentState;

    #region manager
    private List<IManager> managers = new();

    public ObjectPoolManager ObjectPoolManager
    {
        get; private set;
    }

    private TempleRunStyleRoadMaker roadMaker;
    private MapObjectManager mapObjectManager;

    #endregion

    private void Awake()
    {
        InitializeStateEnterExitActions();

        SetGameState(GameState.WaitLoading);


        ObjectPoolManager = new ObjectPoolManager();
        managers.Add(ObjectPoolManager);
    }

    private void InitializeStateEnterExitActions()
    {
        gameStateEnterAction = new List<Action>((int)GameState.Max);
        gameStateExitAction = new List<Action>((int)GameState.Max);
    }

    public void AddGameStateEnterAction(GameState state, Action action)
    {
        gameStateEnterAction[(int)state] += action;
    }

    public void AddGameStateExitAction(GameState state, Action action)
    {
        gameStateExitAction[(int)state] += action;
    }

    public void RemoveGameStateEnterAction(GameState state, Action action)
    {
        gameStateEnterAction[(int)state] -= action;
    }

    public void RemoveGameStateExitAction(GameState state, Action action)
    {
        gameStateExitAction[(int)state] -= action;
    }

    private void Start()
    {
        ObjectPoolManager.SetGameManager(this);

        roadMaker = GameObject.FindObjectOfType<TempleRunStyleRoadMaker>();
        roadMaker.SetGameManager(this);
        managers.Add(roadMaker);

        mapObjectManager =GameObject.FindObjectOfType<MapObjectManager>();
        mapObjectManager.SetGameManager(this);
        managers.Add(mapObjectManager);

        foreach (var manager in managers)
        {
            manager.Initialize();
        }
    }

    private void OnDestroy()
    {
        foreach (var manager in managers)
        {
            manager.Clear();
        }
    }

    public void SetGameState(GameState gameState)
    {
        gameStateExitAction[(int)currentState]?.Invoke();
        currentState = gameState;
        gameStateEnterAction[(int)currentState]?.Invoke();
    }

}
