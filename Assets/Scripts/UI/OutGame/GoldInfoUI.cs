using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldInfoText;
    [SerializeField] private Button goldImageButton;
    [SerializeField] private Button plusButton;

    private void Awake()
    {
        GameDataManager.OnGoldChangedInGameDataManager += SetGoldInfoText;
    }

    private void OnDestroy()
    {
        GameDataManager.OnGoldChangedInGameDataManager -= SetGoldInfoText;
    }
    
    private void SetGoldInfoText(long currentGold)
    {
        goldInfoText.text = currentGold.ToString();
    }
}