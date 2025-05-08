using UnityEngine;

public class AnimalUserData
{
    public AnimalStatData AnimalStatData
    {
        get;
        private set;
    }

    public bool IsUnlock
    {
        get;
        private set;
    }

    public readonly int maxLevel = 10;
    public int Level
    {
        get;
        private set;
    }

    public int AttackPower
    {
        get;
        private set;
    }

    public bool IsMaxLevel
    {
        get => Level >= maxLevel;
    }

    public AnimalUserData(AnimalStatData animalStatData)
    {
        AnimalStatData = animalStatData;
        //파라메터로 로드할때 뭔갈 받아와서 프로퍼티들 값을 적용해준다.
        //아래코드는 테스트용이며 무조건 사라져야할 코드임
        if (GameDataManager.Instance.AnimalUserDataList.initialAnimalID == AnimalStatData.AnimalID)
        {
            UnlockAnimal();
        }
    }

    public void UpdateData()
    {
        //저장한대로 복구하는 코드 들어있어야
        UpdateAttackPower();
    }

    public void UnlockAnimal()
    {
        if (!IsUnlock)
        {
            IsUnlock = true;
            Level = 1;
        }
        else
        {
            Debug.Assert(false, $"Animal is already unlocked in : {AnimalStatData.AnimalID}");
        }

        UpdateData();
    }

    public void LevelUp()
    {
        Level++;
        UpdateAttackPower();
    }

    private void UpdateAttackPower()
    {
        AttackPower = AnimalStatData.AttackPower;

        if (Level > 1)
        {
            AttackPower += DataTableManager.enforceAnimalDataTable.Get(AnimalStatData.Grade, Level).AttackPower;
        }
    }
}