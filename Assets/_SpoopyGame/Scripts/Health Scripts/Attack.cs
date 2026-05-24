using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public enum Apply
    {
        Instant,
        Overtime
    }

    [SerializeField] private float damageAmount;
    [SerializeField] private Apply apply;
    
    private void DealDamage(GameObject victim)
    {
        //Prevents attacking an object that is of the same type
        if(victim.GetComponent<Health>() && victim.layer != gameObject.layer)
        {
            victim.GetComponent<Health>().TakeDamage(damageAmount);
            Debug.Log("Attacked with 1 damage");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DealDamage(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        DealDamage(other);
    }
}
