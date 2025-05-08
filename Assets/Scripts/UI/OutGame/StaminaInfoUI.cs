using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaInfoText;
    [SerializeField] private Button staminaImageButton;
    [SerializeField] private Button plusButton;
    
    [SerializeField] private GameObject StaminaRecoveryPanel;

    private void Awake()
    {
        StaminaSystem.onStaminaChanged += SetStaminaInfoText;
    }

    private void Start()
    {
        staminaImageButton.onClick.AddListener(SwitchActiveStaminaRecoveryPanel);
    }

    private void OnDestroy()
    {
        StaminaSystem.onStaminaChanged -= SetStaminaInfoText;
        staminaImageButton.onClick.RemoveAllListeners();
    }

    private void SetStaminaInfoText(int currentStamina, int maxStamina)
    {
        staminaInfoText.text = $"{currentStamina}/{maxStamina}";
    }

    private void SwitchActiveStaminaRecoveryPanel()
    {
        StaminaRecoveryPanel.SetActive(!StaminaRecoveryPanel.activeSelf);
    }
}