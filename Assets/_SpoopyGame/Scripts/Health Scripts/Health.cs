using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthMax;
    [SerializeField] private float shieldMax;

    // Event other scripts can hook into when health hits 0
    public UnityEvent onNoHealth;

    [SerializeField] private float healthCurrent;
    [SerializeField] private float shieldCurrent; // shield for later

    // Set current health to max at game start
    void Start()
    {
        healthCurrent = healthMax;    
    }

    // Debug only - test damage and healing with keyboard
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(50f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Heal(50f);
        }
    }

    // Subtract damage from health, clamp to valid range, trigger death if at 0
    public void TakeDamage(float damage)
    {
        healthCurrent = Mathf.Clamp(healthCurrent - damage, 0, healthMax);
        if(healthCurrent <= 0)
        {
            NoHealth();
        }
    }

    // Add health, clamp so it never exceeds max
    public void Heal(float amount)
    {
        healthCurrent = Mathf.Clamp(healthCurrent + amount, 0, healthMax);
    }

    // Called when health hits 0 - fires the Unity event for other scripts
    private void NoHealth()
    {
        Debug.Log(name + " has met a cruel end.");
        onNoHealth.Invoke();
    }

    // Returns how damaged this object is as a 0-1 value (0 = full, 1 = dead)
    public float GetDamagePercent()
    {
        return 1 - (healthCurrent / healthMax);
    }
}