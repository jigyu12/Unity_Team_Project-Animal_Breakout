using TMPro;
using UnityEngine;

public class FPSDisplayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private void OnGUI()
    {      
        float ms = Time.deltaTime * 1000f;
        float fps = 1.0f / Time.deltaTime;
        text.text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);
    }
}
