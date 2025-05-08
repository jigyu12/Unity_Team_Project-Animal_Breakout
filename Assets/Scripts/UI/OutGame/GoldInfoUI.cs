using System;
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
        GoldAnimalTokenKeySystem.onGoldChanged += SetGoldInfoText;
    }

    private void Start()
    {
        SetGoldInfoText(GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentGolds);
    }

    private void OnDestroy()
    {
        GoldAnimalTokenKeySystem.onGoldChanged -= SetGoldInfoText;
    }

    private void SetGoldInfoText(long currentGold)
    {
        goldInfoText.text = currentGold.ToString();
    }
}