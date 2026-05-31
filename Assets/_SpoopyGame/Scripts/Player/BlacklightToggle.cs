using UnityEngine;
using UnityEngine.UIElements;

public class BlacklightToggle : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private CamCorderPositions camCorder;
    [SerializeField] private GameObject blacklight;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject blackLightVision;

    [Header("LightToggles")]
    [SerializeField] private bool blackLightToggle;
    [SerializeField] private bool flashLightToggle;

    [Header("Keybinds")] 
    [SerializeField] private KeyCode blackLightKeybind = KeyCode.B;
    [SerializeField] private KeyCode flashLightKeybind = KeyCode.F;

    // Update is called once per frame
    void Update()
    {
        bool usingCamera = camCorder.CurrentState == CamCorderPositions.CamCorderState.ScreenCamera;

        if (Input.GetKeyDown(blackLightKeybind) && usingCamera)
            ToggleBlacklight();
        
        if(Input.GetKeyDown(flashLightKeybind) && usingCamera)
           ToggleFlashlight();

        if(!usingCamera)
            ClearLights();
    }

    private void ToggleBlacklight()
    {
        if (blacklight == null) { Debug.LogError("Missing Blacklight"); return; }
  
        blackLightToggle = !blackLightToggle;
        blacklight.SetActive(blackLightToggle);
        blackLightVision.SetActive(blackLightToggle);
        
    }
    
    private void ToggleFlashlight()
    {
        if (flashlight == null) { Debug.LogError("Missing Flashlight"); return; }

        flashLightToggle = !flashLightToggle;
        flashlight.SetActive(flashLightToggle);
        
    }

    private void ClearLights()
    {
        flashLightToggle = false;
        blackLightToggle = false;
        flashlight.SetActive(flashLightToggle);
        blacklight.SetActive(blackLightToggle);
    }
}
