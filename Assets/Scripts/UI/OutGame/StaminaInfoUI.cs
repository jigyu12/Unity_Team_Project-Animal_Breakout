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
    
    [SerializeField] private TMP_Text nextRecoveryTimeText;
    [SerializeField] private TMP_Text totalRecoveryTimeText;
    private float lastRecoverySecond;
    private bool isLastRecoverySecondSaved;

    private void Awake()
    {
        StaminaSystem.onStaminaChanged += SetStaminaInfoText;

        lastRecoverySecond = 0f;
        isLastRecoverySecondSaved = false;
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

    private void Update()
    {
        if (!GameDataManager.Instance.StaminaSystem.IsStaminaFull)
        {
            if (!isLastRecoverySecondSaved)
            {
                isLastRecoverySecondSaved = true;
                lastRecoverySecond = GameDataManager.Instance.StaminaSystem.GetLeftTimeToGetNextStamina();

                SetNextRecoveryTimeText(lastRecoverySecond);
                SetTotalRecoveryTimeText(lastRecoverySecond + 
                                         (StaminaSystem.maxStaminaCanFilled
                                         - GameDataManager.Instance.StaminaSystem.CurrentStamina
                                         - 1) * StaminaSystem.TimeToGetNextStamina);
            }
            else
            {
                if (lastRecoverySecond - GameDataManager.Instance.StaminaSystem.GetLeftTimeToGetNextStamina() >= 1f)
                {
                    isLastRecoverySecondSaved = false;
                }
            }
        }
        else
        {
            lastRecoverySecond = 0f;
            
            SetNextRecoveryTimeText(0f);
            SetTotalRecoveryTimeText(0f);
        }
    }

    private void SetStaminaInfoText(int currentStamina, int maxStamina)
    {
        staminaInfoText.text = $"{currentStamina}/{maxStamina}";
    }

    private void SwitchActiveStaminaRecoveryPanel()
    {
        StaminaRecoveryPanel.SetActive(!StaminaRecoveryPanel.activeSelf);
    }

    private void SetNextRecoveryTimeText(float nextRecoveryTime)
    {
        int minutes = (int)nextRecoveryTime / 60;
        int seconds = (int)nextRecoveryTime % 60;
        
        nextRecoveryTimeText.text = $"{minutes:00} : {seconds:00}";
    }
    
    private void SetTotalRecoveryTimeText(float totalRecoveryTime)
    {
        int minutes = (int)totalRecoveryTime / 60;
        int seconds = (int)totalRecoveryTime % 60;
        
        totalRecoveryTimeText.text = $"{minutes:00} : {seconds:00}";
    }
}