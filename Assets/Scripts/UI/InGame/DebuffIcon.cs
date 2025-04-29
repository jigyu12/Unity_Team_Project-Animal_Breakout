using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebuffIcon : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text countText;

    public void UpdateCountText(int count)
    {
        if (countText != null)
        {
            // countText.text = remainingCount > 0 ? remainingCount.ToString() : "";
            countText.text = count.ToString();
        }
    }
}
