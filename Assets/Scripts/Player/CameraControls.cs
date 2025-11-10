using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Sensitivity Config")]
    [SerializeField] private float sensitivity;

    [SerializeField] private float minRotation;
    [SerializeField] private float maxRotation;

    [SerializeField] float playerEyeOffset;


    private float currentHorizontalRotation;
    private float currentVerticalRotation;


    //-------------------------------------------------\\


    private void Start()
    {
        // Sets current rotation to starting rotation
        currentHorizontalRotation = transform.localEulerAngles.y;
        currentVerticalRotation = transform.localEulerAngles.x;

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
        transform.position = player.position;
        transform.position += Vector3.up * playerEyeOffset;
    }
}
