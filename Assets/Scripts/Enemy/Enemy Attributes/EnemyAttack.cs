using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageInterval = 1.5f;

    private float lastDamageTime = 0f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount, transform.position);
                    lastDamageTime = Time.time;
                }

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                    float knockbackSpeed = 5f;
                    playerRb.linearVelocity = knockbackDir * knockbackSpeed;
                }
            }
        }
    }
}
