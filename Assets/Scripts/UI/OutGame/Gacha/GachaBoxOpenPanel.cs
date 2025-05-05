using UnityEngine;
using UnityEngine.InputSystem;

public class GachaBoxOpenPanel : GachaPanelBase
{
    [SerializeField] private ObjectTouchEventInvoker boxTouchEventInvoker;

    protected override void Awake()
    {
        base.Awake();
        
        var actionMap = inputActions.FindActionMap("PlayerActions");
        touchAction = actionMap?.FindAction("TouchGacha1");

        boxTouchEventInvoker.onObjectTouched += OnObjectTouchedHandler;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        boxTouchEventInvoker.onObjectTouched -= OnObjectTouchedHandler;
    }

    private void OnObjectTouchedHandler()
    {
        gachaPanelController?.ShowNextGachaPanel();
    }

    protected override void OnTouchPerformed(InputAction.CallbackContext context)
    {
    }
}