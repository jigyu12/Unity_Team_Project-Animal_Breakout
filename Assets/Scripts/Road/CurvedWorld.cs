using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWorld : MonoBehaviour
{
    //public float curvature;
    //public float trimming;

    private void Awake()
    {
        //Shader.SetGlobalFloat("_Curvature", 0);
        //Shader.SetGlobalFloat("_Trimming", 0);
    }

    private void Start()
    {
        Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE");
        UpdateShaderValue();
    }

    [ContextMenu("Update Shader Value")]
    public void UpdateShaderValue()
    {
        //Shader.SetGlobalFloat("_Curvature", curvature);
        //Shader.SetGlobalFloat("_Trimming", trimming);

    }

    [ContextMenu("X+")]
    public void SetPositiveX()
    {
        Shader.DisableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE");
        Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE");
    }
    [ContextMenu("Z+")]
    public void SetPositiveZ()
    {
        Shader.DisableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE");
        Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Y_POSITIVE");
    }
}
