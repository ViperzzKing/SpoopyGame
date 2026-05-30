using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Highlight : MonoBehaviour
{
    [Header("Highlighted Object")]
    public GameObject CurrentObject { get; private set; }    // The object currently being looked at
    public bool Interactable { get; private set; }           // Whether the current object can be interacted with
    public OutlineMesh currentHighlight; // The outline component on the current object

    [Header("Player")]
    [SerializeField] private bool debugRay;
    [SerializeField] private float playerReach = 3f;       // How far the player can interact
    [SerializeField] private LayerMask interactableMask;   // Only raycast against these layers

    private Camera mainCamera;
    
    private Vector3 origin;
    private Vector3 lookDirection;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        SetToCameraPosition();
        DrawLineOfSight();
        HandleObjectHighlight();
    }

    // Draws a red debug ray in the editor to visualise player reach
    private void DrawLineOfSight()
    {
        if(debugRay)
            Debug.DrawRay(origin, lookDirection * playerReach, Color.red);
    }

    private void HandleObjectHighlight()
    {
        RaycastHit hit;
        // Raycast forward from camera, only hitting interactable layer objects
        bool playerLookingAtInteractable = Physics.Raycast(origin, lookDirection, out hit, playerReach, interactableMask);

        if (playerLookingAtInteractable)
        {
            GameObject objectHit = hit.collider.gameObject;

            // Only switch if we're looking at a different object than before
            if (objectHit != CurrentObject)
            {
                SwitchHighlight(objectHit);
            }
        }
        else if (CurrentObject != null)
        {
            // Nothing in reach - clear the highlight
            ClearHighlight();
        }
    }

    private void SwitchHighlight(GameObject newObject)
    {
        // Turn off the previous object's outline before switching
        if (currentHighlight != null)
        {
            currentHighlight.enabled = false;
        }

        CurrentObject = newObject;
        Interactable = true;

        // Grab and enable the outline on the new object
        currentHighlight = newObject.GetComponent<OutlineMesh>();

        if (currentHighlight != null)
        {
            currentHighlight.ToggleOutline();
            Debug.Log("Enable highlight on " + newObject.name);
        }
        else
        {
            Debug.LogWarning("No Outline Scrpt found on " + newObject.name);
        }
    }

    private void ClearHighlight()
    {
        // Toggle outline off before clearing the reference
        if (currentHighlight != null)
        {
            currentHighlight.ToggleOutline();
        }

        CurrentObject = null;
        currentHighlight = null;
        Interactable = false;

        Debug.Log("Disable highlight");
    }

    // Update origin and direction to match the camera each frame
    private void SetToCameraPosition()
    {
        if (mainCamera == null) return;

        origin = mainCamera.transform.position;
        lookDirection = mainCamera.transform.forward;
    }
}