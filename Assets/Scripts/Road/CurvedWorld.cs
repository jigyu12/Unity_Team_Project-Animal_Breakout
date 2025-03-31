using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWorld : MonoBehaviour
{
    public float curvature;
    public float trimming;

    private void Awake()
    {
        Shader.SetGlobalFloat("_Curvature", 0);
        Shader.SetGlobalFloat("_Trimming", 0);
    }

    private void Start()
    {
        UpdateShaderValue();
    }

    [ContextMenu("Update Shader Value")]
    public void UpdateShaderValue()
    {
        Shader.SetGlobalFloat("_Curvature", curvature);
        Shader.SetGlobalFloat("_Trimming", trimming);
    }
}
