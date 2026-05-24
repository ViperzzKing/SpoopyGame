using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthMax;
    [SerializeField] private float shieldMax;

    public UnityEvent onNoHealth;

    [SerializeField] private float healthCurrent;
    [SerializeField] private float shieldCurrent; // shield for later
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthCurrent = healthMax;    
    }

    //Debug Loop, for texting UI
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


    public void TakeDamage (float damage)
    {
        //Take the current health and subtract it by damage while making sure it doesn't exeed Max or Min health
        healthCurrent = Mathf.Clamp(healthCurrent - damage, 0, healthMax);
        if(healthCurrent <= 0)
        {
            NoHealth();
        }
    }

    public void Heal(float amount)
    {
        //Take the current health and subtract it by damage while making sure it doesn't exeed Max or Min health
        healthCurrent = Mathf.Clamp(healthCurrent + amount , 0, healthMax);
    }

    private void NoHealth()
    {
        Debug.Log(name + " has met a cruel end.");
        //Sets up an event in Unity that other scripts can use
        onNoHealth.Invoke();
    }
    public float GetDamagePercent()
    {
        return 1 - (healthCurrent / healthMax);
    }
}
