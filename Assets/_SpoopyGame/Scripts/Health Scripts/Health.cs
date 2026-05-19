using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthMax;

    public UnityEvent onNoHealth;

    [SerializeField] private float healthCurrent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthCurrent = healthMax;    
    }

    //Debug Loop, for texting UI
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Numlock))
        {
            TakeDamage(20f);
        }
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            Heal(20f);
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
