using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;

    private Image healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Every frame, update the image alpha to match how damaged the health component is
    void Update()
    {
        healthBar.color = new Color(healthBar.color.r, healthBar.color.b, healthBar.color.g, health.GetDamagePercent());
    }
}
