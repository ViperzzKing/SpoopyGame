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

    // Update is called once per frame
    public void TakeDamage (float damage)
    {
        healthCurrent = Mathf.Clamp(healthCurrent - damage, 0, healthMax);
        if(healthCurrent <= 0)
        {
            NoHealth();
        }
    }

    public void Heal(float amount)
    {
        healthCurrent = Mathf.Clamp(healthCurrent + amount , 0, healthMax);
    }

    private void NoHealth()
    {
        Debug.Log(name + " has met a cruel end.");
        onNoHealth.Invoke();
    }
    public float GetDamagePercent()
    {
        return 1 - (healthCurrent / healthMax);
    }
}
