using System;
using UnityEngine;

public class RuneSlot : MonoBehaviour
{
    [SerializeField] private bool isSlotted;
    [SerializeField] private LayerMask acceptedRuneLayer;
    private RuneCheckmarks.RuneEnding endingType;

    private String slottedRune;
    
    [Header("References")]
    public InspectObject itemPosition;
    public Rigidbody rb;
    private RuneHolder runeHolder;

    private void OnTriggerEnter(Collider other)
    {
        if (isSlotted) return;
        
        if (IsAcceptedRuneLayer(other.gameObject))
        {
            runeHolder = other.gameObject.GetComponent<RuneHolder>();
            if (runeHolder == null)
            {
                Debug.Log("RUNE ENTER: No Runeholder component!");
                return;
            }
            if (runeHolder.HasObject) return;
            
            isSlotted = true;
            runeHolder.ToggleRuneHolding(true);
            Debug.Log(other.gameObject.name);
            slottedRune = other.gameObject.name;
            transform.parent = other.transform;
            
            itemPosition.SaveItemPosition(other.transform);
            rb.isKinematic = true;
            itemPosition.ReturnItem(gameObject.transform);
            

            // checks for ending tags and runs change finish
            RuneCheckmarks.Instance?.ChangeFinish((int)endingType, 1);
            
            RuneCheckmarks.Instance?.EndingTrigger(); // runs ending trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsAcceptedRuneLayer(other.gameObject))
        {

            runeHolder = other.gameObject.GetComponent<RuneHolder>();

            if (runeHolder == null)
            {
                Debug.Log("RUNE EXIT: No Runeholder component!");
                return;
            }

            if (other.gameObject.name == slottedRune)
            {
                isSlotted = false;
                runeHolder.ToggleRuneHolding(false);
            }
            // checks tags to change finish
            if (other.CompareTag("RuneSlot0"))
            {
                RuneCheckmarks.Instance.ChangeFinish(0, -1);
            }
            if (other.CompareTag("RuneSlot"))
            {
                RuneCheckmarks.Instance.ChangeFinish(1, -1);
            }
            else if (other.CompareTag("RuneSlot2"))
            {
                RuneCheckmarks.Instance.ChangeFinish(2, -1);
            }
            else if (other.CompareTag("RuneSlot3"))
            {
                RuneCheckmarks.Instance.ChangeFinish(3, -1);
            }
        }
    }

    private bool IsAcceptedRuneLayer(GameObject obj)
    {
        return (acceptedRuneLayer.value & (1 << obj.layer)) != 0;
    }
}
