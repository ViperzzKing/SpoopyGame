using System;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public Camera cam;
    public CameraControls camControls;
    public BasicMovement movement;
    public HighlightObjects highlight;
    public GameObject onTopVolume;

    public bool playerIsInspecting;
    public Transform currentItemInspecting;
    private Vector3 itemPosition;
    private Quaternion itemRotation;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryToggleInspect();

    }

    private void TryToggleInspect()
    {
        // Check if its a interactable
        bool canInspect = highlight.interactable && highlight.currentObject.CompareTag("Inspectable");

        if (canInspect)
            ToggleInspect();
    }
    
    private void ToggleInspect()
    {
        if (playerIsInspecting)
            StopInspecting();
        else
        {
            Debug.Log("Inspect");
            InspectItem();
        }
    }

    private void InspectItem()
    {

        currentItemInspecting = GetItemToInspectFromHighlight();
        SaveItemPosition(currentItemInspecting);
        
        
        if (currentItemInspecting != null)
        {
            currentItemInspecting.position = cam.transform.position + cam.transform.forward * 2;

            Debug.Log("Inspecting");
            WhenPlayerInspects(inspecting: true);
        }
    }

    private void StopInspecting()
    {
        ReturnItem(currentItemInspecting);
        Debug.Log("Stop Inspecting");
        WhenPlayerInspects(inspecting: false);
    }
    
    private void WhenPlayerInspects(bool inspecting)
    {
        Transform highlightFolder = highlight.currentObject.transform.GetChild(0);
        currentItemInspecting.LookAt(Camera.main.transform);

        highlightFolder.gameObject.SetActive(!inspecting);
        onTopVolume.SetActive(inspecting);
        camControls.enabled = !inspecting;
        movement.enabled = !inspecting;
        playerIsInspecting = inspecting;
    }
    
    private Transform GetItemToInspectFromHighlight()
    {
        if (highlight.currentObject == null)
            return null;

        return FindItemsParent(highlight.currentObject);
    }
    
    private Transform FindItemsParent(GameObject item)
    {
        return item.transform.parent;
    }

    private void SaveItemPosition(Transform item)
    {
        itemRotation = item.rotation;
        itemPosition = item.position;
    }

    private void ReturnItem(Transform item)
    {
        item.rotation = itemRotation;
        item.position = itemPosition;
    }
}
