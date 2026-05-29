
using UnityEngine;
using UnityEngine.UI;

public class InspectObject : MonoBehaviour
{
    public Camera cam; // gets camera.main
    public Rigidbody runeRB; // gets runes rigidbody
    public CameraControls camControls; // camera controller
    public BasicMovement movement; // player controller
    public Highlight highlight; // highlight script
    public GameObject onTopVolume; // Overlay Volume for when rune is in ground while inspecting
    public Image crosshair;
    public Image inspectBorder;
    public RuneSlot runeSlot; // the script to manage the rune slots

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
        
        // if left mouse key try toggle inspect function
        if (leftMouse)
            TryToggleInspect();
        
        // if ebutton toggle pickup
        if (eButton)
            TogglePickup();
        if (itemDropInput) // throw manager
            throwInputManagment();
    }

    private void TogglePickup()
    {
        bool canPickup = !playerHoldingSomething;

        // only pickup when player is inspecting
        if (canPickup && playerIsInspecting)
            PickUpItem();
        else if (playerHoldingSomething && !playerIsInspecting) // drop instead of pickup
            itemDropInput = true;
    }

    //item throw manager
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
        // set all WhenPlayInspects to false
        WhenPlayerInspects(false);
        currentItemInspecting.parent = cam.transform; // change parant to camera
        currentItemInspecting.localPosition = new Vector3(-0.27f, -0.1f, 0.36f); // change position
        currentItemInspecting.localRotation = Quaternion.identity; // reset rotation

        SaveItemPosition(currentItemInspecting); // save for stop inspection
        ReturnItem(currentItemInspecting); // return for stop inspection

        currentItemInspecting.localScale = new Vector3(0.2f, 0.2f, 0.2f); // change size
        currentItemInspecting.GetComponent<BoxCollider>().isTrigger = true; // turn collider to trigger

        currentItemHolding = currentItemInspecting; // set item holding to item inspecting
        runeRB.isKinematic = true; // keep the rune in place

        playerHoldingSomething = true; // bool for checks
    }

    private void DropItem(bool isThrowing)
    {
        // location used DropAtLocation()
        Vector3 location = DropLocation.dropLocation.DropAtLocation();

        currentItemHolding.parent = null; // get rid of the parant
        currentItemHolding.position = location; // drop where looking
        currentItemHolding.localScale = new Vector3(0.5f, 0.5f, 0.5f); // change size
        currentItemHolding.GetComponent<BoxCollider>().isTrigger = false; // turn off trigger

        runeRB.isKinematic = false; // turn these off
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
        
        SaveItemPosition(currentItemInspecting); // save item position for stop inspecting
        
        currentItemInspecting.localScale = new Vector3(0.85f, 0.85f, 0.85f); // inspecting scale
        currentItemInspecting.position = cam.transform.position + cam.transform.forward * 2; // position
        currentItemInspecting.LookAt(Camera.main.transform); // looks the item at the player

            Debug.Log("Inspecting");
            WhenPlayerInspects(inspecting: true); 
        
    }

    private void StopInspecting()
    {
        currentItemInspecting.localScale = new Vector3(0.5f, 0.5f, 0.5f); // change scale
        Debug.Log("Stop Inspecting");
        WhenPlayerInspects(inspecting: false);
        ReturnItem(currentItemInspecting); // return item
    }
    
    private void WhenPlayerInspects(bool inspecting)
    {
        runeRB.isKinematic = inspecting; // Rune Rigidbody
        OutlineMesh highlightOutline = highlight.currentHighlight; // Outline Mesh Scirpt
        runeSlot = currentItemInspecting.GetComponent<RuneSlot>(); // rune slot script
        
        highlightOutline.ToggleOutline(); // runs toggle outline
        onTopVolume.SetActive(inspecting); // overlay
        camControls.enabled = !inspecting; // cam controls
        movement.enabled = !inspecting; // player movement
        crosshair.gameObject.SetActive(!inspecting); // ui crosshair
        inspectBorder.gameObject.SetActive(inspecting); // ui inspect border
        playerIsInspecting = inspecting; // inspecting bool
        runeSlot.enabled = !inspecting; // don't have rune slot on while inspecting
    }
    
    private Transform GetItemToInspectFromHighlight()
    {
        if (highlight.currentObject == null)
            return null;

        return highlight.currentObject.transform; // object your looking at
    }
    
    // saves positon and rotation
    public void SaveItemPosition(Transform item)
    {
        itemRotation = item.rotation;
        itemPosition = item.position;
    }

    // puts back to where it was saved
    public void ReturnItem(Transform item)
    {
        Transform itemMesh = item.GetChild(0);

        item.rotation = itemRotation;
        item.position = itemPosition;

        itemMesh.localRotation = Quaternion.identity;
        itemMesh.position = item.position;
    }
}
