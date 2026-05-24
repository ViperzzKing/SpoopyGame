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

    private bool itemDropInput;
    [Header("Input Config")]
    [Tooltip("The key needed to drop or throw an obejct.")]
    [SerializeField] private KeyCode dropKey = KeyCode.E;
    [Tooltip("The amount of seconds needed to hold the input to throw instead of drop.")]
    [SerializeField] private float inputThrowTime = 1f;

    [HideInInspector] public float timePressingDropkey;
    
    [Header("Throwing Config")]
    [Tooltip("A multiplier for the force of which the obejct is thrown.")]
    [SerializeField] private float throwForce = 12f;
    [Tooltip("An added ofset to the force for height. The higher the value, the higher the object will be initially thrown.")]
    [SerializeField] private float throwArc = 2f;

    private void Update()
    {
        bool leftMouse = Input.GetMouseButtonDown(0);
        bool eButton = Input.GetKeyDown(dropKey);
        
        if (leftMouse)
            TryToggleInspect();
        
        if (eButton)
            TogglePickup();
        if (itemDropInput)
            throwInputManagment();
    }

    private void TogglePickup()
    {
        bool canPickup = !playerHoldingSomething;

        if (canPickup && playerIsInspecting)
            PickUpItem();
        else if (playerHoldingSomething && !playerIsInspecting)
            itemDropInput = true;
    }

    private void throwInputManagment()
    {
        timePressingDropkey += Time.deltaTime;
        if (Input.GetKeyUp(dropKey))
        {
            if(timePressingDropkey < inputThrowTime)
                DropItem(false);
            else
                DropItem(true);
            itemDropInput = false;
            timePressingDropkey = 0;
        }
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

    private void DropItem(bool isThrowing)
    {
        Vector3 location = DropLocation.dropLocation.DropAtLocation();

        currentItemHolding.parent = null;
        currentItemHolding.position = location;
        currentItemHolding.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        currentItemHolding.GetComponent<BoxCollider>().isTrigger = false;

        runeRB.isKinematic = false;
        playerHoldingSomething = false;
        currentItemHolding = null;

        //Uses Unitys RigidBody system to add a force to the object if it is being thrown.
        if (isThrowing)
        {
            runeRB.AddForce(cam.transform.forward * throwForce + new Vector3(0, throwArc, 0), ForceMode.VelocityChange);
        }
    }
    
    private void InspectItem()
    {
        currentItemInspecting = GetItemToInspectFromHighlight();
        runeRB = currentItemInspecting.GetComponent<Rigidbody>();
        SaveItemPosition(currentItemInspecting);
        currentItemInspecting.localScale = new Vector3(0.85f, 0.85f, 0.85f);
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
        runeRB.isKinematic = inspecting;
        OutlineMesh highlightOutline = highlight.currentHighlight;
        runeSlot = currentItemInspecting.GetComponent<RuneSlot>();
        
        highlightOutline.ToggleOutline();
        onTopVolume.SetActive(inspecting);
        camControls.enabled = !inspecting;
        movement.enabled = !inspecting;
        crosshair.gameObject.SetActive(!inspecting);
        inspectBorder.gameObject.SetActive(inspecting);
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
