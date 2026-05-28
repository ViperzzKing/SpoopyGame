using System;
using UnityEngine;

public class CamCorderPositions : MonoBehaviour
{
    public Vector3[] camPositions = new Vector3[3];
    public Vector3[] camRotations = new Vector3[3];

    public bool noCameraKeybind;
    public bool pauseCameraKeybind;
    public bool screenCameraKeybind;

    public bool cameraOut;
    public bool pauseReady;
    public bool screenCamera;

    private void Update()
    {
        PositionKeybinds();
    }

    private void PositionKeybinds()
    {
        noCameraKeybind = cameraOut && Input.GetMouseButtonDown(1) || pauseReady && Input.GetMouseButtonDown(1) 
                                                                   || pauseReady && Input.GetKeyDown(KeyCode.Escape)
                                                                   || screenCamera && Input.GetKeyDown(KeyCode.Escape);
        pauseCameraKeybind = Input.GetKeyDown(KeyCode.Escape);
        screenCameraKeybind = !cameraOut && Input.GetMouseButtonDown(1) || pauseReady && Input.GetMouseButtonDown(0);

        if (noCameraKeybind)
        {
            screenCamera = false;
            pauseReady = false;
            cameraOut = false;
            transform.localPosition = camPositions[0];
            transform.localRotation = Quaternion.Euler(camRotations[0]);
        }
        else if (pauseCameraKeybind)
        {
            screenCamera = false;
            pauseReady = true;
            transform.localPosition = camPositions[1];
            transform.localRotation = Quaternion.Euler(camRotations[1]);
        }
        else if (screenCameraKeybind)
        {
            screenCamera = true;
            pauseReady = false;
            cameraOut = true;
            transform.localPosition = camPositions[2];
            transform.localRotation = Quaternion.Euler(camRotations[2]);
        }
    }
}
