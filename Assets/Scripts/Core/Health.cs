using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI (Optional)")]
    public Slider healthSlider;
    public Text healthText;

    [Header("Damage Settings")]
    public int damageFromPlayer = 10;
    public float knockbackStrength = 6f;

    [Header("Respawn Settings")]
    public float respawnDelay = 5f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        Debug.Log($"{gameObject.name} starting with {currentHealth} HP.");
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

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
        Debug.Log($"{gameObject.name} has died. Will respawn in {respawnDelay} seconds.");
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
        if (collision.gameObject.CompareTag("AttackPlayer"))
        {
            // Hitung arah knockback untuk Enemy
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized; // Enemy dari AttackPlayer
            float knockbackStrength = 5f; // Sesuaikan kekuatan knockback

            // Apply knockback ke Enemy (this object)
            Rigidbody2D enemyRb = GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.linearVelocity = knockbackDir * knockbackStrength; // Langsung set velocity
            }

            // Terima damage
            Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}. Taking {damageFromPlayer} damage.");
            TakeDamage(damageFromPlayer);
        }
    }
}