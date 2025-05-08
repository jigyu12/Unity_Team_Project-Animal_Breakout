using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedAnimalPanel : MonoBehaviour
{
    //Raw데이터말고 AnimalUserData로 변동
    //private AnimalDataTable.AnimalRawData animalStatData;

    private AnimalUserData animalUserData;
     [SerializeField] private Image animalImage;

    public int animalId => animalUserData.AnimalStatData.AnimalID;
    
    //public void SetAnimalStatData(AnimalDataTable.AnimalRawData statData)
    //{
    //    animalStatData = statData;
    //    tempAnimalNameText.text = statData.StringID;
    //}

    public void SetAnimalUserData(AnimalUserData userData)
    {
        animalUserData = userData;
        animalImage.sprite = animalUserData.AnimalStatData.AnimalSprite;
    }
}