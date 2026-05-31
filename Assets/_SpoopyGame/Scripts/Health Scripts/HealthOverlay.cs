using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthOverlay : MonoBehaviour
{
    [SerializeField] private Health health;
    private bool overlayOn;

    private Image healthOverlay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        healthOverlay = GetComponent<Image>();

        if (health == null)
        {
            Debug.LogError("missing Health reference");
        }
        if (healthOverlay == null)
        {
            Debug.LogError("missing healthOverlay reference");
        }
    }

    // Every frame, update the image alpha to match how damaged the health component is
    void Update()
    {
        if (health.HealthCurrent <= health.HealthMax / 2)
        {
            if (overlayOn) return;
            overlayOn = true;
            healthOverlay.enabled = true;
        }
        else
        {
            if(!overlayOn) return;
            overlayOn = false;
            healthOverlay.enabled = false;
        }
    }
}
