using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public InspectObject inspect;
    
    void Update()
    { 
        // Do nothing when not inspecting
        while (!inspect.playerIsInspecting) return;

        float mouseX = Input.GetAxis("Horizontal");
        float mouseY = Input.GetAxis("Vertical");

        Transform item = inspect.currentItemInspecting;
        
        item.Rotate(-mouseY, -mouseX, 0, Space.World);

    }
}
