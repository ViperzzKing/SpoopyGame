using System;
using UnityEngine;

public class RuneSlot : MonoBehaviour
{
    public LayerMask runeSlot;
    
    public InspectObject itemPosition;
    public Rigidbody rb;

    private bool isSlotted;

    private void OnTriggerEnter(Collider other)
    {
        if (isSlotted) return;
        
        if (other.gameObject.layer == 12)
        {
            isSlotted = true;
            Debug.Log("Slotting Rune");
            itemPosition.SaveItemPosition(other.transform);
            rb.isKinematic = true;
            itemPosition.ReturnItem(gameObject.transform);

            if (other.CompareTag("RuneSlot"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(1, 1);
            }
            else if (other.CompareTag("RuneSlot2"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(2, 1);
            }
            else if (other.CompareTag("RuneSlot3"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(3, 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isSlotted = false;
        
        if (other.gameObject.layer == 12)
        {
            if (other.CompareTag("RuneSlot"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(1, -1);
            }
            else if (other.CompareTag("RuneSlot2"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(2, -1);
            }
            else if (other.CompareTag("RuneSlot3"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(3, -1);
            }
        }
    }
}
