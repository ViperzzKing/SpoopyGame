using NUnit.Framework;
using UnityEngine;

public class RuneHolder : MonoBehaviour
{
    public bool HasObject { get; private set; }
    public RuneCheckmarks.RuneEnding endingType;
    
    public void ToggleRuneHolding(bool holding)
    {
        HasObject = holding;
    }
}
