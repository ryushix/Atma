using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI (Optional)")]
    public Slider healthSlider;
    public Text healthText;

    [Header("Knockback Settings")]
    public float knockbackStrength = 6f;

    [Header("Respawn Settings")]
    public float respawnDelay = 5f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        Debug.Log($"{gameObject.name} starting with {currentHealth} HP.");
        UpdateUI();
    }

    public void TakeDamage(int damage, Vector2 hitSource)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Apply knockback
        if (rb != null)
        {
            Vector2 knockbackDir = ((Vector2)transform.position - hitSource).normalized;
            rb.linearVelocity = knockbackDir * knockbackStrength;
        }

        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHealth}");

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

        if (healthText != null)
            healthText.text = currentHealth + " / " + maxHealth;
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died. Respawning in {respawnDelay} seconds.");
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        Debug.Log($"{gameObject.name} has respawned with {currentHealth} HP.");
        UpdateUI();
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            Debug.Log($"{gameObject.name} triggered by {collision.gameObject.name}");

            // Contoh serangan dari enemy (nilai damage bisa diambil dari script enemy juga)
            TakeDamage(10, collision.transform.position);
        }
    }
}
