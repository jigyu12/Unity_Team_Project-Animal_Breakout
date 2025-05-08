using UnityEngine;

public class InitializeRectTransformAnchoredPosition : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    
    private void OnEnable()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }
}