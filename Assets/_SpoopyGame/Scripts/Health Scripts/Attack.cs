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
        }
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
