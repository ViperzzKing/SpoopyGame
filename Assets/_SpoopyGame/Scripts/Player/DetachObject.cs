using UnityEngine;

public class DetachObject : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
    }
}
