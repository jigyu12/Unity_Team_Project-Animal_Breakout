using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentKeyCountPanel : MonoBehaviour
{
    [SerializeField] private Image keyImage;
    [SerializeField] private TMP_Text currentKeyCountText;

    private void Awake()
    {
        GoldAnimalTokenKeySystem.onKeyChanged += SetCurrentKeyCountText;
    }
    
    private void OnDestroy()
    {
        GoldAnimalTokenKeySystem.onKeyChanged -= SetCurrentKeyCountText;
    }
    
    public void SetKeyImage(Sprite keyImage)
    {
        this.keyImage.sprite = keyImage;
    }
    
    public void SetCurrentKeyCountText(int currentKeyCount)
    {
        currentKeyCountText.text = currentKeyCount.ToString();
    }
}