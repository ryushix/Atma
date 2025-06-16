using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI (Optional)")]
    public Slider healthSlider;

    [Header("Knockback Settings")]
    public float knockbackStrength = 30f;

    [Header("Respawn Settings")]
    public float respawnDelay = 5f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    [Header("Damage Settings")]
    public float damageCooldown = 1f;
    [HideInInspector] public float lastDamageTime = -Mathf.Infinity;
    // public int damageTaken;

    private Rigidbody2D rb;
    // public Vector2 hitSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Debug.Log($"{gameObject.name} starting with {currentHealth} HP.");
        UpdateUI();
    }

    public void TakeDamage(int damage, Vector2 hitSource)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Apply knockback
        if (rb != null)
        {
            Vector2 knockbackDir = ((Vector2)transform.position - hitSource);
            if (knockbackDir.magnitude < 0.1f)
                knockbackDir = Vector2.right;

            knockbackDir = knockbackDir.normalized;
            rb.AddForce(knockbackDir * knockbackStrength, ForceMode2D.Impulse);
        }

        // Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHealth}");

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        // Debug.Log($"{gameObject.name} has died. Respawning in {respawnDelay} seconds.");
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Debug.Log($"{gameObject.name} has respawned with {currentHealth} HP.");
        UpdateUI();
        gameObject.SetActive(true);
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        bool enemyCollider = collision.collider.CompareTag("EnemyAttack");
        bool hazardCollider = collision.collider.CompareTag("Hazard");
        if (enemyCollider || hazardCollider)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                int damage = 10;
                Vector2 hitSource = collision.transform.position;

                EnemyAttack enemyAttack = collision.collider.GetComponent<EnemyAttack>();
                if (enemyAttack != null)
                {
                    damage = enemyAttack.damageAmount;
                }
                var stateManager = GetComponent<PlayerStateManager>();
            if (stateManager != null)
            {
                stateManager.hitState.SetupHit(damage, hitSource);
                stateManager.SwitchState(stateManager.hitState);
            }
                // TakeDamage(damage, hitSource);
                lastDamageTime = Time.time;
            }
        }
    }
}
