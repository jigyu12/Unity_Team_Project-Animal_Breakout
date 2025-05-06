using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentKeyCountPanel : MonoBehaviour
{
    [SerializeField] private Image keyImage;
    [SerializeField] private TMP_Text currentKeyCountText;

    private void Start()
    {
        SetCurrentKeyCount(0);
    }
    
    public void SetKeyImage(Sprite keyImage)
    {
        this.keyImage.sprite = keyImage;
    }
    
    public void SetCurrentKeyCount(int currentKeyCount)
    {
        currentKeyCountText.text = currentKeyCount.ToString();
    }
}