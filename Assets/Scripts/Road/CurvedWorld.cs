using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWorld : MonoBehaviour
{
    //public float curvature;
    //public float trimming;
    public StageManager stageManager;

    public float maxHorizonCurvatureValue;
    public float maxVirticalCurvatureValue;

    public AmazingAssets.CurvedWorld.CurvedWorldController curvedWorldController;

    private void Awake()
    {
        //Shader.SetGlobalFloat("_Curvature", 0);
        //Shader.SetGlobalFloat("_Trimming", 0);
    }



    private void Start()
    {
        //Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE");
    }

    private void Update()
    {
        //최악. 그런데 그냥 임시로 이렇게 가자
        if (stageManager.IsPlayerInBossStage)
        {
            curvedWorldController.SetBendHorizontalSize(0);
        }
        else
        {
            UpdateShaderValue();
        }
    }

    [ContextMenu("Update Shader Value")]
    public void UpdateShaderValue()
    {
        curvedWorldController.SetBendHorizontalSize(Mathf.Sin(Time.time / 5f) * maxHorizonCurvatureValue);
        //curvedWorldController.SetBendVerticalSize(Mathf.Sin(Time.time/5f)*maxVirticalCurvatureValue);
    }

    //[ContextMenu("X+")]
    //public void SetPositiveX()
    //{
    //    Shader.DisableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE");
    //    Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE");
    //}
    //[ContextMenu("Z+")]
    //public void SetPositiveZ()
    //{
    //    Shader.DisableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE");
    //    Shader.EnableKeyword("CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Y_POSITIVE");
    //}
}
