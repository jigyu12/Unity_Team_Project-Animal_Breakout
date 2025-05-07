using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GachaPanelBase : MonoBehaviour
{
    [SerializeField] protected GachaPanelController gachaPanelController;
    
    [SerializeField] protected InputActionAsset inputActions;

    protected InputAction touchAction;

    protected GachaManager gachaManager;
    protected List<GachaData> gachaDataList;
    protected List<bool> animalFirstUnlockInfoList;
    
    protected OutGameUIManager outGameUIManager;

    protected virtual void Awake()
    {
        GachaManager.onGachaDo += OnGachaDoHandler;
        GachaManager.onAnimalFirstUnlockedListSet += OnAnimalFirstUnlockedListSetHandler;
    }
    
    protected virtual void Start()
    {
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
        gachaManager = outGameManager.GachaManager;
        outGameUIManager = outGameManager.OutGameUIManager;
    }

    protected virtual void OnEnable()
    {
        if (touchAction is not null)
        {
            touchAction.performed += OnTouchPerformed;
            touchAction.Enable();
        }
    }

    protected virtual void OnDisable()
    {
        if (touchAction is not null)
        {
            touchAction.performed -= OnTouchPerformed;
            touchAction.Disable();
        }
    }

    protected virtual void OnDestroy()
    {
        if (gachaManager is null)
        {
            return;
        }
        
        GachaManager.onGachaDo -= OnGachaDoHandler;
        GachaManager.onAnimalFirstUnlockedListSet -= OnAnimalFirstUnlockedListSetHandler;
    }

    protected virtual void OnTouchPerformed(InputAction.CallbackContext context)
    {
        gachaPanelController?.ShowNextGachaPanel();
    }

    protected void OnGachaDoHandler(List<GachaData> gachaDataList)
    {
        this.gachaDataList = gachaDataList;
    }
    
    protected void OnAnimalFirstUnlockedListSetHandler(List<bool> animalUnlockInfoList)
    {
        this.animalFirstUnlockInfoList = animalUnlockInfoList;
    }
}