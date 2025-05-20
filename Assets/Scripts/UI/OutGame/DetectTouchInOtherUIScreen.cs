using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class DetectTouchInOtherUIScreen : MonoBehaviour
{
    protected OutGameUIManager outGameUIManager;
    
    protected readonly List<RaycastResult> results = new();
    protected EventSystem currentEventSystem;
    protected PointerEventData eventData;
    protected Vector2 touchPos;

    protected virtual void Start()
    {
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
        outGameUIManager = outGameManager.OutGameUIManager;
        
        currentEventSystem = EventSystem.current;
        eventData = new PointerEventData(currentEventSystem);
    }

    protected virtual void Update()
    {
        if (Touchscreen.current == null)
        {
            return;
        }

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            
            ProcessTouch(touchPos);
        }
    }

    protected virtual void ProcessTouch(Vector2 screenPosition)
    {
        eventData.position = screenPosition;
        
        results.Clear();
        
        currentEventSystem.RaycastAll(eventData, results);

        bool isOnMyPanel = false;
        foreach (var hit in results)
        {
            if (hit.gameObject == gameObject || hit.gameObject.transform.IsChildOf(transform))
            {
                isOnMyPanel = true;
                
                break;
            }
        }

        if (!isOnMyPanel)
        {
            DoInOtherUIScreenTouch();
        }
    }

    protected abstract void DoInOtherUIScreenTouch();
}