using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AnimalChooseButton : MonoBehaviour
{
    private Button animalChooseButton;
    [SerializeField] [ReadOnly] private int animalID;
    
    private void Awake()
    {
        TryGetComponent(out animalChooseButton);
        
        //AnimalUnlockPanel.onSetStartAnimalIDInPanel 
    }

    private void OnDestroy()
    {
        
    }

    public void SetAnimalID(int id)
    {
        animalID = id;
    }

    public void SetAnimalChooseText(bool isChoose)
    {
        //animalChooseText.text = isChoose ? "선택됨" : "선택하기";
        animalChooseButton.interactable = !isChoose;
    }
}