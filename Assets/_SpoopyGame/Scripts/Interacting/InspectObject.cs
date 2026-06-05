
using UnityEngine;
using UnityEngine.UI;

public class InspectObject : MonoBehaviour
{
    public static InspectObject Instance;
    
    [Header("References")]
    [SerializeField] private Camera cam; // gets camera.main
    [SerializeField] private CameraControls camControls; // camera controller
    [SerializeField] private BasicMovement movement; // player controller
    [SerializeField] private Highlight highlight; // highlight script
    [SerializeField] private GameObject onTopVolume; // Overlay Volume for when rune is in ground while inspecting
    [SerializeField] private Image crosshair;
    [SerializeField] private Image inspectBorder;
    [SerializeField] private Rigidbody runeRB;
    [SerializeField] private RuneSlot runeSlot;
    
    [Header("Object Settings")]
    public bool PlayerIsInspecting { get; private set; }
    public Transform CurrentItemInspecting { get; private set; }
    public Transform CurrentItemHolding { get; private set; }
    private Vector3 itemPosition;
    private Quaternion itemRotation;
    [SerializeField] private bool playerHoldingSomething;
    
    [Header("Throw Settings")]
    public float HoldThrowTime { get; private set;} = 1f;
    [SerializeField] private float thrownForceMultiplier = 12f;
    [SerializeField] private float throwHeight = 2f;
    
    [Header("Inputs")]
    [SerializeField] private KeyCode releaseObjectKey = KeyCode.E;

    public KeyCode ReleaseObjectKey
    {
        get => releaseObjectKey;
        private set => releaseObjectKey = value;
    }

    public bool ItemDropInput { get; private set; }
    public float TimePressingDropkey { get; private set; }
    

    private void Update()
    {
        bool leftMouse = Input.GetMouseButtonDown(0);
        bool eButton = Input.GetKeyDown(releaseObjectKey);
        
        // if left mouse key try toggle inspect function
        if (leftMouse)
            TryToggleInspect();
        
        // if ebutton toggle pickup
        if (eButton)
            TogglePickup();
        if (ItemDropInput) // throw manager
            HandleThrowCharge();
    }

    private void TogglePickup()
    {
        bool canPickup = !playerHoldingSomething;

        // only pickup when player is inspecting
        if (canPickup && PlayerIsInspecting)
            PickUpItem();
        else if (playerHoldingSomething && !PlayerIsInspecting) // drop instead of pickup
            ItemDropInput = true;
    }

    //item throw manager
    private void HandleThrowCharge()
    {
        TimePressingDropkey += Time.deltaTime;
        if (Input.GetKeyUp(releaseObjectKey))
        {
            if (TimePressingDropkey < HoldThrowTime)
            {
                DropItem(false);

            }
            else
            {
                DropItem(true);
            }
            TimePressingDropkey = 0;
            ItemDropInput = false;
        }
    }
    private void TryToggleInspect()
    {
        // Check if its a interactable
        bool canInspect = highlight.Interactable && highlight.CurrentObject.TryGetComponent<RuneSlot>(out RuneSlot isRune);

        if (canInspect)
            ToggleInspect();
    }
    
    private void ToggleInspect()
    {
        if (PlayerIsInspecting)
            StopInspecting();
        else
        {
            StartInspecting();
        }
    }

    private void PickUpItem()
    {
        // set all WhenPlayInspects to false
        CurrentItemInspecting.parent = cam.transform; // change parant to camera
        CurrentItemInspecting.localPosition = new Vector3(-0.27f, -0.1f, 0.36f); // change position
        CurrentItemInspecting.localRotation = Quaternion.identity; // reset rotation
        SetInspectionState(false);

        SaveItemPosition(CurrentItemInspecting); // save for stop inspection
        ReturnItem(CurrentItemInspecting); // return for stop inspection

        CurrentItemInspecting.localScale = new Vector3(0.2f, 0.2f, 0.2f); // change size
        CurrentItemInspecting.GetComponent<BoxCollider>().isTrigger = true; // turn collider to trigger

        CurrentItemHolding = CurrentItemInspecting; // set item holding to item inspecting
        runeRB.isKinematic = true; // keep the rune in place

        playerHoldingSomething = true; // bool for checks
    }

    private void DropItem(bool isThrowing)
    {
        // location used DropAtLocation()
        Vector3 location = DropLocation.Instance.GetDropPosition();

        CurrentItemHolding.parent = null; // get rid of the parant
        CurrentItemHolding.position = location; // drop where looking
        CurrentItemHolding.localScale = new Vector3(0.5f, 0.5f, 0.5f); // change size
        CurrentItemHolding.GetComponent<BoxCollider>().isTrigger = false; // turn off trigger

        runeRB.isKinematic = false; // turn these off
        playerHoldingSomething = false;
        CurrentItemHolding = null;

        //Uses Unitys RigidBody system to add a force to the object if it is being thrown.
        if (isThrowing)
        {
            runeRB.AddForce(cam.transform.forward * thrownForceMultiplier + new Vector3(0, throwHeight, 0), ForceMode.VelocityChange);
        }
    }
    
    private void StartInspecting()
    {
        CurrentItemInspecting = GetItemToInspectFromHighlight();
        CurrentItemInspecting.parent = null;
        runeRB = CurrentItemInspecting.GetComponent<Rigidbody>();
        
        SaveItemPosition(CurrentItemInspecting); // save item position for stop inspecting
        
        CurrentItemInspecting.localScale = new Vector3(0.85f, 0.85f, 0.85f); // inspecting scale
        CurrentItemInspecting.position = cam.transform.position + cam.transform.forward * 2; // position
        CurrentItemInspecting.LookAt(Camera.main.transform); // looks the item at the player
        
        SetInspectionState(inspecting: true); 
        
    }

    private void StopInspecting()
    {
        CurrentItemInspecting.localScale = new Vector3(0.5f, 0.5f, 0.5f); // change scale
        SetInspectionState(inspecting: false);
        ReturnItem(CurrentItemInspecting); // return item
    }
    
    private void SetInspectionState(bool inspecting)
    {
        runeRB.isKinematic = inspecting; // Rune Rigidbody
        OutlineMesh highlightOutline = highlight.currentHighlight; // Outline Mesh Scirpt
        runeSlot = CurrentItemInspecting.GetComponent<RuneSlot>(); // rune slot script
        
        highlightOutline.ToggleOutline(); // runs toggle outline
        onTopVolume.SetActive(inspecting); // overlay
        camControls.enabled = !inspecting; // cam controls
        movement.enabled = !inspecting; // player movement
        crosshair.gameObject.SetActive(!inspecting); // ui crosshair
        inspectBorder.gameObject.SetActive(inspecting); // ui inspect border
        PlayerIsInspecting = inspecting; // inspecting bool
        runeSlot.enabled = !inspecting; // don't have rune slot on while inspecting
    }
    
    private Transform GetItemToInspectFromHighlight()
    {
        if (highlight.CurrentObject == null)
            return null;

        return highlight.CurrentObject.transform; // object your looking at
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
