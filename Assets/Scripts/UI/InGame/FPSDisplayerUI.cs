using TMPro;
using UnityEngine;

public class FPSDisplayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private float deltaTime = 0.0f;

    //private void OnGUI()
    //{      
    //    float ms = Time.deltaTime * 1000f;
    //    float fps = 1.0f / Time.deltaTime;
    //    text.text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);
    //}


    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; // 스무딩

        float fps = 1.0f / deltaTime;
        float ms = deltaTime * 1000.0f;

        text.text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);
    }
}
