using UnityEngine;

public class AnimalUnlockPanel : MonoBehaviour
{
    private void Awake()
    {
        OutGameUIManager.onAnimalUnlockPanelInstantiated += OnAnimalUnlockPanelInstantiatedHandler;
    }

    private void OnDestroy()
    {
        OutGameUIManager.onAnimalUnlockPanelInstantiated -= OnAnimalUnlockPanelInstantiatedHandler;
    }
    
    private void OnAnimalUnlockPanelInstantiatedHandler(GameObject animalUnlockPanel)
    {
        animalUnlockPanel.transform.SetParent(transform);
    }
}