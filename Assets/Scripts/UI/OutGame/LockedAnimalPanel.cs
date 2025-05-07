using TMPro;
using UnityEngine;

public class LockedAnimalPanel : MonoBehaviour
{
    //Raw데이터말고 AnimalUserData로 변동
    //private AnimalDataTable.AnimalRawData animalStatData;

    private AnimalUserData animalUserData;
    [SerializeField] private TMP_Text tempAnimalNameText;

    public int animalId => animalUserData.AnimalStatData.AnimalID;

    //public void SetAnimalStatData(AnimalDataTable.AnimalRawData statData)
    //{
    //    animalStatData = statData;
    //    tempAnimalNameText.text = statData.StringID;
    //}

    public void SetAnimalUserData(AnimalUserData userData)
    {
        animalUserData = userData;
        tempAnimalNameText.text = animalUserData.AnimalStatData.StringID;
    }
}