using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWorld : MonoBehaviour
{
    //public float curvature;
    //public float trimming;

    public AmazingAssets.CurvedWorld.CurvedWorldController curvedWorldControllerZ;
    public AmazingAssets.CurvedWorld.CurvedWorldController curvedWorldControllerX;

    private void Awake()
    {
        //Shader.SetGlobalFloat("_Curvature", 0);
        //Shader.SetGlobalFloat("_Trimming", 0);
    }

    private void Start()
    {
        Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE");
    }

    private void Update()
    {
        UpdateShaderValue();
    }

    [ContextMenu("Update Shader Value")]
    public void UpdateShaderValue()
    {
        curvedWorldControllerZ.SetBendHorizontalSize(Mathf.Sin(Time.time) * 2f);
        curvedWorldControllerZ.SetBendVerticalSize(Mathf.Sin(Time.time));
        curvedWorldControllerX.SetBendHorizontalSize(Mathf.Sin(Time.time + Mathf.PI) * 2f);
        curvedWorldControllerX.SetBendVerticalSize(Mathf.Sin(Time.time + Mathf.PI));
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
