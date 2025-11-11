using UnityEngine;

public class HidePlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] CameraControls camControls;
    [SerializeField] HighlightObjects highlight;
    [SerializeField] BasicMovement movement;
    [SerializeField] Rigidbody rb;

    [Header("Hiding")]
    [SerializeField] private bool playerIsHiding = false;
    private Vector3 outsidePosition;
    private Transform currentHidingSpot;
    

    //---------------------------------------------------------\\


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryToggleHiding();
    }


    //------------------ HIDING HANDLER -------------------------\\


    private void TryToggleHiding()
    {
        // Check if its a hiding spot
        bool canHide = highlight.interactable && !playerIsHiding && highlight.currentObject.CompareTag("HidingSpot");
        bool canUnhide = playerIsHiding;

        if (canHide || canUnhide)
            ToggleHiding();
    }

    private void ToggleHiding()
    {
        if (playerIsHiding)
            UnhidePlayer();
        else
            HidePlayerAtSpot();
    }

    private void HidePlayerAtSpot()
    {
        outsidePosition = transform.position;
        currentHidingSpot = GetHidingSpotFromHighlight();

        if (currentHidingSpot != null)
        {
            // Teleport player to hiding spot
            transform.position = currentHidingSpot.position;
            cam.transform.localRotation = currentHidingSpot.localRotation;

            cam.transform.position = new Vector3(transform.position.x, 
                                                 transform.position.y + camControls.standingEyeOffset, 
                                                 transform.position.z);

            Debug.Log("Hide");
            WhenPlayerHides(hidden: true);
        }

    }

    private void UnhidePlayer()
    {
        Debug.Log("Unhide");
        rb.position = outsidePosition;
        WhenPlayerHides(hidden: false);
        currentHidingSpot = null;
    }

    private void WhenPlayerHides(bool hidden)
    {
        camControls.enabled = !hidden;
        movement.enabled = !hidden;
        rb.isKinematic = hidden;
        playerIsHiding = hidden;
    }

    private Transform GetHidingSpotFromHighlight()
    {
        if (highlight.currentObject == null)
            return null;

        return FindHidingSpot(highlight.currentObject);
    }

    private Transform FindHidingSpot(GameObject obj)
    {
        // Check Teleport Position
        foreach (Transform child in obj.transform)
        {
            if (child.CompareTag("HidingSpot"))
                return child;
        }
        return null;
    }
}
