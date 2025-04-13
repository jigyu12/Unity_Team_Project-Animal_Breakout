using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedAnimalPanel : MonoBehaviour
{
    private AnimalDataTable.AnimalData animalStatData;
    [SerializeField] private Button animalChooseButton;
    [SerializeField] private TMP_Text tempAnimalNameText;
    [SerializeField] private AnimalChooseButton animalChooseButtonScript;
    
    public int animalId => animalStatData.AnimalID;
    
    public static event Action<int> onSetStartAnimalIDInPanel;

    private void Start()
    {
        animalChooseButton.onClick.RemoveAllListeners();
        animalChooseButton.onClick.AddListener(() =>
        {
            onSetStartAnimalIDInPanel?.Invoke(animalStatData.AnimalID);
            
            Debug.Log($"Set Start Animal ID :{animalStatData.AnimalID}");
        });
    }
    
    public void SetAnimalStatData(AnimalDataTable.AnimalData statData)
    {
        animalStatData = statData;
        tempAnimalNameText.text = statData.StringID;
        animalChooseButtonScript.SetAnimalID(statData.AnimalID);
    }
}