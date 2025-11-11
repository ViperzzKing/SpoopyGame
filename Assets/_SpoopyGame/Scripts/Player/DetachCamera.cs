using UnityEngine;

public class DetachCamera : MonoBehaviour
{
    private void Awake()
    {
        BecomeAnOrphan();
    }

    private void BecomeAnOrphan()
    {
        transform.parent = null;
    }
}
