using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectTouchEventInvoker : MonoBehaviour, IPointerClickHandler
{
    public event Action onObjectTouched;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        onObjectTouched?.Invoke();
    }
}