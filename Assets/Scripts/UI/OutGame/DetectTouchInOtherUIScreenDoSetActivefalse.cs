using UnityEngine;
using UnityEngine.UI;

public class DetectTouchInOtherUIScreenDoSetActivefalse : DetectTouchInOtherUIScreen
{
    [SerializeField] private Button staminaImageButton; 
    
    protected override void DoInOtherUIScreenTouch()
    {
        gameObject.SetActive(false);
    }

    protected override void ProcessTouch(Vector2 screenPosition)
    {
        eventData.position = screenPosition;
        
        results.Clear();
        
        currentEventSystem.RaycastAll(eventData, results);

        bool isOnMyPanel = false;
        foreach (var hit in results)
        {
            if (hit.gameObject == gameObject || hit.gameObject.transform.IsChildOf(transform) || hit.gameObject == staminaImageButton.gameObject)
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
}