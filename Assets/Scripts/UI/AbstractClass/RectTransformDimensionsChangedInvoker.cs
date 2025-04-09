using System;
using UnityEngine;

public abstract class RectTransformDimensionsChangedInvoker<T> : MonoBehaviour
    where T : RectTransformDimensionsChangedInvoker<T>
{
    public static event Action<RectTransform> OnRectTransformDimensionsChanged;

    protected RectTransform rectTransform;

    protected virtual void Awake()
    {
        TryGetComponent(out rectTransform);
    }

    protected virtual void OnRectTransformDimensionsChange()
    {
        OnRectTransformDimensionsChanged?.Invoke(rectTransform);
    }
}