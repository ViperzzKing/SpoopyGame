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

    private PlayerSounds playerSounds;
   
    [Header("Keybinds")] 
    [SerializeField] private KeyCode blackLightKeybind = KeyCode.B;
    [SerializeField] private KeyCode flashLightKeybind = KeyCode.F;

    //means that we dont have to link it in unity, keeps the unity scene clean.
    private void Awake()
    {
        playerSounds = FindAnyObjectByType<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        bool usingCamera = camCorder.CurrentState == CamCorderPositions.CamCorderState.ScreenCamera;

        if (Input.GetKeyDown(blackLightKeybind) && usingCamera)
            ToggleBlacklight();
        
        if(Input.GetKeyDown(flashLightKeybind) && usingCamera)
           ToggleFlashlight();

        if(!usingCamera && (flashLightToggle||blackLightToggle))
            ClearLights();
    }

    private void ToggleBlacklight()
    {
        if (blacklight == null) { Debug.LogError("Missing Blacklight"); return; }
  
        blackLightToggle = !blackLightToggle;
        playerSounds.StopAudio(playerSounds.flashlightSound);
        playerSounds.PlayAudio(playerSounds.flashlightSound);
        blacklight.SetActive(blackLightToggle);
        blackLightVision.SetActive(blackLightToggle);
        
    }
    
    private void ToggleFlashlight()
    {
        if (flashlight == null) { Debug.LogError("Missing Flashlight"); return; }

        flashLightToggle = !flashLightToggle;
        playerSounds.StopAudio(playerSounds.flashlightSound);
        playerSounds.PlayAudio(playerSounds.flashlightSound);
        flashlight.SetActive(flashLightToggle);
        
    }

    private void ClearLights()
    {
        flashLightToggle = false;
        blackLightToggle = false;
        playerSounds.StopAudio(playerSounds.flashlightSound);
        playerSounds.PlayAudio(playerSounds.flashlightSound);
        flashlight.SetActive(flashLightToggle);
        blacklight.SetActive(blackLightToggle);
        blackLightVision.SetActive(false);
    }
}
