using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Sensitivity Config")]
    [SerializeField] private float sensitivity;

    [SerializeField] private float minRotation;
    [SerializeField] private float maxRotation;

    public float currentEyeOffset;
    public float standingEyeOffset;
    public float crouchingEyeOffset;


    private float currentHorizontalRotation;
    private float currentVerticalRotation;


    //-------------------------------------------------\\


    private void Start()
    {
        // Sets current rotation to starting rotation
        currentHorizontalRotation = transform.localEulerAngles.y;
        currentVerticalRotation = transform.localEulerAngles.x;

        currentEyeOffset = standingEyeOffset;
    }

    private void Update()
    {
        HandleMouseMovements();
        LockCameraRotation();

        SetCameraRotation();
        PositionCamera();
    }

    //------------------- CAMERA HANDLER --------------\\


    private void HandleMouseMovements()
    {
        currentHorizontalRotation += Input.GetAxis("Mouse X") * sensitivity;
        currentVerticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
    }

    private void LockCameraRotation()
    {
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minRotation, maxRotation);
    }

    private void SetCameraRotation()
    {
        Vector3 newRotation = new Vector3();

        newRotation.x = currentVerticalRotation;
        newRotation.y = currentHorizontalRotation;

        transform.localEulerAngles = newRotation;
    }

    private void PositionCamera()
    {
        // Allows camera to follow player, ignoring y
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);

        // Changes Y
        Vector3 target = player.position + Vector3.up * currentEyeOffset;
        Vector3 newPos = transform.position;

        // Makes Crouching Smooth
        newPos.y = Mathf.Lerp(transform.position.y, target.y, Time.deltaTime * 10f);
        transform.position = newPos;
    }
}
