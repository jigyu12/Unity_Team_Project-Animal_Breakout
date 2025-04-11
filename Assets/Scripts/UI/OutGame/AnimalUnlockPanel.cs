using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AnimalUnlockPanel : MonoBehaviour
{
    [SerializeField] [ReadOnly] private AnimalStatData animalStatData;
    [SerializeField] private Button animalChooseButton;
    [SerializeField] private AnimalChooseButton animalChooseButtonScript;
    [SerializeField] private TMP_Text tempAnimalNameText;
    [SerializeField] private TMP_Text animalChooseText;
    
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
    
    public void SetAnimalStatData(AnimalStatData statData)
    {
        animalStatData = statData;
        tempAnimalNameText.text = statData.name;
        animalChooseText.text = $"Select {statData.StringID}";
    }
}