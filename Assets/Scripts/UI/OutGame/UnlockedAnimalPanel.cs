using System;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedAnimalPanel : MonoBehaviour
{
    //Raw데이터말고 AnimalUserData로 변동
    //private AnimalDataTable.AnimalRawData animalStatData;

    private AnimalUserData animalUserData;
    [SerializeField] private Button animalChooseButton;
    [SerializeField] private Button animalImageButton;
    private Image animalImage;
    [SerializeField] private AnimalChooseButton animalChooseButtonScript;
    
    public int animalId => animalUserData.AnimalStatData.AnimalID;
    
    public static event Action<int> onSetStartAnimalIDInPanel;

    private void Awake()
    {
        animalImageButton.gameObject.TryGetComponent(out animalImage);
    }
    
    private void Start()
    {
        animalChooseButton.onClick.RemoveAllListeners();
        animalChooseButton.onClick.AddListener(() =>
        {
            onSetStartAnimalIDInPanel?.Invoke(animalUserData.AnimalStatData.AnimalID);
            
            Debug.Log($"Set Start Animal ID :{animalUserData.AnimalStatData.AnimalID}");
        });
    }

    public void SetAnimalUserData(AnimalUserData userData)
    {
        animalUserData = userData;
        animalImage.sprite = animalUserData.AnimalStatData.iconImage;
        animalChooseButtonScript.SetAnimalID(animalUserData.AnimalStatData.AnimalID);
    }
}