using System;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    private void Update()
    {
        SetTarget(target);
    }

    public void SetTarget(Transform targetedObject)
    {
        if (targetedObject == null) return;
        transform.LookAt(targetedObject);
    }
}
