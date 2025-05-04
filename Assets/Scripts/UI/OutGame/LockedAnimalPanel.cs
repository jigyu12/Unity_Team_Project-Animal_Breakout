using TMPro;
using UnityEngine;

public class LockedAnimalPanel : MonoBehaviour
{
    private AnimalDataTable.AnimalRawData animalStatData;
    [SerializeField] private TMP_Text tempAnimalNameText;
    
    public int animalId => animalStatData.AnimalID;
    
    public void SetAnimalStatData(AnimalDataTable.AnimalRawData statData)
    {
        animalStatData = statData;
        tempAnimalNameText.text = statData.StringID;
    }
}