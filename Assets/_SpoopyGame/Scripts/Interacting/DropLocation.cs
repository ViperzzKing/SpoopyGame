using System;
using UnityEngine;

public class DropLocation : MonoBehaviour
{
    // Singleton so any script can access drop location without a reference
    public static DropLocation dropLocation;

    private void Awake()
    {
        dropLocation = this;
    }

    // Editor test to print the current drop position
    [ContextMenu("Drop Item")]
    private void TestDrop()
    {
        Debug.Log(DropAtLocation());
    }

    // How far in front of the camera an item can be dropped
    public float dropDistance = 2f;

    private Vector3 origin;
    private Vector3 lookDirection;

    public Vector3 DropAtLocation()
    {
        origin = Camera.main.transform.position;
        lookDirection = Camera.main.transform.forward;

        RaycastHit hit;
        // Cast a ray forward from the camera up to dropDistance
        bool playerLookingAtLocation = Physics.Raycast(origin, lookDirection, out hit, dropDistance);

        if (playerLookingAtLocation)
        {
            Vector3 locationHit = hit.point;
            Debug.Log(locationHit);

            // Slightly above the hit surface so the item doesn't clip into it
            return new Vector3(locationHit.x, locationHit.y + 0.3f, locationHit.z);
        }
        else
        {
            // Nothing hit - drop at max distance in front of camera
            return origin + lookDirection * dropDistance;
        }
    }
}