using UnityEngine;

public class AnimalLockPanel : MonoBehaviour
{
    private void Awake()
    {
        OutGameUIManager.onAnimalLockPanelInstantiated += OnAnimalLockPanelInstantiatedHandler;
    }

    private void OnDestroy()
    {
        OutGameUIManager.onAnimalLockPanelInstantiated -= OnAnimalLockPanelInstantiatedHandler;
    }
    
    private void OnAnimalLockPanelInstantiatedHandler(GameObject animalLockPanel)
    {
        animalLockPanel.transform.SetParent(transform);
    }
}