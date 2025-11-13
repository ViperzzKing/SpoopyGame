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
        Transform itemMesh = item.GetChild(0);
        
        itemMesh.Rotate(0, -mouseX, 0, Space.World);
        item.Rotate(-mouseY, 0, 0, Space.Self);

    }
}
