using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
