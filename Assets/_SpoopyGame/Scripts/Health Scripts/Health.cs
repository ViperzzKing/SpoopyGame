using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    public float HealthMax { get; private set; }
    public float HealthCurrent { get; private set; }
    [SerializeField] private bool hasShield; // Future System
    [SerializeField] private bool isDead;
    
    // Event other scripts can hook into when health hits 0
    public UnityEvent onNoHealth;


    // Set current health to max at game start
    void Start()
    {
        HealthMax = 100;
        HealthCurrent = HealthMax;    
    }
    

    // Subtract damage from health, clamp to valid range, trigger death if at 0
    public void TakeDamage(float damage)
    {
        HealthCurrent = Mathf.Clamp(HealthCurrent - damage, 0, HealthMax);
        Debug.Log(HealthCurrent + " " + gameObject);
        if(HealthCurrent <= 0)
        {
            NoHealth();
        }
    }

    // Add health, clamp so it never exceeds max
    public void Heal(float amount)
    {
        HealthCurrent = Mathf.Clamp(HealthCurrent + amount, 0, HealthMax);
    }

    // Called when health hits 0 - fires the Unity event for other scripts
    private void NoHealth()
    {
        if (isDead) return;
        Debug.Log(name + " has met a cruel end.");
        isDead = true;
        onNoHealth?.Invoke();
    }

    // Returns how damaged this object is as a 0-1 value (0 = full, 1 = dead)
    public float GetDamagePercent()
    {
        return 1 - (HealthCurrent / HealthMax);
    }
}