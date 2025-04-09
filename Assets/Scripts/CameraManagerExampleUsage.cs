using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraManagerExampleUsage : MonoBehaviour
{
    private GameManager_new gameManager;
    private CinemachineVirtualCamera specificVirtualCam;
    private Action<CinemachineVirtualCamera> specificCamAction;
    
    private float BlendTime = 1f;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager);
        CameraManager.OnCameraChanged += OnVirtualCameraChanged;
        
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
        
        specificVirtualCam = gameManager.CameraManager.GetVirtualCamera(1);
        specificCamAction = OnSpecificCameraActivated;
        gameManager.CameraManager.RegisterCameraAction(specificVirtualCam, specificCamAction);
        gameManager.CameraManager.RegisterCameraCoroutine(gameManager.CameraManager.GetVirtualCamera(1), AfterSpecificCamActivatedCoroutine);
        gameManager.CameraManager.RegisterCameraCoroutine(gameManager.CameraManager.GetVirtualCamera(2), AfterSpecificCamActivatedCoroutine);
        
        // gameManager.CameraManager.UnregisterCameraAction(specificVirtualCam, specificCamAction);
        // gameManager.CameraManager.UnregisterCameraCoroutine(specificVirtualCam, AfterSpecificCamActivatedCoroutine);
    }

    private void OnDestroy()
    {
        CameraManager.OnCameraChanged -= OnVirtualCameraChanged;
    }

    private void OnVirtualCameraChanged(ICinemachineCamera camera)
    {
        Debug.Log($"OnVirtualCameraChanged: {camera.Name}");
    }

    private void OnSpecificCameraActivated(CinemachineVirtualCamera virtualCam)
    {
        Debug.Log($"OnSpecificCamActivated: {virtualCam.Name}");
    }

    private IEnumerator AfterSpecificCamActivatedCoroutine(CinemachineVirtualCamera virtualCam)
    {
        yield return new WaitForSeconds(BlendTime);
        
        Debug.Log($"Coroutine SpecificCamActivated: {virtualCam.Name}");
    }
}   