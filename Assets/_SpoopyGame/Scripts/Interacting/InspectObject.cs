using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class InspectObject : MonoBehaviour
{
    public Camera cam;
    public CameraControls camControls;
    public BasicMovement movement;
    public Highlight highlight;
    public GameObject onTopVolume;
    public Image crosshair;
    public Image inspectBorder;

    public bool playerIsInspecting;
    public Transform currentItemInspecting;
    private Vector3 itemPosition;
    private Quaternion itemRotation;

    private void Update()
    {
        bool leftMouse = Input.GetMouseButtonDown(0);
        
        if (leftMouse)
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
        currentItemInspecting.localScale = new Vector3(1, 1, 1);
        currentItemInspecting.position = cam.transform.position + cam.transform.forward * 2;
        currentItemInspecting.LookAt(Camera.main.transform);

            Debug.Log("Inspecting");
            WhenPlayerInspects(inspecting: true);
        
    }

    private void StopInspecting()
    {
        currentItemInspecting.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Debug.Log("Stop Inspecting");
        WhenPlayerInspects(inspecting: false);
        ReturnItem(currentItemInspecting);
    }
    
    private void WhenPlayerInspects(bool inspecting)
    {
        CustomPassVolume highlightOutline = highlight.currentHighlight;

        highlightOutline.enabled = !inspecting;
        onTopVolume.SetActive(inspecting);
        camControls.enabled = !inspecting;
        movement.enabled = !inspecting;
        crosshair.gameObject.SetActive(!inspecting);
        inspectBorder.gameObject.SetActive(inspecting);
        
        playerIsInspecting = inspecting;
    }
    
    private Transform GetItemToInspectFromHighlight()
    {
        if (highlight.currentObject == null)
            return null;

        return highlight.currentObject.transform;
    }
    
    private void SaveItemPosition(Transform item)
    {
        itemRotation = item.rotation;
        itemPosition = item.position;
    }

    private void ReturnItem(Transform item)
    {
        Transform itemMesh = currentItemInspecting.GetChild(0);
        
        item.rotation = itemRotation;
        item.position = itemPosition;
        
        itemMesh.localRotation = new Quaternion(0, 0, 0, 1);
        itemMesh.position = item.position;

    }
}
