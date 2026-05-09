using System;
using UnityEngine;

public class DropLocation : MonoBehaviour
{
    public static DropLocation dropLocation;

    private void Awake()
    {
        dropLocation = this;
    }
    
    [ContextMenu("Drop Item")]
    private void TestDrop()
    {
        Debug.Log(DropAtLocation());
    }
    
    public float dropDistance = 2f;

    private Vector3 origin;
    private Vector3 lookDirection;
    
    public Vector3 DropAtLocation()
    {
        origin = Camera.main.transform.position;
        lookDirection = Camera.main.transform.forward;
        
        RaycastHit hit;
        bool playerLookingAtLocation = Physics.Raycast(origin, lookDirection, out hit, dropDistance);

        if (playerLookingAtLocation)
        {
            Vector3 locationHit = hit.point;
            Debug.Log(locationHit);

            return new Vector3(locationHit.x, locationHit.y + 0.3f, locationHit.z);
        }
        else
        {
            return origin + lookDirection * dropDistance;
        }
    }
}
