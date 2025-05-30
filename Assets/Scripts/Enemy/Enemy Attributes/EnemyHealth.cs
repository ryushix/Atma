using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public GameObject healthUI; // Parent container (Canvas, etc)
    public Slider healthSlider;
    public Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
        ShowHealthUI(false);
    }

    public void TakeDamage(int damage, Vector2 hitSource)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        // Optional knockback
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDir = ((Vector2)transform.position - hitSource).normalized;
            float knockbackForce = 5f;
            rb.linearVelocity = knockbackDir * knockbackForce;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHealth / maxHealth;

        if (healthText != null)
            healthText.text = currentHealth + " / " + maxHealth;
    }

    public void ShowHealthUI(bool visible)
    {
        if (healthUI != null)
        {
            healthUI.SetActive(visible);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        gameObject.SetActive(false);
    }
}
