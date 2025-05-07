using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImageSwitcher : MonoBehaviour
{
    private Toggle toggle;
    private Image targetImage;
    [SerializeField] private Sprite firstImage;
    [SerializeField] private Sprite secondImage;

    private void Start()
    {
        TryGetComponent(out toggle);
        targetImage = GetComponentInChildren<Image>();
        
        toggle.onValueChanged.AddListener(ChangeImage);
        
        ChangeImage(toggle.isOn);
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    private void ChangeImage(bool isOn)
    {
        targetImage.sprite = isOn ? firstImage : secondImage;
    }
}