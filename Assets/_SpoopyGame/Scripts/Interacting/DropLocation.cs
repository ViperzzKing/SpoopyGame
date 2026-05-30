using System;
using UnityEngine;

public class DropLocation : MonoBehaviour
{
    // Singleton so any script can access drop location without a reference
    public static DropLocation Instance;
    private Camera mainCamera;

    [Header("Drop Settings")] 
    [SerializeField] private float dropHeight = 0.3f;
    [SerializeField] private float dropDistance = 2f;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        
        if(mainCamera == null) Debug.LogWarning("No Main CameraFound");
    }

    // Editor test to print the current drop position
    [ContextMenu("Drop Item")]
    private void TestDrop()
    {
        Debug.Log(GetDropPosition());
    }

    // How far in front of the camera an item can be dropped

    private Vector3 origin;
    private Vector3 lookDirection;

    public Vector3 GetDropPosition()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("No main camera found, dropping at own postion");
            return transform.position + transform.forward * dropDistance;
        }
            
        
        origin = mainCamera.transform.position;
        lookDirection = mainCamera.transform.forward;

        RaycastHit hit;
        // Cast a ray forward from the camera up to dropDistance
        bool playerLookingAtLocation = Physics.Raycast(origin, lookDirection, out hit, dropDistance);

        if (playerLookingAtLocation)
        {
            Vector3 locationHit = hit.point;
            Debug.Log(locationHit);

            // Slightly above the hit surface so the item doesn't clip into it
            return new Vector3(locationHit.x, locationHit.y + dropHeight, locationHit.z);
        }
        else
        {
            // Nothing hit - drop at max distance in front of camera
            return origin + lookDirection * dropDistance;
        }
    }
}