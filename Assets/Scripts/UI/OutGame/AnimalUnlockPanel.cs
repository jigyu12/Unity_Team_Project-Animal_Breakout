using System;
using UnityEngine;
using UnityEngine.UI;

public class AnimalUnlockPanel : MonoBehaviour
{
    [SerializeField] private int animalID;
    [SerializeField] private Button chooseButton;
    
    public static event Action<int> onSetStartAnimalID;

    private void Start()
    {
        chooseButton.onClick.RemoveAllListeners();
        chooseButton.onClick.AddListener(() =>
        {
            onSetStartAnimalID?.Invoke(animalID);
            
            Debug.Log($"Set Start Animal ID :{animalID}");
        });
    }
}