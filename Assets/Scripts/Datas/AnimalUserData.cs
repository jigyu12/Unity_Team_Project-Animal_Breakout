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

    public int Level
    {
        get;
        private set;
    }

    public AnimalUserData(AnimalStatData animalStatData)
    {
        AnimalStatData = animalStatData;
        UpdateData();
    }

    public void UpdateData()
    {
        //파라메터로 로드할때 뭔갈 받아와서 프로퍼티들 값을 적용해준다.
        //아래코드는 테스트용이며 무조건 사라져야할 코드임
        if(GameDataManager.Instance.AnimalUserDataList.initialAnimalID == AnimalStatData.AnimalID)
        {
            IsUnlock = true;
            Level = 1;

        }
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
    }
}