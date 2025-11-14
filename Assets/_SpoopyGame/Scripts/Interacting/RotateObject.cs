using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public InspectObject inspect;
    
    void Update()
    { 
        // Do nothing when not inspecting
        while (!inspect.playerIsInspecting) return;



        Transform item = inspect.currentItemInspecting;
        Transform itemMesh = item.GetChild(0);
        
        // WASD Inputs
        float inputY = Input.GetAxis("Horizontal");
        float inputX = Input.GetAxis("Vertical");
        
        item.Rotate(-inputX, 0, 0, Space.Self); 
        itemMesh.Rotate(0, -inputY, 0, Space.World);
        
        // Mouse Inputs
        float mouseY = Input.GetAxis("Mouse Y") * 3;
        float mouseX = Input.GetAxis("Mouse X") * 3;
        
        itemMesh.Rotate(0, -mouseX, 0, Space.World);
        item.Rotate(-mouseY, 0, 0, Space.Self);

    }
}
