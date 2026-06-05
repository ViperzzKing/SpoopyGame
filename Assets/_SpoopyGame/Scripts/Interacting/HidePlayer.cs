using System;
using UnityEngine;

public class HidePlayer : MonoBehaviour
{
    // Singleton so other scripts can check hiding state globally
    public static HidePlayer Instance;

    [Header("References")]
    [SerializeField] CameraControls camControls;
    [SerializeField] Highlight highlight;
    [SerializeField] BasicMovement movement;
    [SerializeField] Rigidbody rb;
    private Camera mainCamera;

    [Header("Hiding")]
    [field: SerializeField] public bool PlayerIsHiding { get; private set; }
    public bool caughtHiding;
    private Vector3 outsidePosition;       // Where the player was before hiding
    private Transform currentHidingSpot;   // The spot the player is currently inside

    //---------------------------------------------------------\\

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogWarning("No main camera found");
        }
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody found");
        }
        if (movement == null)
        {
            Debug.LogWarning("No movement found");
        }
        if (highlight == null)
        {
            Debug.LogWarning("No highlight found");
        }
    }

    private void Update()
    {
        bool leftMouse = Input.GetMouseButtonDown(0);

        if (leftMouse)
        {
            if(!NullChecks()) return;
            TryToggleHiding();
            
        }

        // While transitioning into hiding, lerp player to the hiding spot
        if (caughtHiding)
        {
            transform.position = Vector3.Lerp(transform.position, currentHidingSpot.position, 0.1f);

            // Keep camera locked to player's head during transition
            mainCamera.transform.position = new Vector3(transform.position.x,
                transform.position.y + camControls.StandingEyeOffset,
                transform.position.z);
        }
    }

    //------------------ HIDING HANDLER -------------------------\\

    // Only allow hiding if looking at a HidingSpot, only allow unhiding if already hidden
    private void TryToggleHiding()
    {
        bool canHide = highlight.Interactable && !PlayerIsHiding && highlight.CurrentObject.CompareTag("HidingSpot");
        bool canUnhide = PlayerIsHiding;

        if (canHide || canUnhide)
        {
            if (PlayerIsHiding)
                Unhide();
            else
                HideAtCurrentSpot();
        }
    }

    private void HideAtCurrentSpot()
    {
        // Save position so we can return here when unhiding
        outsidePosition = transform.position;
        currentHidingSpot = GetHidingSpotFromHighlight();

        if (currentHidingSpot != null)
        {
            // Match camera rotation to the hiding spot's orientation
            mainCamera.transform.localRotation = currentHidingSpot.localRotation;

            Debug.Log("Hide");
            SetHiddenState(hidden: true);
        }
    }

    private void Unhide()
    {
        Debug.Log("Unhide");
        // Snap rigidbody back to where the player was before hiding
        rb.position = outsidePosition;
        SetHiddenState(hidden: false);
        currentHidingSpot = null;
    }

    // Handles all state changes when hiding or unhiding
    private void SetHiddenState(bool hidden)
    {
        caughtHiding = hidden;

        // Give player a moment before they're fully safe (transition window)
        if (hidden)
        {
            caughtHiding = false;
        }

        // Disable controls while hidden, enable when unhiding
        camControls.enabled = !hidden;
        movement.enabled = !hidden;
        rb.isKinematic = hidden;
        PlayerIsHiding = hidden;
    }

    // Get the hiding spot transform from whatever the player is looking at
    private Transform GetHidingSpotFromHighlight()
    {
        if (highlight.CurrentObject == null)
            return null;

        return FindHidingSpot(highlight.CurrentObject);
    }

    // Look through children of the object for one tagged HidingSpot
    private Transform FindHidingSpot(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            if (child.CompareTag("HidingSpot"))
                return child;
        }
        return null;
    }

    private bool NullChecks()
    {
        if (mainCamera == null || rb == null || movement == null || highlight == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}