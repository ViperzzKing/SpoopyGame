using UnityEngine;
using UnityEngine.UIElements;

public class BlacklightToggle : MonoBehaviour
{
    [Header("References")]
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
        if(Input.GetKeyDown(blackLightKeybind))
            ToggleBlacklight();
        
        if(Input.GetKeyDown(flashLightKeybind))
           ToggleFlashlight();
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
}
