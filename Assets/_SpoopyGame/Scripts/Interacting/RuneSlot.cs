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
            
            Debug.Log("Slotting Rune", this);
            itemPosition.SaveItemPosition(other.transform);
            rb.isKinematic = true;
            itemPosition.ReturnItem(gameObject.transform);
            

            
            if (other.CompareTag("RuneSlot0"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(0, 1);
                transform.parent = RuneCheckmarks.RuneManager.tutorialRunes.transform;
            }
            else if (other.CompareTag("RuneSlot"))
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
            RuneCheckmarks.RuneManager.EndingTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isSlotted = false;
        
        if (other.gameObject.layer == 12)
        {
            if (other.CompareTag("RuneSlot0"))
            {
                RuneCheckmarks.RuneManager.ChangeFinish(0, -1);
            }
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
