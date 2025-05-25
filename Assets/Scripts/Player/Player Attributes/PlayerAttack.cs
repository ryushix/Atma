using UnityEngine;
using System.Collections;

public class AttackCombo : MonoBehaviour
{
    [Header("Combo Settings")]
    public float comboResetTime = 1f;
    public int maxCombo = 3;
    public float comboCooldownTime = 0.7f;

    [Header("Attack Settings")]
    public float attackCooldownTime = 0.5f;
    public KeyCode attackKey = KeyCode.Mouse0;
    public bool canAttack = true;
    public float attack3Duration = 0.8f; // Misalnya, durasi animasi Attack 3


    private int currentCombo = 0;
    private float lastAttackTime;
    private bool isComboCooldown = false;
    private bool isAttackCooldown = false;
    private bool comboResetTriggered = false;

    private Animator playerAnim;

    void Awake()
    {
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && canAttack && !isComboCooldown && !isAttackCooldown)
        {
            TryAttack();
        }

        if (!isComboCooldown && currentCombo > 0 && !comboResetTriggered)
        {
            if (Time.time - lastAttackTime > comboResetTime)
            {
                StartCoroutine(ComboCooldown());
                comboResetTriggered = true;
            }
        }
    }

    void TryAttack()
    {
        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        float animProgress = stateInfo.normalizedTime % 1f;

        // Cek apakah sedang dalam animasi attack
        if ((stateInfo.IsName("PlayerAttack_1") || stateInfo.IsName("PlayerAttack_2") || stateInfo.IsName("PlayerAttack_3")) && animProgress < 0.7f)
        {
            Debug.Log("Tunggu dulu, animasi belum cukup selesai!");
            return;
        }

        Attack();
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        currentCombo++;

        if (currentCombo > maxCombo)
        {
            currentCombo = 1;
        }

        Debug.Log($"Attack Combo {currentCombo}");

        switch (currentCombo)
        {
            case 1:
                playerAnim.Play("PlayerAttack_1", 0, 0f);
                break;
            case 2:
                playerAnim.Play("PlayerAttack_2", 0, 0f);
                break;
            case 3:
                playerAnim.Play("PlayerAttack_3", 0, 0f);
                StartCoroutine(DelayedComboCooldown());
                break;
        }

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        isAttackCooldown = true;
        canAttack = false;

        yield return new WaitForSeconds(attackCooldownTime);

        isAttackCooldown = false;
        canAttack = true;
    }

    IEnumerator DelayedComboCooldown()
    {
        // Tunggu dulu biar animasi Attack 3 selesai
        yield return new WaitForSeconds(attack3Duration);
        StartCoroutine(ComboCooldown());
    }

    IEnumerator ComboCooldown()
    {
        isComboCooldown = true;
        canAttack = false;

        Debug.Log("Combo selesai! Combo cooldown dimulai...");

        playerAnim.Play("ComboReset", 0, 0f);

        yield return new WaitForSeconds(comboCooldownTime);

        currentCombo = 0;
        comboResetTriggered = false;
        canAttack = true;
        isComboCooldown = false;

        Debug.Log("Combo cooldown selesai. Bisa serang lagi!");
    }
}
