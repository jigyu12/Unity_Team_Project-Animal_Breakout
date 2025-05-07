using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaInfoText;
    [SerializeField] private Button staminaImageButton;
    [SerializeField] private Button plusButton;

    private void Awake()
    {
        StaminaSystem.onStaminaChanged += SetStaminaInfoText;
    }

    private void OnDestroy()
    {
        StaminaSystem.onStaminaChanged -= SetStaminaInfoText;
    }

    private void SetStaminaInfoText(int currentStamina, int maxStamina)
    {
        staminaInfoText.text = $"{currentStamina}/{maxStamina}";
    }
}