using UnityEngine;
using UnityEngine.UI;

public class SliderColorController : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;

    void Update()
    {
        // float value = slider.value / slider.maxValue;
        // fillImage.color = GetColorByValue(value);
    }

    Color GetColorByValue(float value)
    {
        if (value > 0.5f)
        {
            float t = (value - 0.5f) * 2f;
            return Color.Lerp(Color.yellow, Color.green, t);
        }
        else
        {
            float t = value * 2f;
            return Color.Lerp(Color.red, Color.yellow, t);
        }
    }
}
