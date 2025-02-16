using UnityEngine;

public class PlayerProjectile : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage);
        }
    }
}
