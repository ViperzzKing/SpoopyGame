using System;
using UnityEngine;

public class RuneSlot : MonoBehaviour
{
    public InspectObject itemPosition;
    public Rigidbody rb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RuneSlot"))
        {
            Debug.Log("Slotting Rune");
            itemPosition.SaveItemPosition(other.transform);
            rb.isKinematic = true;
            itemPosition.ReturnItem(gameObject.transform);
        }
    }
}
