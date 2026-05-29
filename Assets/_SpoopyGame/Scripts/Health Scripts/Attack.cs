using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Whether damage is applied once or over time
    public enum Apply
    {
        Instant,
        Overtime
    }

    [SerializeField] private float damageAmount;
    [SerializeField] private Apply apply;

    // Deals damage to a victim if they have Health and aren't on the same layer
    private void DealDamage(GameObject victim)
    {
        if(victim.GetComponent<Health>() && victim.layer != gameObject.layer)
        {
            victim.GetComponent<Health>().TakeDamage(damageAmount);
            Debug.Log("Attacked with 1 damage");
        }
    }

    // Handles trigger overlap
    private void OnTriggerEnter(Collider other)
    {
        DealDamage(other.gameObject);
    }

    // Handles physical collision
    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }

    // Handles particle system hits
    private void OnParticleCollision(GameObject other)
    {
        DealDamage(other);
    }
}