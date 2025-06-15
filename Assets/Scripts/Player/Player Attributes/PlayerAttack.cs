using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D playerRb;

    [Header("Combo Settings")]
    public float comboResetTime = 0.6f;
    public int maxCombo = 3;
    public float comboCooldownTime = 0.2f;
    [HideInInspector] public bool comboFinished;
    public System.Action<int> OnComboStepChanged;

    [Header("Attack Settings")]
    public bool canAttack;
    public int damageAmount = 10;
    public float attackCooldownTime = 0f;
    public float attack3Duration = 0.4f;
    public float attackPushForce = 100f;

    private int currentCombo = 0;
    private float lastAttackTime;
    private bool isComboCooldown = false;
    private bool isAttackCooldown = false;
    private bool comboResetTriggered = false;
    private bool isAttacking = false;
    
    

    private Animator playerAnim;

    void Awake()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    public void EnableAttack()
    {
        canAttack = true;
    }

    public void DisableAttack()
    {
        canAttack = false;
    }

    public void StartCombo()
    {
        if (!canAttack) return;
        currentCombo = 1;
        PlayComboAnimation(currentCombo);
        isAttacking = true;
        comboFinished = false;
        lastAttackTime = Time.time;
    }

    public void ContinueCombo()
    {
        if (!canAttack) return;
        if (currentCombo < maxCombo)
        {
            currentCombo++;
            PlayComboAnimation(currentCombo);
            lastAttackTime = Time.time;
            OnComboStepChanged?.Invoke(currentCombo);

            if (currentCombo == maxCombo)
                StartCoroutine(DelayedComboCooldown());
        }
    }

    public bool CanContinueCombo()
    {
        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        float animProgress = stateInfo.normalizedTime % 1f;

        // Bisa lanjut combo hanya antara 60% sampai 90% animasi
        return isAttacking &&
            (stateInfo.IsName($"PlayerAttack_{currentCombo}")) &&
            animProgress > 0.9;
    }

    public bool IsComboOver()
    {
        return IsAnimationFinished() && !isComboCooldown && !isAttackCooldown;
    }


    private void PlayComboAnimation(int comboIndex)
    {
        playerAnim.Play($"PlayerAttack_{comboIndex}", 0, 0f);
        StartCoroutine(AttackCooldown());
    }

    private void ApplyAttackPush()
    {
        Vector2 pushDir = new Vector2(transform.localScale.x, 0).normalized;
        playerRb.AddForce(pushDir * attackPushForce, ForceMode2D.Impulse);
    }

    public IEnumerator AttackCooldown()
    {
        isAttackCooldown = true;

        yield return new WaitForSeconds(attackCooldownTime);

        isAttackCooldown = false;
    }

    public IEnumerator DelayedComboCooldown()
    {
        yield return new WaitForSeconds(attack3Duration);
        StartCoroutine(ComboCooldown());
    }

    public IEnumerator ComboCooldown()
    {
        isComboCooldown = true;

        playerAnim.Play("ComboReset", 0, 0f);

        yield return new WaitForSeconds(comboCooldownTime);

        currentCombo = 0;
        comboResetTriggered = false;
        isComboCooldown = false;
        comboFinished = true;
        isAttacking = false;
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f &&
            (stateInfo.IsName($"PlayerAttack_{currentCombo}") || stateInfo.IsName("ComboReset"));
    }
}
