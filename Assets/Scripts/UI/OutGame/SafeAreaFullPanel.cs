using System.Collections.Generic;
using UnityEngine;

public class SafeAreaFullPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> fullScreenPanels;
    
    private void Awake()
    {
        OutGameUIManager.onSpecificFullScreenActive += OnSpecificFullScreenActiveHandler;
    }
    
    private void OnDestroy()
    {
        OutGameUIManager.onSpecificFullScreenActive -= OnSpecificFullScreenActiveHandler;
    }

    private void OnDisable()
    {
        for(int i = 0; i < fullScreenPanels.Count; ++i)
        {
            fullScreenPanels[i].SetActive(false);
        }
    }

    private void OnSpecificFullScreenActiveHandler(FullScreenType fullScreenType)
    {
        for(int i = 0; i < fullScreenPanels.Count; ++i)
        {
            fullScreenPanels[i].SetActive(fullScreenType == (FullScreenType)i);
        }
    }
}