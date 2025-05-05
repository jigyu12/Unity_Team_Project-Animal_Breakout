using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GachaTotalResultPanel : GachaPanelBase
{
    [SerializeField] private List<GameObject> gachaResultSlotPanelList;

    private readonly List<GameObject> gachaResultSlotList = new();

    protected override void Awake()
    {
        base.Awake();
        
        var actionMap = inputActions.FindActionMap("UIActions");
        touchAction = actionMap?.FindAction("TouchGacha3");
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        AddAllCurrentGachaResultSlotPanel();
    }

    protected override void OnTouchPerformed(InputAction.CallbackContext context)
    {
        RemoveAllCurrentGachaResultSlotPanel();
        
        base.OnTouchPerformed(context);
    }

    private void AddAllCurrentGachaResultSlotPanel()
    {
        if (gachaDataList is null)
        {
            return;
        }
        
        if (gachaDataList.Count == 0 || gachaDataList.Count > gachaResultSlotPanelList.Count)
        {
            Debug.Assert(false, "InValid Gacha Data List");
        }
        
        gachaResultSlotList.Clear();

        if (outGameUIManager is null)
        {
            GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
            outGameUIManager = outGameManager.OutGameUIManager;
        }
        
        for(int i = 0; i < gachaDataList.Count; ++i)
        {
            var gachaResultSlot = outGameUIManager.GetGachaResultSlot();
            
            gachaResultSlot.transform.SetParent(gachaResultSlotPanelList[i].transform);
            gachaResultSlot.transform.localScale = Vector3.one;
            gachaResultSlot.transform.localPosition = Vector3.zero;
            
            gachaResultSlotList.Add(gachaResultSlot);
        }
    }
    
    private void RemoveAllCurrentGachaResultSlotPanel()
    {
        for(int i = 0; i < gachaResultSlotList.Count; ++i)
        {
            outGameUIManager.ReleaseGachaResultSlot(gachaResultSlotList[i]);
        }
        
        gachaResultSlotList.Clear();
    }
}