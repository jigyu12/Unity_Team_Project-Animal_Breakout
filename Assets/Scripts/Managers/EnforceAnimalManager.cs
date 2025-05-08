using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceAnimalManager : MonoBehaviour, IManager
{

    public void Initialize()
    {
       
    }

    public void Clear()
    {
      
    }

    //맥스레벨인지는 밖에서 확인해야 한다

    public bool IsEnforceAnimalPossible(AnimalUserData animalUserData, out bool hasEnoughTokens, out bool hasEnoughGolds)
    {
        EnforceAnimalData data = DataTableManager.enforceAnimalDataTable.Get(animalUserData.AnimalStatData.Grade, animalUserData.Level + 1);

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