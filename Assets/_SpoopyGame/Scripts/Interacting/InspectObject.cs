using System;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public Camera cam;
    public CameraControls camControls;
    public HighlightObjects highlight;
    public BasicMovement movement;

    public Transform rune;
    
    private void Update()
    {
        if (highlight.interactable == true && highlight.currentObject.CompareTag("Inspectable") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Inspecting");

            movement.enabled = false;
            camControls.enabled = false;

            rune.position = cam.transform.position + cam.transform.forward * 1f;
        }
    }
}
