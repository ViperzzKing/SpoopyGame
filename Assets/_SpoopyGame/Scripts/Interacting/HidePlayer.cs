using System;
using UnityEngine;

public class HidePlayer : MonoBehaviour
{
    // Singleton so other scripts can check hiding state globally
    public static HidePlayer playerHider;

    [Header("References")]
    [SerializeField] CameraControls camControls;
    [SerializeField] Highlight highlight;
    [SerializeField] BasicMovement movement;
    [SerializeField] Rigidbody rb;

    [Header("Hiding")]
    public bool playerIsHiding = false;
    public bool caughtHiding;
    private Vector3 outsidePosition;       // Where the player was before hiding
    private Transform currentHidingSpot;   // The spot the player is currently inside

    //---------------------------------------------------------\\

    private void Awake()
    {
        playerHider = this;
    }

    private void Update()
    {
        bool leftMouse = Input.GetMouseButtonDown(0);

        if (leftMouse)
            TryToggleHiding();

        // While transitioning into hiding, lerp player to the hiding spot
        if (caughtHiding)
        {
            transform.position = Vector3.Lerp(transform.position, currentHidingSpot.position, 0.1f);

            // Keep camera locked to player's head during transition
            Camera.main.transform.position = new Vector3(transform.position.x,
                transform.position.y + camControls.standingEyeOffset,
                transform.position.z);
        }
    }

    //------------------ HIDING HANDLER -------------------------\\

    // Only allow hiding if looking at a HidingSpot, only allow unhiding if already hidden
    private void TryToggleHiding()
    {
        bool canHide = highlight.interactable && !playerIsHiding && highlight.currentObject.CompareTag("HidingSpot");
        bool canUnhide = playerIsHiding;

        if (canHide || canUnhide)
            ToggleHiding();
    }

    // Route to hide or unhide depending on current state
    private void ToggleHiding()
    {
        if (playerIsHiding)
            UnhidePlayer();
        else
            HidePlayerAtSpot();
    }

    private void HidePlayerAtSpot()
    {
        // Save position so we can return here when unhiding
        outsidePosition = transform.position;
        currentHidingSpot = GetHidingSpotFromHighlight();

        if (currentHidingSpot != null)
        {
            // Match camera rotation to the hiding spot's orientation
            Camera.main.transform.localRotation = currentHidingSpot.localRotation;

            Debug.Log("Hide");
            WhenPlayerHides(hidden: true);
        }
    }

    private void UnhidePlayer()
    {
        Debug.Log("Unhide");
        // Snap rigidbody back to where the player was before hiding
        rb.position = outsidePosition;
        WhenPlayerHides(hidden: false);
        currentHidingSpot = null;
    }

    // Handles all state changes when hiding or unhiding
    private void WhenPlayerHides(bool hidden)
    {
        caughtHiding = hidden;

        // Give player a moment before they're fully safe (transition window)
        if (hidden)
        {
            Invoke("SafeFromHiding", 1f);
        }

        // Disable controls while hidden, enable when unhiding
        camControls.enabled = !hidden;
        movement.enabled = !hidden;
        rb.isKinematic = hidden;
        playerIsHiding = hidden;
    }

    // Get the hiding spot transform from whatever the player is looking at
    private Transform GetHidingSpotFromHighlight()
    {
        if (highlight.currentObject == null)
            return null;

        return FindHidingSpot(highlight.currentObject);
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

    // Called after 1 second delay - marks the hiding transition as complete
    private void SafeFromHiding()
    {
        caughtHiding = false;
    }
}