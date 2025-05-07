using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : Singleton<GameDataManager>
{
    #region globalDataSystems

    public GoldAnimalTokenKeySystem GoldAnimalTokenKeySystem
    {
        get;
        private set;
    }

    public PlayerLevelSystem PlayerLevelSystem
    {
        get;
        private set;
    }

    public StaminaSystem StaminaSystem
    {
        get;
        private set;
    }

    #endregion

    private float additionalScoreGoldRate;

    public static event Action<LevelUpInfoData> onLevelExpInitialized;
    public static Action<int> onExpChanged;

    public static readonly Dictionary<int, int> expToLevelUpDictionary = new();
    public static readonly Dictionary<int, int> maxStaminaByLevelDictionary = new();
    public static readonly Dictionary<int, LevelUpRewardData> levelUpRewardDataDictionary = new();
    public readonly int maxLevel = 5;
    private static bool isAddToDictInInitialize;

    private long maxScore;
    private long currentGolds;
    private int currentStamina;
    public int currentLevel { get; private set; }
    public int nextExp { get; private set; }
    public int currentExp { get; private set; }

    private long inGameScore;

    private OutGameUIManager outGameUIManager;
    private OutGameManager outGameManager;
    private GameManager_new gameManager;

    //동물당 해금 여부, 강화여부 등을 들고있는 데이터다
    public AnimalUserDataList AnimalUserDataList
    {
        get;
        private set;
    }

    //여기저기서 쓰는 곳이 많아보여서 일단 이렇게 봉합하였습니다, 이렇게 바꾸어도 되나요?
    public int startAnimalID
    {
        get => AnimalUserDataList.CurrentAnimalID;
        //private set;
    }
    //public int StartAnimalID => startAnimalID;

    public static event Action<int, int> onSetStartAnimalIDInGameDataManager;

    public int MinMapObjectId { get; private set; } = 0;
    public int MinRewardItemId { get; private set; } = 0;
    public int MaxMapObjectId { get; private set; } = 0;
    public int MaxRewardItemId { get; private set; } = 0;

    public static event Action<int, int> onStaminaChangedInGameDataManager;
    //public static event Action<long> OnGoldChangedInGameDataManager;
    public const int minStamina = 0;
    public const int maxStamina = 999;

    private LevelUpInfoData initialData;
    
    public const long keyPrice = 5000;
    
    public long staminaGoldUseCost { get; private set; }
    public int staminaToAdd { get; private set; }

    private void Awake()
    {
        //골드,토큰을 관리하는 시스템
        GoldAnimalTokenKeySystem = new();

        //플레이어 레벨, 경험치를 관리하는 시스템
        PlayerLevelSystem = new();

        //스테미나를 관리하는 시스템
        StaminaSystem = new();

        //동물당 해금 여부, 강화여부 등을 들고있는 데이터 초기화
        AnimalUserDataList = new();
        AnimalUserDataList.Load();

        SetMaxMapObjectId(DataTableManager.mapObjectsDataTable.maxId);
        SetMinMapObjectId(DataTableManager.mapObjectsDataTable.minId);
        SetMaxRewardItemId(DataTableManager.rewardItemsDataTable.maxId);
        SetMinRewardItemId(DataTableManager.rewardItemsDataTable.minId);
    }

    private void Start()
    {
        //BaseCollisionBehaviour.OnScoreChanged += AddScoreInGame;
        SetInitializeData();


        UnlockedAnimalPanel.onSetStartAnimalIDInPanel += OnSetAnimalIDInPanel;

        SceneManager.sceneLoaded += OnChangeSceneHandler;

        //LevelSlider.onLevelUp += OnLevelUpHandler;
        //LevelSlider.onExpChangedInSameLevel += onExpChangedInSameLevelHandler;

        LobbyPanel.onGameStartButtonClicked += OnGameStartButtonClickedHandler;

        OutGameUIManager.onAnimalUnlockPanelInstantiated += onAnimalUnlockPanelInstantiatedHandler;

        GachaManager.onTokenAdded += OnTokenAddedHandler;
        
        StaminaGoldUseButton.onStaminaGoldUseButtonClicked += OnStaminaGoldUseButtonClickedHandler;

        onSetStartAnimalIDInGameDataManager?.Invoke(startAnimalID, StaminaSystem.CurrentStamina);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnChangeSceneHandler;

        UnlockedAnimalPanel.onSetStartAnimalIDInPanel -= OnSetAnimalIDInPanel;

        SceneManager.sceneLoaded -= OnChangeSceneHandler;

        //LevelSlider.onLevelUp -= OnLevelUpHandler;
        //LevelSlider.onExpChangedInSameLevel -= onExpChangedInSameLevelHandler;

        LobbyPanel.onGameStartButtonClicked -= OnGameStartButtonClickedHandler;

        OutGameUIManager.onAnimalUnlockPanelInstantiated -= onAnimalUnlockPanelInstantiatedHandler;
        
        GachaManager.onTokenAdded -= OnTokenAddedHandler;
        
        StaminaGoldUseButton.onStaminaGoldUseButtonClicked -= OnStaminaGoldUseButtonClickedHandler;
    }


    private void SetMaxMapObjectId(int maxMapObjectCount)
    {
        MaxMapObjectId = maxMapObjectCount;
    }

    private void SetMinMapObjectId(int minMapObjectCount)
    {
        MinMapObjectId = minMapObjectCount;
    }

    private void SetMaxRewardItemId(int maxRewardItemCount)
    {
        MaxRewardItemId = maxRewardItemCount;
    }
    private void SetMinRewardItemId(int minRewardItemCount)
    {
        MinRewardItemId = minRewardItemCount;
    }

    public void Initialize()
    {
        if (SceneManager.GetActiveScene().name == "MainTitleSceneCopy")
        {
            TryFindOutGameUIManager();
        }
        else if (SceneManager.GetActiveScene().name == "Run_new")
        {
            TryFindGameManager();
        }

        //ClearInGameData();
    }

    private void TryFindOutGameUIManager()
    {
        //Debug.Assert(GameObject.FindGameObjectWithTag("OutGameUIManager").TryGetComponent(out outGameUIManager), "Cant find OutGameUIManager");
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out outGameManager);
        outGameUIManager = outGameManager.OutGameUIManager;
    }

    private void TryFindGameManager()
    {
        Debug.Assert(GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager)
        , "Cant find GameManager");
    }

    private void SetInitializeData()
    {
        PlayerLevelSystem.SetInitialValue(1, 0);
        //string temp = "05/06/2025 22:20:13";
        //StaminaSystem.SetInitialValue(0, DateTime.Parse(temp));    //임시로 now갈겨놓은 것이니 추후 저장후 확인
        GoldAnimalTokenKeySystem.SetInitialValue(100000, 1000, 1000, 1000, 12);
        StaminaSystem.SetInitialValue(900, DateTime.Now);    //임시로 now갈겨놓은 것이니 추후 저장후 확인


        //// TempCode //

        //if (!isAddToDictInInitialize)
        //{
        //    isAddToDictInInitialize = true;

        //    expToLevelUpDictionary.Add(1, 110);
        //    expToLevelUpDictionary.Add(2, 120);
        //    expToLevelUpDictionary.Add(3, 130);
        //    expToLevelUpDictionary.Add(4, 140);
        //    expToLevelUpDictionary.Add(5, 150);

        //    int maxStamina1 = 5;
        //    int maxStamina2 = 10;
        //    int maxStamina3 = 15;
        //    int maxStamina4 = 20;
        //    int maxStamina5 = 25;

        //    maxStaminaByLevelDictionary.Add(1, maxStamina1);
        //    maxStaminaByLevelDictionary.Add(2, maxStamina2);
        //    maxStaminaByLevelDictionary.Add(3, maxStamina3);
        //    maxStaminaByLevelDictionary.Add(4, maxStamina4);
        //    maxStaminaByLevelDictionary.Add(5, maxStamina5);

        //    {
        //        var levelUpRewardData1 = new LevelUpRewardData();
        //        levelUpRewardData1.SaveLevelUpRewardData(maxStamina1, 1000);
        //        levelUpRewardDataDictionary.Add(1, levelUpRewardData1);
        //    }
        //    {
        //        var levelUpRewardData2 = new LevelUpRewardData();
        //        levelUpRewardData2.SaveLevelUpRewardData(maxStamina2, 2000);
        //        levelUpRewardDataDictionary.Add(2, levelUpRewardData2);
        //    }
        //    {
        //        var levelUpRewardData3 = new LevelUpRewardData();
        //        levelUpRewardData3.SaveLevelUpRewardData(maxStamina3, 3000);
        //        levelUpRewardDataDictionary.Add(3, levelUpRewardData3);
        //    }
        //    {
        //        var levelUpRewardData4 = new LevelUpRewardData();
        //        levelUpRewardData4.SaveLevelUpRewardData(maxStamina4, 4000);
        //        levelUpRewardDataDictionary.Add(4, levelUpRewardData4);
        //    }
        //    {
        //        var levelUpRewardData5 = new LevelUpRewardData();
        //        levelUpRewardData5.SaveLevelUpRewardData(5, 5000);
        //        levelUpRewardDataDictionary.Add(5, levelUpRewardData5);
        //    }
        //}



        //currentStamina = 10; // 5
        //currentLevel = 1;
        //nextExp = expToLevelUpDictionary[currentLevel];
        //currentExp = 0;

        //initialData = new(maxLevel);
        //initialData.SaveLevelUpInfoData(currentLevel, nextExp, currentExp);

        //onLevelExpInitialized?.Invoke(initialData);

        //// TempCode //

        //currentStamina = Math.Clamp(currentStamina, minStamina, maxStamina);

        //onStaminaChangedInGameDataManager?.Invoke(currentStamina, maxStaminaByLevelDictionary[currentLevel]);

        ////ClearInGameData();
    }

    //private void ClearInGameData()
    //{
    //    inGameScore = 0;
    //}

    //private void AddScoreInGame(long scoreToAdd)
    //{
    //    inGameScore += scoreToAdd;

    //    if (inGameScore <= 0)
    //    {
    //        inGameScore = 0;
    //    }

    //    UpdateScoreUI();
    //}

    //private void UpdateScoreUI()
    //{
    //    if (GameObject.FindGameObjectWithTag("ScoreUI")?.TryGetComponent(out ScoreUI scoreUI) == true)
    //    {
    //        scoreUI.UpdateScore(inGameScore);
    //    }
    //}

    public void ClearAdditionalScoreGoldRate()
    {
        additionalScoreGoldRate = 0f;
    }

    public void AddAdditionalScoreGoldRate(float rate)
    {
        additionalScoreGoldRate += rate;
    }


    public void ApplyRunResult(long score, float playTime)
    {
        //수정필

        //임시 값 적용
        var resultGold = score / 100;

        GoldAnimalTokenKeySystem.AddGold(resultGold+ Mathf.FloorToInt(resultGold*additionalScoreGoldRate));
        PlayerLevelSystem.AddExperienceValue(40 + Mathf.RoundToInt(playTime * 0.31f));
    }

    private void OnChangeSceneHandler(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "MainTitleScene")
        {
            TryFindOutGameUIManager();

            //if (maxScore < inGameScore)
            //{
            //    maxScore = inGameScore;
            //}
            //Debug.Log($"InGameScore : {inGameScore}");
            //Debug.Log($"MaxScore : {maxScore}");

            //GoldTokenSystem.AddGold(inGameScore / 100);

            //onLevelExpInitialized?.Invoke(initialData);
            //onExpChanged?.Invoke(100);
            //currentStamina = Math.Clamp(currentStamina, minStamina, maxStamina);
            //onStaminaChangedInGameDataManager?.Invoke(currentStamina, maxStaminaByLevelDictionary[currentLevel]);
        }
        else if (SceneManager.GetActiveScene().name == "Run")
        {
            TryFindGameManager();
        }

        Time.timeScale = 1;
    }

    private void OnSetAnimalIDInPanel(int id)
    {
        //startAnimalID = id;

        AnimalUserDataList.SetCurrentAnimalPlayer(id);
        onSetStartAnimalIDInGameDataManager?.Invoke(id, StaminaSystem.CurrentStamina);
    }

    //private void OnLevelUpHandler(int nextLevel, int nextExp, int remainingExp)
    //{
    //    currentLevel = nextLevel;
    //    this.nextExp = nextExp;
    //    currentExp = remainingExp;
    //    initialData.SaveLevelUpInfoData(currentLevel, this.nextExp, currentExp);

    //    GoldTokenSystem.AddGold(levelUpRewardDataDictionary[currentLevel].goldToAdd);

    //    currentStamina += levelUpRewardDataDictionary[currentLevel].staminaToAdd;
    //    currentStamina = Math.Clamp(currentStamina, minStamina, maxStamina);

    //    onStaminaChangedInGameDataManager?.Invoke(currentStamina, maxStaminaByLevelDictionary[currentLevel]);
    //}

    private void OnGameStartButtonClickedHandler(int staminaRequiredToStartGame)
    {
        if (StaminaSystem.CurrentStamina < staminaRequiredToStartGame)
        {
            Debug.Assert(false, "Stamina is low to start game");

            return;
        }

        StaminaSystem.PayStamina(staminaRequiredToStartGame);
        //currentStamina -= staminaRequiredToStartGame;
        //onStaminaChangedInGameDataManager?.Invoke(currentStamina, maxStaminaByLevelDictionary[currentLevel]);
    }

    private void onAnimalUnlockPanelInstantiatedHandler(GameObject animalUnlockPanel)
    {
        onSetStartAnimalIDInGameDataManager?.Invoke(startAnimalID, StaminaSystem.CurrentStamina);
    }

    //public void IncreaseStamina(int staminaAmount)
    //{
    //    currentStamina += staminaAmount;
    //    currentStamina = Math.Clamp(currentStamina, minStamina, maxStamina);

    //    onStaminaChangedInGameDataManager?.Invoke(currentStamina, maxStaminaByLevelDictionary[currentLevel]);

    //    Debug.Log($"Stamina has been increased. Current Stamina : {currentStamina}");
    //}

    private void onExpChangedInSameLevelHandler(int currentLevel, int nextExp, int currentExp)
    {
        this.currentLevel = currentLevel;
        this.nextExp = nextExp;
        this.currentExp = currentExp;
        initialData.SaveLevelUpInfoData(this.currentLevel, this.nextExp, this.currentExp);
    }


    public void AddStaminaRepeat(int currentStamina, int maxStaminaCanFilled)
    {
        if (StaminaSystem.coAddStamina == null&&!StaminaSystem.IsStaminaFull)
        {
            StaminaSystem.coAddStamina = StartCoroutine(StaminaSystem.CoAddStamina());
        }
    }

    private void OnTokenAddedHandler(TokenType type, int tokenValue)
    {
        switch (type)
        {
            case TokenType.BronzeToken:
                {
                    GoldAnimalTokenKeySystem.AddBronzeToken(tokenValue);
                    
                    return;
                }
            case TokenType.SilverToken:
                {
                    GoldAnimalTokenKeySystem.AddSliverToken(tokenValue);
                    
                    return;
                }
            case TokenType.GoldToken:
                {
                    GoldAnimalTokenKeySystem.AddGoldToken(tokenValue);
                    
                    return;
                }
        }
        
        Debug.Assert(false, "Invalid tokenType");
    }

    private void OnStaminaGoldUseButtonClickedHandler(long staminaGoldUseCost, int staminaToAdd)
    {
        this.staminaGoldUseCost = staminaGoldUseCost;
        this.staminaToAdd = staminaToAdd;
    }
}