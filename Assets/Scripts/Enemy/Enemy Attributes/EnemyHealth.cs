using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Rigidbody2D enemyRb;

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    public GameObject healthUI; // Parent container (Canvas, etc)
    public Slider healthSlider;

    [Header("Damage Cooldown")]
    public float damageCooldown = 0.5f;
    private float lastDamageTime = -Mathf.Infinity;

    [Header("Knockback Settings")]
    public float knockbackStrength = 100f;

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
        if (enemyRb != null)
        { 
            Vector2 knockbackDir = ((Vector2)transform.position - hitSource);
            if (knockbackDir.magnitude < 0.1f)
                knockbackDir = Vector2.right;

            knockbackDir = knockbackDir.normalized;
            enemyRb.AddForce(knockbackDir * knockbackStrength, ForceMode2D.Impulse);
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
        // Debug.Log($"{gameObject.name} has died!");
        gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerAttack"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                int damage = 10;
                Vector2 hitSource = collision.transform.position;

                PlayerAttack playerAttack = collision.collider.GetComponent<PlayerAttack>();
                if (playerAttack != null)
                {
                    Debug.Log("Test");
                    damage = playerAttack.damageAmount;
                }

                var stateManager = GetComponent<EnemyStateManager>();
                if (stateManager != null)
                {
                    stateManager.hitState.SetupHit(damage, hitSource);
                    stateManager.SwitchState(stateManager.hitState);
                }
                    lastDamageTime = Time.time;
                }
            else
            {
                Debug.Log("Damage on cooldown!");
            }
        }
    }
}
