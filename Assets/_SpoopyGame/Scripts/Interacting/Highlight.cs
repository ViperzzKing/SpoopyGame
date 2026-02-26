using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Highlight : MonoBehaviour
{
    [Header("Highlighted Object")]
    public GameObject currentObject;
    public bool interactable;
    public CustomPassVolume currentHighlight;
    
    [Header("Player")]
    [SerializeField] private float playerReach = 3f;
    [SerializeField] private LayerMask interactableMask;

    private Vector3 origin;
    private Vector3 lookDirection;

    private void Update()
    {
        SetToCameraPosition();
        DrawLineOfSight();
        HandleObjectHighlight();
    }

    private void DrawLineOfSight()
    {
        Debug.DrawRay(origin, lookDirection * playerReach, Color.red);
    }

    private void HandleObjectHighlight()
    {
        RaycastHit hit;
        bool playerLookingAtInteractable = Physics.Raycast(origin, lookDirection, out hit, playerReach, interactableMask);

        if (playerLookingAtInteractable)
        {
            GameObject objectHit = hit.collider.gameObject;

            if (objectHit != currentObject)
            {
                SwitchHighlight(objectHit);
            }
        }
        else if (currentObject != null)
        {
            DisableHighlight();
        }
    }

    private void SwitchHighlight(GameObject newObject)
    {
        // Disable previous objects highlight
        if (currentHighlight != null)
        {
            currentHighlight.enabled = false;
        }

        currentObject = newObject;
        interactable = true;

        // Grab the highlight from the new object
        currentHighlight = newObject.GetComponent<CustomPassVolume>();

        if (currentHighlight != null)
        {
            currentHighlight.enabled = true;
            Debug.Log("Enable highlight on " + newObject.name);
        }
        else
        {
            Debug.LogWarning("No CustomPassVolume found on " + newObject.name);
        }
    }

    private void DisableHighlight()
    {
        if (currentHighlight != null)
        {
            currentHighlight.enabled = false;
        }

        currentObject = null;
        currentHighlight = null;
        interactable = false;

        Debug.Log("Disable highlight");
    }

    private void SetToCameraPosition()
    {
        if (Camera.main == null) return;

        origin = Camera.main.transform.position;
        lookDirection = Camera.main.transform.forward;
    }
}
