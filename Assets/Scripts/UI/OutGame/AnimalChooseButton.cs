using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AnimalChooseButton : MonoBehaviour
{
    private Button animalChooseButton;
    [SerializeField] [ReadOnly] private int animalID;
    [SerializeField] private TMP_Text animalChooseText;
    
    private void Awake()
    {
        TryGetComponent(out animalChooseButton);

        GameDataManager.onSetStartAnimalIDInGameDataManager += SetAnimalChooseText;
        UnlockedAnimalPanel.onSetStartAnimalIDInPanel += SetAnimalChooseText;
    }

    private void OnDestroy()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager -= SetAnimalChooseText;
        UnlockedAnimalPanel.onSetStartAnimalIDInPanel -= SetAnimalChooseText;
    }

    public void SetAnimalID(int id)
    {
        animalID = id;
    }

    public void SetAnimalChooseText(int id)
    {
        bool isSameWithCurrentAnimalId = id == animalID;
        animalChooseText.text = isSameWithCurrentAnimalId ? Utils.AnimalSelectedString : Utils.AnimalSelectableString;
        animalChooseButton.interactable = !isSameWithCurrentAnimalId;
    }
}