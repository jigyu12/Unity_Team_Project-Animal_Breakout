using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameManager_new gameManager;
    private Action<CinemachineVirtualCamera> specificCamAction;

    private float BlendTime = 1f;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);
        {
            CinemachineBlendListCamera.Instruction[] instructionArray = new CinemachineBlendListCamera.Instruction[3];
            instructionArray[0].m_VirtualCamera = gameManager.CameraManager.GetVirtualCamera(0);
            instructionArray[0].m_Hold = 2f;

            instructionArray[1].m_VirtualCamera = gameManager.CameraManager.GetVirtualCamera(1);
            var def1 = new CinemachineBlendDefinition
            {
                m_Style = CinemachineBlendDefinition.Style.EaseIn,
                m_Time = BlendTime
            };
            instructionArray[1].m_Blend = def1;
            instructionArray[1].m_Hold = 0f;

            instructionArray[2].m_VirtualCamera = gameManager.CameraManager.GetVirtualCamera(2);
            var def2 = new CinemachineBlendDefinition
            {
                m_Style = CinemachineBlendDefinition.Style.EaseOut,
                m_Time = BlendTime
            };
            instructionArray[2].m_Blend = def2;

            gameManager.CameraManager.SetBlendListCameraInstructions(instructionArray);
        }

        gameManager.CameraManager.RegisterCameraCoroutine(gameManager.CameraManager.GetVirtualCamera(2), AfterFollowCamActivatedCoroutine);
    }

    private IEnumerator AfterFollowCamActivatedCoroutine(CinemachineVirtualCamera virtualCam)
    {
        yield return new WaitForSeconds(BlendTime);
        gameManager.SetGameState(GameManager_new.GameState.GamePlay);
    }
}
