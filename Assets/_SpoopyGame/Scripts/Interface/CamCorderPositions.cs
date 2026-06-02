using System;
using UnityEngine;

public class CamCorderPositions : MonoBehaviour
{
    [SerializeField] private Vector3 hiddenPosition, hiddenRotation;
    [SerializeField] private Vector3 pausePosition, pauseRotation;
    [SerializeField] private Vector3 screenPosition, screenRotation;

    public bool NoCameraKeybind { get; private set; }
    public bool PauseCameraKeybind { get; private set; }
    public bool ScreenCameraKeybind { get; private set; }

    public bool PauseReady => CurrentState == CamCorderState.PauseReady;
    public bool ScreenCamera => CurrentState == CamCorderState.ScreenCamera;

    public bool CameraOut => CurrentState == CamCorderState.ScreenCamera || CurrentState == CamCorderState.PauseReady;

    public enum CamCorderState
    {
        Hidden,
        PauseReady,
        ScreenCamera
    }

    public CamCorderState CurrentState { get; private set; }
    
    private void Update()
    {
        PositionKeybinds();
    }

    public void SetState(CamCorderState newState)
    {
        CurrentState = newState;
        ApplyTransform();
    }

    private void PositionKeybinds()
    {
        NoCameraKeybind = CameraOut && Input.GetMouseButtonDown(1) || PauseReady && Input.GetMouseButtonDown(1)
                                                                   || ScreenCamera && Input.GetKeyDown(KeyCode.Escape)
                                                                   || ScreenCamera && Input.GetMouseButtonDown(1);
        
        ScreenCameraKeybind = !CameraOut && Input.GetMouseButtonDown(1) || PauseReady && Input.GetMouseButtonDown(0);

        
        // just the keybinds ^
        if (NoCameraKeybind)
            SetState(CamCorderState.Hidden);
        else if (ScreenCameraKeybind)
            SetState(CamCorderState.ScreenCamera);
        
    }

    private void ApplyTransform()
    {
        switch (CurrentState)
        {
            case CamCorderState.Hidden:
                transform.localPosition = hiddenPosition;
                transform.localRotation = Quaternion.Euler(hiddenRotation);
                break;
            case CamCorderState.PauseReady:
                transform.localPosition = pausePosition;
                transform.localRotation = Quaternion.Euler(pauseRotation);
                break;
            case CamCorderState.ScreenCamera:
                transform.localPosition = screenPosition;
                transform.localRotation = Quaternion.Euler(screenRotation);
                break;
        }
    }
}
