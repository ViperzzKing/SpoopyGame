using System;
using UnityEngine;

public class TutorialPopups : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;

        if (gameObject.CompareTag("Activate Camera"))
        {
            // Activate Camera Tutorial
        }

        if (gameObject.CompareTag("Use Blacklight"))
        {
            // Use Blacklight Tutorial
        }

        if (gameObject.CompareTag("Use Flashlight"))
        {
            // Use Flashlight Tutorial
        }
    }
}
