using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : InGameManager
{
    [SerializeField] private CinemachineBlendListCamera blendListCamera;
    private List<CinemachineVirtualCamera> virtualCameraList = new();
    
    private CinemachineBrain cinemachineBrain;
    
    public int virtualCameraCount => virtualCameraList.Count;

    private readonly Dictionary<CinemachineVirtualCamera, List<Action<CinemachineVirtualCamera>>> cameraActionsDictionary = new();
    private readonly Dictionary<CinemachineVirtualCamera, List<Func<CinemachineVirtualCamera, IEnumerator>>> cameraCoroutinesDictionary = new();

    public static Action<ICinemachineCamera> OnCameraChanged;
    
    public override void Initialize()
    {
        base.Initialize();

        SetBlendListCamera(blendListCamera);

        Camera.main.TryGetComponent(out cinemachineBrain);
        cinemachineBrain.m_CameraActivatedEvent.AddListener(OnOtherVirtualCameraActivated);

        foreach (var virtualCamera in virtualCameraList)
        {
            cameraActionsDictionary.Add(virtualCamera, new List<Action<CinemachineVirtualCamera>>());
            cameraCoroutinesDictionary.Add(virtualCamera, new List<Func<CinemachineVirtualCamera, IEnumerator>>());
        }
    }

    public override void Clear()
    {
        base.Clear();

        blendListCamera = null;
        virtualCameraList.Clear();

        if (cinemachineBrain is not null)
        {
            cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnOtherVirtualCameraActivated);
        }
        cinemachineBrain = null;

        cameraActionsDictionary.Clear();
        cameraCoroutinesDictionary.Clear();
        OnCameraChanged = null;
    }

    public void SetBlendListCamera(CinemachineBlendListCamera blendListCamera)
    {
        if (blendListCamera is null)
        {
            Debug.Assert(false, "BlendListCamera is null");
            
            return;
        }

        this.blendListCamera = blendListCamera;

        virtualCameraList.Clear();
        for (int i = 0; i < blendListCamera.ChildCameras.Length; ++i)
        {
            var virtualCamera = blendListCamera.ChildCameras[i] as CinemachineVirtualCamera;
            virtualCameraList.Add(virtualCamera);
        }
    }

    public CinemachineVirtualCamera GetVirtualCamera(int index)
    {
        if (index < 0 || index >= virtualCameraCount)
        {
            Debug.Assert(false, "Invalid Virtual Camera index");
            return null;
        }

        return virtualCameraList[index];
    }

    public void SetBlendListCameraInstructions(CinemachineBlendListCamera.Instruction[] instructionArray)
    {
        blendListCamera.m_Instructions = instructionArray;
    }

    private void OnOtherVirtualCameraActivated(ICinemachineCamera newCam, ICinemachineCamera previousCam)
    {
        if (newCam is null)
        {
            Debug.Assert(false, "New Camera is null");
            
            return;
        }

        if (newCam is CinemachineBlendListCamera)
        {
            return;
        }

        var newVirtualCam = newCam as CinemachineVirtualCamera;

        if (!virtualCameraList.Contains(newVirtualCam))
        {
            Debug.Assert(false, "New Camera is not blendListCamera in Child");
            
            return;
        }

        if (newVirtualCam is null)
        {
            Debug.Assert(false, "New Camera is not Virtual Camera");
            
            return;
        }

        OnCameraChanged?.Invoke(newCam);

        if (cameraActionsDictionary.TryGetValue(newVirtualCam, out var actionList))
        {
            foreach (var action in actionList)
            {
                action?.Invoke(newVirtualCam);
            }
        }
        else
        {
            Debug.Assert(false, "Virtual Camera is not registered");
        }
        
        if (cameraCoroutinesDictionary.TryGetValue(newVirtualCam, out var coroutineList))
        {
            foreach (var coroutine in coroutineList)
            {
                StartCoroutine(coroutine(newVirtualCam));
            }
        }
        else
        {
            Debug.Assert(false, "Virtual Camera is not registered");
        }
    }

    public void RegisterCameraAction(CinemachineVirtualCamera virtualCam, Action<CinemachineVirtualCamera> action)
    {
        if (virtualCam is null)
        {
            Debug.Assert(false, "Virtual Camera is null");
            
            return;
        }

        if (action is null)
        {
            Debug.Assert(false, "Action is null");
            
            return;
        }

        cameraActionsDictionary[virtualCam].Add(action);
    }

    public void UnregisterCameraAction(CinemachineVirtualCamera virtualCam, Action<CinemachineVirtualCamera> action)
    {
        if (virtualCam is null)
        {
            Debug.Assert(false, "Virtual Camera is null");
            
            return;
        }

        if (action is null)
        {
            Debug.Assert(false, "Action is null");
            
            return;
        }

        if (cameraActionsDictionary.TryGetValue(virtualCam, out var actionList))
        {
            if (!actionList.Contains(action))
            {
                Debug.Assert(false, "Action is not registered");
                
                return;
            }
            
            actionList.Remove(action);
        }
        else
        {
            Debug.Assert(false, "Virtual Camera is not registered");
        }
    }

    public void RegisterCameraCoroutine(CinemachineVirtualCamera virtualCam, Func<CinemachineVirtualCamera, IEnumerator> coroutine)
    {
        if (virtualCam is null)
        {
            Debug.Assert(false, "Virtual Camera is null");
            
            return;
        }

        if (coroutine is null)
        {
            Debug.Assert(false, "Coroutine is null");
            
            return;
        }

        cameraCoroutinesDictionary[virtualCam].Add(coroutine);
    }
    
    public void UnregisterCameraCoroutine(CinemachineVirtualCamera virtualCam, Func<CinemachineVirtualCamera, IEnumerator> coroutine)
    {
        if (virtualCam is null)
        {
            Debug.Assert(false, "Virtual Camera is null");
            
            return;
        }

        if (coroutine is null)
        {
            Debug.Assert(false, "Coroutine is null");
            
            return;
        }

        if (cameraCoroutinesDictionary.TryGetValue(virtualCam, out var coroutineList))
        {
            if (!coroutineList.Contains(coroutine))
            {
                Debug.Assert(false, "Coroutine is not registered");
                
                return;
            }
            
            coroutineList.Remove(coroutine);
        }
        else
        {
            Debug.Assert(false, "Virtual Camera is not registered");
        }
    }
    
    public void ActivateCameraByIndex(int index)
    {
        CinemachineBlendListCamera.Instruction[] instructionArray = new CinemachineBlendListCamera.Instruction[1];
        
        instructionArray[0].m_VirtualCamera = GetVirtualCamera(index);
        
        SetBlendListCameraInstructions(instructionArray);
    }
}