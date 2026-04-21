using UnityEngine;

public class Attack : MonoBehaviour
{
    public enum Apply
    {
        Instant,
        Overtime
    }

    public enum Type
    {
        Standard,
        Bleed,
        Electric,
        Fire,
        Ice,
        Explosion,
        Fall
    }
    [SerializeField] private float damageAmount;
    [SerializeField] private Apply apply;
    [SerializeField] private Type type;
    
    private void DealDamage(GameObject victim)
    {
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
