using System;
using Unity.Mathematics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class InspectObject : MonoBehaviour
{
    public Camera cam;
    public Rigidbody runeRB;
    public CameraControls camControls;
    public BasicMovement movement;
    public Highlight highlight;
    public GameObject onTopVolume;
    public Image crosshair;
    public Image inspectBorder;
    public RuneSlot runeSlot;

    public bool playerIsInspecting;
    public bool playerHoldingSomething;
    public Transform currentItemInspecting;
    public Transform currentItemHolding;
    private Vector3 itemPosition;
    private Quaternion itemRotation;

    private void Update()
    {
        bool leftMouse = Input.GetMouseButtonDown(0);
        bool eButton = Input.GetKeyDown(KeyCode.E);
        
        if (leftMouse)
            TryToggleInspect();
        
        if (eButton)
            TogglePickup();

    }

    private void TogglePickup()
    {
        bool canPickup = !playerHoldingSomething;

        if (canPickup && playerIsInspecting)
            PickUpItem();
        else if (playerHoldingSomething && !playerIsInspecting)
            DropItem();
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

    private void PickUpItem()
    {
        WhenPlayerInspects(false);
        currentItemInspecting.parent = cam.transform;
        currentItemInspecting.localPosition = new Vector3(-0.27f, -0.1f, 0.36f);
        currentItemInspecting.localRotation = Quaternion.identity;

        SaveItemPosition(currentItemInspecting);
        ReturnItem(currentItemInspecting);

        currentItemInspecting.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        currentItemInspecting.GetComponent<BoxCollider>().isTrigger = true;

        currentItemHolding = currentItemInspecting;
        runeRB.isKinematic = true;

        playerHoldingSomething = true;
    }

    private void DropItem()
    {
        Vector3 location = DropLocation.dropLocation.DropAtLocation();

        currentItemHolding.parent = null;
        currentItemHolding.position = location;
        currentItemHolding.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        currentItemHolding.GetComponent<BoxCollider>().isTrigger = false;

        runeRB.isKinematic = false;
        playerHoldingSomething = false;
        currentItemHolding = null;
    }
    
    private void InspectItem()
    {
        currentItemInspecting = GetItemToInspectFromHighlight();
        runeRB = currentItemInspecting.GetComponent<Rigidbody>();
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
        OutlineMesh highlightOutline = highlight.currentHighlight;
        runeSlot = currentItemInspecting.GetComponent<RuneSlot>();
        
        highlightOutline.ToggleOutline();
        onTopVolume.SetActive(inspecting);
        camControls.enabled = !inspecting;
        movement.enabled = !inspecting;
        crosshair.gameObject.SetActive(!inspecting);
        inspectBorder.gameObject.SetActive(inspecting);
        runeRB.isKinematic = inspecting;
        playerIsInspecting = inspecting;
        runeSlot.enabled = !inspecting;
    }
    
    private Transform GetItemToInspectFromHighlight()
    {
        if (highlight.currentObject == null)
            return null;

        return highlight.currentObject.transform;
    }
    
    public void SaveItemPosition(Transform item)
    {
        itemRotation = item.rotation;
        itemPosition = item.position;
    }

    public void ReturnItem(Transform item)
    {
        Transform itemMesh = item.GetChild(0);

        item.rotation = itemRotation;
        item.position = itemPosition;

        itemMesh.localRotation = Quaternion.identity;
        itemMesh.position = item.position;
    }
}
