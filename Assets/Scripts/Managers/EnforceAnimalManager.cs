using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceAnimalManager : MonoBehaviour, IManager
{
    private OutGameManager outGameManager;
    private EnforceAnimalDataTable EnforceAnimalDataTable
    {
        get;
        set;
    }

    public void Initialize()
    {
        EnforceAnimalDataTable = DataTableManager.enforceAnimalDataTable;
    }

    public void Clear()
    {
      
    }

    public void SetOutGameManager(OutGameManager outGameManager)
    {
        this.outGameManager = outGameManager;
    }

    //맥스레벨인지는 밖에서 확인해야 한다

    public void ExpectedEnforcedAnimalUserData(AnimalStatData statData, int level, out int enforcedAttackPower, out int tokenCost, out int goldCost)
    {
        EnforceAnimalData data = EnforceAnimalDataTable.Get(statData.Grade, level);
        enforcedAttackPower = statData.AttackPower + data.AttackPower;
        tokenCost = data.TokenValue;
        goldCost = data.Cost;
    }

    public bool IsEnforceAnimalPossible(AnimalUserData animalUserData, out bool hasEnoughTokens, out bool hasEnoughGolds)
    {
        EnforceAnimalData data = EnforceAnimalDataTable.Get(animalUserData.AnimalStatData.Grade, animalUserData.Level + 1);

        hasEnoughTokens = data.TokenValue <= GameDataManager.Instance.GoldAnimalTokenKeySystem.GetCurrentToken(animalUserData.AnimalStatData.Grade);
        hasEnoughGolds = data.Cost <= GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentGolds;

        return hasEnoughTokens && hasEnoughGolds;
    }

    public void EnforceAnimal(AnimalUserData animalUserData)
    {
        animalUserData.LevelUp();

        EnforceAnimalData data = DataTableManager.enforceAnimalDataTable.Get(animalUserData.AnimalStatData.Grade, animalUserData.Level + 1);
        GameDataManager.Instance.GoldAnimalTokenKeySystem.PayToken(animalUserData.AnimalStatData.Grade, data.TokenValue);
        GameDataManager.Instance.GoldAnimalTokenKeySystem.PayGold(data.Cost);
    }

}