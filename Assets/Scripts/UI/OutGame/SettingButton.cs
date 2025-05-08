using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingButton : MonoBehaviour
{
    private Button settingButton;
    private OutGameUIManager outGameUIManager;
    
    private void Start()
    {
        TryGetComponent(out settingButton);

        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
        outGameUIManager = outGameManager.OutGameUIManager;
        
        settingButton.onClick.AddListener(outGameUIManager.ShowSettingPanel);
    }

    private void OnDestroy()
    {
        settingButton.onClick.RemoveAllListeners();
    }
}