using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform player;
    
    void Update()
    {
        transform.LookAt(player);
    }
}
