using UnityEngine;
using UnityEngine.UI;

public class ToggleImageSwitcher : MonoBehaviour
{
    private Toggle toggle;
    private Image targetImage;
    [SerializeField] private Sprite firstImage;
    [SerializeField] private Sprite secondImage;

    void Start()
    {
        TryGetComponent(out toggle);
        targetImage = GetComponentInChildren<Image>();
        
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(ChangeImage);
        
        ChangeImage(toggle.isOn);
    }

    private void ChangeImage(bool isOn)
    {
        targetImage.sprite = isOn ? firstImage : secondImage;
    }
}